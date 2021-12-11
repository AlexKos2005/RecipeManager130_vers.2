using RecipeManager.DAL.DataContext;
using RecipeManager.DAL.DTO;
using RecipeManager.DAL.Extentions;
using RecipeManager.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeManager.DAL.Repositories
{
    public class DoseRepository : IDoseRepository
    {
        private DbDoseContext _dbDose;
        public (bool IsError, string Message, List<DoseDto> dosesDto) GetDosesDtoByDateTimeWithResult(DateTime startDate, TimeSpan startTime, DateTime endDate, TimeSpan endTime)
        {
            _dbDose = new DbDoseContext();
            try
            {

                var result11 = _dbDose.Doses
                            .Where((p => p.Date >= startDate.Date && p.Date <= endDate.Date)).ToList();
                var result12 = result11
                           .Where(p => p.Date == startDate.Date).Where(t => t.Time >= startTime).ToList();

                var result13 = result11
                           .Where(p => p.Date == endDate.Date).Where(t => t.Time <= endTime).ToList();

                result12.AddRange(result13);
                var result3 = result12.OrderBy(p => p.Date).ToList();

                var result4 = result3.Select(c => DoseDtoExtention.FromModel(c)).ToList();


                return (false, "",result4);
            }
            catch (Exception e)
            {
                return (true, e.Message,null);
            }
            finally
            {
                _dbDose.Dispose();
            }
        }

        public (bool IsError, string Message) SetDosesDtoWithResult(List<DoseDto> dosesDto)
        {
            _dbDose = new DbDoseContext();
            try
            {
                _dbDose.AddRange(dosesDto.Select(c => DoseDtoExtention.ToModel(c)));
                _dbDose.SaveChanges();

                return (false, "");
            }
            catch(Exception e)
            {
                return (true, e.Message);
            }
            finally
            {
                _dbDose.Dispose();
            }
        }
    }
}
