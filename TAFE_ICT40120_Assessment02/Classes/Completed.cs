using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using TAFE_ICT40120_Assessment02.Classes;
using TAFE_ICT40120_Assessment02.Enums;

namespace TAFE_ICT40120_Assessment02.Classes
{
    /// <summary>
    /// Completed job will move to this class for handling
    /// </summary>
    public class Completed
    {
        public Contractor CompletedBy;
        public Job CompletedJob;
        public DateTime CompletedTime { get; set; }
        public int CompletedTimeDay { get; set; }
        public int CompletedTimeMonth { get; set; }
        public int CompletedTimeYear { get; set; }
        
        public Completed(Contractor _completedBy, Job _completedJob)
        {
            CompletedBy = _completedBy;
            CompletedJob = _completedJob;
            CompletedTimeDay = Convert.ToInt32(DateTime.Now.Day);
            CompletedTimeMonth = Convert.ToInt32(DateTime.Now.Month);
            CompletedTimeYear = Convert.ToInt32(DateTime.Now.Year);
            CompletedTime = new DateTime(int.Parse(CompletedTimeYear.ToString().Substring(2)), CompletedTimeMonth, CompletedTimeDay);
        }


        public override string? ToString()
        { 
            
            string printAssignedContractor = $"{CompletedBy.Level} {CompletedBy.FullName}";
            
            int count = 20;
            if (printAssignedContractor.Length > count)
            {
                string printingName = $"{printAssignedContractor.Substring(0, count)}...";
            }
            string printingJob = $"{CompletedJob.Level}, {CompletedJob.Title}";
            if (printingJob.Length > count)
            {
                printingJob = $"{printingJob.Substring(0, count)}...";
            }
            return $" {printingJob,-25}\t{CompletedTime.ToString("dd/MM/yy")}  ${CompletedJob.Cost.ToString("000.###")}\t By {printAssignedContractor}";
        }
    }
}

