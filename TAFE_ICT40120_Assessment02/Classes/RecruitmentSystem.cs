using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using TAFE_ICT40120_Assessment02.Classes;
using TAFE_ICT40120_Assessment02.Enums;
using static System.Reflection.Metadata.BlobBuilder;


namespace TAFE_ICT40120_Assessment02.Classes
{
    public class RecruitmentSystem
    {
        public List<Contractor> contractors;
        public List<Job> jobs;
        public List<Completed> completed;
        public List<Cancel> cancelJobs;
        public NoticeBoard noticeBoard;

        public int MinimumSearchCost { get; set; }
        public int MaximumSearchCost { get; set; }
        public static double minimumContractorWage = 30;
        public static double maximumContractorWage = 60;
        public static int minimumJobCost = 100;
        public static int maximumJobCost = 3000;
        public static int minimumYear = 2020;
        public static int currentYear = DateTime.Now.Year;

        public RecruitmentSystem()
        {

        }

        /// <summary>
        /// Main RecruitmentSystem Controlling Class 
        /// </summary>
        /// <param name="_enableDummyData"></param>
        public RecruitmentSystem(bool _enableDummyData)
        {
            if (_enableDummyData == false) return;
            int contractorAvailable = 10;
            int contractorHoliday = 2;
            int ContractorETC = 1;
            int JobPending = 10;
            int JobCancel = 5;
            int JobCompleted = 2;
            DummyData dummyData = new DummyData();
            contractors = dummyData.LoadDummyContractors(contractorAvailable, contractorHoliday, ContractorETC);
            jobs = dummyData.LoadDummyJobs(JobPending, JobCancel);
            completed = dummyData.LoadDummyCompletedJobs(contractors, jobs, JobCompleted);
            cancelJobs = new List<Cancel>();
            dummyData.GetDummyJobCancel(jobs, cancelJobs);

            noticeBoard = new NoticeBoard();
        }

        /// <summary>
        /// Contractor add to selected List
        /// </summary>
        /// <param name="_contractors"></param>
        /// <param name="_newContractor"></param>
        /// <returns></returns>
        public bool AddContractor(List<Contractor> _contractors, Contractor _newContractor)
        {
            Contractor newContractor = _newContractor;
            if (!ErrorCheck.CheckValidString(newContractor.FirstName))
            {
                MessageBox.Show("Check Your FirstName");
                return false;
            }
            else if (!ErrorCheck.CheckValidString(newContractor.LastName))
            {
                MessageBox.Show("Check Your LastName");
                return false;
            }
            else if (!ErrorCheck.CheckComboBoxValidDate(newContractor.StartDateDay, newContractor.StartDateMonth, newContractor.StartDateYear))
            {
                MessageBox.Show("Invalid Date");
                return false;
            }
            _contractors.Add(_newContractor);
            return true;
        }

        /// <summary>
        /// Selected Contractor removed from selected List
        /// </summary>
        /// <param name="_contractors"></param>
        /// <param name="_selectedContractor"></param>
        public void RemoveContractor(List<Contractor> _contractors, Contractor _selectedContractor)
        {
            {
                if (!ErrorCheck.CheckIsNotNull(_selectedContractor))
                {
                    MessageBox.Show("Select Contractor");
                    return;
                }
                _contractors.Remove(_selectedContractor);

            }
        }


        /// <summary>
        /// Add jobs into selected List
        /// </summary>
        /// <param name="_jobs"></param>
        /// <param name="_newJob"></param>
        public void AddJob(List<Job> _jobs, Job _newJob)
        {
            Job newJob = _newJob;
            if (!ErrorCheck.CheckValidString(newJob.Title))
            {
                MessageBox.Show("Check Your Title");
                return;
            }
            else if (!ErrorCheck.CheckComboBoxValidDate(newJob.DeadLineDay, newJob.DeadLineMonth, newJob.DeadLineYear))
            {
                MessageBox.Show("Invalid Date");
                return;
            }
            _jobs.Add(newJob);
        }

        /// <summary>
        /// selected job removed from selected List
        /// </summary>
        /// <param name="_jobs"></param>
        /// <param name="_selectedJob"></param>
        public void RemoveJob(List<Job> _jobs, Job _selectedJob)
        {
            if (!ErrorCheck.CheckIsNotNull(_selectedJob))
            {
                MessageBox.Show("Select Job");
                return;
            }
            _jobs.Remove(_selectedJob);
        }


