using Microsoft.AspNetCore.Mvc;
using TodoApi.Models;

namespace TodoApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TaskController : ControllerBase
    {
        private readonly TaskContext _context;
        public TaskController (TaskContext context)
        {
            _context = context;

            //Seed initial data
            if (_context.TaskItems.Count() == 0)
            {
                _context.TaskItems.Add(new TaskItem 
                {
                    Id = Guid.NewGuid(), 
                    Name = "Buying groceries in the morning", 
                    IsCompleted = false
                });
                _context.SaveChanges();
            }
        }

        //Get: api/task
        [HttpGet]
        public ActionResult<IEnumerable<TaskItem>> GetTasks()
        {
            return _context.TaskItems.ToList();
        }

        //Get: api/task/{id}
        [HttpGet("{id}")]
        public ActionResult<TaskItem> GetTask(Guid id)
        {
            var task = _context.TaskItems.Find(id);
            if (task == null)
            {
                return NotFound();
            }
            return task;
        }

        // POST: api/task
        [HttpPost]
        public ActionResult<TaskItem> CreateTask(TaskItem task)
        {
            task.Id = Guid.NewGuid();
            _context.TaskItems.Add(task);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetTask), new { id = task.Id}, task);
        }

        //PUT: api/task/{id}
        [HttpPut("{id}")]
        public IActionResult UpdateTask(Guid id, TaskItem task)
        {
            var existingTask = _context.TaskItems.Find(id);
            if (existingTask == null)
            {
                return NotFound();
            }

            existingTask.Name = task.Name;
            existingTask.IsCompleted = task.IsCompleted;

            _context.SaveChanges();

            return NoContent();
        }

        //DELETE: api/test/{id}
        [HttpDelete("{id}")]
        public IActionResult DeleteTask(Guid id)
        {
            var task = _context.TaskItems.Find(id);
            if ( task == null) {
                return NotFound();
            }

            _context.TaskItems.Remove(task);
            _context.SaveChanges();

            return NoContent();
        }
    }
}