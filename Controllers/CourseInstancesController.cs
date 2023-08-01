/*
 * Caleb Edwards   
 * Andrew Dron
 * u0829971
 * u1027713
 * This class controls all the accesses for the links that are accessing course views
 * 
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
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using System.Net.Http;

namespace Learning_Outcomes.Controllers
{
    public class CourseInstancesController : Controller
    {
        private readonly Learning_OutcomesContext _context;
        private UserManager<IdentityUser> _userManager;

        public CourseInstancesController(Learning_OutcomesContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        /*
         * This function changes the note based on ID and saves it to the db/note table
         */
        [HttpPost]
        public JsonResult ChangeNote(string note, int noteID, int CourseInstancesID)
        {
            var note_from_db = _context.CourseNotes.Find(noteID);
            if(note_from_db == null)
            {
                var course = _context.Courses.Find(CourseInstancesID);

                CourseNoteInstances newNote = new CourseNoteInstances
                {
                    //CourseNoteInstancesID = noteID,
                    CourseInstancesID = CourseInstancesID,
                    CourseNote = note,
                    Course = course,
                    DateTime = DateTime.Now.ToString("MM/dd/yyyy")
                };
                course.Note = newNote;
                _context.CourseNotes.Add(newNote);
                _context.SaveChanges();
                
                noteID = newNote.CourseNoteInstancesID;
            }
            else
            {
                note_from_db.CourseNote = note;
                note_from_db.DateTime = DateTime.Now.ToString("MM/dd/yyyy");
                note_from_db.Approved = false;
                _context.SaveChanges();
            }
            
            return Json(new { success=true, CourseNote=note, CourseNoteInstancesID=noteID});
        }

        // GET: CourseInstances
        // Shows all the courses to the admin
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            await updateCompletion();
            return View(await _context.Courses.ToListAsync());
        }

        // Shows all the courses to the chair
        [Authorize(Roles = "Chair")]
        public async Task<IActionResult> ChairView()
        {
            await updateCompletion();
            return View(await _context.Courses.ToListAsync());
        }

        //Shows the instructors their specific classes
        [Authorize(Roles = "Instructor")]
        public async Task<IActionResult> ProfessorView()
        {
            await updateCompletion();
            if (User.IsInRole("Instructor"))
            {
                var userUsed = User.Identity.Name;
                var InstructorUsed = await _context.Instructors
                    .Include(s => s.Courses)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(m => m.InstructorUserName == userUsed);
                var courseList = new List<CourseInstances>();

                foreach (var course in InstructorUsed.Courses)
                {
                    courseList.Add(course);
                }
                return View(courseList);
            }
            else
                //return View(await _context.Courses.ToListAsync());
                return NotFound();
        }

        // Shows an uneditable version of LO's for chair and instr
        public async Task<IActionResult> InstrAndChairLOView(int ?id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var courseInstances = await _context.Courses
                .Include(s => s.LearningOutcomes)
                .Include(n => n.Note)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.CourseInstancesID == id);

            var learningOutcomes = _context.LearningOutcomes.Where(s=> s.CourseInstancesID == id).Include(s => s.DefenitionFile).Include(s => s.OutcomeFiles).ToList();

            ViewData["Outcomes"] = learningOutcomes;
            //check if the current user has a non empty api key
            if (User.IsInRole("Instructor"))
            {
                var userName = User.FindFirstValue(ClaimTypes.Name);
                var inst = await _context.Instructors.Where(i => i.InstructorUserName == userName).FirstOrDefaultAsync();
            
                ViewData["Key"] = inst.apiKey != "";
            }

            if (courseInstances == null)
            {
                return NotFound();
            }

            return View(courseInstances);
        }

        // GET: CourseInstances/Details/5
        // Shows editable LO's for the Admin
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var courseInstances = await _context.Courses
                .Include(s => s.LearningOutcomes)
                .Include(n => n.Note)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.CourseInstancesID == id);

            if (courseInstances == null)
            {
                return NotFound();
            }

            var learningOutcomes = _context.LearningOutcomes.Where(s => s.CourseInstancesID == id).Include(s => s.DefenitionFile).Include(s => s.OutcomeFiles).ToList();

            ViewData["Outcomes"] = learningOutcomes;

            return View(courseInstances);
        }

        // GET: CourseInstances/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: CourseInstances/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("CourseID,InstructorInstancesID,CourseName,Description,Dept,Semester,Year")] CourseInstances courseInstances)
        {
            if (ModelState.IsValid)
            {
                _context.Add(courseInstances);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(courseInstances);
        }

        // GET: CourseInstances/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var courseInstances = await _context.Courses.FindAsync(id);
            if (courseInstances == null)
            {
                return NotFound();
            }
            return View(courseInstances);
        }

        // POST: CourseInstances/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CourseID,InstructorInstancesID,CourseName,Description,Dept,Semester,Year")] CourseInstances courseInstances)
        {
            if (id != courseInstances.CourseInstancesID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(courseInstances);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CourseInstancesExists(courseInstances.CourseInstancesID))
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
            return View(courseInstances);
        }

        // GET: CourseInstances/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var courseInstances = await _context.Courses
                .FirstOrDefaultAsync(m => m.CourseInstancesID == id);
            if (courseInstances == null)
            {
                return NotFound();
            }

            return View(courseInstances);
        }

        // POST: CourseInstances/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var courseInstances = await _context.Courses.FindAsync(id);
            _context.Courses.Remove(courseInstances);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CourseInstancesExists(int id)
        {
            return _context.Courses.Any(e => e.CourseInstancesID == id);
        }
        /// <summary>
        /// Approves a note
        /// </summary>
        /// <param name="noteID"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ApproveNote(int noteID)
        {
            var note = _context.CourseNotes.Find(noteID);
            note.Approved = true;
            _context.SaveChanges();
            return Json(new { success = true });
        }

        private async Task<bool> updateCompletion()
        {
            try
            {
                var courses = await _context.Courses.Include(e => e.LearningOutcomes).ThenInclude(e => e.OutcomeFiles).ToListAsync();
                foreach (CourseInstances course in courses)
                {
                    double courseCount = course.LearningOutcomes.Count;
                    double completeCount = 0;
                    foreach (LearningOutcomeInstances outcome in course.LearningOutcomes)
                    {
                        if (outcome.DefenitionFile != null && outcome.OutcomeFiles.Count != 0)
                        {
                            completeCount++;
                        }
                    }
                    course.Completion = Convert.ToInt32(completeCount / courseCount * 100);

                }
                await _context.SaveChangesAsync();
            }
            catch(Exception e)
            {
                Console.WriteLine("error updating course completion");
                return false;
            }
            return true;
        }
        /// <summary>
        /// uploads an apikey to the server
        /// </summary>
        /// <param name="apiKey"></param>
        /// <returns></returns>
        /// 
        [HttpGet]
        public async Task<JsonResult> uploadApiKey(string apiKey)
        {
            var userName = User.FindFirstValue(ClaimTypes.Name);
            var inst = await _context.Instructors.Where(i => i.InstructorUserName == userName).FirstOrDefaultAsync();
            inst.apiKey = apiKey;
            await _context.SaveChangesAsync();
            return Json( new{ success = true});
        }
    }
}
