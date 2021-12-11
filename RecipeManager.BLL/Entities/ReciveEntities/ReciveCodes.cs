using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeManager.BLL.Entities.ReciveEntities
{
    public class ReciveCodes
    {
        public List<short> YearMonth { get; set; } = new List<short>();
        public List<short> DayHour { get; set; } = new List<short>();
        public List<short> MinuteSecond { get; set; } = new List<short>();
        public List<short> RecipesNames { get; set; } = new List<short>();
        public List<short> ComponentsCodes { get; set; } = new List<short>();

        public List<short> ComponentsWeights { get; set; } = new List<short>();
        public List<short> WaterWeights { get; set; } = new List<short>();
        public List<short> WaterTempers { get; set; } = new List<short>();
    }
}
