using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeManager.Core.Entities
{
   public class Report
    {
        [Key]

        public int Id { get; set; }
        public DateTime ReportDate { get; set; }
        public TimeSpan ReportTime { get; set; }

        public bool ReportingFlag { get; set; }

        public string ReportingMessage { get; set; }
    }
}
