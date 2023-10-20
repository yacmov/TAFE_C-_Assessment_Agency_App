using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAFE_ICT40120_Assessment02.Enums;

namespace TAFE_ICT40120_Assessment02.Classes
{
    /// <summary>
    /// This is Contractors handling class 
    /// </summary>
    public class Contractor
    {
        private string firstName;
        private string lastName;
        private double hourlyWage;


        public LEVEL Level { get; set; }
        public static int CountID { get; set; } = 1;
        public int Id { get; set; } = 0;
        public int NumberOfCompletedJob { get; set; } = 0;
        public string FullName { get; set; }
        public string FirstName
        {
            get
            {
                return firstName;
            }

            set
            {
                if (ErrorCheck.CheckValidString(value)) firstName = value;
            }
        }
        public string LastName
        {
            get
            {
                return lastName;
            }

            set
            {
                if (ErrorCheck.CheckValidString(value))
                {
                    lastName = value;

                }
            }
        }
        public DateTime StartDate { get; set; }
        public int StartDateDay { get; set; }
        public int StartDateMonth { get; set; }
        public int StartDateYear { get; set; }
        public double HourlyWage
        {
            get
            {
                return hourlyWage;
            }

            set
            {
                hourlyWage = ErrorCheck.CheckContractorWageValue(value);
            }
        }

        public CONTRACTOR_STATUS Status { get; set; }
        public Job? JobAssigned { get; set; }

        public Contractor()
        {
            Id = Id += CountID;
            CountID++;
        }

        /// <summary>
        /// Contractor constructor
        /// </summary>
        /// <param name="_level"></param>
        /// <param name="_firstName"></param>
        /// <param name="_lastName"></param>
        /// <param name="_dateTime"></param>
        /// <param name="_hourlyWage"></param>
        /// <param name="_state"></param>
        public Contractor(LEVEL _level, string _firstName, string _lastName, DateTime _dateTime, double _hourlyWage, CONTRACTOR_STATUS _state)
        {
            Level = _level;
            Id = Id += CountID;
            CountID++;
            FullName = _firstName + " " + _lastName;
            FirstName = _firstName;
            LastName = _lastName;
            try
            {
                StartDate = _dateTime;
            }
            catch
            {
                StartDate = DateTime.Today;
            }

            StartDateDay = StartDate.Day;
            StartDateMonth = StartDate.Month;
            StartDateYear = int.Parse(StartDate.Year.ToString().Substring(2));
            HourlyWage = _hourlyWage;
            Status = _state;
        }


        public override string ToString()
        {
            int count = 12;
            if (FullName.Length >= count)
            {
                FullName = $"{FullName.Substring(0, count)}...";
            }
            string jobAssigned;
            if (JobAssigned == null)
            {
                jobAssigned = $"{"",-25}\t "; ;
            }
            else
            {
                string titleFix = $"{JobAssigned.Level} {JobAssigned.Title.ToString(),-25}\t";
                jobAssigned = $"{titleFix.Substring(0, 21)}\t ";
            }
            return $"[{Id.ToString("000.###")}]  {Level},  {FullName,-14}\t |{"",2}{StartDate.ToString("dd/MM/yy"),-12}|{"",6}$ {HourlyWage.ToString("F2"),-7}  |{"",3}{Status,-7}\t | {"",3}{jobAssigned} |";

        }
    }
}
