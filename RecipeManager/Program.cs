using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using RecipeManager.BLL.Interfaces;
using RecipeManager.BLL.Services;
using RecipeManager.DAL.Interfaces;
using RecipeManager.DAL.Repositories;
using NLog;
using System.Diagnostics;
using RecipeManager.DAL.DTO;

namespace RecipeManager
{
    static class Program
    {
        private static AppSettingsDTO _appSettingsDTO;
        private static ILogger _logger;
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            _logger = LogManager.GetCurrentClassLogger();
            if (Process.GetProcessesByName("RecipeManager").Count() > 1)
            {
                MessageBox.Show("Программа уже запущена!");
                Process.GetCurrentProcess().Kill();
            }

            var appSettings = new AppSettingsRepository();
            var result = appSettings.GetSettingsWithResult();
            if (!result.IsError)
            {
                _appSettingsDTO = result.Settings;
            }
            else
            {
                MessageBox.Show($"При считывании файла настроек произошла ошибка!\n Закройте приложение и обратитесь к системному администратору \n ", "Системное сообщение", MessageBoxButtons.OK, MessageBoxIcon.Error);
                _logger.Error($"{result.Message}//{DateTime.Now.ToShortDateString()}-{DateTime.Now.ToShortTimeString()}//");
                throw new Exception();
            }

            var txtParser = new TxtRecipeParser();
            var codeTransformator = new PLCCodeTransformator();
            var pLCCellsChecker = new OmronPLCCellsChecker(_appSettingsDTO.IpAddressPLC);
            var pLCDataTransceiver = new OmronPLCDataTransceiver(_appSettingsDTO.IpAddressPLC);
            var plcDataReciver = new OmronPLCDataReciver(_appSettingsDTO.IpAddressPLC);
            var codesConverter = new CodesConverter();
            var doses = new DoseRepository();
            var reports = new ReportsRepository();
            var reportMaker = new TxtReportMaker();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var form = new Form1(_appSettingsDTO,txtParser, codeTransformator, pLCCellsChecker, pLCDataTransceiver, plcDataReciver, codesConverter, doses, reports, reportMaker);
            form.Text = $"Recipe Manager vers.{2}";
            Application.Run(form);
        }

    }
}
