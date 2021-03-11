using Microsoft.AspNetCore.Mvc;
using NotesAPI.Controllers;
using NUnit.Framework;
using System;


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

            Note noteToDelete = new Note();
            noteToDelete.Id = "DeleteID";
            noteToDelete.text = "This note will be deleted.";
            _notesController.addNote(noteToDelete);

            Note noteThatAlreadyExistrs = new Note();
            noteThatAlreadyExistrs.Id = "ExistingNote";
            noteThatAlreadyExistrs.text = "This note exists in the db.";
            _notesController.addNote(noteThatAlreadyExistrs);
        }

        [Test]
        public void ApiAdd_NoteDoesNotExistsInDB_ReturnNoteCreated()
        {
            var controller = new NotesController();
            Note newNote = new Note();
            newNote.Id = Guid.NewGuid().ToString();
            newNote.text = "This is a new note.";
            IActionResult result = controller.addNote(newNote);
            var okResult = result as OkObjectResult;
            var savedNote = okResult.Value;
            Assert.AreEqual(newNote, savedNote);
        }

        [Test]
        public void ApiAdd_NoteDoesExistsInDB_BadRequest()
        {
            var controller = new NotesController();
            Note newNote = new Note();
            newNote.Id = "ExistingNote";
            newNote.text = "This note should fail to add.";
            IActionResult result = controller.addNote(newNote);
            var badResult = result as BadRequestObjectResult;
            Assert.AreEqual("Note already exists with id: ExistingNote", badResult.Value);
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
