using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using System.Reflection;
using TestPOI.Definition;
using TestPOI.Data;
using TestPOI.DataFilter;

namespace TestPOI.Pivot
{
    public class PivotTableController
    {
        private const string DefinitionKeyTemplate = "xKey:[{0}]-yKey:[{1}]";

        /// <summary>
        /// Logger
        /// </summary>
        private static readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public GroupingDefinition GroupingX { get; set; }

        public GroupingDefinition GroupingY { get; set; }

        public List<TransactionInfo> Data { get; set; }

        public List<CalculateDefinition> CalculateDefinitions { get; set; }

        public DataFilteringController DataFiltering { get; set; }

        private Dictionary<string, Combine2DDimensionGroupingDefinition> _dDimensionGroupingDefinitions = null;

        private Dictionary<string, CombineDataKey> _keyDictionary = null;

        private Dictionary<CombineDataKey, CombineDataResult> _dataDictionary = null;

        public void AnalyzeData()
        {
            _logger.DebugFormat("Start Analyze Data");

            _dDimensionGroupingDefinitions = new Dictionary<string, Combine2DDimensionGroupingDefinition>();
            _keyDictionary = new Dictionary<string, CombineDataKey>();
            _dataDictionary = new Dictionary<CombineDataKey, CombineDataResult>();

            LoopGroupDefinitionX(_dDimensionGroupingDefinitions, this.GroupingX, this.GroupingY);

            foreach (var dataItem in this.Data)
            {
                bool checkOk = true;
                if (DataFiltering != null)
                {
                    checkOk = DataFiltering.PerformCheck(dataItem);
                }

                if (checkOk)
                {
                    foreach (var combineGroupingDefinition in _dDimensionGroupingDefinitions.Values)
                    {
                        CombineDataKey combineDataKey = this.GetDataCombineGroupKey(combineGroupingDefinition, dataItem);
                        if (_keyDictionary.ContainsKey(combineDataKey.CombineKey) == false)
                        {
                            _keyDictionary.Add(combineDataKey.CombineKey, combineDataKey);
                        }
                        else
                        {
                            combineDataKey = _keyDictionary[combineDataKey.CombineKey];
                        }

                        CombineDataResult dataResult = null;
                        if (_dataDictionary.TryGetValue(combineDataKey, out dataResult) == false)
                        {
                            dataResult = new CombineDataResult(combineDataKey, this.CalculateDefinitions);
                            _dataDictionary.Add(combineDataKey, dataResult);
                        }
                        dataResult.AddData(dataItem);
                    }
                }
            }

            _logger.DebugFormat("Finish Analyze Data");
        }

        public void LoopGroupDefinitionX(Dictionary<string, Combine2DDimensionGroupingDefinition> dictionary,
            GroupingDefinition groupXGroupingDefinition,
            GroupingDefinition groupYGroupingDefinition)
        {
            if (groupXGroupingDefinition != null)
            {
                LoopGroupDefinitionX(dictionary, groupXGroupingDefinition.InnerDefinition, groupYGroupingDefinition);
            }

            LoopGroupDefinitionY(dictionary, groupXGroupingDefinition, groupYGroupingDefinition);
        }

        private CombineDataKey GetDataCombineGroupKey(Combine2DDimensionGroupingDefinition combineGroupingDefinition,
                                       TransactionInfo data)
        {
            CombineDataKey combineDataKey = new CombineDataKey()
            {
                XKey = GetDataGroupKey(combineGroupingDefinition.XDefinition, data),
                YKey = GetDataGroupKey(combineGroupingDefinition.YDefinition, data),
            };

            return combineDataKey;
        }

        private string GetDataGroupKey(GroupingDefinition groupingDefinition, TransactionInfo data)
        {
            if (groupingDefinition == null)
            {
                return string.Empty;
            }

            StringBuilder builder = new StringBuilder();
            builder.Append(groupingDefinition.GetKeyMethod.GetValue(data, null));
            if (groupingDefinition.UpperDefinition != null)
            {
                builder.Append(",");
                string upperKey = GetDataGroupKey(groupingDefinition.UpperDefinition, data);
                builder.Append(upperKey);
            }

            return builder.ToString();
        }

        private String GenerateGroupingConditionKey(GroupingDefinition groupingDefinition)
        {
            StringBuilder builder = new StringBuilder();
            if (groupingDefinition == null)
            {
                return string.Empty;
            }
            else if (groupingDefinition.UpperDefinition != null)
            {
                string upperKey = GenerateGroupingConditionKey(groupingDefinition.UpperDefinition);
                builder.Append(upperKey);
                builder.Append(",");

            }
            builder.Append(groupingDefinition.KeyName);

            return builder.ToString();
        }

        private String GenerateGroupingConditionKey(GroupingDefinition groupXGroupingDefinition,
                                                    GroupingDefinition groupYGroupingDefinition)
        {
            string xKey = GenerateGroupingConditionKey(groupXGroupingDefinition);
            string yKey = GenerateGroupingConditionKey(groupYGroupingDefinition);

            return string.Format(DefinitionKeyTemplate, xKey, yKey);
        }

        private void LoopGroupDefinitionY(Dictionary<string, Combine2DDimensionGroupingDefinition> dictionary,
            GroupingDefinition groupXGroupingDefinition,
            GroupingDefinition groupYGroupingDefinition)
        {
            Combine2DDimensionGroupingDefinition root = new Combine2DDimensionGroupingDefinition()
            {
                ListCalculateDefinition = this.CalculateDefinitions,
                XDefinition = groupXGroupingDefinition,
                YDefinition = groupYGroupingDefinition,
                Key = GenerateGroupingConditionKey(groupXGroupingDefinition, groupYGroupingDefinition),
            };

            _logger.DebugFormat("LoopGroupY: {0}", root.Key);

            if (groupYGroupingDefinition != null)
            {
                LoopGroupDefinitionY(dictionary, groupXGroupingDefinition, groupYGroupingDefinition.InnerDefinition);
            }


            dictionary.Add(root.Key, root);
        }
    }

}
