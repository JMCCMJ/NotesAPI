using Microsoft.EntityFrameworkCore;

namespace NotesAPI
{
    public class NotesContext : DbContext
    {
        public DbSet<Note> Notes { get; set; }

        public NotesContext(): base()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlite("Data Source=notes.db");
        }
            
    }

}
