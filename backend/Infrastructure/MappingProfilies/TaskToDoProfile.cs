using AutoMapper;
using BussinesLogic.EntityDtos;
using Domain.Entities;

namespace Infrastructure.MappingProfilies
{
    public class TaskToDoProfile : Profile
    {
        public TaskToDoProfile()
        {
            CreateMap<TaskToDoDto, TaskToDo>()
                .ReverseMap();
        }
    }
}
