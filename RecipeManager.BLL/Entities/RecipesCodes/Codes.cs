using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeManager.BLL.Entities.RecipesCodes
{
    public class Codes
    {
        public List<RecipeNameCodes> RecipesNameCodes { get; set; } = new List<RecipeNameCodes>();
        public List<RecipeWeightCodes> RecipesWeightCodes { get; set; } = new List<RecipeWeightCodes>();

        public List<ComponentCodeCodes> ComponentsCodeCodes { get; set; } = new List<ComponentCodeCodes>();
        public List<ComponentNameCodes> ComponentsNameCodes { get; set; } = new List<ComponentNameCodes>();

        public List<ComponentWeightCodes> ComponentsWeightCodes { get; set; } = new List<ComponentWeightCodes>();

        public List<WaterWeightCodes> WaterWeightCodes { get; set; } = new List<WaterWeightCodes>();

        public List<WaterTempCodes> WaterTempCodes { get; set; } = new List<WaterTempCodes>();


    }
}
