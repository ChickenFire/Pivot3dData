using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using TestPOI.Data;

namespace TestPOI.Definition
{
    public class CalculateDefinition
    {
        private string _calculateName = null;

        public string CalculateName
        {
            get
            {
                return this._calculateName;
            }
            set
            {
                this._calculateName = value;

                var prop = Utils.GetPropertyInfo(this._calculateName, typeof(TransactionInfo));
                if (prop != null)
                {
                    this.CalculateMethod = prop;
                }
            }
        }

        public PropertyInfo CalculateMethod { get; private set; }

        public CalculateType Type { get; set; }
    }
}
