﻿using Microsoft.AspNetCore.Http;
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

        
        /*[Url(ErrorMessage = "Image must be a valid URL")]*/
        public IFormFile Image { get; set; }

        public string Note { get; set; }
    }
}
