namespace SimpleTaskManager.Service.Dtos
{
    public class TaskDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Status { get; set; }
        public UserDto User { get; set; }
    }
}
