using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeBridgeTestTask.Application.Exceptions
{
    public class InvalidJsonException : Exception
    {
        public InvalidJsonException(string message) : base(message) { }
    }
}
