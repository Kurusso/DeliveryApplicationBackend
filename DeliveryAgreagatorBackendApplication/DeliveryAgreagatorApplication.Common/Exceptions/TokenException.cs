using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryAgreagatorApplication.Common.Exceptions
{
    public class TokenException : Exception
    {
        public readonly int StatusCode = 401;
        public TokenException() : base("Token is not actual or correct!") {}
    }
}
