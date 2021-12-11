using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RecipeManager.BLL.Entities.ReciveEntities;

namespace RecipeManager.BLL.Entities.ReciveEntities
{
    public class ReciveRecipeCodes
    {
       public List<short> Date { get; set; } = new List<short>();
       public List<short> Time { get; set; } = new List<short>();
       public List<short> RecipeName { get; set; } = new List<short>();

       public List<ReciveComponentCodes> ComponentCodes { get; set; } = new List<ReciveComponentCodes>();
        public ReciveWaterCodes WaterCodes { get; set; } = new ReciveWaterCodes();
    }
}
