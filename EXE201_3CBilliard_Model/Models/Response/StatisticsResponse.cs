using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXE201_3CBilliard_Model.Models.Response
{
    public class StatisticsResponse
    {
        public int TotalActiveClubs { get; set; }
        public int TotalActiveUsers { get; set; }
        public double TotalRevenue { get; set; }
        public int TotalActiveTransactions { get; set; }
    }
}