        /// <summary>
        /// cooperate 2 class between job and contractor each of them able to access each other objects 
        /// </summary>
        /// <param name="_contractors"></param>
        /// <param name="_newJob"></param>
        /// <param name="_comboBoxJobStatus"></param>
        /// <param name="_comboBoxJobContractor"></param>
        public bool AssignJob(Contractor _selectedContractors, Job _newJob, string _comboBoxJobStatus)
        {
            if (_comboBoxJobStatus != JOB_STATUS.Assigned.ToString()) return false;
            if (!ErrorCheck.CheckComparingLevelBetween(_newJob, _selectedContractors))
            {
               return false;
            }
            _newJob.ContractorAssigned = _selectedContractors;
            _selectedContractors.JobAssigned = _newJob;
            if (!_selectedContractors.FullName.ToUpper().Contains("TEST"))
            {
                MessageBox.Show($"Job assigned by\n[ {_newJob.ContractorAssigned.Level} {_newJob.ContractorAssigned.FullName} ]");
            }


            _newJob.ContractorAssigned.Status = CONTRACTOR_STATUS.Assigned;
            _newJob.ContractorAssigned.JobAssigned = _newJob;
            _newJob.Status = JOB_STATUS.Assigned;
            return true;
        }



        /// <summary>
        /// Move selected object move to selected list as complted
        /// </summary>
        /// <param name="_completed"></param>
        /// <param name="_newJob"></param>
        public void CompleteJob(List<Completed> _completed, Job _newJob)
        {
            Completed newCompleted = new Completed(_newJob.ContractorAssigned, _newJob);
            Job completedJob = _newJob;
            if (completedJob.ContractorAssigned == null) return;
            completedJob.ContractorAssigned.Status = CONTRACTOR_STATUS.Available;
            completedJob.ContractorAssigned.JobAssigned = null;
            completedJob.ContractorAssigned = null;
            completedJob.Status = JOB_STATUS.Completed;

            _completed.Add(newCompleted);
            _completed[_completed.Count - 1].CompletedBy.NumberOfCompletedJob += 1;

        }


        /// <summary>
        /// selected job change to cancel then move to canceled class 
        /// </summary>
        /// <param name="_newJob"></param>
        public void CancelJob(Job _newJob)
        {
            if (_newJob.Status != JOB_STATUS.CANCEL) return;
            Cancel newCancel = new Cancel();
            _newJob.Status = JOB_STATUS.CANCEL;
            if (_newJob.ContractorAssigned != null)
            {
                _newJob.ContractorAssigned.JobAssigned = null;
            }
            _newJob.ContractorAssigned = null;
            newCancel.cancelJob = _newJob;
            cancelJobs.Add(newCancel);
            newCancel.SetCancelValue();
        }


        public Contractor GetSelectedContractor(List<Contractor> _contractors, string _comboBoxJobContractor)
        {
            for (int index = _contractors.Count - 1; index >= 0; index--)
            {
                if ($"{_contractors[index].Level} {_contractors[index].FullName}" == _comboBoxJobContractor)
                {
                   
                    return _contractors[index];
                }
            }
            return null;
        }


