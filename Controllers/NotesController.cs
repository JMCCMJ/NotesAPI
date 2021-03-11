using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace NotesAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NotesController : ControllerBase
    {

        //Adds a new note
        //*Currently user provides an id*
        //*Better version would auto generate the id with Guid.NewGuid.ToString()*
        [HttpPost("/api/[controller]/addNote")]
        public IActionResult addNote(Note note)
        {
            using (var db = new NotesContext())
            {
                //Use this if user is not sending their own ID
                //note.Id = Guid.NewGuid().ToString();
                Note checkIfExists = findNote(note.Id);

                if (checkIfExists == null)
                {
                    db.Add(note);
                    db.SaveChanges();
                    Console.WriteLine("Added note with id: " + note.Id);
                    return Ok(note);
                }
                else
                {
                    Console.WriteLine("Note already exists with id: " + note.Id);
                    return BadRequest("Note already exists with id: " + note.Id);
                }
            }
        }

        //Gets a list of all notes in the DB
        [HttpGet("/api/[controller]/getAll")]
        public Note[] getAll()
        {
            using (var db = new NotesContext())
            {
                Console.WriteLine("Querying for all notes");
                Note[] notes = db.Notes.ToArray();
                return notes;
            }
        }

        //Get a specific note by it's id
        [HttpGet("/api/[controller]/getNote/{id}")]
        public IActionResult getNote(string id)
        {
            using (var db = new NotesContext())
            {
                Console.WriteLine("Request for note with id: " + id);
                Note note = findNote(id);
                if (note != null)
                {
                    Console.WriteLine("Found note with id: " + id);
                    return Ok(note);
                }
                else
                {
                    Console.WriteLine("Could not find note with id: " + id);
                    return BadRequest("Could not find note with id: " + id);
                }
            }
        }

        
        //Finds a note by id
        [ApiExplorerSettings(IgnoreApi = true)]
        public Note findNote(string id)
        {
            using (var db = new NotesContext())
            {
                Note note = db.Notes.FirstOrDefault(note => note.Id == id);
                return note;
            }
        }
    }
}
