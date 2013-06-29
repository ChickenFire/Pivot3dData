using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using TestPOI.Data;

namespace TestPOI.DataFilter
{
    public class FilteringItem
    {
        private string _filterName = null;

        public string FilterName
        {
            get
            {
                return this._filterName;
            }
            set
            {
                this._filterName = value;

                var prop = Utils.GetPropertyInfo(this._filterName, typeof(TransactionInfo));
                if (prop != null)
                {
                    this.FilterMethod = prop;
                }
            }
        }

        public PropertyInfo FilterMethod { get; private set; }

        public object FilterValue { get; set; }
    }
}
