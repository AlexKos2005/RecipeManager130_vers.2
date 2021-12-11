using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RecipeManager.BLL.Entities.ReciveEntities;
using RecipeManager.DAL.DTO;

namespace RecipeManager.BLL.Interfaces
{
   public interface IArrayToCodesConverter
    {
        (bool IsError, string Message, List<DoseDto> Doses) ConvertArraysCodes(List<short[]> Codes);
    }
}
