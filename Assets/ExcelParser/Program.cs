using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelParser
{
    internal class Program
    {
        static StringBuilder m_sb = new StringBuilder();
        static void Main(string[] args)
        {
            //실행 안내
            if(args.Length < 2)
            {
                string message = "전체 Sheet를 Export 하려면 실행 파일 .exe 엑셀파일 .exe / all \r\n1개 sheet를 Export 하려면 실행파일.exe 엑셀파일.exe 시트이름";
                Console.WriteLine(message);
                Console.ReadKey();
                return;
            }
            //모든 Sheet불러오기
            if (args[1].ToLower().Equals("/all")) 
            {
                string[] sheetNames = ExcelDataReader.GetSheetNames(args[0]);
                foreach(string sheetName in sheetNames)
                {
                    //parsing 안할 sheet 넘기기 (#_ 으로 시작되는 sheet는 안불러옴)
                    var tockens = sheetName.Split('_');
                    if (tockens[0].Equals("#"))
                    {
                        continue;
                    }
                    Console.WriteLine("Export : " + sheetName);
                    ExportExcelToText(args[0],sheetName);
                    
                }
            }
            //Sheet 1개만 불러오기
            else
            {
                Console.WriteLine("Export : " + args[1]);
                ExportExcelToText(args[0], args[1]);
            }
        }

        //엑셀파일을 text파일로 Export
        static void ExportExcelToText(string file, string sheetName)
        {
            //출력이름
            string output = sheetName + ".txt";
            //경로
            string path = string.Empty;

            //sheet 데이터 불러오기
            int rowCount;
            int columnCount;            
            var dataRows = ExcelDataReader.GetExcelDataRows(file, sheetName, out rowCount, out columnCount);

            //text로 파일 저장하기
            StreamWriter sw = null;
            try
            {
                //엑셀 파일명으로 만든 text폴더로 경로지정
                path = "./" + Path.GetFileNameWithoutExtension(file) + "/text";
                //경로에 directory가 존재 확인
                DirectoryInfo directoryInfo = new DirectoryInfo(path);
                if(!directoryInfo.Exists)
                {
                    directoryInfo.Create();
                }
                //파일 생성
                sw = new StreamWriter(path + "/" + output);

            }catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
            sw.WriteLine(rowCount);
            sw.WriteLine(columnCount);

            for(int i = 0; i< rowCount; i++)
            {
                for (int j = 0; j < columnCount; j++)
                {
                    //열 데이터를 tab으로 구분
                    m_sb.Append('\t');

                    //데이터가 없을경우
                    if (dataRows[i].ItemArray[j].ToString().Length == 0)
                    {
                        m_sb.Append(" ");
                    }
                    else
                    {
                        m_sb.Append(dataRows[i].ItemArray[j].ToString());
                    }
                }
                m_sb.Append("\n");
            }
            m_sb.Append('\t');
            //불러온 데이터 파일에 작성
            sw.Write(m_sb.ToString());
            sw.Close();
            Console.WriteLine(m_sb.ToString());
            m_sb.Clear();
        }
    }
}
