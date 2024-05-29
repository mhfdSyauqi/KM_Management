using KM_Management.Commons.Mediator;
using KM_Management.EndPoint.General.Models;
using KM_Management.Shared;
using System.Net;

namespace KM_Management.EndPoint.General.Query;

public record GetConfigGeneralQuery : IQuery<ResponseConfigGeneral>;

public class GetConfigGeneralHandler : IQueryHandler<GetConfigGeneralQuery, ResponseConfigGeneral>
{
    private readonly IGeneralRepository _generalRepository;

    public GetConfigGeneralHandler(IGeneralRepository generalRepository)
    {
        _generalRepository = generalRepository;
    }

    public async Task<Result<ResponseConfigGeneral>> Handle(GetConfigGeneralQuery request, CancellationToken cancellationToken)
    {
        var generalCfg = await _generalRepository.GetConfigurationGeneralAsync(cancellationToken);

        if (generalCfg == null)
        {
            return Result.Failure<ResponseConfigGeneral>(new(HttpStatusCode.NotFound, "Configuration not found, please ask ICT"));
        }

        var resposne = new ResponseConfigGeneral()
        {
            Category = new()
            {
                LAYER_ONE_LIMIT = generalCfg.LAYER_ONE_LIMIT,
                LAYER_TWO_LIMIT = generalCfg.LAYER_TWO_LIMIT,
                LAYER_THREE_LIMIT = generalCfg.LAYER_THREE_LIMIT,
                SUGGESTION_LIMIT = generalCfg.SUGGESTION_LIMIT
            },
            Timing = new()
            {
                DELAY_TYPING = generalCfg.DELAY_TYPING,
                IDLE_DURATION = generalCfg.IDLE_DURATION,
            },
            Mailing = new()
            {
                MAIL_HISTORY_FROM = generalCfg.MAIL_HISTORY_FROM,
                MAIL_HISTORY_SUBJECT = generalCfg.MAIL_HISTORY_SUBJECT,
            },
            Helpdesk = new()
            {
                MAIL_HELPDESK_FROM = generalCfg.MAIL_HELPDESK_FROM,
                MAIL_HELPDESK_TO = generalCfg.MAIL_HELPDESK_TO,
                MAIL_HELPDESK_SUBJECT = generalCfg.MAIL_HELPDESK_SUBJECT,
                MAIL_HELPDESK_CONTENT = generalCfg.MAIL_HELPDESK_CONTENT,
                MAIL_HELPDESK_CONTENT_HTML = generalCfg.MAIL_HELPDESK_CONTENT_HTML
            },
            Others = new()
            {
                KEYWORDS = generalCfg.KEYWORDS
            }
        };

        return Result.Success(resposne);
    }
}
