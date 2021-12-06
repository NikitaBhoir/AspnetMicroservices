using System;

namespace Ordering.Application.Exceptions
{
    public class NotFoundException :ApplicationException //comes from system lib.
    {
        public NotFoundException(string name, object key) : base($"Entity \"{name}\" {key} was not found...")
        {

        }
    }
}
