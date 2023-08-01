/*
 * Caleb Edwards
 * u0829971
 * Creates the tables for courses, LO's, and Instructors
 */

using Learning_Outcomes.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Learning_Outcomes.Data
{
    public class Learning_OutcomesContext : DbContext
    {
        public Learning_OutcomesContext(DbContextOptions<Learning_OutcomesContext> options) : base(options)
        {
        }
        public DbSet<CourseInstances> Courses { get; set; }
        public DbSet<InstructorInstances> Instructors { get; set; }
        public DbSet<LearningOutcomeInstances> LearningOutcomes { get; set; }
        public DbSet<CourseNoteInstances> CourseNotes { get; set; }
        public DbSet<LearningOutcomeNoteInstances> LearningOutcomeNotes { get; set; }
        public DbSet<LearningOutcomeFile> learningOutcomeFiles { get; set; }

        public DbSet<RequestedRole> RequestedRoles { get; set; }

        protected override void OnModelCreating(ModelBuilder bldr)
        {
            bldr.Entity<CourseInstances>().ToTable("CourseInstances");
            bldr.Entity<LearningOutcomeInstances>().ToTable("LearningOutcomeInstances");
            bldr.Entity<InstructorInstances>().ToTable("InstructorInstances");
            bldr.Entity<CourseNoteInstances>().ToTable("CourseNoteInstances");
            bldr.Entity<LearningOutcomeNoteInstances>().ToTable("LearningOutcomeNoteInstances");
            bldr.Entity<RequestedRole>().ToTable("RequestedRole");
        }
    }   
}
