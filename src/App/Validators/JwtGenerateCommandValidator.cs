using App.Commands;
using App.Extensions;
using FluentValidation;

namespace App.Validators;

public class JwtGenerateCommandValidator : AbstractValidator<JwtGenerateCommand>
{
    public JwtGenerateCommandValidator()
    {
        RuleFor(x => x.ExpireInMinutes)
            .InclusiveBetween(1, int.MaxValue);
        
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