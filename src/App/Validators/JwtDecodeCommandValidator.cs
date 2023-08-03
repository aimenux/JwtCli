using App.Commands;
using FluentValidation;

namespace App.Validators;

public class JwtDecodeCommandValidator : AbstractValidator<JwtDecodeCommand>
{
    public JwtDecodeCommandValidator()
    {
        RuleFor(x => x.Token)
            .NotEmpty();
    }
}