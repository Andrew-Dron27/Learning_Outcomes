/*
 * Caleb Edwards
 * Andrew Dron
 * u0829971
 * u1027713
 * This controller is mainly used for the role page view and returns that view
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Learning_Outcomes.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Learning_Outcomes.Data;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Learning_Outcomes.Controllers
{
    public class HomeController : Controller
    {
        private UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly Learning_OutcomesContext _context;

        /// <summary>
        /// Initialize globals
        /// </summary>
        /// <param name="userManager"></param>
        /// <param name="roleManager"></param>
        /// <param name="context"></param>
        public HomeController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, Learning_OutcomesContext context)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _context = context;
        }


        /// <summary>
        /// deletes user from table and denies request for role change
        /// </summary>
        /// <param name="username"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<JsonResult> Deny_RoleRequest(string username, string role)
        {
            var user = await _userManager.FindByNameAsync(username);

            var remove = await _context.RequestedRoles
                        .AsNoTracking()
                        .FirstOrDefaultAsync(m => m.Username == username);

            _context.RequestedRoles.Remove(remove);
            _context.SaveChanges();


            //change users role
            return Json(new { success = true, responseText = "denied role request to table" });
        }
         
        /// <summary>
        /// Changes the role of requested user from admin
        /// deletes user from requested role table
        /// </summary>
        /// <param name="username"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<JsonResult> Submit_RoleRequest(string username, string role)
        {
            var user = await _userManager.FindByNameAsync(username);
            var userrole = await _roleManager.FindByNameAsync(username);

            await _userManager.RemoveFromRoleAsync(user, userrole.ToString());
            await _userManager.AddToRoleAsync(user, role);
            var remove = await _context.RequestedRoles
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Username == username);

            _context.RequestedRoles.Remove(remove);
            _context.SaveChanges();


            //change users role
            return Json(new { success = true, responseText = "Added role request to table" });
        }



        /// <summary>
        /// Stores to the db the requested rol
        /// </summary>
        /// <param name="username"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = "Instructor, Chair")]
        public async Task<JsonResult> Request_role(string username, string role)
        {
            var roles = _roleManager.Roles.Select(x => x.Name).ToList();
            var user = await _userManager.FindByNameAsync(username);

            var userexists = await _context.RequestedRoles
                                    .AsNoTracking()
                                    .FirstOrDefaultAsync(m => m.Username == username);

            if(userexists != null)
            {
                return Json(new { success = false, responseText = "Already requested change" });
            }
            else
            {
                _context.RequestedRoles.Add(new RequestedRole { Username = username, RoleRequested = role });
                _context.SaveChanges();
                return Json(new { success = true, responseText = "Added role request to table" });
            }

        }

        /// <summary>
        /// Returns the help or overview page
        /// </summary>
        /// <returns></returns>
        public IActionResult HelpPage()
        {
            return View();
        }

    
        /// <summary>
        /// Returns the course that was searched
        /// </summary>
        /// <param name="searchString"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin, Instructor, Chair")]
        public async Task<IActionResult> Search(string searchString)
        {
            var searchedCourse = await _context.Courses
                .Include(s => s.LearningOutcomes)
                .Include(n => n.Note)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.CourseName == searchString);

            var learningOutcomes = _context.LearningOutcomes.Where(s => s.CourseInstancesID == searchedCourse.CourseInstancesID).Include(s => s.DefenitionFile).Include(s => s.OutcomeFiles).ToList();
            ViewData["Outcomes"] = learningOutcomes;

            if (User.IsInRole("Instructor"))
            {
                var userName = User.FindFirstValue(ClaimTypes.Name);
                var inst = await _context.Instructors.Where(i => i.InstructorUserName == userName).FirstOrDefaultAsync();

                ViewData["Key"] = inst.apiKey != "";
            }

            if (searchedCourse == null)
            {
                return NotFound();
            }
            return View(searchedCourse);
        }


        /*
         * Changes the role based on user
         * Can only have one user and have to have at least one admin
         */
        [Authorize(Roles = "Admin")]
        public  async Task<JsonResult> ChangeRole(bool check, string username,string role)
        {
            var user = await _userManager.FindByNameAsync(username);
            var roles = _roleManager.Roles.Select(x => x.Name).ToList();
            var users = _userManager.Users;
            //remove the users role
            if (!check)
            {
                if(role == "Admin")
                {
                    int count = 0;
                    foreach (var userName in users)
                    {
                        if (await _userManager.IsInRoleAsync(userName, "admin"))
                        {
                            count++;
                        }

                    }
                    if (count <= 1)
                    {
                        return Json(new { success = false, responseText = "An Admin MUST ALWAYS EXIST!!!" });
                    }
                }
                //check if only one admin exists
                await _userManager.RemoveFromRoleAsync(user, role);
            }
            //add new role to user
            else
            {
                foreach(var roleName in roles)
                {
                    if(await _userManager.IsInRoleAsync(user,roleName))
                    {
                        return Json(new { success = false, responseText = "YOU CAN ONLY HAVE ONE ROLE" });
                    }
                }
                await _userManager.AddToRoleAsync(user, role);
            }

            return Json(new { success = true, username = username, role = role});
        }

        /// <summary>
        /// Returns the requested roles to the index home page
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Index()
        {
            var roles_req = await _context.RequestedRoles
                                .AsNoTracking()
                                .ToListAsync();

            return View(roles_req);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        // Return the role page that displays all the roles of the users
        [Authorize(Roles = "Admin")]
        public IActionResult RolesPage()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
