using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.Exceptions
{
    public class OverBudgetException : Exception
    {
        public OverBudgetException() { }

        public OverBudgetException(string message) : base(message) { }
    }
}
