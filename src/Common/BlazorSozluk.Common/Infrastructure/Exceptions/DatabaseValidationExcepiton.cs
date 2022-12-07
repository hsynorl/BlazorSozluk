using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BlazorSozluk.Common.Infrastructure.Exceptions
{
    public class DatabaseValidationExcepiton : Exception
    {
        public DatabaseValidationExcepiton()
        {
        }

        public DatabaseValidationExcepiton(string? message) : base(message)
        {
        }

        public DatabaseValidationExcepiton(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected DatabaseValidationExcepiton(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
