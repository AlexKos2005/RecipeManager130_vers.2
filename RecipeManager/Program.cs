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

namespace RecipeManager
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            if (Process.GetProcessesByName("RecipeManager").Count() > 1)
            {
                MessageBox.Show("Программа уже запущена!");
                Process.GetCurrentProcess().Kill();
            }
            var nLog = LogManager.GetCurrentClassLogger();
            var txtParser = new TxtRecipeParser();
            var codeTransformator = new PLCCodeTransformator();
            var pLCCellsChecker = new OmronPLCCellsChecker("192.168.0.201");
            var pLCDataTransceiver = new OmronPLCDataTransceiver("192.168.0.201");
            var appSettings = new AppSettingsRepository();
            var plcDataReciver = new OmronPLCDataReciver("192.168.0.201");
            var codesConverter = new CodesConverter();
            var doses = new DoseRepository();
            var reports = new ReportsRepository();
            var reportMaker = new TxtReportMaker();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var form = new Form1(txtParser, codeTransformator, pLCCellsChecker, pLCDataTransceiver, nLog,
                appSettings, plcDataReciver, codesConverter, doses, reports, reportMaker);
            form.Text = $"Recipe Manager vers.{2}";
            Application.Run(form);
        }
    }
}
