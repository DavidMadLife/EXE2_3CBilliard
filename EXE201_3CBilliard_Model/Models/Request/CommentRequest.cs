using System;
using System.ComponentModel.DataAnnotations;

namespace EXE201_3CBilliard_Model.Models.Request
{
    public class CommentRequest
    {
        public string Content { get; set; }
        public long UserId { get; set; }
        public long PostId { get; set; }

    }
}
