using BL.Dtos;
using BL.Dtos.ToDo;
using BL.Services.ToDoService;
using lab_2.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace lab_2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ToDoController : ControllerBase
    {
        readonly IToDoService _toDoService;

        public ToDoController(IToDoService toDoService)
        {
            _toDoService = toDoService;
        }

        [HttpGet]
        [Authorize]
        public ActionResult<List<ToDoItemDetailsDto>> GetAll()
        {
            return _toDoService.GetAll();
        }
        [HttpGet]
        [Route("{id}")]
        [Authorize]
        public ActionResult<ToDoItemDetailsDto> Get(int id)
        {
            ToDoItemDetailsDto? item = _toDoService.GetToDoItem(id);
            if (item == null) { return NotFound(); }
            return Ok(item);
        }
        [HttpPost]
        [Authorize]
        public ActionResult Add(AddToDoItemDto item)
        {
            var id = _toDoService.AddToDoItem(item);
            return Ok(new AddItemOutputDto
            {
                Id = id,
            });
        }
        [HttpDelete]
        [Route("{id}")]
        [Authorize]
        public ActionResult Delete(int id)
        {
            bool success = _toDoService.DeleteToDoItem(id);
            if (success) { return Ok(); }
            return NotFound();

        }
        [HttpPut]
        [Authorize]
        public ActionResult Update(ToDoItemDetailsDto item)
        {
            bool success = _toDoService.UpdateToDoItem(item);
            if (success) { return Ok(); }
            return NotFound();
        }
    }
}
