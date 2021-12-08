using AutoMapper;
using SimpleTaskManager.Domain;
using SimpleTaskManager.Service.Dtos;
using SimpleTaskManager.Service.UseCases.Boards.CreateBoard;
using System.Linq;

namespace SimpleTaskManager.Service.Configurations.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CreateBoardRequest, Board>();

            CreateMap<Board, BoardDto>()
                .ForMember(
                    boardDto => boardDto.Users,
                    opt => opt.MapFrom(entity =>
                        entity.BoardUsers.Select(bu => bu.User)));

            CreateMap<TaskItem, TaskDto>()
                .ForMember(
                    taskDto => taskDto.Status, 
                    opt => opt.MapFrom(entity => entity.Status.ToString()));

            CreateMap<User, UserDto>();
        }
    }
}
