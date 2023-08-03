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
                    .Cascade(CascadeMode.Stop)
                    .NotEmpty()
                    .Must(File.Exists).WithMessage("File '{PropertyValue}' does not exist.");
                RuleFor(x => x.Password)
                    .NotEmpty();
            })
            .Otherwise(() =>
            {
                RuleFor(x => x.ParametersFile)
                    .Cascade(CascadeMode.Stop)
                    .NotEmpty()
                    .Must(File.Exists);
            });
    }
}