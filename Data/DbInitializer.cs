/*
 * Caleb Edwards
 * Andrew Dron
 * u0829971
 * u1027713
 * This class is used to seed the database, create roles and users and courses are filled up with init data
 */

using Learning_Outcomes.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Learning_Outcomes.Data
{
    public class DbInitializer
    {
        // Initialize the Courses with LO's
        //public static void Initialize(Learning_OutcomesContext CourseContext)
        public static async Task Initialize(IServiceProvider serviceProvider, IdentityContext Identitycontext, Learning_OutcomesContext CourseContext)
        {
            CourseContext.Database.EnsureDeleted();
            CourseContext.Database.EnsureCreated();
            Identitycontext.Database.Migrate();

            var courses = new CourseInstances[]
            {
                new CourseInstances{CourseInstancesID=1010, InstructorInstancesID=1, CourseName="CS4540",Dept="CS",Semester="Fall",Description="Web Architecture",Year=2019},
                new CourseInstances{CourseInstancesID=1011, InstructorInstancesID=1, CourseName="CS2420",Dept="CS",Semester="Fall",Description="Algorithms",Year=2019},
                new CourseInstances{CourseInstancesID=1012, InstructorInstancesID=1, CourseName="CS3500",Dept="CS",Semester="Fall",Description="Software Practice",Year=2019},
                new CourseInstances{CourseInstancesID=1014, InstructorInstancesID=2, CourseName="CS4400",Dept="CS",Semester="Fall",Description="Computer Systems",Year=2019},
                new CourseInstances{CourseInstancesID=1015, InstructorInstancesID=2, CourseName="CS2100",Dept="CS",Semester="Fall",Description="Discrete Structures",Year=2019},
                new CourseInstances{CourseInstancesID=1016, InstructorInstancesID=3, CourseName="CS3500",Dept="CS",Semester="Fall",Description="Software Practice",Year=2019}
            };
            foreach (CourseInstances s in courses)
            {
                CourseContext.Courses.Add(s);
            }

            var notes = new CourseNoteInstances[]
            {
                new CourseNoteInstances{CourseNote="CS4540 Note", CourseInstancesID=1010},
                new CourseNoteInstances{CourseNote="CS2420 Note", CourseInstancesID=1011},
                new CourseNoteInstances{CourseNote="CS3500 Note Jim", CourseInstancesID=1012},
                new CourseNoteInstances{CourseNote="CS4400 Note", CourseInstancesID=1014},
                new CourseNoteInstances{CourseNote="CS2100 Note", CourseInstancesID=1015},
                new CourseNoteInstances{CourseNote="CS3500 Note Danny", CourseInstancesID=1016},
            };
            foreach(CourseNoteInstances s in notes)
            {
                s.DateTime = DateTime.Now.ToString("MM/dd/yyyy");
                s.Approved = false;
                CourseContext.CourseNotes.Add(s);
            }

            var lo = new LearningOutcomeInstances[]
            {   
                //CS 2100
                new LearningOutcomeInstances{Name="LO1",Description="use symbolic logic to model real-world situations by converting informal language statements to propositional and predicate logic expressions, as well as apply formal methods to propositions and predicates (such as computing normal forms and calculating validity)", CourseInstancesID=1015},
                new LearningOutcomeInstances{Name="LO2",Description="analyze problems to determine underlying recurrence relations, as well as solve such relations by rephrasing as closed formulas", CourseInstancesID=1015},
                new LearningOutcomeInstances{Name="LO3",Description="assign practical examples to the appropriate set, function, or relation model, while employing the associated terminology and operations", CourseInstancesID=1015},
                new LearningOutcomeInstances{Name="LO4",Description="map real-world applications to appropriate counting formalisms, including permutations and combinations of sets, as well as exercise the rules of combinatorics (such as sums, products, and inclusion-exclusion)", CourseInstancesID=1015},
                new LearningOutcomeInstances{Name="LO5",Description="calculate probabilities of independent and dependent events, in addition to expectations of random variables", CourseInstancesID=1015},
                new LearningOutcomeInstances{Name="LO6",Description="illustrate by example the basic terminology of graph theory, as wells as properties and special cases (such as Eulerian graphs, spanning trees, isomorphism, and planarity)", CourseInstancesID=1015},
                new LearningOutcomeInstances{Name="LO7",Description="employ formal proof techniques (such as direct proof, proof by contradiction, induction, and the pigeonhole principle) to construct sound arguments about properties of numbers, sets, functions, relations, and graphs", CourseInstancesID=1015},
                //CS 2420
                new LearningOutcomeInstances{Name="LO1",Description="implement, and analyze for efficiency, fundamental data structures (including lists, graphs, and trees) and algorithms (including searching, sorting, and hashing)", CourseInstancesID=1011},
                new LearningOutcomeInstances{Name="LO2",Description="employ Big-O notation to describe and compare the asymptotic complexity of algorithms, as well as perform empirical studies to validate hypotheses about running time)", CourseInstancesID=1011},
                new LearningOutcomeInstances{Name="LO3",Description="recognize and describe common applications of abstract data types (including stacks, queues, priority queues, sets, and maps)", CourseInstancesID=1011},
                new LearningOutcomeInstances{Name="L04",Description="apply algorithmic solutions to real-world data", CourseInstancesID=1011},
                new LearningOutcomeInstances{Name="L05",Description="use generics to abstract over functions that differ only in their types", CourseInstancesID=1011},
                new LearningOutcomeInstances{Name="L06",Description="appreciate the collaborative nature of computer science by discussing the benefits of pair programming", CourseInstancesID=1011},
                // CS 3500 Jim and Danny
                new LearningOutcomeInstances{Name="L01",Description="design and implement large and complex software systems (including concurrent software) through the use of process models (such as waterfall and agile), libraries (both standard and custom), and modern software development tools (such as debuggers, profilers, and revision control systems)", CourseInstancesID=1012},
                new LearningOutcomeInstances{Name="L01",Description="design and implement large and complex software systems (including concurrent software) through the use of process models (such as waterfall and agile), libraries (both standard and custom), and modern software development tools (such as debuggers, profilers, and revision control systems)", CourseInstancesID=1016},
                new LearningOutcomeInstances{Name="L02",Description="perform input validation and error handling, as well as employ advanced testing principles and tools to systematically evaluate software", CourseInstancesID=1012},
                new LearningOutcomeInstances{Name="L02",Description="perform input validation and error handling, as well as employ advanced testing principles and tools to systematically evaluate software", CourseInstancesID=1016},
                new LearningOutcomeInstances{Name="L03",Description="apply the model-view-controller pattern and event handling fundamentals to create a graphical user interface", CourseInstancesID=1012},
                new LearningOutcomeInstances{Name="L03",Description="apply the model-view-controller pattern and event handling fundamentals to create a graphical user interface", CourseInstancesID=1016},
                new LearningOutcomeInstances{Name="L04",Description="exercise the client-server model and high-level networking APIs to build a web-based software system", CourseInstancesID=1012},
                new LearningOutcomeInstances{Name="L04",Description="exercise the client-server model and high-level networking APIs to build a web-based software system", CourseInstancesID=1016},
                new LearningOutcomeInstances{Name="L05",Description="operate a modern relational database to define relations, as well as store and retrieve data", CourseInstancesID=1012},
                new LearningOutcomeInstances{Name="L05",Description="operate a modern relational database to define relations, as well as store and retrieve data", CourseInstancesID=1016},
                new LearningOutcomeInstances{Name="L06",Description="appreciate the collaborative nature of software development by discussing the benefits of peer code reviews", CourseInstancesID=1012},
                new LearningOutcomeInstances{Name="L06",Description="appreciate the collaborative nature of software development by discussing the benefits of peer code reviews", CourseInstancesID=1016},
                //CS 4400
                new LearningOutcomeInstances{Name="L01",Description="explain the objectives and functions of abstraction layers in modern computing systems, including operating systems, programming languages, compilers, and applications", CourseInstancesID=1014},
                new LearningOutcomeInstances{Name="L02",Description="understand cross-layer communications and how each layer of abstraction is implemented in the next layer of abstraction (such as how C programs are translated into assembly code and how C library allocators are implemented in terms of operating system memory management)", CourseInstancesID=1014},
                new LearningOutcomeInstances{Name="L03",Description="analyze how the performance characteristics of one layer of abstraction affect the layers above it (such as how caching and services of the operating system affect the performance of C programs)", CourseInstancesID=1014},
                new LearningOutcomeInstances{Name="L04",Description="construct applications using operating-system concepts (such as processes, threads, signals, virtual memory, I/O)", CourseInstancesID=1014},
                new LearningOutcomeInstances{Name="L05",Description="synthesize operating-system and networking facilities to build concurrent, communicating applications", CourseInstancesID=1014},
                new LearningOutcomeInstances{Name="L06",Description="implement reliable concurrent and parallel programs using appropriate synchronization constructs", CourseInstancesID=1014},
                //CS 4540
                new LearningOutcomeInstances{Name="L01",Description="Construct web pages using modern HTML and CSS practices, including modern frameworks.", CourseInstancesID=1010},
                new LearningOutcomeInstances{Name="L02",Description="Define accessibility and utilize techniques to create accessible web pages.", CourseInstancesID=1010},
                new LearningOutcomeInstances{Name="L03",Description="Outline and utilize MVC technologies across the “full-stack” of web design (including front-end, back-end, and databases) to create interesting web applications. Deploy an application to a “Cloud” provider.", CourseInstancesID=1010},
                new LearningOutcomeInstances{Name="L04",Description="Describe the browser security model and HTTP; utilize techniques for data validation, secure session communication, cookies, single sign-on, and separate roles.  ", CourseInstancesID=1010},
                new LearningOutcomeInstances{Name="L05",Description="Utilize JavaScript for simple page manipulations and AJAX for more complex/complete “single-page” applications.", CourseInstancesID=1010},
                new LearningOutcomeInstances{Name="L06",Description="Demonstrate how responsive techniques can be used to construct applications that are usable at a variety of page sizes.  Define and discuss responsive, reactive, and adaptive.", CourseInstancesID=1010},
                new LearningOutcomeInstances{Name="L07",Description="Construct a simple web-crawler for validation of page functionality and data scraping.", CourseInstancesID=1010},
            };

            foreach (LearningOutcomeInstances c in lo)
            {
                var LONote = new LearningOutcomeNoteInstances
                {
                    CourseInstancesID = c.CourseInstancesID,
                    LearningOutcome = c,
                    LearningOutcomeInstancesID = c.LearningOutcomeInstancesID
                ,
                    LONote = "dd",
                    DateTime = DateTime.Now.ToString("MM/dd/yyyy"),
                    ChairEdited = false
                };
                CourseContext.LearningOutcomeNotes.Add(LONote);
                CourseContext.LearningOutcomes.Add(c);
            }

            var instr = new InstructorInstances[]
            {
                new InstructorInstances{InstructorInstancesID=1, InstructorUserName="professor_jim@cs.utah.edu", apiKey=""},
                new InstructorInstances{InstructorInstancesID=2, InstructorUserName="professor_mary@cs.utah.edu",apiKey=""},
                new InstructorInstances{InstructorInstancesID=3, InstructorUserName="professor_danny@cs.utah.edu",apiKey=""},
            };
            foreach (InstructorInstances t in instr)
            {
                CourseContext.Instructors.Add(t);
            }
            CourseContext.SaveChanges();


            //https://gooroo.io/GoorooTHINK/Article/17333/Custom-user-roles-and-rolebased-authorization-in-ASPNET-core/28380#.WbgShcgjGUl
            // Start seeding the users
            var UserManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            string[] roleNames = { "Admin", "Instructor", "Chair" };
            IdentityResult roleResult;

            foreach (var roleName in roleNames)
            {
                //creating the roles and seeding them to the database
                var roleExist = await RoleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    roleResult = await RoleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            string UserPassword = "123ABC!@#def";

            //creating a super user who could maintain the web app
            var poweruser = new IdentityUser
            {
                UserName = "admin_erin@cs.utah.edu",
                Email = "admin_erin@cs.utah.edu"
            };
            poweruser.EmailConfirmed = true;
            var _user = await UserManager.FindByEmailAsync("admin_erin@cs.utah.edu");

            if (_user == null)
            {
                var createPowerUser = await UserManager.CreateAsync(poweruser, UserPassword);
                if (createPowerUser.Succeeded)
                {
                    //here we tie the new user to the "Admin" role 
                    await UserManager.AddToRoleAsync(poweruser, roleNames[0]);
                }
            }


            var chair = new IdentityUser
            {
                UserName = "chair_whitaker@cs.utah.edu",
                Email = "chair_whitaker@cs.utah.edu"
            };
            chair.EmailConfirmed = true;
            var _user1 = await UserManager.FindByEmailAsync("chair_whitaker@cs.utah.edu");

            if (_user1 == null)
            {
                var createChairUser = await UserManager.CreateAsync(chair, UserPassword);
                if (createChairUser.Succeeded)
                {
                    //here we tie the new user to the "Admin" role 
                    await UserManager.AddToRoleAsync(chair, roleNames[2]);

                }
            }


            var P1 = new IdentityUser
            {
                UserName = "professor_jim@cs.utah.edu",
                Email = "professor_jim@cs.utah.edu"
            };
            P1.EmailConfirmed = true;

            var _user2 = await UserManager.FindByEmailAsync("professor_jim@cs.utah.edu");

            if (_user2 == null)
            {
                var createP1User = await UserManager.CreateAsync(P1, UserPassword);
                if (createP1User.Succeeded)
                {
                    //here we tie the new user to the "Admin" role 
                    await UserManager.AddToRoleAsync(P1, roleNames[1]);

                }
            }


            var P2 = new IdentityUser
            {
                UserName = "professor_mary@cs.utah.edu",
                Email = "professor_mary@cs.utah.edu"
            };
            P2.EmailConfirmed = true;
            var _user3 = await UserManager.FindByEmailAsync("professor_mary@cs.utah.edu");

            if (_user3 == null)
            {
                var createP2User = await UserManager.CreateAsync(P2, UserPassword);
                if (createP2User.Succeeded)
                {
                    //here we tie the new user to the "Admin" role 
                    await UserManager.AddToRoleAsync(P2, roleNames[1]);

                }
            }


            var P3 = new IdentityUser
            {
                UserName = "professor_danny@cs.utah.edu",
                Email = "professor_danny@cs.utah.edu"
            };
            P3.EmailConfirmed = true;
            var _user4 = await UserManager.FindByEmailAsync("professor_danny@cs.utah.edu");

            if (_user4 == null)
            {
                var createP3User = await UserManager.CreateAsync(P3, UserPassword);
                if (createP3User.Succeeded)
                {
                    //here we tie the new user to the "Admin" role 
                    await UserManager.AddToRoleAsync(P3, roleNames[1]);
                }
            }

            Identitycontext.SaveChanges();
        }
    }
}
