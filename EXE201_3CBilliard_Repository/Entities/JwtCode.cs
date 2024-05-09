using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXE201_3CBilliard_Repository.Entities
{
    [Table("JwtCode")]
    public class JwtCode
    {
        [Key]
        public long Id { get; set; }
        [Required]
        public long UserId { get; set; }
        [Required]
        public string Code { get; set; }
        [Required]
        public string Status { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }
    }
}
