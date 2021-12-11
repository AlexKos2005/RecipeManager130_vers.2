using System.Collections.Generic;
using RecipeManager.Core.Entities;
using RecipeManager.BLL.Entities;
using RecipeManager.BLL.Entities.RecipesCodes;
using RecipeManager.BLL.Entities.ReciveEntities;
using RecipeManager.DAL.DTO;

namespace RecipeManager.BLL.Interfaces
{
   public interface ICodesTransformator
    {
        (bool IsError, string Message,List<RecipeCodes> codesRecipes, List<Codes> codes) GetCodesWithResult(List<Recipe> recipes);

        (bool IsError, string Message, List<DoseDto> doses) GetRecipesWithResult(ReciveCodes codes);
    }
}
