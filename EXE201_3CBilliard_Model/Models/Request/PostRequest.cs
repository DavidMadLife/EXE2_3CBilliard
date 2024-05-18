using System;
using System.ComponentModel.DataAnnotations;

namespace EXE201_3CBilliard_Model.Models.Request
{
    public class PostRequest
    {
        [Required(ErrorMessage = "UserId is required")]
        public long UserId { get; set; }

        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Image is required")]
        /*[Url(ErrorMessage = "Image must be a valid URL")]*/
        public string Image { get; set; }

        [Required(ErrorMessage = "CreateAt is required")]
        public DateTime CreateAt { get; set; }

        [Required(ErrorMessage = "ModifineAt is required")]
        public DateTime ModifyAt { get; set; }

        
    }
}
