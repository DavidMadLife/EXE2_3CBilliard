using System;
using System.ComponentModel.DataAnnotations;

namespace EXE201_3CBilliard_Model.Models.Request
{
    public class BidaTableRequest
    {
        [Required(ErrorMessage = "BidaCludId is required")]
        public long BidaCludId { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
        public double Price { get; set; }

        [Required(ErrorMessage = "TableName is required")]
        public string TableName { get; set; }

        [Required(ErrorMessage = "Image is required")]
        /*[Url(ErrorMessage = "Image must be a valid URL")]*/
        public string Image { get; set; }

        [Required(ErrorMessage = "CreateAt is required")]
        public DateTime CreateAt { get; set; }
        public string Note { get; set; }
    }
}
