using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Exeptions
{
    public abstract class BadRequestException : Exception
    {
        protected BadRequestException(String message) : base(message) 
        {
            
        }
    }
}
