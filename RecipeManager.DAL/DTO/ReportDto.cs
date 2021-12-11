using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeManager.DAL.DTO
{
   public class ReportDto
    {
        public int Id { get; set; }
        public DateTime ReportDate { get; set; }
        public TimeSpan ReportTime { get; set; }

        public bool ReportingFlag { get; set; }

        public string ReportingMessage { get; set; }
    }
}
