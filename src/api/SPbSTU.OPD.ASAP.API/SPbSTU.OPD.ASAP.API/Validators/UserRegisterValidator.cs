using FluentValidation;
using SPbSTU.OPD.ASAP.API.Domain.Enums;
using SPbSTU.OPD.ASAP.API.Dto;

namespace SPbSTU.OPD.ASAP.API.Validators;

public class UserRegisterValidator : AbstractValidator<UserRegisterDto>
{
    public UserRegisterValidator()
    {
        RuleFor(user => user.Email)
            .NotEmpty()
            .EmailAddress();
        RuleFor(user => user.Role)
            .NotEmpty()
            .Must(r => r == Enum.GetName(Role.Mentor) || r == Enum.GetName(Role.Student));
    }
}