using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RecipeManager.DAL.DTO;

namespace RecipeManager.DAL.Interfaces
{
   public interface IJsonRepository
    {
        (bool IsError, string Message, AppSettingsDTO Settings) GetSettingsWithResult();
        (bool IsError, string Message) SetSettingsWithResult(AppSettingsDTO appSettingsDTO);

    }
}
