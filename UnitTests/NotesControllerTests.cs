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
        public void ApiAdd_NoteDoesNotExistsInDB_ReturnCreatedNote()
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
            var badResult = result as BadRequestObjectResult;
            Assert.AreEqual("Could not find note with id: BadID", badResult.Value);
        }

        [Test]
        public void ApiEdit_NoteDoesExistsInDB_ReturnEditedNote()
        {
            var controller = new NotesController();
            Note editNote = new Note();
            editNote.Id = "BadID";
            editNote.text = "This note does not exists in the db.";
            IActionResult result = controller.editNote(editNote);
            var badResult = result as BadRequestObjectResult;
            Assert.AreEqual("Could not find note with id: BadID", badResult.Value);
        }

        [Test]
        public void ApiEdit_NoteDoesNotExistsInDB_BadRequest()
        {
            var controller = new NotesController();
            Note editNote = new Note();
            editNote.Id = "ExistingNote";
            editNote.text = "This note exists in the db.";
            IActionResult result = controller.editNote(editNote);
            var okResult = result as OkObjectResult;
            var savedNote = okResult.Value;
            Assert.AreEqual(editNote, savedNote);
        }

        [Test]
        public void ApiGet_NoteDoesExistsInDB_ReturnNote()
        {
            var controller = new NotesController();
            IActionResult result = controller.getNote("ExistingNote");
            var okResult = result as OkObjectResult;
            var savedNote = okResult.Value as Note;
            Assert.AreEqual("ExistingNote", savedNote.Id);
        }

        [Test]
        public void ApiGet_NoteDoesNotExistsInDB_BadRequest()
        {
            var controller = new NotesController();
            IActionResult result = controller.getNote("BadID");
            var badResult = result as BadRequestObjectResult;
            Assert.AreEqual("Could not find note with id: BadID", badResult.Value);
        }

        [Test]
        public void ApiGetAll_DbExists_ReturnNoteArray()
        {
            var controller = new NotesController();
            var result = controller.getAll();
            Assert.NotNull(result);
        }
    }
}
