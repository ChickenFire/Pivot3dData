using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using NPOI.HSSF.UserModel;
using System.IO;
using NPOI.SS.UserModel;
using TestPOI.Data;

namespace TestPOI
{
    public class ExcelRead
    {
        private HSSFWorkbook hssfworkbook = null;

        public void InitializeWorkbook(string path)
        {
            //read the template via FileStream, it is suggested to use FileAccess.Read to prevent file lock.
            //book1.xls is an Excel-2007-generated file, so some new unknown BIFF records are added. 
            using (FileStream file = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                hssfworkbook = new HSSFWorkbook(file);
            }
        }

        public List<ProductInfo> ReadProductData()
        {
            var result = new List<ProductInfo>();

            ISheet sheet = hssfworkbook.GetSheet("Bảng mã");
            System.Collections.IEnumerator rows = sheet.GetRowEnumerator();
            if (rows.MoveNext())
            {
                while (rows.MoveNext())
                {
                    IRow row = (HSSFRow)rows.Current;
                    var info = new ProductInfo()
                    {
                        Producer = row.GetCell(0).StringCellValue,
                        Category = row.GetCell(1).StringCellValue,
                        Description = row.GetCell(2).StringCellValue,
                    };

                    result.Add(info);
                }   
            }
            return result;
        }

        public List<TransactionInfo> ReadTransactionData(List<ProductInfo> listProductInfo)
        {
            var result = new List<TransactionInfo>();

            ISheet sheet = hssfworkbook.GetSheet("Data");
            System.Collections.IEnumerator rows = sheet.GetRowEnumerator();

            if (rows.MoveNext())
            {
                // skip the 1st column header
                while (rows.MoveNext())
                {
                    IRow row = (HSSFRow) rows.Current;

                    string productCode = row.GetCell(4).StringCellValue;
                    if (!string.IsNullOrEmpty(productCode))
                    {
                        DateTime transactionTimestamp = row.GetCell(0).DateCellValue;
                        string customerCode = row.GetCell(1).StringCellValue;
                        string customerName = row.GetCell(2).StringCellValue;
                        string description = row.GetCell(3).StringCellValue;

                        string productName = row.GetCell(5).StringCellValue;
                        double quantity = row.GetCell(6).NumericCellValue;
                        double contractPrice = row.GetCell(7).NumericCellValue;
                        double sellPrice = row.GetCell(8).NumericCellValue;
                        string sellPerson = row.GetCell(9).StringCellValue;
                        int year = transactionTimestamp.Year;
                        int month = transactionTimestamp.Month;
                        int quarter = transactionTimestamp.Month / 4 + 1;

                        string producer = string.Empty;
                        string productCategory = string.Empty;
                        string productType = string.Empty;

                        int hyphenIndex = productCode.IndexOf("-");
                        if (hyphenIndex >= 0)
                        {
                            producer = productCode.Substring(0, hyphenIndex);
                            productCategory = (from v in listProductInfo
                                               where v.Producer.Equals(producer)
                                               select v.Category).FirstOrDefault();
                            productType = productCode.Substring(hyphenIndex + 1);
                        }

                        result.Add(new TransactionInfo()
                            {
                                TransactionTimestamp = transactionTimestamp,
                                CustomerCode = customerCode,
                                CustomerName = customerName,
                                Description = description,
                                ProductCode = productCode,
                                ProductName = productName,
                                Quantity = quantity,
                                SellPrice = sellPrice,
                                ContractPrice = contractPrice,
                                SellPerson = sellPerson,
                                Year = year,
                                Month = month,
                                Quarter = quarter,
                                ProductCategory = productCategory,
                                ProductType = productType,
                                Producer = producer,
                            });
                    }
                }
            }

            return result;
        }
    }
}
