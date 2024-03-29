using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoApi.Models;

namespace TodoApi.Controllers
{
    //this is the route or url path for a get method.
    //show's how it's constructed
    //You can change the route name: "[Route("api/[controller]")] from
    //controller to the name of the controller file, but lower case.

    [Route("api/todo")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        private readonly TodoContext _context;
        public TodoController(TodoContext context)
        {
            _context = context;
            if (_context.TodoItems.Count() == 0)
            {
                // Create a new TodoItem if collection is empty,
                // which means you can't delete all TodoItems.
                _context.TodoItems.Add(new TodoItem { Name = "Item1" });
                _context.SaveChanges();


            }
        }

        // GET: api/Todo - this is an endpoint that gets the entire list
        //this signals a method that responds to a http get request.
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoItem>>> GetTodoItems()
        {
            return await _context.TodoItems.ToListAsync();
        }

        // GET: api/Todo/5 - this is an endpoint that gets by an id
        [HttpGet("{id}")]
        //actionresult helps to return the body of the response message
        //it also handles the response code - such as notfound()
        public async Task<ActionResult<TodoItem>> GetTodoItem(long id)
        {
            var todoItem = await _context.TodoItems.FindAsync(id);

            if (todoItem == null)
            {
                return NotFound();
            }

            return todoItem;
        }

        //Post: api/Todo an endpoint that creates a new item
        //creates the value of the to-do item from the body of
        //of the http request
        [HttpPost]
        public async Task<ActionResult<TodoItem>> PostTodoItem(TodoItem item)
        {
            _context.TodoItems.Add(item);
            await _context.SaveChangesAsync();

            //CreatedAtAction return a 201 status code (ok) if successful
            //also adds a location header to the response.
            //references the GetTodoItem action to create the location header
            //nameof is used to prevent hard-coding the action name in the CreatedAtAction
            return CreatedAtAction(nameof(GetTodoItem), new { id = item.Id}, item);
        }

        //Put: api/Todo/5 endpoint to change an item.
        //updates the entire item, like a form.
        //if you want to update just parts of it use a patch method
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTodoItem(long id, TodoItem item)
        {
            if (id != item.Id)
            {
                return BadRequest();
            }

            //saying tat we are tracking the items state in the
            //database then saving it
            _context.Entry(item).State = EntityState.Modified;
            //savechangesasync() saves changes to database
            await _context.SaveChangesAsync();

            //returns a 204 No content response
            return NoContent();
        }


        //Delete: api/Todo/5 an endpoint that deletes an item
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodoItem(long id)
        {
            //FindAsync looks for the todo item
            var todoItem = await _context.TodoItems.FindAsync(id);
            if (todoItem == null)
            {
                return NotFound();
            }
            _context.TodoItems.Remove(todoItem);
            await _context.SaveChangesAsync();

            //returns a 204 No content response
            return NoContent();
        }
    }
}
