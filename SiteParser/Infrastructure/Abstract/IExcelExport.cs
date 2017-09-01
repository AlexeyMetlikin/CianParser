using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;

namespace SiteParser.Infrastructure.Abstract
{
    public abstract class IExcelExport
    {
        public string FullPath { get; private set; } // Полный путь до сформированного Excel-отчета

        // Метод формирования отчета
        public abstract void ExportExcel(DataTable dataRows, string heading = "Объявления");

        // Метод определения полного пути
        public void SetFullPath(string path, string fileName)
        {
            FullPath = System.IO.Path.Combine(path, fileName);  // Формируем путь
            if (System.IO.File.Exists(FullPath))                // Если файл существует
            {
                string extension = System.IO.Path.GetExtension(FullPath);
                fileName = System.IO.Path.GetFileNameWithoutExtension(FullPath);
                int i = 1;
                while (System.IO.File.Exists(FullPath))
                {
                    FullPath = System.IO.Path.Combine(path, fileName + " (" + i.ToString() + ")" + extension); // Добавляем к файлу номер: (1), (2), ..., (n)
                    i++;
                }
            }
        }

        // Метод формирования таблицы DataTable
        public DataTable ListToDataTable<T>(List<T> data)
        {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));  // получаем типы всех данных из типа T
            DataTable dataTable = new DataTable();

            for (int i = 0; i < properties.Count; i++)
            {
                PropertyDescriptor property = properties[i];

                // Добавляем колонки с нужным типом данных
                dataTable.Columns.Add(property.Name, Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType);
            }

            object[] values = new object[properties.Count];
            foreach (var item in data)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    values[i] = properties[i].GetValue(item);   // Формируем массив данных для одной строки из списка
                }

                dataTable.Rows.Add(values); // Заполняем строки DataTable
            }
            return dataTable;
        }

        // Проставление заголовков колонок dataTable
        public DataTable SetHeadersCaption(DataTable dataTable, string[] headersCaption)
        {
            for (int i = 0; i < headersCaption.Length; i++)
            {
                dataTable.Columns[i].Caption = headersCaption[i];
            }

            return dataTable;
        }
    }
}
