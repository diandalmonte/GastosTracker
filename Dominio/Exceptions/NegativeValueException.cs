using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Exceptions
{
    public class NegativeValueException : Exception
    {
        public NegativeValueException() { }

        public NegativeValueException(string message) : base(message) { }
    }
}
