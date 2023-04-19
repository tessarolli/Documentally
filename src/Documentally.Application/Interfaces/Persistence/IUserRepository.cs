using Documentally.Domain.Entities;
using FluentResults;

namespace Documentally.Application.Interfaces.Persistence;

public interface IUserRepository
{
    User? GetById(Guid id);

    User? GetByEmail(string email);

    Result<User> Add(User user);
}
