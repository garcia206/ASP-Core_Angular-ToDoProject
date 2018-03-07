using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using realDemoProject.Models;
using System.Linq;

namespace realDemoProject.Controllers
{
    /*
     Replaces [controller] with the name of the controller, which is the controller class 
     name minus the "Controller" suffix. For this sample, the controller class name is TodoController 
     and the root name is "todo". ASP.NET Core routing isn't case sensitive.
     */
    [Route("api/[Controller]")]
    public class TodoController: Controller
    {
        private readonly TodoContext _context;

        /* Constructor
         The constructor uses Dependency Injection to inject the database context (TodoContext)
         into the controller. The database context is used in each of the CRUD methods in the controller.
         */
        public TodoController(TodoContext context)
        {
            _context = context;

            //Add an 2 items to in-memory db if empty
            if (_context.TodoItems.Count() == 0)
            {
                _context.TodoItems.Add(new TodoItem {Name = "Walk Dog"});
                _context.TodoItems.Add(new TodoItem {Name = "Buy Groceries"});
                _context.SaveChanges();
            }
        }

        /*
         The GetAll method returns an IEnumerable. MVC automatically serializes
         the object to JSON and writes the JSON into the body of the response message. 
         The response code for this method is 200, assuming there are no unhandled exceptions.
         */
        // GET /api/todo
        [HttpGet]
        public IEnumerable<TodoItem> GetAll()
        {
            return _context.TodoItems.ToList();
        }

        /*
         "{id}" is a placeholder variable for the ID of the todo item. When GetById is invoked, it assigns 
         the value of "{id}" in the URL to the method's id parameter.
         */
        // GET /api/todo/{id}
        [HttpGet("{id}", Name = "GetTodo")]
        public IActionResult GetById(long id)
        {
            var item = _context.TodoItems.FirstOrDefault(t => t.id == id);
            if (item == null)
            {
                // Returning NotFound returns an HTTP 404 response.
                return NotFound();
            }
            // Returning ObjectResult returns an HTTP 200 response.
            return new ObjectResult(item);
        }

        /*
         The [FromBody] attribute tells MVC to get the value of the to-do 
         item from the body of the HTTP request.
         */
        // POST /api/todo
        public IActionResult Create([FromBody] TodoItem item)
        {
            if (item == null)
            {
                return BadRequest();
            }

            _context.TodoItems.Add(item);
            _context.SaveChanges();
            
            /*
             The CreatedAtRoute method returns a 201 response. 
             1. HTTP 201 is the standard response 
                for an HTTP POST method that creates a new resource on the server.
             2. Adds a Location header to the response. The Location header specifies 
                the URI of the newly created to-do item.
             3. Uses the "GetTodo" named route to create the URL. The "GetTodo" named route is defined in GetById:
             */
            return CreatedAtRoute("GetTodo", new {id = item.id}, item);
        }

        // A PUT request requires the client to send the entire updated entity, not just the deltas.
        // PUT /api/todo/{id}
        [HttpPut("{id}")]
        public IActionResult Update(long id, [FromBody] TodoItem item)
        {
            if (item == null || item.id != id)
            {
                return BadRequest();
            }

            var todo = _context.TodoItems.FirstOrDefault(t => t.id == id);
            if(todo == null)
            {
                return NotFound();
            }

            // Set found Todo item properties equal to passed in item
            todo.IsComplete = item.IsComplete;
            todo.Name = item.Name;

            _context.TodoItems.Update(todo);
            _context.SaveChanges();

            // The response is 204 (No Content).
            return new NoContentResult();
        }

        // DELETE /api/todo/{id}
        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            var todo = _context.TodoItems.FirstOrDefault(t => t.id == id);
            if (todo == null)
            {
                return NotFound();
            }

            _context.TodoItems.Remove(todo);
            _context.SaveChanges();
            return new NoContentResult();
        }

    }
}