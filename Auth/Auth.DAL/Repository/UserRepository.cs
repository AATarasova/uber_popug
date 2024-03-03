using Auth.DAL.Context;
using Auth.Domain.Credentials;
using Auth.Domain.Roles;
using Auth.Domain.Users;
using Microsoft.EntityFrameworkCore;
using DbUser = Auth.Domain.Models.User;

namespace Auth.DAL.Repository;

public class UserRepository(AuthDbContext dbContext) : IUserRepository, ICredentialsService, IRolesManager
{
    public async Task<User> GetById(UserId id)
    {
        var dbTemplate = await dbContext.Users.FirstAsync(t => t.UserId == id.Value);
        return Convert(dbTemplate);
    }

    public async Task<IReadOnlyCollection<User>> ListAll()
    {
        var templates = dbContext.Users
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
        await dbContext.AddAsync(entity);
        await dbContext.SaveChangesAsync();

        return new UserId(entity.UserId);
    }

    private User Convert(DbUser t) => new()
    {
        Id = new UserId(t.UserId),
        PublicId = t.PublicId,
        Email = t.Email,
        Password = t.Password,
        UserName = new UserName(t.LastName, t.FirstName, t.MiddleName),
        Role = t.Role
    };

    public async Task<UserId?> FindUser(string email, string password)
    {
        var user =  await dbContext.Users.FirstOrDefaultAsync(t =>
            t.Email == email.ToLower() && t.Password == password);
        return user is null ? null : new UserId(user.UserId);
    }

    public async Task ChangeRole(UserId userId, Role newRole)
    {
        var user = await dbContext.Users.FirstAsync(t => t.UserId == userId.Value);
        user.Role = newRole;
        dbContext.Update(user);
        
        await dbContext.SaveChangesAsync();
    }
}