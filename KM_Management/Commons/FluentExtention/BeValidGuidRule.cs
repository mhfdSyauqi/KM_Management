using FluentValidation;

namespace KM_Management.Commons.FluentExtention;

public static class BeValidGuidRule
{
    public static IRuleBuilderOptions<T, string> BeValidGuid<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
                .NotNull()
                .NotEmpty()
                .Must(prop =>
                {
                    bool isValidGuid = Guid.TryParse(prop, out Guid result);
                    return isValidGuid;
                })
                .WithMessage("Identity is not vaild");
    }
}
