/*
 * Caleb Edwards
 * Andrew Dron
 * u0829971
 * u1027713
 * Model for the Course table
 */

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Learning_Outcomes.Models
{
    public class CourseInstances
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CourseInstancesID { get; set; }
        [Required]
        public string CourseName { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string Dept { get; set; }
        [Required]
        public string Semester { get; set; }
        public int Year { get; set; }
        public int InstructorInstancesID { get; set; }
        public int Completion { get; set; }
        public InstructorInstances InstructorInstances { get; set; }
        public CourseNoteInstances Note { get; set; }
        public ICollection<LearningOutcomeInstances> LearningOutcomes { get; set; }
    }
}
