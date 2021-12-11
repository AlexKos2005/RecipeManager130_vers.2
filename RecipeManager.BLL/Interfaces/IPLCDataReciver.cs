using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RecipeManager.BLL.Entities.ReciveEntities;

namespace RecipeManager.BLL.Interfaces
{
   public interface IPLCDataReciver
    {
        (bool IsError,string Message, ReciveCodes CodesArray) GetDataWithResult();
        (bool IsError, string Message) ClearMemory();
    }
}
