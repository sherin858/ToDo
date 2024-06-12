using BL.Dtos;
using BL.Dtos.ToDo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Services.ToDoService
{
    public interface IToDoService
    {
        public List<ToDoItemDetailsDto> GetAll();
        public ToDoItemDetailsDto? GetToDoItem(int id);
        public bool UpdateToDoItem(ToDoItemDetailsDto item);
        public bool DeleteToDoItem(int id);
        public int AddToDoItem(AddToDoItemDto item);
    }
}
