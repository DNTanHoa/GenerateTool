using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerateTool.Helper
{
    public class ExcelHelper
    {
        /// <summary>
        /// Read excel file at sheet name determine by sheet index
        /// Author: Hoa-Bui
        /// </summary>
        /// <param name="indexSheet"></param>
        /// <param name="pathFile"></param>
        /// <param name="sheetName"></param>
        /// <returns></returns>
        public static DataTable ExcelHelperGetTable(int indexSheet, string pathFile, out string sheetName)
        {
            sheetName = string.Empty;

            DataTable _sheet = new DataTable();
            DataColumn _nameProperty = new DataColumn();
            DataColumn _typeData = new DataColumn();
            DataColumn _relationship = new DataColumn();
            DataColumn _nameAssociation = new DataColumn();
            DataRow _rowtable;
            if (!File.Exists(pathFile)) return null;

            byte[] bin = File.ReadAllBytes(pathFile);

            MemoryStream stream = new MemoryStream(bin);
            ExcelPackage excelPackage = new ExcelPackage(stream);
            ExcelWorksheet workSheet = excelPackage.Workbook.Worksheets.ElementAt(indexSheet);

            /*Get the sheet name for class name and file name*/
            sheetName = workSheet.Name;

            _nameProperty.ColumnName = "Property Name";
            _nameProperty.DataType = System.Type.GetType("System.String");
            _typeData.ColumnName = "Type Value";
            _typeData.DataType = System.Type.GetType("System.String");
            _relationship.ColumnName = "Relationship";
            _relationship.DataType = System.Type.GetType("System.String");
            _nameAssociation.ColumnName = "Association Name";
            _nameAssociation.DataType = System.Type.GetType("System.String");
            _sheet.Columns.Add(_nameProperty);
            _sheet.Columns.Add(_typeData);
            _sheet.Columns.Add(_relationship);
            _sheet.Columns.Add(_nameAssociation);

            /*Read all row to get data*/
            for (int row = 8; row < workSheet.Dimension.End.Row; row++)
            {
                _rowtable = _sheet.NewRow();
                _rowtable[_nameProperty] = workSheet.Cells[row, 1].Value != null?  workSheet.Cells[row, 1].Value.ToString() : "";
                _rowtable[_typeData] = workSheet.Cells[row, 2].Value != null? workSheet.Cells[row, 2].Value.ToString() : "";
                _rowtable[_relationship] = workSheet.Cells[row, 3].Value != null? workSheet.Cells[row, 3].Value.ToString() : "";
                _rowtable[_nameAssociation] = workSheet.Cells[row, 4].Value != null? workSheet.Cells[row, 4].Value.ToString() : "";
                _sheet.Rows.Add(_rowtable);
            }
            return _sheet;
        }
    }
}
