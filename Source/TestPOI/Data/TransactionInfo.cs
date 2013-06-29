using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestPOI.Data
{
    public class TransactionInfo
    {
        public DateTime TransactionTimestamp { get; set; }

        public string CustomerCode { get; set; }

        public string CustomerName { get; set; }

        public string Description { get; set; }

        public string ProductCode { get; set; }

        public string ProductName { get; set; }

        public double Quantity { get; set; }

        public double ContractPrice { get; set; }
        
        public double SellPrice { get; set; }

        public string SellPerson { get; set; }

        public int Year { get; set; }

        public int Month { get; set; }

        public int Quarter { get; set; }

        public string ProductCategory { get; set; }

        public string ProductType { get; set; }

        public string Producer { get; set; }
    }
}
