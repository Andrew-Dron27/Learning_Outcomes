using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Learning_Outcomes.Models
{
    public class LearningOutcomeFile
    {
        public int Id { get; set; }

        public byte[] Content { get; set; }

        public int LearningOutcomeInstancesID { get; set; }

        public int? Score { get; set; }

        public bool isDef { get; set; }

        [Display(Name = "File Name")]
        public string UntrustedName { get; set; }

        [Display(Name = "Size (bytes)")]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public long Size { get; set; }

        [Display(Name = "Uploaded (UTC)")]
        [DisplayFormat(DataFormatString = "{0:G}")]
        public DateTime UploadDT { get; set; }
    }
}
