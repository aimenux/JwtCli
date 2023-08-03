using App.Commands;
using FluentValidation;

namespace App.Validators;

public class JwtValidateCommandValidator : AbstractValidator<JwtValidateCommand>
{
    public JwtValidateCommandValidator()
    {
        RuleFor(x => x.Token)
            .NotEmpty();
        
        When(x => string.IsNullOrWhiteSpace(x.ParametersFile), () =>
            {
                RuleFor(x => x.Certificate)
                    .NotEmpty()
                    .Must(File.Exists).WithMessage("File '{PropertyValue}' does not exist.");
                RuleFor(x => x.Password)
                    .NotEmpty();
            })
            .Otherwise(() =>
            {
                RuleFor(x => x.ParametersFile)
                    .NotEmpty()
                    .Must(File.Exists);
            });
    }
}