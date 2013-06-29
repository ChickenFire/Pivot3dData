using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestPOI.Definition;
using TestPOI.Data;

namespace TestPOI.SimpleGroup
{
    public class CombineCalculateData
    {
        public List<TransactionInfo> ListData { get; set; }

        public List<CalculateDefinition> ListCalculateDefinition { get; set; }

        public Dictionary<CalculateDefinition, double> Values { get; private set; }

        public string Key { get; set; }

        public CombineCalculateData()
        {
            this.ListData = new List<TransactionInfo>();
            this.ListCalculateDefinition = new List<CalculateDefinition>();
        }

        public void Calculate()
        {
            Values = new Dictionary<CalculateDefinition, double>();

            foreach (var definition in this.ListCalculateDefinition)
            {
                Values.Add(definition, 0.0);
            }

            foreach (var transactionInfo in this.ListData)
            {
                foreach (var definition in this.ListCalculateDefinition)
                {
                    object tempData = definition.CalculateMethod.GetValue(transactionInfo, null);
                    if (tempData != null)
                    {
                        Values[definition] += Convert.ToDouble(tempData);
                    }
                }
            }

            foreach (var definition in this.ListCalculateDefinition)
            {
                switch (definition.Type)
                {
                    case CalculateType.Average:
                        Values[definition] /= this.ListData.Count;
                        break;
                }
            }
        }
    }
}
