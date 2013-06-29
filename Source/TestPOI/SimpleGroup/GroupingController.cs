using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestPOI.Definition;
using System.Reflection;
using TestPOI.Data;

namespace TestPOI.SimpleGroup
{
    public class GroupingController
    {
        public static string GetItemKey(PropertyInfo getKeyMethod, TransactionInfo data)
        {
            object value = getKeyMethod.GetValue(data, null);
            string itemKey = (value == null ? string.Empty : value.ToString());

            return itemKey;
        }

        public static string GetKey(string rootKey, string itemKey)
        {
            string key = new StringBuilder(rootKey).Append("_").Append(itemKey).ToString();
            return key;
        }

        private void AddDataGroup(GroupInfo root, GroupingDefinition definition)
        {
            foreach (var info in root.ListData)
            {
                string rootKey = root.Key;
                string groupKey = GetItemKey(definition.GetKeyMethod, info);
                string key = GetKey(rootKey, groupKey);

                GroupInfo groupInfo = null;
                if (!root.InnerGroupData.TryGetValue(key, out groupInfo))
                {
                    groupInfo = new GroupInfo()
                    {
                        RootKey = rootKey,
                        GroupKey = groupKey,
                        Key = key,
                        GroupingDefinition = definition,
                        ListData = new List<TransactionInfo>(),
                        InnerGroupData = new Dictionary<string, GroupInfo>(),
                        ListCalculateDefinition = root.ListCalculateDefinition
                    };
                    root.InnerGroupData.Add(key, groupInfo);
                }

                groupInfo.ListData.Add(info);
            }

            foreach (var groupInfoInner in root.InnerGroupData.Values)
            {
                if (definition.InnerDefinition != null)
                {
                    AddDataGroup(groupInfoInner, definition.InnerDefinition);
                }
                groupInfoInner.RefreshCalculateValue();
            }
        }

        public GroupInfo AddData(GroupingDefinition definition, List<CalculateDefinition> listCalculateDefinition, List<TransactionInfo> transactionInfo)
        {
            var root = new GroupInfo()
            {
                Key = string.Empty,
                ListData = transactionInfo,
                InnerGroupData = new Dictionary<string, GroupInfo>(),
                ListCalculateDefinition = listCalculateDefinition,
            };
            root.RefreshCalculateValue();
            AddDataGroup(root, definition);

            return root;
        }
    }
}
