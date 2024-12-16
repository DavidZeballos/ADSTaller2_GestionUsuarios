namespace src.Application.DTOs
{
    public class ProgressDTO
    {
        public List<string> AddSubjectIds { get; set; } = new List<string>();
        public List<string> RemoveSubjectIds { get; set; } = new List<string>();
    }
}