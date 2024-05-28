using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXE201_3CBilliard_Model.Models.Response
{
    public class BillResponse
    {
        public long Id { get; set; }
        public string User { get; set; }
        public double Price { get; set; }
        public DateTime CreateAt { get; set; }
        public string OrderCode { get; set; }
        public string Descrpition { get; set; }
        public string Status { get; set; }
    }
}
