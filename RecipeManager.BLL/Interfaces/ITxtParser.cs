using System.Collections.Generic;
using RecipeManager.Core.Entities;

namespace RecipeManager.BLL.Interfaces
{
   public interface ITxtParser
    {
        (bool IsError, string Message,List<Recipe> recipes) ParseTxtWithResult(string filePath);
    }
}
