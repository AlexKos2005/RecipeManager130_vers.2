
using System.Collections.Generic;
using RecipeManager.BLL.Entities;


namespace RecipeManager.BLL.Entities
{
   public class RecipeCodes
    {
        public List<short> RecipeNameCodes { get; set; }
        public short ProductCount_LowWord { get; set; }
        public short ProductCount_HighWord { get; set; }

        public List<ComponentCodes> ComponentsCodes { get; set; } = new List<ComponentCodes>();

        public WaterCodes WaterCodes { get; set; } = new WaterCodes();
    
    }
}
