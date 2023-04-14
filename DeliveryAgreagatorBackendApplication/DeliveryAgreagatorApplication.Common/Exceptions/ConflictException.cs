using DeliveryAgreagatorApplication.Common.Models.Enums;
using Microsoft.OpenApi.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryAgreagatorApplication.Common.Exceptions
{
    public class ConflictException : Exception
    {
        public readonly int StatusCode = 409;
        public ConflictException(ConflictExceptionSubjects conflictSubject, string value) : base($"There is already User with this {value} {conflictSubject}") { }
    }
}
