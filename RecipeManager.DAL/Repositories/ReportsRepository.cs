using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RecipeManager.DAL.Interfaces;
using RecipeManager.DAL.DataContext;
using RecipeManager.DAL.DTO;
using RecipeManager.DAL.Extentions;
using RecipeManager.Core.Entities;


namespace RecipeManager.DAL.Repositories
{
    public class ReportsRepository : IReportsRepository
    {
        private DbReportsContext _db;
        public (bool IsError, string Message) ClearDbByDatesWithResult(DateTime date)
        {
            _db = new DbReportsContext();

            try
            {
                return (false, "");
            }
            catch(Exception e)
            {
                return (true, e.Message);
            }
            finally
            {
                
            }
        }

        public (bool IsError, string Message) SetReportInfoWithResult(ReportDto reportDto)
        {
            _db = new DbReportsContext();

            try
            {
                _db.Reports.Add(ReportsDtoExtention.ToModel(reportDto));
                _db.SaveChanges();
                return (false, "");
            }
            catch (Exception e)
            {
                return (true, e.Message);
            }
            finally
            {
                _db?.Dispose();
            }
        }

        public (bool IsError, string Message,ReportDto report) FindReportByDateAndTimeWithResult(DateTime date, TimeSpan time)
        {
            _db = new DbReportsContext();

            try
            {
                var result = _db.Reports.Where(p => p.ReportDate == date && p.ReportTime == time).Select(c => ReportsDtoExtention.FromModel(c)).FirstOrDefault();
                return (false, "",result);
            }
            catch (Exception e)
            {
                return (true, e.Message,null);
            }
            finally
            {
                _db?.Dispose();
            }
        }
    }
}
