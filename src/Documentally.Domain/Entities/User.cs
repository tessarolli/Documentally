using Documentally.Domain.BaseClasses;
using Documentally.Domain.ValueObjects;

namespace Documentally.Domain.Entities;

public class User : Entity<UserId>
{
    public string FirstName { get; private set; } = null!;
    public string LastName { get; private set; } = null!;
    public string Email { get; private set; } = null!;
    public string Password { get; private set; } = null!;
    public string Role { get; private set; } = null!;

    public static User Create(string firstName, string lastName, string email, string password)
    {
        return new User
        {
            Id = new UserId(),
            FirstName = firstName,
            LastName = lastName,
            Email = email,
            Password = password
        };
    }
}
