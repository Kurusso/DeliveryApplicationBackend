using DeliveryAgreagatorApplication.Common.Models.Enums;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.OpenApi.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryAgreagatorApplication.Common.Exceptions
{
    public class WrongIdException : Exception
    {
        public readonly int StatusCode = 404;
        public WrongIdException(WrongIdExceptionSubject wrongIdExceptionSubject, Guid Id, string additionalInfo="") : base($"There is no {wrongIdExceptionSubject} with this {Id} id {additionalInfo}!") { }
    }
}
