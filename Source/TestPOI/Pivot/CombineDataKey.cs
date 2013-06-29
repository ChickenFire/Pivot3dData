using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestPOI.Pivot
{
    public class CombineDataKey
    {
        const string KeyTemplate = "xKey:[{0}]-yKey:[{1}]";

        public string XKey { get; set; }

        public string YKey { get; set; }

        public string XKeyDisplay
        {
            get
            {
                return XKey.Split(',')[0];
            }
        }

        public string YKeyDisplay
        {
            get
            {
                return YKey.Split(',')[0];
            }
        }

        public string CombineKey
        {
            get
            {
                return string.Format(KeyTemplate, XKey, YKey);
            }
        }
    }
}
