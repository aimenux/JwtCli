using FluentValidation;

namespace App.Extensions;

public static class FluentValidationExtensions
{
    public static IRuleBuilderOptions<T, TProperty> Required<T, TProperty>(this IRuleBuilderInitial<T, TProperty> ruleBuilder)
    {
        DefaultValidatorOptions.Configurable(ruleBuilder).CascadeMode = CascadeMode.Stop;
            
        return ruleBuilder
            .NotNull()
            .WithMessage("'{PropertyName}' is required.")
            .NotEmpty()
            .WithMessage("'{PropertyName}' must not be empty.");
    }
    
    public static IRuleBuilderOptions<T, string> FileExists<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .Must<T, string>((Func<T, string, bool>) ((x, val) => File.Exists(val)))
            .WithMessage("File '{PropertyValue}' does not exist.");
    }
}