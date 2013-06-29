using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestPOI.Definition;

namespace TestPOI.Pivot
{
    public class Combine2DDimensionGroupingDefinition
    {
        public GroupingDefinition XDefinition { get; set; }

        public GroupingDefinition YDefinition { get; set; }

        public List<CalculateDefinition> ListCalculateDefinition { get; set; }

        public string Key { get; set; }
    }
}
