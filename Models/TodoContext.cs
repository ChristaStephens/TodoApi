
using Microsoft.EntityFrameworkCore;

namespace TodoApi.Models{
    public class TodoContext: DbContext
    {
        public TodoContext(DbContextOptions<TodoContext> options)
            :base(options){

            }
            public DbSet<TodoItem> TodoItems {get; set;}
    }
}





//will coordinate Entity Framework Functionality for a data model
