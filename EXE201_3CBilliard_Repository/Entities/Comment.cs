using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXE201_3CBilliard_Repository.Entities
{
    public enum CommentStatus
    {
        ACTIVE,
        INACTIVE,
        WAITING,
        DELETED
    }

    [Table("Comment")]
    public class Comment
    {
        [Key]
        public long Id { get; set; }
        [Required]
        public long PostId { get; set; }
        [Required]
        public long UserId { get; set; }
        [Required]
        public string Descrpition { get; set; }
        [Required]
        public string Image { get; set; }
        [Required]
        public long Like { get; set; }
        [Required]
        public long Evalution { get; set; }
        [Required]
        public DateTime CreateAt { get; set; }
        [Required]
        public string Note { get; set; }
        [Required]
        public CommentStatus Status { get; set; }
        [ForeignKey("PostId")]
        public Post Post { get; set; }
        /*[ForeignKey("UserId")]
        public User User { get; set; }*/

    }
}
