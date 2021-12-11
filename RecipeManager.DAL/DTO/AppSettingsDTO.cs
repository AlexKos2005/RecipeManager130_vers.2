using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeManager.DAL.DTO
{
    public class AppSettingsDTO
    {
        public string IpAddressPLC { get; set; }

        public string SaveReportPath { get; set; }
        public int GetDataCycleTimer { get; set; }

        public string TimeForFirstReport { get; set; }
        public string TimeForSecondReport { get; set; }
    }
}
