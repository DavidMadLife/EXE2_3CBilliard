using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXE201_3CBilliard_Model.Models.Response
{
    public class ValidateOtpResponse
    {
        public string Message { get; set; }
        public bool IsValid { get; set; }
    }
}
