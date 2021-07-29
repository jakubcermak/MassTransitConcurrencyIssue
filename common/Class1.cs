using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace common
{
    using System.Threading;

    public class FoxMessage
    {
        static int counter = 0;

        public int Id { get; set; }

        public FoxMessage()
        {
            Id = Interlocked.Increment(ref counter);
        }
    }
}
