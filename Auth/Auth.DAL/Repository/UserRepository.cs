using Auth.DAL.Context;
using Auth.Domain.Credentials;
using Auth.Domain.Roles;
using Auth.Domain.Users;
using Microsoft.EntityFrameworkCore;
using DbUser = Auth.Domain.Models.User;

namespace Auth.DAL.Repository;

public class UserRepository : IUserRepository, ICredentialsService, IRolesManager
{
    private readonly AuthDbContext _dbContext;
            
    public UserRepository(AuthDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<User> GetById(UserId id)
    {
        var dbTemplate = await _dbContext.Users.FirstAsync(t => t.UserId == id.Value);
        return Convert(dbTemplate);
    }

    public async Task<IReadOnlyCollection<User>> ListAll()
    {
        var templates = _dbContext.Users
            .Select(Convert)
            .ToList();
        return await Task.FromResult(templates);
    }

    public async Task<UserId> Create(CreateUserDto user)
    {
        var entity = new DbUser
        {
            Email = user.Email.ToLower(),
            Password = user.Password,
            LastName = user.LastName,
            FirstName = user.FirstName,
            MiddleName = user.MiddleName,
            Role = user.Role    
        };
        await _dbContext.AddAsync(entity);
        await _dbContext.SaveChangesAsync();

        return new UserId(entity.UserId);
    }

    private User Convert(DbUser t) => new()
    {
        Id = new UserId(t.UserId),
        PublicId = t.PublicId,
        Email = t.Email,
        Password = t.Password,
        UserName = new UserName(t.LastName, t.FirstName, t.MiddleName)
    };

    public async Task<UserId?> FindUser(string email, string password)
    {
        var user =  await _dbContext.Users.FirstOrDefaultAsync(t =>
            t.Email == email.ToLower() && t.Password == password);
        return user is null ? null : new UserId(user.UserId);
    }

    public async Task ChangeRole(UserId userId, Role newRole)
    {
        var user = await _dbContext.Users.FirstAsync(t => t.UserId == userId.Value);
        user.Role = newRole;
        _dbContext.Update(user);
        
        await _dbContext.SaveChangesAsync();
    }
}