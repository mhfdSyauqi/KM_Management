﻿namespace KM_Management.EndPoint.General.Models;

public class ResponseConfigGeneral
{
    public ConfigCategory Category { get; set; }
    public ConfigTiming Timing { get; set; }
    public ConfigMail Mailing { get; set; }
    public ConfigHelpdesk Helpdesk { get; set; }
    public ConfigOthers Others { get; set; }
}