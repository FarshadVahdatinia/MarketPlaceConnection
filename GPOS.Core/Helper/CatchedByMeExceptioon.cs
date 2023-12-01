using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPOS.Core.Helper
{
    public class CatchedByMeException : Exception
    {
        public CatchedByMeException()
        {
        }

        public CatchedByMeException(string message)
        : base(message)
        {
        }

        public CatchedByMeException(string message, Exception inner)
        : base(message, inner)
        {
        }
    }
}
