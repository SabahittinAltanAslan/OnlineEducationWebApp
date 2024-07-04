namespace OnlineEducationWebApp.Models.DTO
{
    public class LessonCreateRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime LessonDate { get; set; }
        public TimeSpan LessonTime { get; set; }
    }
}