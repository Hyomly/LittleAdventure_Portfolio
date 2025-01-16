using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Excel;

namespace ExcelParser
{
    internal class ExcelDataReader
    {
        public static DataRow[] GetExcelDataRows(string path, string sheetName, out int rowCount, out int columnCount)
        {
            DataTable dataTable = GetDataTable(path, sheetName);
            DataRow[] dataRows = dataTable.Select();

            rowCount =dataTable.Rows.Count;
            columnCount = dataTable.Columns.Count;

            return dataRows;
        }
        //여러 sheet 이름 불러오기
        public static string[] GetSheetNames(string path)
        {
            //즉시 메모리 반환
            using (FileStream fs = File.Open(path, FileMode.Open, FileAccess.Read))
            {

                IExcelDataReader ireader = null;
                try
                {
                    if (path.EndsWith(".xls"))
                    {
                        ireader = ExcelReaderFactory.CreateBinaryReader(fs);
                    }
                    else if (path.EndsWith(".xlsx"))
                    {
                        ireader = ExcelReaderFactory.CreateOpenXmlReader(fs);
                    }
                    var dataset = ireader.AsDataSet();
                    string[] sheets = new string[dataset.Tables.Count];
                    for (int i = 0; i < dataset.Tables.Count; i++)
                    {
                        sheets[i] = dataset.Tables[i].TableName;
                    }
                    return sheets;

                }
                catch (Exception ex)
                {
                    //오류
                    Console.WriteLine(ex.Message);
                    throw;
                }
                finally
                {
                    //정상적 처리 될때 종료
                    ireader.Close();
                    fs.Close();
                }
            }
        }

        //파일 경로, 시트 이름으로  데이터 불러오기
        static DataTable GetDataTable(string path, string sheetName)
        {
            
            //즉시 메모리 반환
            using (FileStream fs = File.Open(path, FileMode.Open, FileAccess.Read))
            {

                IExcelDataReader ireader = null;
                try
                {
                    if (path.EndsWith(".xls"))
                    {
                        ireader = ExcelReaderFactory.CreateBinaryReader(fs);
                    }
                    else if (path.EndsWith(".xlsx"))
                    {
                        ireader = ExcelReaderFactory.CreateOpenXmlReader(fs);
                    }
                    var dataset = ireader.AsDataSet();
                    var sheet = dataset.Tables[sheetName];

                    return sheet;

                }
                catch (Exception ex)
                {
                    //오류
                    Console.WriteLine(ex.Message);
                    throw;
                }
                finally
                {
                    //정상적 처리 될때 종료
                    ireader.Close();
                    fs.Close();
                }
            }
        }
    }
}
