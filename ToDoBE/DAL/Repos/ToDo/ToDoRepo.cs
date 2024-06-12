using DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repos.ToDo
{
    public class ToDoRepo : IToDoRepo
    {
        protected ToDoContext _toDoContext;
        public ToDoRepo(ToDoContext toDoContext) 
        { 
            _toDoContext = toDoContext;
        }
        public int AddToDoItem(ToDoItem item)
        {
            _toDoContext.TodoItems.Add(item);
            _toDoContext.SaveChanges();
            return item.Id;
        }

        public bool DeleteToDoItem(int id)
        {
            var todoItem = _toDoContext.TodoItems.Find(id);
            if (todoItem == null)
            {
                return false;
            }

            _toDoContext.TodoItems.Remove(todoItem);
            _toDoContext.SaveChanges();
            return true;
        }

        public List<ToDoItem> GetAll()
        {
            var todos =  _toDoContext.TodoItems.ToList();
            return todos;
        }

        public ToDoItem? GetToDoItem(int id)
        {
            var todo = _toDoContext.TodoItems.Find(id);
            return todo;

        }

        public bool UpdateToDoItem(ToDoItem item)
        {
            var toDoItem = _toDoContext.TodoItems.Find(item.Id);
            if (toDoItem == null) 
            { 
                return false; 
            }
            toDoItem.Title = item.Title;
            toDoItem.Description = item.Description;
            _toDoContext.SaveChanges();
            return true;
        }
    }
}
