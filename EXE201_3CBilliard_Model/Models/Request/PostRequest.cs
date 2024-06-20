using System;
using System.ComponentModel.DataAnnotations;

namespace EXE201_3CBilliard_Model.Models.Request
{
    public class PostRequest
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public long UserId { get; set; }


    }
}
