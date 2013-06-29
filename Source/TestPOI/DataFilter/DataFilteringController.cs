using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using TestPOI.Data;

namespace TestPOI.DataFilter
{
    public class DataFilteringController
    {
        public List<FilteringItem> FilterProps { get; set; }

        public bool PerformCheck(TransactionInfo transactionDetail)
        {
            var checkOk = true;
            if (FilterProps != null)
            {
                for (int i = 0; i < FilterProps.Count && checkOk; i++)
                {
                    var filterProp = FilterProps[i];
                    if (filterProp.FilterValue != null)
                    {
                        checkOk = filterProp.FilterValue.Equals(
                            filterProp.FilterMethod.GetValue(transactionDetail, null));
                    }
                }
            }

            return checkOk;
        }

        public List<TransactionInfo> GetFilterData(List<TransactionInfo> transactions)
        {
            var data = (from transactionDetail in transactions
                        where PerformCheck(transactionDetail)
                        select transactionDetail).ToList();

            return data;
        }
    }
}
