using FluentValidation;
using Todo.Core.DTOs.AuthDTOs;

namespace Todo.Core.Validators.Auth;

public class RegisterUserDtoValidator : AbstractValidator<RegisterUserDto>
{
  public RegisterUserDtoValidator()
  {
      RuleFor(x => x.Id)
        .NotNull().WithMessage("Id is required.")
        .NotEmpty().WithMessage("Id cannot be empty.")
        .MaximumLength(64).WithMessage("Id must not exceed 64 characters.");

    RuleFor(x => x.Email)
        .NotEmpty().WithMessage("Email is required.")
        .MaximumLength(256).WithMessage("Email must not exceed 256 characters.")
        .EmailAddress().WithMessage("Email is not valid.");

    RuleFor(x => x.Username)
        .NotNull().WithMessage("Username is required.")
        .NotEmpty().WithMessage("Username cannot be empty.")
        .MinimumLength(3).WithMessage("Username must be at least 3 characters long.")
        .MaximumLength(256).WithMessage("Username must not exceed 256 characters.");

    RuleFor(x => x.FirstName)
        .NotNull().WithMessage("First name is required.")
        .NotEmpty().WithMessage("First name cannot be empty.")
        .MinimumLength(3).WithMessage("First name must be at least 3 characters long.")
        .MaximumLength(128).WithMessage("First name must not exceed 128 characters.");

    RuleFor(x => x.LastName)
        .NotNull().WithMessage("Last name is required.")
        .NotEmpty().WithMessage("Last name cannot be empty.")
        .MinimumLength(3).WithMessage("Last name must be at least 3 characters long.")
        .MaximumLength(128).WithMessage("Last name must not exceed 128 characters.");
  }
}
