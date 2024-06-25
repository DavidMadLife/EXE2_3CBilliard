using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXE201_3CBilliard_Model.Models.Response
{
    public class ListStatisticsResponse<T>
    {
        public List<T> Statistics { get; set; }
    }
}
