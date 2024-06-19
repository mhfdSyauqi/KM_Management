namespace KM_Management.EndPoint.General.Models;

public class EntityConfigGeneral
{
    public int LAYER_ONE_LIMIT { get; init; }
    public int LAYER_TWO_LIMIT { get; init; }
    public int LAYER_THREE_LIMIT { get; init; }
    public int SUGGESTION_LIMIT { get; init; }
    public int DELAY_TYPING { get; init; }
    public int IDLE_DURATION { get; init; }
    public int KEYWORDS { get; init; }

    public string MAIL_HISTORY_FROM { get; init; }
    public string MAIL_HISTORY_SUBJECT { get; init; }
    public string MAIL_HELPDESK_FROM { get; init; }
    public string MAIL_HELPDESK_TO { get; init; }
    public string MAIL_HELPDESK_SUBJECT { get; init; }
    public string MAIL_HELPDESK_CONTENT { get; init; }
    public string MAIL_HELPDESK_CONTENT_HTML { get; init; }

    public string MAIL_CONFIG_USERNAME { get; init; }
    public string MAIL_CONFIG_SERVER { get; init; }
    public int MAIL_CONFIG_PORT { get; init; }

}
