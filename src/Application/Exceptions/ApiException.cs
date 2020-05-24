using System;
using System.Collections.Generic;

namespace Application.Exceptions
{
    public class ApiException : Exception
    {
        public Dictionary<string, string[]> ErrorList { get; }

        public ApiException(Dictionary<string, string[]> errorList) : base()
        {
            ErrorList = errorList;
        }
    }
}
