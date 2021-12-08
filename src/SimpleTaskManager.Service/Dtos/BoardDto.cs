using System.Collections.Generic;
using System.Linq;

namespace SimpleTaskManager.Service.Dtos
{
    public class BoardDto
    {
        public BoardDto()
        {
            Users = Enumerable.Empty<UserDto>();
            Tasks = Enumerable.Empty<TaskDto>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<UserDto> Users { get; set; }
        public IEnumerable<TaskDto> Tasks { get; set; }
    }
}
