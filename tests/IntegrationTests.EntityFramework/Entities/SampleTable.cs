using System;
using System.Collections.Generic;

namespace IntegrationTests.EntityFramework.Entities
{
    public partial class SampleTable
    {
        public long SampleTableId { get; set; }
        public int SampleInt { get; set; }
        public short SampleSmallInt { get; set; }
        public byte SampleTinyInt { get; set; }
        public bool SampleBit { get; set; }
        public decimal SampleDecimal { get; set; }
        public double SampleFloat { get; set; }
        public DateTime SampleDateTime { get; set; }
        public Guid SampleUniqueIdentifier { get; set; }
        public string SampleVarChar { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
