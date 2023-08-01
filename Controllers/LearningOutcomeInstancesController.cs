/*
 * Caleb Edwards
 * Andrew Dron
 * u0829971
 * u1027713
 * This controller is not used yet
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Learning_Outcomes.Data;
using Learning_Outcomes.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Net.Http.Headers;
using System.Text;
using Learning_Outcomes.Utilities;
using System.IO;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using System.Globalization;
using System.Net.Http;
using System.Security.Cryptography;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Security.Claims;

namespace Learning_Outcomes.Controllers
{
    [Authorize]
    public class LearningOutcomeInstancesController : Controller
    {

        private readonly long _fileSizeLimit = 2097152;
        private readonly string[] _permittedExtensions = {".pdf",".zip",".txt"};

        // Get the default form options so that we can use them to set the default 
        // limits for request body data.
        private static readonly FormOptions _defaultFormOptions = new FormOptions();

        private readonly Learning_OutcomesContext _context;

        public LearningOutcomeInstancesController(Learning_OutcomesContext context)
        {
            _context = context;
            //_fileSizeLimit = config.GetValue<long>("FileSizeLimit");
        }

        // GET: LearningOutcomeInstances
        public async Task<IActionResult> Index()
        {
            var learning_OutcomesContext = _context.LearningOutcomes.Include(l => l.CourseInstances);
            return View(await learning_OutcomesContext.ToListAsync());
        }

        // GET: LearningOutcomeInstances/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var learningOutcomeInstances = await _context.LearningOutcomes
                .Include(l => l.CourseInstances)
                .FirstOrDefaultAsync(m => m.LearningOutcomeInstancesID == id);
            if (learningOutcomeInstances == null)
            {
                return NotFound();
            }

            return View(learningOutcomeInstances);
        }

        // GET: LearningOutcomeInstances/Create
        public IActionResult Create()
        {
            ViewData["CourseInstancesID"] = new SelectList(_context.Courses, "CourseInstancesID", "CourseName");
            return View();
        }

        // POST: LearningOutcomeInstances/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LearningOutcomeInstancesID,Name,Description,CourseInstancesID")] LearningOutcomeInstances learningOutcomeInstances)
        {
            if (ModelState.IsValid)
            {
                _context.Add(learningOutcomeInstances);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CourseInstancesID"] = new SelectList(_context.Courses, "CourseInstancesID", "CourseName", learningOutcomeInstances.CourseInstancesID);
            return View(learningOutcomeInstances);
        }

        // GET: LearningOutcomeInstances/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var learningOutcomeInstances = await _context.LearningOutcomes.FindAsync(id);
            if (learningOutcomeInstances == null)
            {
                return NotFound();
            }
            ViewData["CourseInstancesID"] = new SelectList(_context.Courses, "CourseInstancesID", "CourseName", learningOutcomeInstances.CourseInstancesID);
            return View(learningOutcomeInstances);
        }

        // POST: LearningOutcomeInstances/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("LearningOutcomeInstancesID,Name,Description,CourseInstancesID")] LearningOutcomeInstances learningOutcomeInstances)
        {
            if (id != learningOutcomeInstances.LearningOutcomeInstancesID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(learningOutcomeInstances);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LearningOutcomeInstancesExists(learningOutcomeInstances.LearningOutcomeInstancesID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CourseInstancesID"] = new SelectList(_context.Courses, "CourseInstancesID", "CourseName", learningOutcomeInstances.CourseInstancesID);
            return View(learningOutcomeInstances);
        }

        // GET: LearningOutcomeInstances/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var learningOutcomeInstances = await _context.LearningOutcomes
                .Include(l => l.CourseInstances)
                .FirstOrDefaultAsync(m => m.LearningOutcomeInstancesID == id);
            if (learningOutcomeInstances == null)
            {
                return NotFound();
            }

            return View(learningOutcomeInstances);
        }

        // POST: LearningOutcomeInstances/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var learningOutcomeInstances = await _context.LearningOutcomes.FindAsync(id);
            _context.LearningOutcomes.Remove(learningOutcomeInstances);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LearningOutcomeInstancesExists(int id)
        {
            return _context.LearningOutcomes.Any(e => e.LearningOutcomeInstancesID == id);
        }

        public async Task<IActionResult> NotePage(int id)
        {
            var outcome = await _context.LearningOutcomes.Include(s => s.Note).FirstOrDefaultAsync(o => o.LearningOutcomeInstancesID == id);
            return View(outcome);
        }

        [HttpPost]
        public JsonResult ChangeNote(string note, int noteID, int LOID)
        {
            var note_from_db = _context.LearningOutcomeNotes.Find(noteID);
            if (note_from_db == null)
            {
                var outcome = _context.LearningOutcomes.Find(LOID);

                LearningOutcomeNoteInstances newNote = new LearningOutcomeNoteInstances
                {
                    //CourseNoteInstancesID = noteID,
                    LearningOutcomeNoteInstancesID = LOID,
                    LONote = note,
                    DateTime = DateTime.Now.ToString("MM/dd/yyyy")
                };
                if (User.IsInRole("Chair"))
                {
                    newNote.ChairEdited = true;
                }
                outcome.Note = newNote;
                _context.LearningOutcomeNotes.Add(newNote);
                
                _context.SaveChanges();

                noteID = newNote.LearningOutcomeNoteInstancesID;

            }
            else
            {
                note_from_db.LONote = note;
                note_from_db.DateTime = DateTime.Now.ToString("MM/dd/yyyy");
                if (User.IsInRole("Chair"))
                {
                    note_from_db.ChairEdited = true;
                }
                else
                {
                    note_from_db.ChairEdited = false;
                }
                _context.SaveChanges();
            }

            return Json(new { success = true, CourseNote = note, CourseNoteInstancesID = noteID });
        }

        /// <summary>
        /// Approves a note 
        /// </summary>
        /// <param name="noteID"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ApproveNote(int noteID)
        {
            var note = _context.LearningOutcomeNotes.Find(noteID);
            note.ChairEdited = true;
            _context.SaveChanges();
            return Json(new { success = true });
        }

        #region Files

        /// <summary>
        /// Method to upload a file to the database through http stream
        /// Taken from the .net core sample application modified to fit our applitcation
        /// https://docs.microsoft.com/en-us/aspnet/core/mvc/models/file-uploads?view=aspnetcore-3.0#upload-large-files-with-streaming
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [DisableFormValueModelBinding]
        public async Task<IActionResult> UploadDatabase()
        {
            if (!MultipartRequestHelper.IsMultipartContentType(Request.ContentType))
            {
                ModelState.AddModelError("File",
                    $"The request couldn't be processed (Error 1).");

                return BadRequest(ModelState);
            }
            // Accumulate the form data key-value pairs in the request (formAccumulator).
            var formAccumulator = new KeyValueAccumulator();
            var trustedFileNameForDisplay = string.Empty;
            var untrustedFileNameForStorage = string.Empty;
            var streamedFileContent = new byte[0];

            var boundary = MultipartRequestHelper.GetBoundary(
                MediaTypeHeaderValue.Parse(Request.ContentType),
                _defaultFormOptions.MultipartBoundaryLengthLimit);
            var reader = new MultipartReader(boundary, HttpContext.Request.Body);
            var section = await reader.ReadNextSectionAsync();

            while (section != null)
            {
                var hasContentDispositionHeader =
                    ContentDispositionHeaderValue.TryParse(
                        section.ContentDisposition, out var contentDisposition);

                if (hasContentDispositionHeader)
                {
                    if (MultipartRequestHelper
                        .HasFileContentDisposition(contentDisposition))
                    {
                        untrustedFileNameForStorage = contentDisposition.FileName.Value;
                        // Don't trust the file name sent by the client. To display
                        // the file name, HTML-encode the value.
                        trustedFileNameForDisplay = WebUtility.HtmlEncode(
                                contentDisposition.FileName.Value);

                        streamedFileContent =
                            await FileHelpers.ProcessStreamedFile(section, contentDisposition,
                                ModelState, _permittedExtensions, _fileSizeLimit);
                        var errors = ModelState.Where(x => x.Value.Errors.Any())
                        .Select(x => new { x.Key, x.Value.Errors });

                        if (!ModelState.IsValid)
                        {
                            return BadRequest(ModelState);
                        }
                    }
                    else if (MultipartRequestHelper
                        .HasFormDataContentDisposition(contentDisposition))
                    {
                        // Don't limit the key name length because the 
                        // multipart headers length limit is already in effect.
                        var key = HeaderUtilities
                            .RemoveQuotes(contentDisposition.Name).Value;
                        var encoding = GetEncoding(section);

                        if (encoding == null)
                        {
                            ModelState.AddModelError("File",
                                $"The request couldn't be processed (Error 2).");

                            return BadRequest(ModelState);
                        }

                        using (var streamReader = new StreamReader(
                            section.Body,
                            encoding,
                            detectEncodingFromByteOrderMarks: true,
                            bufferSize: 1024,
                            leaveOpen: true))
                        {
                            // The value length limit is enforced by 
                            // MultipartBodyLengthLimit
                            var value = await streamReader.ReadToEndAsync();

                            if (string.Equals(value, "undefined",
                                StringComparison.OrdinalIgnoreCase))
                            {
                                value = string.Empty;
                            }

                            formAccumulator.Append(key, value);

                            if (formAccumulator.ValueCount >
                                _defaultFormOptions.ValueCountLimit)
                            {
                                // Form key count limit of 
                                // _defaultFormOptions.ValueCountLimit 
                                // is exceeded.
                                ModelState.AddModelError("File",
                                    $"The request couldn't be processed (Error 3).");

                                return BadRequest(ModelState);
                            }
                        }
                    }
                }

                // Drain any remaining section body that hasn't been consumed and
                // read the headers for the next section.
                section = await reader.ReadNextSectionAsync();
            }

            // Bind form data to the model
            var formData = new FormData();
            var formValueProvider = new FormValueProvider(
                BindingSource.Form,
                new FormCollection(formAccumulator.GetResults()),
                CultureInfo.CurrentCulture);
            var bindingSuccessful = await TryUpdateModelAsync(formData, prefix: "",
                valueProvider: formValueProvider);

            if (!bindingSuccessful)
            {
                ModelState.AddModelError("File",
                    "The request couldn't be processed (Error 5).");
                return BadRequest(ModelState);
            }

            var headers = HttpContext.Request.Headers;
            int ID = Int32.Parse(headers["OutcomeID"]);
            var outCome = await _context.LearningOutcomes.FindAsync(ID);

            //verify file with virus total
            if (!await verifyFile(streamedFileContent))
            {
                ModelState.AddModelError("File", "File failed virus total check, please" +
                    " scan file with virus total and try again");
                return BadRequest("ERROR VALIDATING FILE");
            }

            var file = new LearningOutcomeFile()
            {
                Content = streamedFileContent,
                UntrustedName = untrustedFileNameForStorage,
                Size = streamedFileContent.Length,
                UploadDT = DateTime.UtcNow,
                LearningOutcomeInstancesID = ID
            };

            _context.learningOutcomeFiles.Add(file);
            await _context.SaveChangesAsync();

            //check file type to be associated with the learning outcome
            if (headers["File-type"] == "Definition")
            {
                file.isDef = true;
                outCome.DefenitionFile = file;
            }
            else if (headers["File-type"] == "Example")
            {
                file.isDef = false;
                file.Score = Int32.Parse(headers["OutcomeScore"]);
            }
            await _context.SaveChangesAsync();

            return Created(nameof(LearningOutcomeFile), file);
        }

        /// <summary>
        /// Retrieves a file from the db given 
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public async Task<FileResult> DownloadFile(int? ID)
        {
            LearningOutcomeFile file = await _context.learningOutcomeFiles.FindAsync(ID);
            if(file == null)
            {
                return null;
            }
            var ext = Path.GetExtension(file.UntrustedName).ToLowerInvariant();
            var types = GetMimeTypes();
            return File(file.Content, types[ext]);
        }

        /// <summary>
        /// Deletes a file from the DB
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> DeleteFile(int? ID)
        {
            LearningOutcomeFile file = await _context.learningOutcomeFiles.FindAsync(ID);
            if (file == null)
            {
                return Json(new { success = false });
            }
            LearningOutcomeInstances outcome = await _context.LearningOutcomes.FindAsync(file.LearningOutcomeInstancesID);

            _context.learningOutcomeFiles.Remove(file);
            await _context.SaveChangesAsync();

            return Json(new { success = true });
        }
        /// <summary>
        /// Gets the current encoding of a HTTP section
        /// Taken from https://docs.microsoft.com/en-us/aspnet/core/mvc/models/file-uploads?view=aspnetcore-3.0#upload-large-files-with-streaming
        /// </summary>
        /// <param name="section"></param>
        /// <returns></returns>
        private static Encoding GetEncoding(MultipartSection section)
        {
            var hasMediaTypeHeader =
                MediaTypeHeaderValue.TryParse(section.ContentType, out var mediaType);

            // UTF-7 is insecure and shouldn't be honored. UTF-8 succeeds in 
            // most cases.
            if (!hasMediaTypeHeader || Encoding.UTF7.Equals(mediaType.Encoding))
            {
                return Encoding.UTF8;
            }

            return mediaType.Encoding;
        }
        /// <summary>
        /// Taken From:
        /// https://www.c-sharpcorner.com/article/upload-download-files-in-asp-net-core-2-0/
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
            {
                {".txt", "text/plain"},
                {".pdf", "application/pdf"},
                {".doc", "application/vnd.ms-word"},
                {".docx", "application/vnd.ms-word"},
                {".xls", "application/vnd.ms-excel"},
                {".xlsx", "application/vnd.openxmlformatsofficedocument.spreadsheetml.sheet"},  
                {".png", "image/png"},
                {".jpg", "image/jpeg"},
                {".jpeg", "image/jpeg"},
                {".gif", "image/gif"},
                {".csv", "text/csv"},
                {".zip","application/zip" }
            };
        }
        /// <summary>
        /// Verify given file content using the virus total API
        /// Returns false if the scan returns positive or 
        /// if the scan returns false
        /// </summary>
        /// <param name="streamedFileContent"></param>
        /// <returns></returns>
        private async Task<bool> verifyFile(byte[] streamedFileContent)
        {
            //ADD FILE VERIFICATION
            //https://stackoverflow.com/questions/42543679/get-md5-checksum-of-byte-arrays-conent-in-c-sharp
            byte[] hash;
            using (var md5 = MD5.Create())
            {
                //md5.TransformFinalBlock(streamedFileContent, 0, streamedFileContent.Length);               
                hash = md5.ComputeHash(streamedFileContent);
            }

            var userName = User.FindFirstValue(ClaimTypes.Name);
            var inst = await _context.Instructors.Where(i => i.InstructorUserName == userName).FirstOrDefaultAsync();
            var Api = inst.apiKey;
            var url = "http://www.virustotal.com/vtapi/v2/file/report?apikey=" + Api + "&resource=" + Encoding.Default.GetString(hash);

            var client = new HttpClient();

            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                builder.Append(hash[i].ToString("x2"));
            }

            // Create the HttpContent for the form to be posted.
            var requestContent = new FormUrlEncodedContent(new[] {
                new KeyValuePair<string, string>("apikey", Api),
                new KeyValuePair<string, string>("resource",builder.ToString())
            });

            try
            {
                // Get the response.
                HttpResponseMessage response = await client.PostAsync(
                    "http://www.virustotal.com/vtapi/v2/file/report",
                    requestContent);

                // Get the response content.
                HttpContent responseContent = response.Content;

                string responseString = await responseContent.ReadAsStringAsync();

                var responseJson = JObject.Parse(responseString);

                if (responseJson["response_code"].ToString() == "0")
                {
                    return false;
                }
                else if (responseJson["positives"].ToString() != "0")
                {
                    return false;
                }
            }
            catch(Exception e)
            {
                return false;
            }

            return true;

        }

        public class FormData
        {
            public string Note { get; set; }
        }
        #endregion
    }
}
