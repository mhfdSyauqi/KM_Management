namespace KM_Management.EndPoint.AssistantProfile.Models
{
    public class EntityPostAssistantProfile
    {
        public string? AppName { get; set; }
        public string? AppImage { get; set; }
        public IList<IFormFile>? Files { get; set; }
    }
}
