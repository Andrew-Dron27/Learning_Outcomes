/*
 * Caleb Edwards
 * u0829971
 * Model for LO's
 */

using System;
/*
 * Caleb Edwards
 * Andrew Dron
 * u0829971
 * u1027713
 * Model for the Course table
 */

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Learning_Outcomes.Models
{
    public class LearningOutcomeInstances
    {
        [Key]
        public int LearningOutcomeInstancesID { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        public int CourseInstancesID { get; set; }
        public CourseInstances CourseInstances { get; set; }
        public LearningOutcomeNoteInstances Note { get; set; }
        public LearningOutcomeFile DefenitionFile { get; set; }
        public ICollection<LearningOutcomeFile> OutcomeFiles { get; set; }
        
    }
}
