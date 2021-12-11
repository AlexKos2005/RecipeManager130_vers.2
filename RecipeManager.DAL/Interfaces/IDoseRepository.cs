using RecipeManager.DAL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeManager.DAL.Interfaces
{
    public interface IDoseRepository
    {
        (bool IsError, string Message) SetDosesDtoWithResult(List<DoseDto> dosesDto);
        (bool IsError, string Message, List<DoseDto> dosesDto) GetDosesDtoByDateTimeWithResult(DateTime startDate, TimeSpan startTime, DateTime endDate, TimeSpan endTime);
    }
}
