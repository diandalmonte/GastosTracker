using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.Interfaces.AplicacionServices
{
    public interface IAuthService
    {
        public Task<string> Login(string email, string password);
    }
}
