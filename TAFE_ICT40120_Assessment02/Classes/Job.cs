using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TAFE_ICT40120_Assessment02.Enums;

namespace TAFE_ICT40120_Assessment02.Classes
{
    /// <summary>
    /// Job handling class
    /// This is primary Job class without this recruitmentSystem will not work. 
    /// </summary>
    public class Job
    {
        private string title;
        private DateTime deadline;
        private double cost;
        public LEVEL Level { get; set; }
        public static int CountID { get; set; } = 1;
        public int Id { get; set; } = 0;

        public string Title 
        { 
            get
            {
                return title;
            }
            set
            {
                if (ErrorCheck.CheckValidString(value)) title = value;
            }
        }
        public string TitleShort { get; set; }
        public DateTime DeadLine 
        { 
            get
            { return deadline; }
            set
            {
                try
                {
                    if (ErrorCheck.CheckValidDate(value)) deadline = value;
                }
                catch { }
            }
        }
        public int DeadLineDay { get; set; }
        public int DeadLineMonth { get; set; }
        public int DeadLineYear { get; set; }

        public double Cost 
        { 
            get
            {
                return cost;
            }
            set
            {
                cost = ErrorCheck.CheckJobCostValue(value);
            } 
        }
        public JOB_STATUS Status { get; set; }
        public Contractor? ContractorAssigned { get; set; }

        public Job() 
        {
            Id = Id + CountID;
            CountID++;
        }

        /// <summary>
        /// Constructor for Job class 
        /// </summary>
        /// <param name="_level"></param>
        /// <param name="_title"></param>
        /// <param name="_deadLine"></param>
        /// <param name="_cost"></param>
        /// <param name="_status"></param>
        public Job(LEVEL _level, string _title, DateTime _deadLine, double _cost, JOB_STATUS _status)
        {
            
            Level = _level;
            Id = Id + CountID;
            CountID++;
            Title = _title;
            DeadLine = _deadLine;
            DeadLineDay = DeadLine.Day;
            DeadLineMonth = DeadLine.Month;
            DeadLineYear = int.Parse(DeadLine.Year.ToString().Substring(2));
            
            Cost = _cost;
            Status = _status;
            
        }


        /// <summary>
        /// This will display when it called as toString() at LisView
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string printAssignedContractor = "";
            if (ContractorAssigned != null)
            {
                printAssignedContractor = $"{ContractorAssigned.Level} {ContractorAssigned.FullName}";
            }
            int count = 8;
            if (Title.Length >= count)
            {
                TitleShort = $"{Title.Substring(0, count)}...";
            }

            return $" {Level}, {TitleShort,-8}\t{DeadLine.ToString("dd/MM/yy")}  ${Cost.ToString("000.###")}\t{Status,-10}\t{printAssignedContractor}";
        }
    }
}