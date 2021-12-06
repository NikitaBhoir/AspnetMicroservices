using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Exceptions
{
    class ValidationException :ApplicationException
    {
        public ValidationException() :base("one or more validation failure occured.")
        {
            Errors = new Dictionary<string, string[]>();
        }

        public ValidationException(IEnumerable<ValidationFailure> failures) :this() //if any ule fail from validation class then this will un
        {
            Errors = failures
                .GroupBy(e => e.PropertyName, e => e.ErrorMessage)
                .ToDictionary(failureGroup => failureGroup.Key, failureGroup => failureGroup.ToArray());
        }
        public Dictionary<string, string[]> Errors { get; private set; } //to conains list of errors,key is sting and value is the array list of errors
    }
}
