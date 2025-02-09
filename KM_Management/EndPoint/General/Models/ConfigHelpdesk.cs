﻿using System.Text.Json.Serialization;

namespace KM_Management.EndPoint.General.Models;

public class ConfigHelpdesk
{
    public string MAIL_HELPDESK_FROM { get; set; }
    public string MAIL_HELPDESK_TO { get; set; }
    public string MAIL_HELPDESK_SUBJECT { get; set; }
    public string MAIL_HELPDESK_CONTENT { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string MAIL_HELPDESK_CONTENT_HTML { get; set; }
}