        /// <summary>
        /// sort class based on the input
        /// </summary>
        /// <param name="_stringSortDetail"></param>
        /// <param name="_mainButtons"></param>
        public void SortClass(string _stringSortDetail, MAINBUTTONS _mainButtons)
        {
            MAINBUTTONS mainButtons = _mainButtons;
            switch (mainButtons)
            {
                case MAINBUTTONS.NOTICE:
                    break;
                case MAINBUTTONS.CONTRACTOR:
                    if (_stringSortDetail.Equals(SORT_CONTRACTOR_BY_DETAILS.By_Id.ToString()))
                    {
                        contractors = contractors.OrderBy(x => x.Id).ToList();
                    }
                    else if (_stringSortDetail.Equals(SORT_CONTRACTOR_BY_DETAILS.By_Level.ToString()))
                    {
                        contractors = contractors.OrderByDescending(x => x.Level).ThenByDescending(x => x.HourlyWage).ToList();
                    }
                    else if (_stringSortDetail.Equals(SORT_CONTRACTOR_BY_DETAILS.By_Wage.ToString()))
                    {
                        contractors = contractors.OrderBy(x => x.HourlyWage).ToList();
                    }
                    break;
                case MAINBUTTONS.JOB:
                    if (_stringSortDetail.Equals(SORT_JOB_BY_DETAILS.By_Due.ToString()))
                    {
                        jobs = jobs.OrderBy(x => x.DeadLineYear).ThenBy(y => y.DeadLineMonth).ThenBy(z => z.DeadLineDay).ToList();
                    }
                    else if (_stringSortDetail.Equals(SORT_JOB_BY_DETAILS.By_Cost.ToString()))
                    {
                        jobs = jobs.OrderBy(x => x.Cost).ToList();
                    }
                    else if (_stringSortDetail.Equals(SORT_JOB_BY_DETAILS.By_Title.ToString()))
                    {
                        jobs = jobs.OrderBy(x => x.Title).ToList();
                    }
                    else if (_stringSortDetail.Equals(SORT_JOB_BY_DETAILS.By_Assigned.ToString()))
                    {
                        jobs = jobs.OrderBy(x => x.ContractorAssigned).ToList();
                    }
                    else if (_stringSortDetail.Equals(SORT_JOB_BY_DETAILS.By_Status.ToString()))
                    {
                        jobs = jobs.OrderBy(x => x.Status).ToList();
                    }
                    break;
                case MAINBUTTONS.HISTORY:
                    break;
                case MAINBUTTONS.COMPLETED:
                    if (_stringSortDetail.Equals(SORT_HISTORY_BY_DETAILS.Default.ToString()))
                    {
                        completed = completed.OrderByDescending(x => x.CompletedJob.Id).ToList();
                    }
                    if (_stringSortDetail.Equals(SORT_HISTORY_BY_DETAILS.By_Date.ToString()))
                    {
                        completed = completed.OrderBy(x => x.CompletedTimeYear).ThenBy(y => y.CompletedTimeMonth).ThenBy(z => z.CompletedTimeDay).ToList();
                    }
                    else if (_stringSortDetail.Equals(SORT_HISTORY_BY_DETAILS.By_Cost.ToString()))
                    {
                        completed = completed.OrderByDescending(x => x.CompletedJob.Cost).ToList();
                    }
                    else if (_stringSortDetail.Equals(SORT_HISTORY_BY_DETAILS.By_Title.ToString()))
                    {
                        completed = completed.OrderBy(x => x.CompletedJob.Title).ToList();

                    }
                    break;
                case MAINBUTTONS.CANCEL:
                    if (_stringSortDetail.Equals(SORT_HISTORY_BY_DETAILS.Default.ToString()))
                    {
                        cancelJobs =  cancelJobs.OrderByDescending(x => x.Id).ToList();
                    }
                    if (_stringSortDetail.Equals(SORT_HISTORY_BY_DETAILS.By_Date.ToString()))
                    {
                        cancelJobs = cancelJobs.OrderByDescending(x => x.DeadLineYear).ThenByDescending(y => y.DeadLineMonth).ThenByDescending(z => z.DeadLineDay).ToList();
                    }
                    else if (_stringSortDetail.Equals(SORT_HISTORY_BY_DETAILS.By_Cost.ToString()))
                    {
                        cancelJobs = cancelJobs.OrderByDescending(x => x.Cost).ToList();
                    }
                    else if (_stringSortDetail.Equals(SORT_HISTORY_BY_DETAILS.By_Title.ToString()))
                    {
                        cancelJobs = cancelJobs.OrderBy(x => x.Title).ToList();

                    }
                    break;
            }

        }

        /// <summary>
        /// based on the number will calculate which level is it. 
        /// </summary>
        /// <param name="_number"></param>
        /// <param name="_classNames"></param>
        /// <returns></returns>
        public LEVEL LevelCalculator(double _number, MAINBUTTONS _classNames)
        {
            int level2MinimumWage = 40;
            int level2MaximumWage = 49;
            int level3MinimumWage = 50;

            int level2MinimumCost = 1000;
            int level2MaximumCost = 1999;
            int level3MinimumCost = 2000;

            MAINBUTTONS classNames = _classNames;
            switch (classNames)
            {
                case MAINBUTTONS.CONTRACTOR:
                    if (level2MinimumWage <= _number && _number <= level2MaximumWage) return LEVEL.L2;
                    else if (level3MinimumWage <= _number) return LEVEL.L3;
                    return LEVEL.L1;

                case MAINBUTTONS.JOB:
                    if (level2MinimumCost <= _number && _number <= level2MaximumCost) return LEVEL.L2;
                    else if (level3MinimumCost <= _number) return LEVEL.L3;
                    return LEVEL.L1;

                default: return LEVEL.L1;
            }

        }

    }
}
