using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeManager.BLL.Entities
{
    public class CellPLC
    {
        public short CellAddress { get; set; }
        public bool IsEmpty { get; set; }
    }
}
