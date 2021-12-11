using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeManager.BLL.Entities.ReciveEntities
{
   public class ReciveWaterCodes
    {
        public List<short> WaterCode { get; set; } = new List<short>();
        public List<short> WaterWeight { get; set; } = new List<short>();
        public List<short> WaterTemper { get; set; } = new List<short>();
    }
}

