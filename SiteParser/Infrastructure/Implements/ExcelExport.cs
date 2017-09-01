using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Data;
using System.IO;
using SiteParser.Infrastructure.Abstract;
using System.Drawing;

namespace SiteParser.Infrastructure.Implements
{
    public class ExcelExport : IExcelExport
    {
        public ExcelExport(string path, string fileName)
        {
            SetFullPath(path, fileName);
        }

        // Переопределение метода формирования отчета
        public override void ExportExcel(DataTable dataRows, string heading = "Объявления")
        {
            if (File.Exists(FullPath))
            {
                InsertRowsInReport(dataRows);
            }
            else
            {
                CreateNewExcelReport(dataRows, heading);
            }
        }

        private void CreateNewExcelReport(DataTable dataRows, string heading)
        {
            using (ExcelPackage package = new ExcelPackage())
            {
                ExcelWorksheet workSheet = package.Workbook.Worksheets.Add(heading);    // heading - Заголовок для листа в Excel

                for (int j = 1; j <= dataRows.Columns.Count; j++)
                {
                    workSheet.Cells[1, j].Value = dataRows.Columns[j - 1].Caption;
                }

                // Стиль для заголовка таблицы - жирный текст, выравнивание по центру, заливка серого цвета
                using (ExcelRange r = workSheet.Cells[1, 1, 1, dataRows.Columns.Count])
                {
                    r.Style.Font.Color.SetColor(Color.Black);
                    r.Style.Font.Bold = true;
                    r.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    r.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    r.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    r.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#cccccc"));
                }

                InsertRowsAtBottom(workSheet, dataRows);

                package.SaveAs(new FileInfo(FullPath)); // Сохраняем документ по пути FullPath
            }
        }

        private void AutoFitColumns(ExcelWorksheet workSheet, DataTable dataRows)
        {
            // Авторазмер колонок (под содержимое)
            int columnIndex = 1;
            foreach (DataColumn column in dataRows.Columns)
            {
                workSheet.Column(columnIndex).AutoFit();
                columnIndex++;
            }
        }

        private void InsertRowsAtBottom(ExcelWorksheet workSheet, DataTable dataRows)
        {
            int lastRow = workSheet.Dimension.End.Row;

            for (int i = lastRow + 1; i <= lastRow + dataRows.Rows.Count; i++)
            {
                for (int j = 1; j <= dataRows.Columns.Count; j++)
                {
                    workSheet.Cells[i, j].Value = dataRows.Rows[i - (lastRow + 1)][j - 1].ToString();
                }
            }

            AutoFitColumns(workSheet, dataRows);

            SetDataCellsStyle(workSheet, dataRows, lastRow);
        }

        private void SetDataCellsStyle(ExcelWorksheet workSheet, DataTable dataRows, int beginRow)
        {
            using (ExcelRange r = workSheet.Cells[beginRow + 1, 1, dataRows.Rows.Count + beginRow, dataRows.Columns.Count])
            {
                r.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                r.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                r.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                r.Style.Border.Right.Style = ExcelBorderStyle.Thin;

                r.Style.Border.Top.Color.SetColor(System.Drawing.Color.Black);
                r.Style.Border.Bottom.Color.SetColor(System.Drawing.Color.Black);
                r.Style.Border.Left.Color.SetColor(System.Drawing.Color.Black);
                r.Style.Border.Right.Color.SetColor(System.Drawing.Color.Black);
            }
        }

        private void InsertRowsInReport(DataTable dataRows)
        {
            using (ExcelPackage package = new ExcelPackage(new FileInfo(FullPath)))
            {
                ExcelWorksheet workSheet = package.Workbook.Worksheets[1];

                // Стиль для ячеек с данными - черная рамка по периметру 
                InsertRowsAtBottom(workSheet, dataRows);

                package.SaveAs(new FileInfo(FullPath)); // Сохраняем документ по пути FullPath
            }
        }
    }
}
