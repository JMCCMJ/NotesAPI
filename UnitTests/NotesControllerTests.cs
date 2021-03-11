using Microsoft.AspNetCore.Mvc;
using NotesAPI.Controllers;
using NUnit.Framework;

namespace NotesAPI
{        

    [TestFixture]
    public class NotesControllerTest
    {

        private NotesController _notesController;

        [SetUp]
        public void SetUp()
        {
            _notesController = new NotesController();

            Note note = new Note();
            note.Id = "DeleteID";
            note.text = "This note will be deleted.";

            _notesController.addNote(note);
        }

        [Test]
        public void ApiDelete_NoteExistsInDB_ReturnNoteDeleted()
        {
            var controller = new NotesController();
            IActionResult result = controller.deleteNote("DeleteID");
            var okResult = result as OkObjectResult;
            Assert.AreEqual("Note Deleted", okResult.Value);
        }

        [Test]
        public void ApiDelete_NoteDoesNotExistsInDB_BadRequest()
        {
            var controller = new NotesController();
            IActionResult result = controller.deleteNote("BadID");
            var okResult = result as OkObjectResult;
            Assert.Null(okResult);
        }
    }
}
