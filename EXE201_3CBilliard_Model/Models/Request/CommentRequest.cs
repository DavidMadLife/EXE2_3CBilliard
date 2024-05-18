using System;
using System.ComponentModel.DataAnnotations;

namespace EXE201_3CBilliard_Model.Models.Request
{
    public class CommentRequest
    {
        [Required(ErrorMessage = "PostId is required")]
        public long PostId { get; set; }

        [Required(ErrorMessage = "UserId is required")]
        public long UserId { get; set; }

        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Image is required")]
        /*[Url(ErrorMessage = "Image must be a valid URL")]*/
        public string Image { get; set; }

        [Required(ErrorMessage = "Like is required")]
        [Range(0, 5, ErrorMessage = "Like must be a non-negative number")]
        public long Like { get; set; }

        [Required(ErrorMessage = "Evaluation is required")]
        [Range(0, 5, ErrorMessage = "Evaluation must be a non-negative number")]
        public long Evaluation { get; set; }

        [Required(ErrorMessage = "CreateAt is required")]
        public DateTime CreateAt { get; set; }

        
    }
}
