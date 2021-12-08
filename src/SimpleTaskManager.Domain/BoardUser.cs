namespace SimpleTaskManager.Domain
{
    public class BoardUser
    {
        public int BoardId { get; set; }
        public Board Board { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }
    }
}
