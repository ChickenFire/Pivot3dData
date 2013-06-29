using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestPOI.Definition;
using TestPOI.Data;

namespace TestPOI.Pivot
{
    public class CombineDataResult
    {
        public CombineDataKey Key { get; private set; }

        private readonly List<CalculateDefinition> _listCalculateDefinition = null;

        private readonly List<TransactionInfo> _listData = null;

        private readonly Dictionary<CalculateDefinition, double> _sumValues = null;

        public CombineDataResult(CombineDataKey key,
            List<CalculateDefinition> listCalculateDefinition)
        {
            Key = key;
            _listData = new List<TransactionInfo>();
            _listCalculateDefinition = listCalculateDefinition;
            _sumValues = new Dictionary<CalculateDefinition, double>();
            foreach (var calculateDefinition in listCalculateDefinition)
            {
                _sumValues.Add(calculateDefinition, 0.0);
            }
        }

        public double GetValueCalculate(CalculateDefinition definition)
        {
            double result = _sumValues[definition];
            if (definition.Type == CalculateType.Average)
            {
                result /= _listData.Count;
            }

            return result;
        }

        public void AddData(TransactionInfo data)
        {
            _listData.Add(data);

            foreach (var calculateDefinition in this._listCalculateDefinition)
            {
                double value = Convert.ToDouble(
                    calculateDefinition.CalculateMethod.GetValue(data, null));

                _sumValues[calculateDefinition] += value;
            }
        }
    }
}
