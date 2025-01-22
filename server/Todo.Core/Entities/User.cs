using System.ComponentModel.DataAnnotations;

namespace Todo.Core.Entities;

/// <summary>
///     Represents a user in the application with a unique Id, Email, and Password.
///     A user has a first and last name, and a list of TaskLists.
///     Inherits from IdentityUser, which provides the Id, Email, and Password properties.
/// </summary>
public class User
{
    /// <summary>
    ///     This is the id of the user.
    /// </summary>
    [Key]
    public string Id { get; set; }

    /// <summary>
    ///     The email of the user.
    /// </summary>
    [Required]
    public string Email { get; set; }

    /// <summary>
    ///     The username of the user.
    /// </summary>
    [Required]
    public string Username { get; set; }

    /// <summary>
    ///     The phone number of the user.
    /// </summary>
    public string? PhoneNumber { get; set; }

    /// <summary>
    ///     The first name of the user.
    ///     Required, must be between 3 and 25 characters.
    /// </summary>
    [Required]
    [StringLength(25, MinimumLength = 3)]
    public string FirstName { get; set; } = string.Empty;

    /// <summary>
    ///     The last name of the user.
    ///     Required, must be between 3 and 25 characters.
    /// </summary>
    [Required]
    [StringLength(25, MinimumLength = 3)]
    public string LastName { get; set; } = string.Empty;

    /// <summary>
    ///     The full name of the user.
    ///     Computed property that returns the concatenation of the first and last name.
    /// </summary>
    public string Name => $"{FirstName} {LastName}";

    /// <summary>
    ///     The list of TaskLists that the user has.
    ///     A user can have multiple TaskLists.
    ///     A TaskList can only belong to one user.
    /// </summary>
    public List<TaskList> Lists { get; set; } = [];

    /// <summary>
    ///     The Id of the refresh token of the user.
    ///     Used for refreshing the access token.
    /// </summary>
    public Guid? RefreshTokenId { get; set; }

    /// <summary>
    ///     The refresh token of the user.
    ///     Used for refreshing the access token.
    ///     A user can have only one refresh token.
    ///     A refresh token can only belong to one user.
    ///     This is a navigation property.
    /// </summary>
    public virtual RefreshToken? RefreshToken { get; set; }
}