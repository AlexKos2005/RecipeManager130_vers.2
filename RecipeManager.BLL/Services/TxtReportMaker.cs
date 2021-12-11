using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RecipeManager.BLL.Interfaces;
using RecipeManager.DAL.DTO;

namespace RecipeManager.BLL.Services
{
    public class TxtReportMaker : IReportMaker
    {
        private string _filePath;
        private string _fileName;

        public (bool IsError, string Message) MakeReport(string filePath,string fileName, List<DoseDto> doses)
        {
            _filePath = filePath;
            _fileName = fileName;
            var convertResult = ConvertDosesToString(doses);
            if(convertResult.IsError)
            {
                return (true, convertResult.Message);
            }
            else
            {
                var saveResult = SaveStrToTxtFile(convertResult.Values);
                if(saveResult.IsError)
                {
                    return (true, saveResult.Message);
                }
                else
                {
                    return (false, "");
                }
            }
        }

        private (bool IsError,string Message,StringBuilder Values) ConvertDosesToString(List<DoseDto> doses)
        {
            var recipeCounter = 0;
            var strResult = new StringBuilder();

            foreach(var dose in doses)
            {
                var result = DoseToString(dose, ref strResult);
                if(result.IsError)
                {
                    return (true, $"{result.Message}//Номер рецепта:{recipeCounter}",null);
                }
            }

            return (false, "", strResult);

        }

        private (bool IsError, string Message) DoseToString(DoseDto doseDto,ref StringBuilder str)
        {
            try
            {
                str.AppendLine(doseDto.Date.Year.ToString());
                str.AppendLine(doseDto.Date.Month.ToString());
                str.AppendLine(doseDto.Date.Day.ToString());

                str.AppendLine(doseDto.Time.Hours.ToString());
                str.AppendLine(doseDto.Time.Minutes.ToString());
                str.AppendLine(doseDto.Time.Seconds.ToString());

                str.AppendLine((doseDto.RecipeName));

                str.AppendLine(doseDto.ComponentCode_1.ToString());
                str.AppendLine(doseDto.ComponentWeight_1.ToString());

                str.AppendLine(doseDto.ComponentCode_2.ToString());
                str.AppendLine(doseDto.ComponentWeight_2.ToString());

                str.AppendLine(doseDto.ComponentCode_3.ToString());
                str.AppendLine(doseDto.ComponentWeight_3.ToString());

                str.AppendLine(doseDto.ComponentCode_4.ToString());
                str.AppendLine(doseDto.ComponentWeight_4.ToString());

                str.AppendLine(doseDto.ComponentCode_5.ToString());
                str.AppendLine(doseDto.ComponentWeight_5.ToString());

                str.AppendLine(doseDto.ComponentCode_6.ToString());
                str.AppendLine(doseDto.ComponentWeight_6.ToString());

                str.AppendLine(doseDto.ComponentCode_7.ToString());
                str.AppendLine(doseDto.ComponentWeight_7.ToString());

                str.AppendLine(doseDto.ComponentCode_8.ToString());
                str.AppendLine(doseDto.ComponentWeight_8.ToString());

                str.AppendLine(doseDto.ComponentCode_9.ToString());
                str.AppendLine(doseDto.ComponentWeight_9.ToString());

                str.AppendLine(doseDto.ComponentCode_10.ToString());
                str.AppendLine(doseDto.ComponentWeight_10.ToString());

                str.AppendLine(doseDto.ComponentCode_11.ToString());
                str.AppendLine(doseDto.ComponentWeight_11.ToString());

                str.AppendLine(doseDto.ComponentCode_12.ToString());
                str.AppendLine(doseDto.ComponentWeight_12.ToString());

                str.AppendLine(doseDto.ComponentCode_13.ToString());
                str.AppendLine(doseDto.ComponentWeight_13.ToString());

                str.AppendLine(doseDto.ComponentCode_14.ToString());
                str.AppendLine(doseDto.ComponentWeight_14.ToString());

                str.AppendLine(doseDto.ComponentCode_15.ToString());
                str.AppendLine(doseDto.ComponentWeight_15.ToString());

                str.AppendLine(doseDto.ComponentCode_16.ToString());
                str.AppendLine(doseDto.ComponentWeight_16.ToString());

                str.AppendLine(doseDto.WaterWeight.ToString());
                str.AppendLine(doseDto.WaterTemper.ToString());

                return (false, "");
            }
            catch(Exception e)
            {
                return (true, e.Message);
            }
           
        }

        private (bool IsError, string Message) SaveStrToTxtFile(StringBuilder str)
        {
            try
            {
                //var fileInfo = new FileInfo(_filePath);
                //if (fileInfo.Exists)
                //{
                //    _fileName = _fileName + DateTime.Now.ToShortTimeString();
                //}

                using (var file = new FileStream($"{_filePath}/{_fileName}.txt", FileMode.OpenOrCreate))
                {
                    var strWr = new StreamWriter(file,Encoding.Default);
                    strWr.WriteAsync(str.ToString()).Wait();
                    strWr.Close();
                }
                return (false, "");
            }

            catch(Exception e)
            {
                return (true, $"Ошибка при сохранении файла отчета // {e.Message}");
            }
            
        }
    }
}
