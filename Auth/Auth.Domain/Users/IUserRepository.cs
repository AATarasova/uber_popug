namespace Auth.Domain.Users;

public interface IUserRepository
{
    Task<User> GetById(UserId id);
    Task<IReadOnlyCollection<User>> ListAll();
    Task<UserId> Create(CreateUserDto user);
}