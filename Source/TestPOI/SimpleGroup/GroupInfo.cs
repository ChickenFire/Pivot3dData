using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using TestPOI.Definition;
using log4net;
using TestPOI.Data;

namespace TestPOI.SimpleGroup
{
    public class GroupInfo
    {
        /// <summary>
        /// Logger
        /// </summary>
        private static readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public string RootKey { get; set; }

        public string GroupKey { get; set; }

        public string Key { get; set; }

        public GroupingDefinition GroupingDefinition { get; set; }

        public List<TransactionInfo> ListData { get; set; }

        public Dictionary<string, GroupInfo> InnerGroupData { get; set; }

        public CombineCalculateData CalculateValue { get; set; }

        public List<CalculateDefinition> ListCalculateDefinition { get; set; }

        public GroupInfo()
        {
            //this.CalculateValue = new Dictionary<CalculateDefinition, double>();
            this.ListCalculateDefinition = new List<CalculateDefinition>();
        }

        public void RefreshCalculateValue()
        {
            CalculateValue = new CombineCalculateData()
            {
                ListData = this.ListData,
                ListCalculateDefinition = ListCalculateDefinition,
                Key = this.Key
            };
            CalculateValue.Calculate();
            _logger.DebugFormat("RefreshCalculateValue: Key:{0}", this.Key);
        }

        public static Dictionary<CalculateDefinition, double> GetValue(List<CalculateDefinition> definitions,
            List<TransactionInfo> listData)
        {
            var mappingTotal = new Dictionary<CalculateDefinition, double>();

            foreach (var definition in definitions)
            {
                mappingTotal.Add(definition, 0.0);
            }

            foreach (var transactionInfo in listData)
            {
                foreach (var definition in definitions)
                {
                    object tempData = definition.CalculateMethod.GetValue(transactionInfo, null);
                    if (tempData != null)
                    {
                        mappingTotal[definition] += Convert.ToDouble(tempData);
                    }
                }
            }

            foreach (var definition in definitions)
            {
                switch (definition.Type)
                {
                    case CalculateType.Average:
                        mappingTotal[definition] /= listData.Count;
                        break;
                }
            }

            return mappingTotal;
        }
    }
}
