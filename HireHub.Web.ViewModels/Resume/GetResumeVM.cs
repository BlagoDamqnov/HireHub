namespace HireHub.Web.ViewModels.Resume
{
    public class GetResumeVM
    {
        public int Id { get; set; }
        public string CreatorId { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string ResumePath { get; set; } = null!;
    }
}