using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RecipeManager.Core.Entities;
using RecipeManager.DAL.DTO;

namespace RecipeManager.DAL.Extentions
{
   public static class AppSettingsDtoExtention
    {
        public static AppSettings ToModel(this AppSettingsDTO settings)
        {
            return new AppSettings()
            {
                IpAddressPLC = settings.IpAddressPLC,
             
            };
        }

        public static AppSettingsDTO FromModel(this AppSettings settings)
        {
            return new AppSettingsDTO()
            {
                IpAddressPLC = settings.IpAddressPLC,
               

            };
        }
    }
}
