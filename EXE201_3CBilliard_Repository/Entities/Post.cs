﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXE201_3CBilliard_Repository.Entities
{
    public enum PostStatus
    {
        ACTIVE,
        INACTIVE,
        WAITING,
        DELETED
    }
    [Table("Post")]
    public class Post
    {
        [Key]
        public long Id { get; set; }
        [Required]
        public long UserId { get; set; }
        [Required]
        public string Descrpition { get; set; }
        [Required]
        public string Image { get; set; }
        [Required]
        public DateTime CreateAt { get; set; }
        [Required]
        public DateTime ModifineAt { get; set; }
        public PostStatus Status { get; set; }
        public string Note { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }
    }
}
