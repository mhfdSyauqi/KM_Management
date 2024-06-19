namespace KM_Management.EndPoint.General.Models;

public class FilterConfigGeneral
{
    public int First_Layer { get; set; }
    public int Second_Layer { get; set; }
    public int Third_Layer { get; set; }
    public int Suggestion { get; set; }
    public int Delay { get; set; }
    public int Idle { get; set; }
    public string History_From { get; set; }
    public string History_Subject { get; set; }
    public string Helpdesk_From { get; set; }
    public string Helpdesk_To { get; set; }
    public string Helpdesk_Subject { get; set; }
    public string Helpdesk_Content { get; set; }
    public string Helpdesk_Content_Html { get; set; }
    public int Keywords { get; set; }
    public string Mailbot_Email { get; set; }
    public string Mailbot_Password { get; set; }
    public string Mailbot_Server { get; set; }
    public int Mailbot_Port { get; set; }
}
