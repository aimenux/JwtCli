using App.Commands;
using App.Extensions;
using FluentValidation;

namespace App.Validators;

public class JwtDecodeCommandValidator : AbstractValidator<JwtDecodeCommand>
{
    public JwtDecodeCommandValidator()
    {
        RuleFor(x => x.Token)
            .Required();
    }
}