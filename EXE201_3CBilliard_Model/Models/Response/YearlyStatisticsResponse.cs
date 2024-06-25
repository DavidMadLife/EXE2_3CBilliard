using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXE201_3CBilliard_Model.Models.Response
{
    public class YearlyStatisticsResponse
    {
        public int Year { get; set; }
        public int UserRegistrations { get; set; }
        public int BidaClubRegistrations { get; set; }
        public int Transactions { get; set; }
        public double TotalRevenue { get; set; }
    }
}
