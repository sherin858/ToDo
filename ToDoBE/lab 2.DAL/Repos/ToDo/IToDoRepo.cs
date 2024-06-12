using DAL.Models;

namespace DAL.Repos
{
    public interface IToDoRepo
    {
        public List<ToDoItem> GetAll();
        public ToDoItem? GetToDoItem(int id);
        public bool UpdateToDoItem(ToDoItem item);
        public bool DeleteToDoItem(int id);
        public int AddToDoItem(ToDoItem item);
    }
}
