using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain.Exceptions
{
  internal class InvalidProductException : Exception
  {
    public InvalidProductException(string message) :base(message){}
  }
}
