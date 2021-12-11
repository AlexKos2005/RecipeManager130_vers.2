using System;
using System.Collections.Generic;
using RecipeManager.BLL.Entities;

namespace RecipeManager.BLL.Interfaces
{
   public interface IPLCCellsChecker 
    {
        (bool IsError, string Message, List<CellPLC> Cells) GetCellStatusByRangeWithResult(List<CellPLC> Cells);
    }
}
