using Documentally.Application.Interfaces.Persistence;
using Documentally.Domain.Entities;
using FluentResults;

namespace Documentally.Infrastructure.Repository;

public class UserRepository : IUserRepository
{
    private static readonly List<User> users = new();

    /// <inheritdoc/>
    public Result<User> Add(User user)
    {
        users.Add(user);
        
        var result = new Result<User>();

        return result.WithSuccess("User add to database");
    }

    /// <inheritdoc/>
    public User? GetByEmail(string email)
    {
        var user = users.Where(x => x.Email.Equals(email)).FirstOrDefault();

        return user;
    }

    /// <inheritdoc/>
    public User? GetById(Guid id)
    {
        var user = users.Where(x => x.Id.Value.Equals(id)).FirstOrDefault();

        return user;
    }
}
