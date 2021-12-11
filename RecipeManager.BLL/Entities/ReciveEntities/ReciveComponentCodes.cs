using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeManager.BLL.Entities.ReciveEntities
{
   public class ReciveComponentCodes
    {
        public List<short> CompCode { get; set; } = new List<short>();
        public List<short> CompWeight { get; set; } = new List<short>();
    }
}
