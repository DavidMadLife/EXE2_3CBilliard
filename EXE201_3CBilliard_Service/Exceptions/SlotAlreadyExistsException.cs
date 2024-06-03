using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXE201_3CBilliard_Service.Exceptions
{
    public class SlotAlreadyExistsException : Exception
    {
        public SlotAlreadyExistsException()
        {
        }

        public SlotAlreadyExistsException(string message)
            : base(message)
        {
        }

        public SlotAlreadyExistsException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
