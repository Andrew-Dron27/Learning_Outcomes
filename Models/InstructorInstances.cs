/*
 * Caleb Edwards
 * u0829971
 * Model for Instructor
 */

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Learning_Outcomes.Models
{
    public class InstructorInstances
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int InstructorInstancesID { get; set; }
        [Required]
        public string InstructorUserName { get; set; }
        public string apiKey { get; set; }
        public ICollection<CourseInstances> Courses { get; set; }
    }
}
