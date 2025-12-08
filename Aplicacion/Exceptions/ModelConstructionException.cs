using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.Exceptions
{
    public class ModelConstructionException : Exception
    {
        public ModelConstructionException() { }

        public ModelConstructionException(string message) : base(message) { }
    }
}
