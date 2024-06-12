using AutoMapper;
using BL.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Models;
using DAL.Repos;
using BL.Dtos.ToDo;

namespace BL.Services.ToDoService
{
    public class ToDoService: IToDoService
    {
        protected IToDoRepo _toDoRepo;
        private readonly IMapper _mapper;
        public ToDoService(IMapper mapper,IToDoRepo toDoRepo)
        {
            _toDoRepo = toDoRepo;
            _mapper = mapper;
        }
        public int AddToDoItem(AddToDoItemDto item)
        {
            var toDoItem = new ToDoItem()
            {
                Title = item.Title,
                Description = item.Description,
                IsDone = item.IsDone
            };
            return _toDoRepo.AddToDoItem(toDoItem);
        }

        public bool DeleteToDoItem(int id)
        {
            return _toDoRepo.DeleteToDoItem(id);
        }

        public List<ToDoItemDetailsDto> GetAll()
        {
            var ToDoItems = _toDoRepo.GetAll();
            return ToDoItems.Select(item => new ToDoItemDetailsDto
            {
                Title= item.Title,
                Description= item.Description,
                IsDone= item.IsDone,
                Id = item.Id

            }).ToList();
        }

        public ToDoItemDetailsDto? GetToDoItem(int id)
        {
            var item = _toDoRepo.GetToDoItem(id);
            if (item == null) return null;
            return new ToDoItemDetailsDto
            {
                Title = item.Title,
                Description = item.Description,
                IsDone = item.IsDone,
                Id = item.Id
            };
        }

        public bool UpdateToDoItem(ToDoItemDetailsDto item)
        {
            var toDoItem = new ToDoItem()
            {
                Title = item.Title,
                Description = item.Description,
                IsDone = item.IsDone
            };
            return _toDoRepo.UpdateToDoItem(toDoItem);
        }
    }
}
