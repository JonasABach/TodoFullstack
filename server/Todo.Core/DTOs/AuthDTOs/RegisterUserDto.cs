using System.ComponentModel.DataAnnotations;

namespace Todo.Core.DTOs.AuthDTOs;

/// <summary>
///     Data transfer object for registering a new user.
/// </summary>
public class RegisterUserDto
{
    /// <summary>
    ///     The id of the user from the firebase authentication.
    /// </summary>
    public required string Id { get; set; }

    /// <summary>
    ///     The email address of the user.
    /// </summary>
    public required string Email { get; set; }

    /// <summary>
    ///     The username of the user.
    /// </summary>
    public required string Username { get; set; }

    /// <summary>
    ///     The first name of the user.
    /// </summary>
    public required string FirstName { get; set; }

    /// <summary>
    ///     The last name of the user.
    /// </summary>
    public required string LastName { get; set; }
}