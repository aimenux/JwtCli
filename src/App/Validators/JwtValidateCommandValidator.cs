using App.Commands;
using App.Extensions;
using FluentValidation;

namespace App.Validators;

public class JwtValidateCommandValidator : AbstractValidator<JwtValidateCommand>
{
    public JwtValidateCommandValidator()
    {
        RuleFor(x => x.Token)
            .Required();
        
        When(x => string.IsNullOrWhiteSpace(x.ParametersFile), () =>
            {
                RuleFor(x => x.Certificate)
                    .Required()
                    .FileExists();
                RuleFor(x => x.Password)
                    .Required();
            })
            .Otherwise(() =>
            {
                RuleFor(x => x.ParametersFile)
                    .Required()
                    .FileExists();
            });
    }
}