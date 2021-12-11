using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RecipeManager.DAL.DTO;

namespace RecipeManager.BLL.Interfaces
{
   public interface IReportMaker
    {
        (bool IsError, string Message) MakeReport(string filePath, string fileName,List<DoseDto> doses);
    }
}
