using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPOI.HSSF.UserModel;
using System.IO;
using TestPOI.Pivot;
using TestPOI.Definition;
using TestPOI.SimpleGroup;
using TestPOI.DataFilter;

namespace TestPOI
{
    class Program
    {
        static void Main(string[] args)
        {
            ExcelRead reader = new ExcelRead();
            reader.InitializeWorkbook("20130417 Tong Doanh Thu 2013-Phan Tich.xls");

            var listProductInfo = reader.ReadProductData();
            var listData = reader.ReadTransactionData(listProductInfo);

            var listFilterRequest = new List<FilteringItem>(new FilteringItem[]
                {
                    new FilteringItem()
                        {
                            FilterName = "Year",
                            FilterValue = 2012,
                        },
                    new FilteringItem()
                        {
                            FilterName = "Month",
                            FilterValue = 1,
                        },
                    new FilteringItem()
                        {
                            FilterName = "CustomerName",
                            FilterValue = "Bác sĩ Nguyễn Văn Cót",
                        }
                });

            var calculateDefinitions = new List<CalculateDefinition>(new CalculateDefinition[]
                {
                    new CalculateDefinition()
                        {
                            CalculateName = "Quantity",
                            Type = CalculateType.Sum,
                        },
                    new CalculateDefinition()
                        {
                            CalculateName = "ContractPrice",
                            Type = CalculateType.Average,
                        },
                    new CalculateDefinition()
                        {
                            CalculateName = "SellPrice",
                            Type = CalculateType.Sum,
                        },
                });

            var groupXDefinitionCustomerName = new GroupingDefinition()
                {
                    KeyName = "CustomerName"
                };

            var groupXDefinitionProductCategory = new GroupingDefinition()
                {
                    KeyName = "ProductCategory"
                };

            var groupXDefinitionProductName = new GroupingDefinition()
                {
                    KeyName = "ProductName"
                };

            groupXDefinitionCustomerName.UpperDefinition = null;
            groupXDefinitionCustomerName.InnerDefinition = groupXDefinitionProductCategory;

            groupXDefinitionProductCategory.UpperDefinition = groupXDefinitionCustomerName;
            groupXDefinitionProductCategory.InnerDefinition = groupXDefinitionProductName;

            groupXDefinitionProductName.UpperDefinition = groupXDefinitionProductCategory;
            groupXDefinitionProductName.InnerDefinition = null;

            var groupYDefinitionYear = new GroupingDefinition()
                {
                    KeyName = "Year",
                };

            var groupYDefinitionMonth = new GroupingDefinition()
                {
                    KeyName = "Month",
                };

            groupYDefinitionYear.UpperDefinition = null;
            groupYDefinitionYear.InnerDefinition = groupYDefinitionMonth;

            groupYDefinitionMonth.UpperDefinition = groupYDefinitionYear;
            groupYDefinitionMonth.InnerDefinition = null;


            Console.WriteLine(listProductInfo.Count);
            Console.WriteLine(listData.Count);

            //DataFilteringController filterControl = new DataFilteringController()
            //    {
            //        FilterProps = listFilterRequest
            //    };
            //var filterData = filterControl.GetFilterData(listData);

            GroupingController controller = new GroupingController();
            var result = controller.AddData(groupYDefinitionYear, calculateDefinitions, listData);

            var pivotTable = new PivotTableController()
                {
                    GroupingX = groupXDefinitionCustomerName,
                    GroupingY = groupYDefinitionYear,
                    Data = listData,
                    //Data = filterData,
                    CalculateDefinitions = calculateDefinitions,
                    DataFiltering = new DataFilteringController()
                    {
                        FilterProps = listFilterRequest
                    }
                };

            pivotTable.AnalyzeData();
            Console.ReadKey();
        }
    }
}
