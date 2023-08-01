/*
 * Caleb Edwards
 * u0829971
 * This controller is used to display a list of the instructors
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

namespace Learning_Outcomes.Controllers
{
    public class InstructorInstancesController : Controller
    {
        private readonly Learning_OutcomesContext _context;

        public InstructorInstancesController(Learning_OutcomesContext context)
        {
            _context = context;
        }

        // GET: InstructorInstances
        public async Task<IActionResult> Index()
        {
            return View(await _context.Instructors.ToListAsync());
        }

        // GET: InstructorInstances/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            /*            var instructorInstances = await _context.Instructors
                            .FirstOrDefaultAsync(m => m.InstructorInstancesID == id);*/
            var instructorInstances = await _context.Instructors
                .Include(s => s.Courses)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.InstructorInstancesID == id);


            if (instructorInstances == null)
            {
                return NotFound();
            }

            return View(instructorInstances);
        }

        // GET: InstructorInstances/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: InstructorInstances/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("InstructorInstancesID,InstructorUserName")] InstructorInstances instructorInstances)
        {
            if (ModelState.IsValid)
            {
                _context.Add(instructorInstances);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(instructorInstances);
        }

        // GET: InstructorInstances/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var instructorInstances = await _context.Instructors.FindAsync(id);
            if (instructorInstances == null)
            {
                return NotFound();
            }
            return View(instructorInstances);
        }

        // POST: InstructorInstances/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("InstructorInstancesID,InstructorUserName")] InstructorInstances instructorInstances)
        {
            if (id != instructorInstances.InstructorInstancesID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(instructorInstances);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InstructorInstancesExists(instructorInstances.InstructorInstancesID))
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
            return View(instructorInstances);
        }

        // GET: InstructorInstances/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var instructorInstances = await _context.Instructors
                .FirstOrDefaultAsync(m => m.InstructorInstancesID == id);
            if (instructorInstances == null)
            {
                return NotFound();
            }

            return View(instructorInstances);
        }

        // POST: InstructorInstances/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var instructorInstances = await _context.Instructors.FindAsync(id);
            _context.Instructors.Remove(instructorInstances);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InstructorInstancesExists(int id)
        {
            return _context.Instructors.Any(e => e.InstructorInstancesID == id);
        }
    }
}
