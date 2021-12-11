using RecipeManager.DAL.DTO;
using RecipeManager.DAL.Interfaces;
using RecipeManager.DAL.Extentions;
using RecipeManager.Core.Entities;
using System;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeManager.DAL.Repositories
{
    public class AppSettingsRepository : IJsonRepository
    {
        private FileStream _fileStream = null;
        public (bool IsError, string Message, AppSettingsDTO Settings) GetSettingsWithResult()
        {
            
            try
            {
                var file = new FileInfo("AppSettings.json");
                if (!(file.Exists))
                {
                    MakeFileStream();
                }

                using (_fileStream = new FileStream("AppSettings.json", FileMode.Open))
                {
                    var rd = new StreamReader(_fileStream);
                    var result = AppSettingsDtoExtention.FromModel(JsonConvert.DeserializeObject<AppSettings>(rd.ReadToEnd()));
                    rd.Close();
                    return (false, "", result);
                }
            }
            catch (Exception err)
            {
                return (true, err.Message, null);
            }
            finally
            {
                // _fileStream?.Dispose();
                _fileStream.Close();
            }

        }

        public (bool IsError, string Message) SetSettingsWithResult(AppSettingsDTO settings)
        {
            try
            {
                var file = new FileInfo("AppSettings.json");
                if (!(file.Exists))
                {
                    MakeFileStream();
                }

                using (_fileStream = new FileStream("AppSettings.json", FileMode.Create))
                {
                    var wr = new StreamWriter(_fileStream);
                    var str = JsonConvert.SerializeObject(AppSettingsDtoExtention.ToModel(settings));
                    wr.WriteLine(str);
                    wr.Close();
                    return (false, "");
                }

            }
            catch (Exception err)
            {
                return (true, err.Message);
            }
            finally
            {
                // _fileStream?.Dispose();
                _fileStream.Close();
            }

        }

        private void MakeFileStream()
        {
            var settings = new AppSettingsDTO()
            {
                IpAddressPLC = "192.168.250.1",
                SaveReportPath = @"C:\Users\User\source\repos\ConsoleApp15\ConsoleApp15\bin\Debug\netcoreapp3.1",
                GetDataCycleTimer = 5,
                TimeForFirstReport = "8:00",
                TimeForSecondReport = "20:00"
            };
            _fileStream = new FileStream("AppSettings.json", FileMode.OpenOrCreate);
            var wr = new StreamWriter(_fileStream);
            var str = JsonConvert.SerializeObject(AppSettingsDtoExtention.ToModel(settings));
            wr.WriteLine(str);
            wr.Close();
           // _fileStream = new FileStream("AppSettings.json", FileMode.Open);

        }

        public void Dispose()
        {

        }
    }
}
