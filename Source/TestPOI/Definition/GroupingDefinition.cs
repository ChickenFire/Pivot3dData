using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using TestPOI.Data;

namespace TestPOI.Definition
{
    public class GroupingDefinition
    {
        private string _keyName = null;

        public string KeyName
        {
            get
            {
                return this._keyName;
            }
            set
            {
                this._keyName = value;

                var prop = Utils.GetPropertyInfo(this._keyName, typeof(TransactionInfo));
                if (prop != null)
                {
                    this.GetKeyMethod = prop;
                }
            }
        }

        public PropertyInfo GetKeyMethod { get; private set; }

        public GroupingDefinition InnerDefinition { get; set; }

        public GroupingDefinition UpperDefinition { get; set; }
    }
}
