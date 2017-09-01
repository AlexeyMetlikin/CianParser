using NUnit.Framework;
using NUnit.Framework.Constraints;
using SiteParser.Infrastructure.Abstract;
using SiteParser.Infrastructure.Implements;
using SiteParser.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Reflection;

namespace SiteParserTests.Infrastructure
{
    [TestFixture]
    public class ExcelExportTest
    {
        [Test]
        public void Can_Set_Valid_Full_Path()
        {
            //Arrange
            IExcelExport excelExport = new ExcelExport("", "");

            string directoryPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            string fileName = "TestReport.csv";

            //Act
            excelExport.SetFullPath(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "TestReport.csv");

            //Assert
            Assert.AreEqual(excelExport.FullPath, Path.Combine(directoryPath, fileName));
        }

        [Test]
        public void Cannot_Set_Null_Full_Path()
        {
            //Arrange
            IExcelExport excelExport = new ExcelExport("", "");

            //Act and Assert
            Assert.That(() => excelExport.SetFullPath(null, null), Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void Can_Get_DataTable_From_Not_Null_List()
        {
            //Arrange
            IExcelExport excelExport = new ExcelExport("", "");

            var dataList = new List<Advertisement>
            {
                new Advertisement { Address = "Address1", Price = "1", Photos = null, Square = 0, PlacingDate = null },
                new Advertisement { Address = "Address2", Price = "2", Photos = null, Square = 0, PlacingDate = null },
            };

            //Act
            var result = excelExport.ListToDataTable(dataList);

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Rows.Count, 2);
            Assert.AreEqual(result.Columns.Count, 5);

            Assert.AreEqual(result.Rows[0]["Address"], "Address1");
            Assert.AreEqual(result.Rows[0]["Price"], "1");
            Assert.AreEqual(result.Rows[1]["Address"], "Address2");
            Assert.AreEqual(result.Rows[1]["Price"], "2");
        }

        [Test]
        public void Cannot_Get_DataTable_From_Null_List()
        {
            //Arrange
            IExcelExport excelExport = new ExcelExport("", "");

            //Act and Assert
            Assert.That(() => excelExport.ListToDataTable<string>(null), Throws.TypeOf<NullReferenceException>());
        }

        [Test]
        public void Can_Set_Header_Caption_For_DataTable_Columns()
        {
            //Arrange
            DataTable dataTable = new DataTable();

            dataTable.Columns.Add("Column 1");
            dataTable.Columns.Add("Column 2");
            dataTable.Columns.Add("Column 3");

            dataTable.Rows.Add(new object[] { "1", "2", "3" });

            IExcelExport excelExport = new ExcelExport("", "");

            var headersCaption = new string[] { "Caption 1", "Caption 2", "Caption 3" };

            //Act
            dataTable = excelExport.SetHeadersCaption(dataTable, headersCaption);

            //Assert
            Assert.AreEqual(dataTable.Columns[0].Caption, "Caption 1");
            Assert.AreEqual(dataTable.Columns[1].Caption, "Caption 2");
            Assert.AreEqual(dataTable.Columns[2].Caption, "Caption 3");
        }

        [Test]
        public void Can_Set_Header_Caption_If_It_Count_Less_Columns_Than_DataTable_Has()
        {
            //Arrange
            DataTable dataTable = new DataTable();

            dataTable.Columns.Add("Column 1");
            dataTable.Columns.Add("Column 2");
            dataTable.Columns.Add("Column 3");

            dataTable.Rows.Add(new object[] { "1", "2", "3" });

            IExcelExport excelExport = new ExcelExport("", "");

            var headersCaption = new string[] { "Caption 1", "Caption 2" };

            //Act
            dataTable = excelExport.SetHeadersCaption(dataTable, headersCaption);

            //Assert
            Assert.AreEqual(dataTable.Columns[0].Caption, "Caption 1");
            Assert.AreEqual(dataTable.Columns[1].Caption, "Caption 2");
            Assert.AreEqual(dataTable.Columns[2].Caption, "Column 3");
        }

        [Test]
        public void Cannot_Set_Header_Caption_For_More_Columns_Than_DataTable_Has()
        {
            //Arrange
            DataTable dataTable = new DataTable();

            dataTable.Columns.Add("Column 1");
            dataTable.Columns.Add("Column 2");
            dataTable.Columns.Add("Column 3");

            dataTable.Rows.Add(new object[] { "1", "2", "3" });

            IExcelExport excelExport = new ExcelExport("", "");

            var headersCaption = new string[] { "Caption 1", "Caption 2", "Caption 3", "Caption 4" };

            //Act and Assert
            Assert.That(() => excelExport.SetHeadersCaption(dataTable, headersCaption), Throws.TypeOf<IndexOutOfRangeException>());
        }

        [Test]
        public void Cannot_Set_Null_Header_Captions()
        {
            //Arrange
            DataTable dataTable = new DataTable();

            dataTable.Columns.Add("Column 1");
            dataTable.Columns.Add("Column 2");
            dataTable.Columns.Add("Column 3");

            dataTable.Rows.Add(new object[] { "1", "2", "3" });
            
            IExcelExport excelExport = new ExcelExport("", "");

            //Act and Assert
            Assert.That(() => excelExport.SetHeadersCaption(dataTable, null), Throws.TypeOf<NullReferenceException>());
        }

        [Test]
        public void Can_Create_New_Excel_Report()
        {
            //Arrange
            IExcelExport excelExport = new ExcelExport("", "TestReport.csv");

            DataTable dataTable = new DataTable();

            dataTable.Columns.Add("Column 1");
            dataTable.Columns.Add("Column 2");
            dataTable.Columns.Add("Column 3");

            dataTable.Rows.Add(new object[] { 1, 2, 3 });
            dataTable.Rows.Add(new object[] { 2, 3, 4 });
            dataTable.Rows.Add(new object[] { 3, 4, 5 });

            //Act
            excelExport.ExportExcel(dataTable, "Тестовый отчет");

            //Assert
            Assert.IsTrue(File.Exists("TestReport.csv"));

            // Удаляем созданный файл
            if (File.Exists("TestReport.csv"))
            {
                File.Delete("TestReport.csv");
            }
        }

        [Test]
        public void Can_Append_Data_To_Existent_Excel_Report()
        {
            //Arrange
            IExcelExport excelExport = new ExcelExport("", "TestReport.csv");

            DataTable dataTable = new DataTable();

            dataTable.Columns.Add("Column 1");
            dataTable.Columns.Add("Column 2");
            dataTable.Columns.Add("Column 3");

            dataTable.Rows.Add(new object[] { 1, 2, 3 });
            dataTable.Rows.Add(new object[] { 2, 3, 4 });
            dataTable.Rows.Add(new object[] { 3, 4, 5 });

            //Act
            excelExport.ExportExcel(dataTable, "Тестовый отчет");

            var oldLength = (new FileInfo("TestReport.csv")).Length;

            excelExport.ExportExcel(dataTable);

            var newLength = (new FileInfo("TestReport.csv")).Length;

            //Assert
            Assert.IsTrue(File.Exists("TestReport.csv"));
            Assert.IsTrue(newLength > oldLength);

            // Удаляем созданный файл
            if (File.Exists("TestReport.csv"))
            {
                File.Delete("TestReport.csv");
            }
        }

        [Test]
        public void Cannot_Export_To_Excel_With_Null_Worksheet_Heading()
        {
            //Arrange
            IExcelExport excelExport = new ExcelExport("", "TestReport.csv");

            DataTable dataTable = new DataTable();

            dataTable.Columns.Add("Column 1");
            dataTable.Columns.Add("Column 2");
            dataTable.Columns.Add("Column 3");

            dataTable.Rows.Add(new object[] { 1, 2, 3 });
            dataTable.Rows.Add(new object[] { 2, 3, 4 });
            dataTable.Rows.Add(new object[] { 3, 4, 5 });
            
            //Act and Assert
            Assert.That(() => excelExport.ExportExcel(dataTable, null), Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void Cannot_Export_To_Excel_With_Null_DataTable()
        {
            //Arrange
            IExcelExport excelExport = new ExcelExport("", "TestReport.csv");

            //Act and Assert
            Assert.That(() => excelExport.ExportExcel(null, "Тестовый отчет"), Throws.TypeOf<NullReferenceException>());
        }

        [Test]
        public void Cannot_Export_To_Excel_With_Empty_Worksheet_Heading()
        {
            //Arrange
            IExcelExport excelExport = new ExcelExport("", "TestReport.csv");

            DataTable dataTable = new DataTable();

            dataTable.Columns.Add("Column 1");
            dataTable.Columns.Add("Column 2");
            dataTable.Columns.Add("Column 3");

            dataTable.Rows.Add(new object[] { 1, 2, 3 });
            dataTable.Rows.Add(new object[] { 2, 3, 4 });
            dataTable.Rows.Add(new object[] { 3, 4, 5 });

            //Act and Assert
            Assert.That(() => excelExport.ExportExcel(dataTable, ""), Throws.TypeOf<ArgumentException>());
        }
    }
}
