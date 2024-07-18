using EXE201_3CBilliard_Repository.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace EXE201_3CBilliard_Model.Models.Request
{
    public class NotificateRequest
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string Descrpition { get; set; }

        [Required]
        public long UserId { get; set; }

        public string BillOrderCode { get; set; }

        public string BillStatus { get; set; }

        [Required]
        public NotificationType Type { get; set; }
    }
}
