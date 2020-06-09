using System;
using System.Collections.Generic;
using System.Text;

namespace FluentCommander.BulkCopy
{
    public class BulkCopyOptionsBuilder
    {
        //internal 

        public BulkCopyOptionsBuilder And { get; set; }
        public BulkCopyOptionsBuilder Not { get; set; }

        public BulkCopyOptionsBuilder()
        {

        }
    }
}
