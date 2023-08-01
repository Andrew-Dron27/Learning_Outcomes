/*
 * Caleb Edwards
 * Andrew Dron
 * u0829971
 * u1027713
 * Model for the Course table
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Learning_Outcomes.Models
{
    public class LearningOutcomeNoteInstances
    {
        public int LearningOutcomeNoteInstancesID { get; set; }
        public string LONote { get; set; }
        public int LearningOutcomeInstancesID { get; set; }
        public int CourseInstancesID { get; set; }
        public LearningOutcomeInstances LearningOutcome { get; set; }
        public string DateTime { get; set; }
        public bool ChairEdited { get; set; }
    }
}
