using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics.X86;
using System.Security.Claims;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Shapes;
using System.Xml.Linq;
using TAFE_ICT40120_Assessment02.Classes;
using TAFE_ICT40120_Assessment02.Enums;

namespace TAFE_ICT40120_Assessment02.Classes
{
    /// <summary>
    /// This will create random DummyData for testing this program.
    /// </summary>
    public class DummyData
    {
        RecruitmentSystem rs = new RecruitmentSystem();
        Random random = new Random();


        string filePathNames = "..\\..\\..\\Sources\\ContractorNames.txt";
        string filePathJobs = $"..\\..\\..\\Sources\\JobTitles.txt";
       

        /// <summary>
        /// reading information from file path located with Sln file as source folder
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public List<string> ReadFile(string filePath)
        {

            List<string> result = new List<string>();
            StreamReader sr = new StreamReader(filePath);

            while (sr.Peek() != -1)
            {
                result.Add(sr.ReadLine());
            }

            return result;
        }

        /// <summary>
        /// Load to List<Contractor> dummy data as 3 different types 
        /// </summary>
        /// <param name="_numberOfAvailable"></param>
        /// <param name="_numberOfHoliday"></param>
        /// <param name="_numberOfETC"></param>
        /// <returns></returns>
        public List<Contractor> LoadDummyContractors(int _numberOfAvailable, int _numberOfHoliday, int _numberOfETC)
        {
            if (_numberOfAvailable <= 0) _numberOfAvailable = 1;
            if (_numberOfHoliday <= 0) _numberOfHoliday = 1;
            if (_numberOfETC <= 0) _numberOfETC = 1;
            List<Contractor> result = new List<Contractor>();
            for (int numberOfAvailable = 0; numberOfAvailable < _numberOfAvailable; numberOfAvailable++)
            {
                Contractor dummyContractor = GetDummyContractor(CONTRACTOR_STATUS.Available);
                result.Add(dummyContractor);
            }

            for (int numberOfAvailable = 0; numberOfAvailable < _numberOfHoliday; numberOfAvailable++)
            {
                Contractor dummyContractor = GetDummyContractor(CONTRACTOR_STATUS.Holiday);
                result.Add(dummyContractor);
            }

            for (int numberOfAvailable = 0; numberOfAvailable < _numberOfETC; numberOfAvailable++)
            {
                Contractor dummyContractor = GetDummyContractor(CONTRACTOR_STATUS.ETC);
                result.Add(dummyContractor);
            }

            return result;
        }

        /// <summary>
        /// Load to List<Job> dummy data as 2 different types
        /// </summary>
        /// <param name="_numberOfPending"></param>
        /// <param name="_numberOfCancel"></param>
        /// <returns></returns>
        public List<Job> LoadDummyJobs(int _numberOfPending, int _numberOfCancel)
        {
            if (_numberOfPending <= 0) _numberOfPending = 1;
            if (_numberOfCancel <= 0) _numberOfCancel = 1;

            List<Job> result = new List<Job>();
            for (int numberOfPending = 0; numberOfPending < _numberOfPending; numberOfPending++)
            {
                Job dummyJob = GetDummyJob(JOB_STATUS.Pending);
                result.Add(dummyJob);
            }

            for (int numberOfCancel = 0; numberOfCancel < _numberOfCancel; numberOfCancel++)
            {
                Job dummyJob = GetDummyJob(JOB_STATUS.CANCEL);
                result.Add(dummyJob);
            }

            return result;
        }

        /// <summary>
        /// Load to List<Completed> dummy data based on dummy jobs and dummy contractors
        /// </summary>
        /// <param name="_contractors"></param>
        /// <param name="_jobs"></param>
        /// <param name="_numberToCreated"></param>
        /// <returns></returns>
        public List<Completed> LoadDummyCompletedJobs(List<Contractor> _contractors, List<Job> _jobs, int _numberToCreated)
        {
            List<Completed> completedJobs = new List<Completed>();
            for (int i = 0; i <= _numberToCreated; i++)
            {
                int randomIndex = random.Next(1, _contractors.Count -1);
                Job job = _jobs[i];

                Contractor contractor = _contractors[randomIndex];
                while (Convert.ToInt32(contractor.Level.ToString().Substring(1)) < Convert.ToInt32(job.Level.ToString().Substring(1)))
                {
                    randomIndex = random.Next(1, _contractors.Count - 1);
                    contractor = _contractors[randomIndex];
                }
                Completed newDummyCompletedJob = new Completed(contractor, job);
                completedJobs.Add(newDummyCompletedJob);
                completedJobs[completedJobs.Count - 1].CompletedBy.NumberOfCompletedJob += 1;


            }
            return completedJobs;
        }

        
        private Contractor GetDummyContractor(CONTRACTOR_STATUS _status)
        {
            List<string> readFile = ReadFile(filePathNames);
            string firstName = readFile[random.Next(0, readFile.Count - 1)];
            string lastName = readFile[random.Next(0, readFile.Count - 1)];
            while (firstName == lastName)
            {
                lastName = readFile[random.Next(0, readFile.Count - 1)];
            }
            int startDay = random.Next(1, 28);
            int startMonth = random.Next(1, 12);
            int startYear = random.Next(RecruitmentSystem.minimumYear, RecruitmentSystem.currentYear);
            double hourlyWage = random.Next(30, 60);
            LEVEL level = rs.LevelCalculator(hourlyWage, MAINBUTTONS.CONTRACTOR);


            Contractor newDummyContractor = new Contractor(level, firstName, lastName, new DateTime(startYear, startMonth, startDay), hourlyWage, _status);
            return newDummyContractor;
        }

        private Job GetDummyJob(JOB_STATUS _status)
        {
            List<string> readfile = ReadFile(filePathJobs);
            string title = readfile[random.Next(0, readfile.Count - 1)];
            int deadLineDay = random.Next(1, 28);
            int deadLineMonth = random.Next(1, 12);
            int deadLineYear = random.Next(RecruitmentSystem.minimumYear, RecruitmentSystem.currentYear);
            double cost = random.Next(100, 3000) / 100;
            cost = Convert.ToDouble(Math.Ceiling(cost) * 100);
            LEVEL level = rs.LevelCalculator(cost, MAINBUTTONS.JOB);

            Job newDummyJob = new Job(level, title, new DateTime(deadLineYear, deadLineMonth, deadLineDay), cost, _status);
            return newDummyJob;
        }


       
        public void GetDummyJobCancel(List<Job> _jobs, List<Cancel> _listCancelJob)
        {
            for (int index = _jobs.Count - 1; index > 0; index--)
            {
                if (_jobs[index].Status == JOB_STATUS.CANCEL)
                {
                    Cancel newCancelJob = new Cancel();
                    newCancelJob.cancelJob = _jobs[index];
                    _jobs.Remove(_jobs[index]);
                    _listCancelJob.Add(newCancelJob);
                    newCancelJob.SetCancelValue();
                } 
            }
        }
    }
}
