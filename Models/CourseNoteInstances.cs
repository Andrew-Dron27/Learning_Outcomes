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
    public class CourseNoteInstances
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CourseNoteInstancesID { get; set; }
        public string CourseNote { get; set; }
        [Required]
        public int CourseInstancesID { get; set; }
        public CourseInstances Course { get; set; }
        public string DateTime { get; set; }
        public bool Approved { get; set; }
    }
}
