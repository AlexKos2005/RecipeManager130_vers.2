using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RecipeManager.DAL.DTO;

namespace RecipeManager.DAL.Interfaces
{
   public interface IReportsRepository
    {
        (bool IsError, string Message) SetReportInfoWithResult(ReportDto reportDto);

        (bool IsError, string Message) ClearDbByDatesWithResult(DateTime date);

        (bool IsError, string Message,ReportDto report) FindReportByDateAndTimeWithResult(DateTime date,TimeSpan time);
    }
}
