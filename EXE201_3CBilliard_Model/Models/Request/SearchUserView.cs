using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXE201_3CBilliard_Model.Models.Request
{
    public class SearchUserView
    {
        public string? Keyword { get; set; }
        public int? Page_number { get; set; }
        public int? Page_size { get; set; }
    }
}
