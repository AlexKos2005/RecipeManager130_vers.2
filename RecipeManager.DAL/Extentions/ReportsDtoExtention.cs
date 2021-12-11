using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RecipeManager.Core.Entities;
using RecipeManager.DAL.DTO;

namespace RecipeManager.DAL.Extentions
{
   public static class ReportsDtoExtention
    {
        public static Report ToModel (this ReportDto reportDto)
        {
           return  new Report()
            {
                ReportDate = reportDto.ReportDate,
                ReportTime = reportDto.ReportTime,
                ReportingFlag = reportDto.ReportingFlag,
                ReportingMessage = reportDto.ReportingMessage

            };
        }

        public static ReportDto FromModel(this Report reportDto)
        {
            return new ReportDto()
            {
                ReportDate = reportDto.ReportDate,
                ReportTime = reportDto.ReportTime,
                ReportingFlag = reportDto.ReportingFlag,
                ReportingMessage = reportDto.ReportingMessage

            };
        }
    }
}
