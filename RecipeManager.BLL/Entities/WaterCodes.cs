using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeManager.BLL.Entities
{
   public class WaterCodes
    {
        public short Comp_Code { get; set; }
        public List<short> Comp_NameCodes { get; set; } = new List<short>();
        public short Comp_Weight_LowWord { get; set; }
        public short Comp_Weight_HighWord { get; set; }
        public short Comp_Temper { get; set; }
    }
}
