using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Learning_Outcomes.Models
{
    public class RequestedRole
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int RequestedRoleID { get; set; }

        [Required]
        public string Username { get; set; }
        [Required]
        public string RoleRequested { get; set; }

    }
}
