using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeManager.Core.Entities
{
   public class Recipe
    {
        public string RecipeName { get; set; }
        public int ProductCount { get; set; }
        public bool DownloadStatusFlag { get; set; }

        public List<Component> Components { get; set; } = new List<Component>();

        public WaterComponent WaterComp { get; set; }


    }
}
