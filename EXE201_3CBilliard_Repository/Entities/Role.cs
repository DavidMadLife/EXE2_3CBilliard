using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXE201_3CBilliard_Repository.Entities
{
    [Table("Role")]
    public class Role
    {
        [Key]
        public long Id { get; set; }
        [Required]
        public string RoleName { get; set; }
        [Required]
        public string Status { get; set; }
    }
}
