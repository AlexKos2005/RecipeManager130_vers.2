using System.Collections.Generic;
using RecipeManager.BLL.Entities;
using RecipeManager.BLL.Entities.RecipesCodes;

namespace RecipeManager.BLL.Interfaces
{
   public interface IPLCDataTransceiver
    {
        (bool IsError, string Message) SetDataWithResult(List<List<short>> codesRecipe, List<Codes> codes, List<CellPLC> cellsPLC,short statusValue);
    }
}
