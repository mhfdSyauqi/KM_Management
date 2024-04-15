using FluentValidation;
using KM_Management.Commons.Mediator;
using KM_Management.EndPoint.General.Models;
using KM_Management.Shared;
using System.Net;

namespace KM_Management.EndPoint.General.Command;

public record PatchConfigGeneralCommand(RequestConfigGeneral Argument) : ICommand;

public class PatchConfigGeneralValidator : AbstractValidator<PatchConfigGeneralCommand>
{
    public PatchConfigGeneralValidator()
    {
        RuleFor(key => key.Argument.Category.LAYER_ONE_LIMIT).GreaterThan(0).WithMessage("Value cannot less than 1");
        RuleFor(key => key.Argument.Category.LAYER_TWO_LIMIT).GreaterThan(0).WithMessage("Value cannot less than 1");
        RuleFor(key => key.Argument.Category.LAYER_THREE_LIMIT).GreaterThan(0).WithMessage("Value cannot less than 1");
        RuleFor(key => key.Argument.Category.SUGGESTION_LIMIT).GreaterThan(0).WithMessage("Value cannot less than 1");

        RuleFor(key => key.Argument.Timing.DELAY_TYPING)
            .GreaterThan(499).WithMessage("Value cannot less than 0.5 sec")
            .LessThan(2000).WithMessage("Value cannot more than 2 sec");
        RuleFor(key => key.Argument.Timing.IDLE_DURATION)
            .GreaterThan(59999).WithMessage("Value cannot less than 1 min");

        RuleFor(key => key.Argument.Mailing.MAIL_HISTORY_FROM).NotEmpty();
        RuleFor(key => key.Argument.Mailing.MAIL_HISTORY_SUBJECT).NotEmpty();

        RuleFor(key => key.Argument.Helpdesk.MAIL_HELPDESK_FROM).NotEmpty();
        RuleFor(key => key.Argument.Helpdesk.MAIL_HELPDESK_TO).NotEmpty();
        RuleFor(key => key.Argument.Helpdesk.MAIL_HELPDESK_SUBJECT).NotEmpty();
        RuleFor(key => key.Argument.Helpdesk.MAIL_HELPDESK_CONTENT).NotEmpty();
        RuleFor(key => key.Argument.Helpdesk.MAIL_HELPDESK_CONTENT_HTML).NotEmpty();

        RuleFor(key => key.Argument.Others.KEYWORDS).NotEmpty().GreaterThanOrEqualTo(3).WithMessage("Value cannot less than 3");
    }
}

public class PatchConfigGeneralHandler : ICommandHandler<PatchConfigGeneralCommand>
{
    private readonly IGeneralRepository _generalRepository;
    private readonly IValidator<PatchConfigGeneralCommand> _validator;

    public PatchConfigGeneralHandler(IGeneralRepository generalRepository, IValidator<PatchConfigGeneralCommand> validator)
    {
        _generalRepository = generalRepository;
        _validator = validator;
    }

    public async Task<Result> Handle(PatchConfigGeneralCommand request, CancellationToken cancellationToken)
    {
        var validation = await _validator.ValidateAsync(request, cancellationToken);

        if (!validation.IsValid)
        {
            var errorMsg = validation.Errors.First()?.ErrorMessage;
            return Result.Failure(new(HttpStatusCode.BadRequest, errorMsg));
        }

        var filterPatchConfig = new FilterConfigGeneral()
        {
            First_Layer = request.Argument.Category.LAYER_ONE_LIMIT,
            Second_Layer = request.Argument.Category.LAYER_TWO_LIMIT,
            Third_Layer = request.Argument.Category.LAYER_THREE_LIMIT,
            Suggestion = request.Argument.Category.SUGGESTION_LIMIT,

            Delay = request.Argument.Timing.DELAY_TYPING,
            Idle = request.Argument.Timing.IDLE_DURATION,

            History_From = request.Argument.Mailing.MAIL_HISTORY_FROM,
            History_Subject = request.Argument.Mailing.MAIL_HISTORY_SUBJECT,

            Helpdesk_From = request.Argument.Helpdesk.MAIL_HELPDESK_FROM,
            Helpdesk_To = request.Argument.Helpdesk.MAIL_HELPDESK_TO,
            Helpdesk_Subject = request.Argument.Helpdesk.MAIL_HELPDESK_SUBJECT,
            Helpdesk_Content = request.Argument.Helpdesk.MAIL_HELPDESK_CONTENT,
            Helpdesk_Content_Html = request.Argument.Helpdesk.MAIL_HELPDESK_CONTENT_HTML,

            Keywords = request.Argument.Others.KEYWORDS
        };

        await _generalRepository.PatchConfigurationGeneralAsync(filterPatchConfig, cancellationToken);
        return Result.Success();
    }
}

