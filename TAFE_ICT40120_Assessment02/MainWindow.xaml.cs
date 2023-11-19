using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TAFE_ICT40120_Assessment02.Classes;
using TAFE_ICT40120_Assessment02.Enums;
using TAFE_ICT40120_Assessment02;
using System.Threading;

namespace TAFE_ICT40120_Assessment02
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        RecruitmentSystem reSystem = new RecruitmentSystem(true);
        Contractor selectedContractor;
        Job selectedJob;

        private const string versionControl = "v1.0";
        public Brush SelectedButtonColour { get; set; } = Brushes.LightSkyBlue;
        public Brush UnSelectedButtonColour { get; set; } = Brushes.AliceBlue;
        public Visibility SelectedView { get; set; } = Visibility.Visible;
        public Visibility UnSelectedView { get; set; } = Visibility.Collapsed;
        int insertIndex;
        public MainWindow()
        {
            InitializeComponent();
            reSystem.MinimumSearchCost = Convert.ToInt32(SliderJobCost.Minimum);
            reSystem.MaximumSearchCost = Convert.ToInt32(SliderJobCost.Maximum);
            reSystem.noticeBoard = new NoticeBoard();
            insertIndex = reSystem.noticeBoard.insertIndex;
            LoadListView(ListViewNotice, MAINBUTTONS.NOTICE, true, false);
            LoadDummyNotice(ListViewNotice, reSystem.completed);
            LoadDummyNotice(ListViewNotice, reSystem.contractors);
            LoadDummyNotice(ListViewNotice, reSystem.jobs);
            LabelMainVersion.Content = versionControl;

        }

        /// <summary>
        /// Get information from UI/UX and try to create contractor
        /// </summary>
        /// <returns></returns>
        private Contractor CreateContractor()
        {
            string firstName = TextBoxFirstName.Text;
            string lastName = TextBoxLastName.Text;
            int day = ErrorCheck.CheckValidNumber(ComboBoxStartDateDay.Text);
            int month = ErrorCheck.CheckValidNumber(ComboBoxStartDateMonth.Text);
            int year = ErrorCheck.CheckValidNumber($"20{ComboBoxStartDateYear.Text}");
            double wage = SliderWage.Value;
            CONTRACTOR_STATUS status = (CONTRACTOR_STATUS)Enum.Parse(typeof(CONTRACTOR_STATUS), ComboBoxContractorStatus.Text);
            LEVEL level = reSystem.LevelCalculator(SliderWage.Value, MAINBUTTONS.CONTRACTOR);
            Contractor newContractor = null;
            if (ErrorCheck.CheckValidDate(year, month, day))
            {
                newContractor = new Contractor(level, firstName, lastName, new DateTime(year, month, day), wage, status);
            }
            return newContractor;
        }


        /// <summary>
        /// Get information from UI/UX and try to create Job
        /// </summary>
        /// <returns></returns>
        private Job CreateJob()
        {
            string title = TextBoxJobTitle.Text;
            int day = ErrorCheck.CheckValidNumber(ComboBoxDeadLineDay.Text);
            int month = ErrorCheck.CheckValidNumber(ComboBoxDeadLineMonth.Text);
            int year = ErrorCheck.CheckValidNumber($"20{ComboBoxDeadLineYear.Text}");
            double cost = SliderJobCost.Value;
            JOB_STATUS status = (JOB_STATUS)Enum.Parse(typeof(JOB_STATUS), ComboBoxJobStatus.Text);
            LEVEL level = reSystem.LevelCalculator(SliderJobCost.Value, MAINBUTTONS.JOB);
            Job newJob = null;

            if (ErrorCheck.CheckValidDate(year, month, day))
            {
                newJob = new Job(level, title, new DateTime(year, month, day), cost, status);
            }
            return newJob;
        }

        private void ClearInformation()
        {
            SetTextBoxDefault();
            SetSliderDefault();
            SetComboBoxDefault(MAINBUTTONS.JOB);
            SetComboBoxDefault(MAINBUTTONS.CONTRACTOR);
            selectedJob = null;


        }

        private void SetSliderDefault()
        {
            int minimumWage = 30;
            int minimumJobCost = 30;
            SliderWage.Value = minimumWage;
            SliderWage.Minimum = RecruitmentSystem.minimumContractorWage;
            SliderWage.Maximum = RecruitmentSystem.maximumContractorWage;
            SliderJobCost.Value = minimumJobCost;
            SliderJobCost.Minimum = RecruitmentSystem.minimumJobCost;
            SliderJobCost.Maximum = RecruitmentSystem.maximumJobCost;
        }

        private void SetTextBoxDefault()
        {
            string empty = "";
            TextBoxFirstName.Text = empty;
            TextBoxLastName.Text = empty;
            TextBoxJobTitle.Text = empty;
        }

        private void SetComboBoxDefault(MAINBUTTONS _mainPages)
        {
            int setDefault = 0;
            int setMinimum = 1;
            int setMaximum = ComboBoxMaxCost.Items.Count - 1;
            int setByID = 1;
            int setAll = 0;
            int setCost = 1;
            int setNormal = 1;
            MAINBUTTONS className = _mainPages;
            switch (className)
            {

                case MAINBUTTONS.NOTICE:
                    ComboBoxNoticeSort.SelectedIndex = setNormal;
                    break;

                case MAINBUTTONS.CONTRACTOR:
                    ComboBoxSortContractorView.SelectedIndex = setAll;
                    ComboBoxSortContractorViewDetail.SelectedIndex = setByID;
                    ComboBoxContractorStatus.SelectedIndex = setDefault;
                    ComboBoxStartDateDay.SelectedIndex = setDefault;
                    ComboBoxStartDateMonth.SelectedIndex = setDefault;
                    ComboBoxStartDateYear.SelectedIndex = setDefault;

                    break;

                case MAINBUTTONS.JOB:
                    ComboBoxSortJobRightView.SelectedIndex = setAll;
                    ComboBoxSortJobRightViewCostAndDueDate.SelectedIndex = setCost;
                    ComboBoxSortJobLeftView.SelectedIndex = setDefault;
                    ComboBoxSortJobRightView.SelectedIndex = setDefault;
                    ComboBoxJobStatus.SelectedIndex = setDefault;
                    ComboBoxMinCost.SelectedIndex = setMinimum;
                    ComboBoxMaxCost.SelectedIndex = setMaximum;
                    ComboBoxDeadLineDay.SelectedIndex = setDefault;
                    ComboBoxDeadLineMonth.SelectedIndex = setDefault;
                    ComboBoxDeadLineYear.SelectedIndex = setDefault;
                    break;

                case MAINBUTTONS.HISTORY:
                    ComboBoxHistoryCompleted.SelectedIndex = setDefault;
                    ComboBoxHistoryCancel.SelectedIndex = setDefault;
                    break;

            }
        }


        /// <summary>
        /// Load list view after search button click for Contractor
        /// </summary>
        /// <param name="_comboBoxSort"></param>
        /// <param name="_listViewTarget"></param>
        /// <param name="_new"></param>
        /// <param name="_detail"></param>
        private void LoadAfterSearch(ComboBox _comboBoxSort, ListView _listViewTarget, CONTRACTOR_STATUS _new, bool _detail)
        {
            if (_comboBoxSort.Text == CONTRACTOR_STATUS.All.ToString())
            {
                LoadListViewFromContractors(_listViewTarget, CONTRACTOR_STATUS.All, _detail);
            }
            if (_comboBoxSort.Text == CONTRACTOR_STATUS.Available.ToString())
            {
                LoadListViewFromContractors(_listViewTarget, CONTRACTOR_STATUS.Available, _detail);
            }
            if (_comboBoxSort.Text == CONTRACTOR_STATUS.Assigned.ToString())
            {
                LoadListViewFromContractors(_listViewTarget, CONTRACTOR_STATUS.Assigned, _detail);
            }
            if (_comboBoxSort.Text == CONTRACTOR_STATUS.Holiday.ToString())
            {
                LoadListViewFromContractors(_listViewTarget, CONTRACTOR_STATUS.Holiday, _detail);
            }
            if (_comboBoxSort.Text == CONTRACTOR_STATUS.ETC.ToString())
            {
                LoadListViewFromContractors(_listViewTarget, CONTRACTOR_STATUS.ETC, _detail);
            }
        }

        /// <summary>
        /// Load list view after search button click for job
        /// </summary>
        /// <param name="_comboBoxSort"></param>
        /// <param name="_listViewTarget"></param>
        /// <param name="_new"></param>
        /// <param name="_detail"></param>
        private void LoadAfterSearch(ComboBox _comboBoxSort, ListView _listViewTarget, JOB_STATUS _new, bool _detail)
        {
            if (_comboBoxSort.Text == JOB_STATUS.All.ToString())
            {
                LoadListViewFromJobs(_listViewTarget, JOB_STATUS.All, false);
            }
            if (_comboBoxSort.Text == JOB_STATUS.Pending.ToString())
            {
                LoadListViewFromJobs(_listViewTarget, JOB_STATUS.Pending, false);
            }
            if (_comboBoxSort.Text == JOB_STATUS.Assigned.ToString())
            {
                LoadListViewFromJobs(_listViewTarget, JOB_STATUS.Assigned, false);
            }
            if (_comboBoxSort.Text == JOB_STATUS.Completed.ToString())
            {
                LoadListViewFromJobs(_listViewTarget, JOB_STATUS.Completed, false);
            }
            if (_comboBoxSort.Text == JOB_STATUS.CANCEL.ToString())
            {
                LoadListViewFromJobs(_listViewTarget, JOB_STATUS.CANCEL, false);
            }
        }

        /// <summary>
        /// Load Target list view
        /// </summary>
        /// <param name="_listViewNotice"></param>
        /// <param name="_comboBox"></param>
        /// <param name="_textBox"></param>
        public void LoadNotice(ListView _listViewNotice, ComboBox _comboBox, TextBox _textBox)
        {
            ListView listView = _listViewNotice;
            string notice = reSystem.noticeBoard.AddNotice(_comboBox.SelectedValue.ToString(), _textBox.Text);
            listView.Items.Insert(insertIndex, notice);

        }

        public void LoadNotice(ListView _listViewNotice, Job _job)
        {
            Job job = _job;
            string addJobNotice = reSystem.noticeBoard.AddNotice(job);
            _listViewNotice.Items.Insert(insertIndex, addJobNotice);


        }

        public void LoadNotice(ListView _listViewNotice, Contractor _contractor)
        {
            Contractor contractor = _contractor;
            string addContractorNotice = reSystem.noticeBoard.AddNotice(contractor);
            _listViewNotice.Items.Insert(insertIndex, addContractorNotice);
        }

        public void LoadNotice(ListView _listViewNotice, Completed _completed)
        {
            Completed completed = _completed;
            string addCompletedContractorNotice = reSystem.noticeBoard.AddNotice(completed);
            _listViewNotice.Items.Insert(insertIndex, addCompletedContractorNotice);

        }

        public void LoadDummyNotice(ListView _listViewNotice, List<Completed> _completed)
        {
            foreach (Completed addDummy in _completed)
            {
                string addDummyCompleted = reSystem.noticeBoard.AddDummyNotice(addDummy);
                _listViewNotice.Items.Insert(insertIndex, addDummyCompleted);
            }
        }

        public void LoadDummyNotice(ListView _listViewNotice, List<Contractor> _contractors)
        {
            foreach (Contractor addDummy in _contractors)
            {
                string addDummyContractors = reSystem.noticeBoard.AddDummyNotice(addDummy);

                _listViewNotice.Items.Insert(insertIndex, addDummyContractors);
            }
        }

        public void LoadDummyNotice(ListView _listViewNotice, List<Job> _jobs)
        {
            foreach (Job addDummy in _jobs)
            {
                string addDummyJobs = reSystem.noticeBoard.AddDummyNotice(addDummy);
                _listViewNotice.Items.Insert(insertIndex, addDummyJobs);
            }
        }

        public ComboBox LoadComboBox(ComboBox _comboBoxTarget, CONTRACTOR_STATUS _contractor_status)
        {
            _comboBoxTarget.Items.Clear();
            ComboBox comboBox = _comboBoxTarget;
            foreach (CONTRACTOR_STATUS type in Enum.GetValues(typeof(CONTRACTOR_STATUS)))
            {
                if (_comboBoxTarget.Name.Equals("ComboBoxContractorStatus") && type == CONTRACTOR_STATUS.All) continue;
                if (_comboBoxTarget.Name.Equals("ComboBoxContractorStatus") && type == CONTRACTOR_STATUS.Assigned) continue;
                comboBox.Items.Add(type);
            }

            return comboBox;
        }

        public ComboBox LoadComboBox(ComboBox _comboBoxTarget, JOB_STATUS _job_status)
        {
            _comboBoxTarget.Items.Clear();
            ComboBox comboBox = _comboBoxTarget;

            foreach (JOB_STATUS type in Enum.GetValues(typeof(JOB_STATUS)))
            {
                if (_comboBoxTarget.Name.Equals("ComboBoxJobStatus") && type == JOB_STATUS.All) continue;
                if (_comboBoxTarget.Name.Equals("ComboBoxJobStatus") && type == JOB_STATUS.Completed) continue;
                if (_comboBoxTarget.Name.Equals("ComboBoxSortJobRightView") && type == JOB_STATUS.Completed) continue;

                comboBox.Items.Add(type);
            }

            return comboBox;
        }

        public ComboBox LoadComboBox(ComboBox _comboBoxTarget, NOTICEBOARD _noticeBoard)
        {
            _comboBoxTarget.Items.Clear();
            ComboBox comboBox = _comboBoxTarget;
            foreach (NOTICEBOARD type in Enum.GetValues(typeof(NOTICEBOARD)))
            {
                comboBox.Items.Add(type);
            }
            return comboBox;
        }

        public ComboBox LoadComboBox(ComboBox _comboBoxTarget, SORT_CONTRACTOR_BY_DETAILS _sort_contractor_by_details)
        {
            _comboBoxTarget.Items.Clear();
            ComboBox comboBox = _comboBoxTarget;
            foreach (SORT_CONTRACTOR_BY_DETAILS type in Enum.GetValues(typeof(SORT_CONTRACTOR_BY_DETAILS)))
            {
                comboBox.Items.Add(type);
            }
            return comboBox;
        }

        public ComboBox LoadComboBox(ComboBox _comboBoxTarget, SORT_JOB_BY_DETAILS _sort_job_by_details)
        {
            _comboBoxTarget.Items.Clear();
            ComboBox comboBox = _comboBoxTarget;
            foreach (SORT_JOB_BY_DETAILS type in Enum.GetValues(typeof(SORT_JOB_BY_DETAILS)))
            {
                comboBox.Items.Add(type);
            }
            return comboBox;
        }

        public ComboBox LoadComboBox(ComboBox _comboBoxTarget, SORT_HISTORY_BY_DETAILS _sort_history_by_details)
        {
            _comboBoxTarget.Items.Clear();
            ComboBox comboBox = _comboBoxTarget;
            foreach (SORT_HISTORY_BY_DETAILS type in Enum.GetValues(typeof(SORT_HISTORY_BY_DETAILS)))
            {
                comboBox.Items.Add(type);
            }
            return comboBox;
        }

        public ComboBox LoadComboBox(ComboBox _comboBoxTarget, MAINBUTTONS _mainButton, bool _fullDetails)
        {
            _comboBoxTarget.Items.Clear();
            ComboBox comboBox = _comboBoxTarget;
            if (_fullDetails == true)
            {
                foreach (Contractor contractor in reSystem.contractors)
                {
                    comboBox.Items.Add(contractor);
                }
            }
            else
            {
                foreach (Contractor contractor in reSystem.contractors)
                {
                    comboBox.Items.Add($"{contractor.Level} {contractor.FullName}");
                }
            }

            return comboBox;
        }

        public ComboBox LoadComboBox(ComboBox _comboBoxTarget, MIN_MAX _min_or_max)
        {
            _comboBoxTarget.Items.Clear();
            ComboBox comboBox = _comboBoxTarget;

            MIN_MAX min_max = _min_or_max;

            if (min_max == MIN_MAX.MIN)
            {
                comboBox.Items.Add("$ Min");
                for (int cost = reSystem.MinimumSearchCost; cost <= reSystem.MaximumSearchCost; cost++)
                {
                    if (cost % 100 != 0) continue;
                    comboBox.Items.Add($"$ {cost}");
                }
            }

            else if (min_max == MIN_MAX.MAX)
            {
                comboBox.Items.Add("$ Max");
                for (int cost = reSystem.MinimumSearchCost; cost <= reSystem.MaximumSearchCost; cost++)
                {
                    if (cost % 100 != 0) continue;
                    comboBox.Items.Add($"$ {cost}");
                }
            }
            return comboBox;
        }

        public ComboBox LoadComboBox(ComboBox _comboBoxTarget, DAYMONTHYEAR _dayMonthYear)
        {
            _comboBoxTarget.Items.Clear();
            ComboBox comboBoxTarget = _comboBoxTarget;
            DAYMONTHYEAR dayMonthYear = _dayMonthYear;
            int startYear = 20;
            int endYear = Convert.ToInt32(DateTime.Now.Year.ToString().Substring(2));
            int startMonth = 1;
            int endMonth = 12;
            int startDay = 1;
            int endDay = 31;

            if (dayMonthYear == DAYMONTHYEAR.DAY)
            {
                comboBoxTarget.Items.Add("D");
                for (int day = startDay; day <= endDay; day++)
                {
                    comboBoxTarget.Items.Add(day);
                }
                return comboBoxTarget;
            }
            if (dayMonthYear == DAYMONTHYEAR.MONTH)
            {
                comboBoxTarget.Items.Add("M");
                for (int month = startMonth; month <= endMonth; month++)
                {
                    comboBoxTarget.Items.Add(month);
                }
                return comboBoxTarget;
            }
            if (dayMonthYear == DAYMONTHYEAR.YEAR)
            {
                comboBoxTarget.Items.Add("Y");
                for (int year = startYear; year <= endYear; year++)
                {
                    comboBoxTarget.Items.Add(year);
                }
                return comboBoxTarget;
            }

            return comboBoxTarget;
        }

        public Label LoadLabel(Label _labelTarget, ListView _targetListView, bool _detail)
        {
            Label newLabel = _labelTarget;
            if (_detail == true)
            {
                newLabel.Content = $"Count: {_targetListView.Items.Count.ToString("000.###")}  ";
                return newLabel;
            }
            newLabel.Content = $"({_targetListView.Items.Count.ToString("000.###")})  ";
            return newLabel;
        }

        public void LoadListView(ListView _listViewTarget, MAINBUTTONS _mainButtons, bool _detail, bool _loadCancelJob)
        {
            _listViewTarget.Items.Clear();
            ListView list = _listViewTarget;
            MAINBUTTONS mainButtons = _mainButtons;

            switch (mainButtons)
            {
                case MAINBUTTONS.NOTICE:
                    ListView lv = _listViewTarget;
                    reSystem.noticeBoard.notice = reSystem.noticeBoard.ReadFile(reSystem.noticeBoard.filePathNotice);
                    foreach (string noticeItem in reSystem.noticeBoard.notice)
                    {
                        lv.Items.Add(noticeItem);
                    }
                    break;

                case MAINBUTTONS.CONTRACTOR:
                    LoadListViewFromContractors(_listViewTarget, _detail);
                    break;

                case MAINBUTTONS.JOB:
                    LoadListViewFromJobs(_listViewTarget, _loadCancelJob);
                    break;

                case MAINBUTTONS.HISTORY:
                    LoadListViewFromHistory(_listViewTarget);
                    break;


            }
        }

        public void LoadListViewFromContractors(ListView _listView, bool _detail)
        {
            if (_detail == true)
            {
                foreach (Contractor contractor in reSystem.contractors)
                {
                    _listView.Items.Add(contractor);
                }
            }
            else
            {
                foreach (Contractor contractor in reSystem.contractors)
                {
                    _listView.Items.Add($"{contractor.Level} {contractor.FullName}");
                }
            }
        }

        public void LoadListViewFromContractors(ListView _listView, CONTRACTOR_STATUS _contractor_status, bool _detail)
        {
            CONTRACTOR_STATUS contractor_status = _contractor_status;
            _listView.Items.Clear();
            switch (contractor_status)
            {
                case CONTRACTOR_STATUS.All:
                    if (_detail == true)
                    {
                        foreach (Contractor contractor in reSystem.contractors)
                        {
                            _listView.Items.Add(contractor);
                        }
                    }
                    else
                    {
                        foreach (Contractor contractor in reSystem.contractors)
                        {
                            _listView.Items.Add($"{contractor.Level} {contractor.FullName}");
                        }
                    }
                    break;
                case CONTRACTOR_STATUS.Available:
                    if (_detail == true)
                    {
                        foreach (Contractor contractor in reSystem.contractors)
                        {
                            if (contractor.Status != CONTRACTOR_STATUS.Available) continue;
                            if (contractor.Status == CONTRACTOR_STATUS.Assigned) continue;
                            _listView.Items.Add(contractor);
                        }
                    }
                    else
                    {
                        foreach (Contractor contractor in reSystem.contractors)
                        {
                            if (contractor.Status != CONTRACTOR_STATUS.Available) continue;
                            if (contractor.Status == CONTRACTOR_STATUS.Assigned) continue;
                            _listView.Items.Add($"{contractor.Level} {contractor.FullName}");
                        }
                    }
                    break;
                case CONTRACTOR_STATUS.Assigned:
                    if (_detail == true)
                    {
                        foreach (Contractor contractor in reSystem.contractors)
                        {
                            if (contractor.Status != CONTRACTOR_STATUS.Assigned) continue;
                            _listView.Items.Add(contractor);
                        }
                    }
                    else
                    {
                        foreach (Contractor contractor in reSystem.contractors)
                        {
                            if (contractor.Status != CONTRACTOR_STATUS.Assigned) continue;
                            _listView.Items.Add($"{contractor.Level} {contractor.FullName}");
                        }
                    }
                    break;
                case CONTRACTOR_STATUS.Holiday:
                    if (_detail == true)
                    {
                        foreach (Contractor contractor in reSystem.contractors)
                        {
                            if (contractor.Status != CONTRACTOR_STATUS.Holiday) continue;
                            _listView.Items.Add(contractor);
                        }
                    }
                    else
                    {
                        foreach (Contractor contractor in reSystem.contractors)
                        {
                            if (contractor.Status != CONTRACTOR_STATUS.Holiday) continue;
                            _listView.Items.Add($"{contractor.Level} {contractor.FullName}");
                        }
                    }
                    break;
                case CONTRACTOR_STATUS.ETC:
                    if (_detail == true)
                    {
                        foreach (Contractor contractor in reSystem.contractors)
                        {
                            if (contractor.Status != CONTRACTOR_STATUS.ETC) continue;
                            _listView.Items.Add(contractor);
                        }
                    }
                    else
                    {
                        foreach (Contractor contractor in reSystem.contractors)
                        {
                            if (contractor.Status != CONTRACTOR_STATUS.ETC) continue;
                            _listView.Items.Add($"{contractor.Level} {contractor.FullName}");
                        }
                    }
                    break;
            }

        }

        public void LoadListViewFromJobs(ListView _listView, bool _loadCancelJob)
        {
            foreach (Job job in reSystem.jobs)
            {
                if (_loadCancelJob == true)
                {
                    if (job.Status != JOB_STATUS.CANCEL) continue;
                }
                _listView.Items.Add(job);
            }
        }

        public void LoadListViewFromJobs(ListView _listView, JOB_STATUS _job_status, bool _loadCancelJob)
        {
            _listView.Items.Clear();
            JOB_STATUS job_status = _job_status;
            switch (job_status)
            {
                case JOB_STATUS.All:
                    foreach (Job job in reSystem.jobs)
                    {
                        if (_loadCancelJob == true)
                        {
                            if (job.Status != JOB_STATUS.CANCEL) continue;
                        }
                        if (reSystem.MinimumSearchCost > job.Cost) continue;
                        if (reSystem.MaximumSearchCost < job.Cost) continue;
                        _listView.Items.Add(job);
                    }
                    break;
                case JOB_STATUS.Pending:
                    foreach (Job job in reSystem.jobs)
                    {
                        if (_loadCancelJob == true)
                        {
                            if (job.Status != JOB_STATUS.CANCEL) continue;
                        }
                        if (job.Status != JOB_STATUS.Pending) continue;
                        if (reSystem.MinimumSearchCost > job.Cost) continue;
                        if (reSystem.MaximumSearchCost < job.Cost) continue;
                        _listView.Items.Add(job);
                    }
                    break;
                case JOB_STATUS.Assigned:
                    foreach (Job job in reSystem.jobs)
                    {
                        if (_loadCancelJob == true)
                        {
                            if (job.Status != JOB_STATUS.CANCEL) continue;
                        }
                        if (job.Status != JOB_STATUS.Assigned) continue;
                        if (reSystem.MinimumSearchCost > job.Cost) continue;
                        if (reSystem.MaximumSearchCost < job.Cost) continue;
                        _listView.Items.Add(job);
                    }
                    break;
                case JOB_STATUS.CANCEL:
                    foreach (Job job in reSystem.jobs)
                    {
                        if (job.Status != JOB_STATUS.CANCEL) continue;
                        if (reSystem.MinimumSearchCost > job.Cost) continue;
                        if (reSystem.MaximumSearchCost < job.Cost) continue;
                        _listView.Items.Add(job);
                    }

                    break;
            }

        }

        public void LoadListViewFromHistory(ListView _listView)
        {
            if (_listView.Name.Equals("ListViewHistoryCompleted"))
            {
                foreach (Completed completedJob in reSystem.completed)
                {
                    _listView.Items.Add(completedJob);
                }
            }
            else
            {
                foreach (Cancel cancel in reSystem.cancelJobs)
                {
                    if (cancel.cancelJob.Status != JOB_STATUS.CANCEL) continue;
                    _listView.Items.Add(cancel);
                }
            }

        }





        private void ButtonMains_Click(object sender, RoutedEventArgs e)
        {
            ButtonMainNotice.Background = UnSelectedButtonColour;
            ButtonMainContractor.Background = UnSelectedButtonColour;
            ButtonMainJob.Background = UnSelectedButtonColour;
            ButtonMainHistory.Background = UnSelectedButtonColour;

            GridViewNotice.Visibility = UnSelectedView;
            GridViewContractor.Visibility = UnSelectedView;
            GridViewJob.Visibility = UnSelectedView;
            GridViewHistory.Visibility = UnSelectedView;

            Button mainButton = (Button)sender;
            mainButton.Background = SelectedButtonColour;

            switch (mainButton.Content)
            {
                case "NOTICE":
                    LabelSubTitleNotice.Visibility = SelectedView;
                    LabelSubTitleContractor.Visibility = UnSelectedView;
                    LabelSubTitleJobLeft.Visibility = UnSelectedView;
                    LabelSubTitleJobRight.Visibility = UnSelectedView;
                    ButtonSubTitleClearInfo.Visibility = UnSelectedView;
                    GridViewNotice.Visibility = SelectedView;
                    GridViewIntro.Visibility = UnSelectedView;

                    LoadComboBox(ComboBoxNoticeSort, new NOTICEBOARD());

                    SetComboBoxDefault(MAINBUTTONS.NOTICE);
                    break;

                case "CONTRACTOR":
                    LabelSubTitleNotice.Visibility = UnSelectedView;
                    LabelSubTitleContractor.Visibility = SelectedView;
                    LabelSubTitleJobLeft.Visibility = UnSelectedView;
                    LabelSubTitleJobRight.Visibility = UnSelectedView;
                    ButtonSubTitleClearInfo.Visibility = SelectedView;
                    GridViewContractor.Visibility = SelectedView;
                    GridViewIntro.Visibility = UnSelectedView;

                    LoadListView(ListViewContractor, MAINBUTTONS.CONTRACTOR, true, false);

                    LoadComboBox(ComboBoxSortContractorView, new CONTRACTOR_STATUS());
                    LoadComboBox(ComboBoxStartDateDay, DAYMONTHYEAR.DAY);
                    LoadComboBox(ComboBoxStartDateMonth, DAYMONTHYEAR.MONTH);
                    LoadComboBox(ComboBoxStartDateYear, DAYMONTHYEAR.YEAR);
                    LoadComboBox(ComboBoxContractorStatus, new CONTRACTOR_STATUS());
                    LoadComboBox(ComboBoxSortContractorViewDetail, new SORT_CONTRACTOR_BY_DETAILS());

                    LoadLabel(LabelContractorCounter, ListViewContractor, true);

                    SetComboBoxDefault(MAINBUTTONS.CONTRACTOR);
                    break;

                case "JOB":
                    LabelSubTitleNotice.Visibility = UnSelectedView;
                    LabelSubTitleContractor.Visibility = UnSelectedView;
                    LabelSubTitleJobLeft.Visibility = SelectedView;
                    LabelSubTitleJobRight.Visibility = SelectedView;
                    ButtonSubTitleClearInfo.Visibility = SelectedView;
                    GridViewJob.Visibility = SelectedView;
                    GridViewIntro.Visibility = UnSelectedView;

                    LoadListView(ListViewJobLeft, MAINBUTTONS.CONTRACTOR, false, false);
                    LoadListView(ListViewJobRight, MAINBUTTONS.JOB, true, false);

                    LoadComboBox(ComboBoxJobContractor, MAINBUTTONS.CONTRACTOR, false);
                    LoadComboBox(ComboBoxJobStatus, new JOB_STATUS());
                    LoadComboBox(ComboBoxSortJobLeftView, new CONTRACTOR_STATUS());
                    LoadComboBox(ComboBoxSortJobRightView, new JOB_STATUS());
                    LoadComboBox(ComboBoxSortJobRightViewCostAndDueDate, new SORT_JOB_BY_DETAILS());
                    LoadComboBox(ComboBoxMinCost, MIN_MAX.MIN);
                    LoadComboBox(ComboBoxMaxCost, MIN_MAX.MAX);
                    LoadComboBox(ComboBoxDeadLineDay, DAYMONTHYEAR.DAY);
                    LoadComboBox(ComboBoxDeadLineMonth, DAYMONTHYEAR.MONTH);
                    LoadComboBox(ComboBoxDeadLineYear, DAYMONTHYEAR.YEAR);

                    LoadLabel(LabelJobLeftCounter, ListViewJobLeft, false);
                    LoadLabel(LabelJobRightCounter, ListViewJobRight, true);

                    SetComboBoxDefault(MAINBUTTONS.JOB);
                    break;

                case "HISTORY":
                    LabelSubTitleNotice.Visibility = UnSelectedView;
                    LabelSubTitleContractor.Visibility = UnSelectedView;
                    LabelSubTitleJobLeft.Visibility = UnSelectedView;
                    LabelSubTitleJobRight.Visibility = UnSelectedView;
                    ButtonSubTitleClearInfo.Visibility = UnSelectedView;
                    GridViewHistory.Visibility = SelectedView;
                    GridViewIntro.Visibility = UnSelectedView;

                    LoadListView(ListViewHistoryCompleted, MAINBUTTONS.HISTORY, false, false);
                    LoadListView(ListViewHistoryCancel, MAINBUTTONS.HISTORY, false, true);

                    LoadComboBox(ComboBoxHistoryCompleted, new SORT_HISTORY_BY_DETAILS());
                    LoadComboBox(ComboBoxHistoryCancel, new SORT_HISTORY_BY_DETAILS());

                    LoadLabel(LabelHistoryCounterCompleted, ListViewHistoryCompleted, true);
                    LoadLabel(LabelHistoryCounterCancel, ListViewHistoryCancel, true);

                    SetComboBoxDefault(MAINBUTTONS.HISTORY);
                    break;
            }
        }

        private void ButtonSubTitleClearInfo_Click(object sender, RoutedEventArgs e)
        {
            ClearInformation();
        }

        private void ButtonNoticeSend_Click(object sender, RoutedEventArgs e)
        {
            if (TextBoxNotice.Text == "") return;
            LoadNotice(ListViewNotice, ComboBoxNoticeSort, TextBoxNotice);
            MessageBox.Show("Message Sent");
            TextBoxNotice.Text = "";
        }

        private void ButtonContractor_Click(object sender, RoutedEventArgs e)
        {
            Button selectedButton = sender as Button;


            if (selectedButton == ButtonViewContractorAdd)
            {
                if (TextBoxFirstName.Text == "" || TextBoxLastName.Text == "")
                {
                    MessageBox.Show("Enter Names");
                    return;
                }
                if (!int.TryParse(ComboBoxStartDateDay.SelectedValue.ToString(), out int startDay))
                {
                    MessageBox.Show("Invalid date");
                    return;
                }
                if (!int.TryParse(ComboBoxStartDateMonth.SelectedValue.ToString(), out int startMonth))
                {
                    MessageBox.Show("Invalid date");
                    return;
                }
                if (!int.TryParse(ComboBoxStartDateYear.SelectedValue.ToString(), out int startYear))
                {
                    MessageBox.Show("Invalid date");
                    return;
                }
                Contractor newContractor = CreateContractor();
                if (!reSystem.AddContractor(reSystem.contractors, newContractor)) return;
                LoadListView(ListViewContractor, MAINBUTTONS.CONTRACTOR, true, false);
                LoadNotice(ListViewNotice, newContractor);
                MessageBox.Show("Added");
            }

            else if (selectedButton == ButtonViewContractorUpdate)
            {
                if (TextBoxFirstName.Text == "" || TextBoxLastName.Text == "")
                {
                    MessageBox.Show("Enter Names");
                    return;
                }
                if (!int.TryParse(ComboBoxStartDateDay.SelectedValue.ToString(), out int startDay))
                {
                    MessageBox.Show("Invalid date");
                    return;
                }
                if (!int.TryParse(ComboBoxStartDateMonth.SelectedValue.ToString(), out int startMonth))
                {
                    MessageBox.Show("Invalid date");
                    return;
                }
                if (!int.TryParse(ComboBoxStartDateYear.SelectedValue.ToString(), out int startYear))
                {
                    MessageBox.Show("Invalid date");
                    return;
                }
                if (selectedContractor == null) { MessageBox.Show("Select Contractor"); return; }
                reSystem.RemoveContractor(reSystem.contractors, selectedContractor);
                Contractor newContractor = CreateContractor();
                newContractor.Id = selectedContractor.Id;
                if (!reSystem.AddContractor(reSystem.contractors, newContractor)) return;
                LoadListView(ListViewContractor, MAINBUTTONS.CONTRACTOR, true, false);
                MessageBox.Show("Updated");

            }
            else if (selectedButton == ButtonViewContractorDelete)
            {
                if (!ErrorCheck.CheckIsNotNull(selectedContractor))
                {
                    MessageBox.Show("Select Contractor");
                    return;
                }
                MessageBoxResult check = MessageBox.Show("Delete?", "Confirm", MessageBoxButton.OKCancel);
                if (check == MessageBoxResult.OK)
                {
                    reSystem.RemoveContractor(reSystem.contractors, selectedContractor);
                    LoadListView(ListViewContractor, MAINBUTTONS.CONTRACTOR, true, false);
                }
                MessageBox.Show("Deleted");
            }
        }

        private void ButtonJob_Click(object sender, RoutedEventArgs e)
        {
            Button selectedButton = sender as Button;
            Job newJob = null;

            if (selectedButton == ButtonViewJobAdd)
            {
                if (ComboBoxJobStatus.SelectedValue.ToString() == "CANCEL" || ComboBoxJobStatus.SelectedValue.ToString() == "Assigned")
                {
                    MessageBox.Show("== Add Job == \n\nSelect Pending first \nbefore Job update assigned or cancel");
                    return;
                }

                newJob = CreateJob();

                if (TextBoxJobTitle.Text == "")
                {
                    MessageBox.Show("Enter Titles");
                    return;
                }
                if (!int.TryParse(ComboBoxDeadLineDay.SelectedValue.ToString(), out int startDay))
                {
                    MessageBox.Show("Invalid date");
                    return;
                }
                if (!int.TryParse(ComboBoxDeadLineMonth.SelectedValue.ToString(), out int startMonth))
                {
                    MessageBox.Show("Invalid date");
                    return;
                }
                if (!int.TryParse(ComboBoxDeadLineYear.SelectedValue.ToString(), out int startYear))
                {
                    MessageBox.Show("Invalid date");
                    return;
                }
                if (newJob == null) return;
                reSystem.AddJob(reSystem.jobs, newJob);
                LoadListView(ListViewJobLeft, MAINBUTTONS.JOB, false, false);
                LoadListView(ListViewJobRight, MAINBUTTONS.JOB, false, false);
                LoadNotice(ListViewNotice, newJob);
                MessageBox.Show("Added");

            }
            else if (selectedButton == ButtonViewJobUpdate)
            {
                newJob = CreateJob();
                if (ComboBoxJobContractor.SelectedValue == null)
                {
                    MessageBox.Show("Select Contractor");
                    return;
                }
                if (TextBoxJobTitle.Text == "")
                {
                    MessageBox.Show("Enter Titles");
                    return;
                }
                if (!int.TryParse(ComboBoxDeadLineDay.SelectedValue.ToString(), out int startDay))
                {
                    MessageBox.Show("Invalid date");
                    return;
                }
                if (!int.TryParse(ComboBoxDeadLineMonth.SelectedValue.ToString(), out int startMonth))
                {
                    MessageBox.Show("Invalid date");
                    return;
                }
                if (!int.TryParse(ComboBoxDeadLineYear.SelectedValue.ToString(), out int startYear))
                {
                    MessageBox.Show("Invalid date");
                    return;
                }
                newJob.Id = selectedJob.Id;
                newJob.Level = selectedJob.Level;
                reSystem.RemoveJob(reSystem.jobs, selectedJob);
                if (ComboBoxJobStatus.Text == JOB_STATUS.Pending.ToString())
                {
                    reSystem.AddJob(reSystem.jobs, selectedJob);
                    if (selectedJob.ContractorAssigned != null)
                    {
                        newJob.ContractorAssigned = selectedJob.ContractorAssigned;
                        newJob.ContractorAssigned.Status = CONTRACTOR_STATUS.Available;
                        newJob.ContractorAssigned.JobAssigned = null;
                        newJob.ContractorAssigned = null;
                    }
                    MessageBox.Show("Updated");
                }
                if (ComboBoxJobStatus.Text == JOB_STATUS.Assigned.ToString())
                {

                    // the job already assigned and ask again to change to another person
                    Contractor currentSelectedContractor = selectedContractor;
                    
                    selectedContractor = reSystem.GetSelectedContractor(reSystem.contractors, ComboBoxJobContractor.SelectedValue.ToString());


                    if (selectedContractor.JobAssigned != null) 
                    {
                        string message = $"""
                            == {selectedContractor.FullName} ==
                            Already has a Job
                            Do you want assign another Job?
                            * Previous job will be back to pool

                            """;
                        MessageBoxResult check = MessageBox.Show(message, "Confirm", MessageBoxButton.OKCancel);
                        if (check != MessageBoxResult.OK)
                        {
                            return;
                        }
                        selectedContractor.JobAssigned.Status = JOB_STATUS.Pending;
                        selectedContractor.JobAssigned.ContractorAssigned = null;
                     
                    }
                    
                    reSystem.AddJob(reSystem.jobs, newJob);
                    if (ListViewJobLeft.SelectedIndex == -1 && ComboBoxJobContractor.SelectedIndex == -1)
                    {
                        MessageBox.Show("Select Contractor");
                        return;
                    }

                    JOB_STATUS tempJobStates = newJob.Status;

                    if (!reSystem.AssignJob(selectedContractor, newJob, ComboBoxJobStatus.SelectedValue.ToString()))
                    {
                        newJob.Status = tempJobStates;
                        string message = $"""
                            Cannot assign lower Level than Job Level
                                     
                            Selected Contractor LV: {ComboBoxJobContractor.SelectedValue.ToString().Substring(0, 2)} 
                            Selected Job LV: {newJob.Level}
                            """;
                        MessageBox.Show(message);
                        return;
                    }
                    reSystem.CancelJob(newJob);
                }
                if (ComboBoxJobStatus.Text == JOB_STATUS.CANCEL.ToString())
                {
                    reSystem.CancelJob(newJob);
                    reSystem.RemoveJob(reSystem.jobs, newJob);
                    MessageBox.Show("Job Cancel");
                }


            }
            else if (selectedButton == ButtonViewJobDelete)
            {
                if (!ErrorCheck.CheckIsNotNull(selectedJob))
                {
                    MessageBox.Show("Please select Job");
                    return;
                }
                MessageBoxResult check = MessageBox.Show("Delete?", "Confirm", MessageBoxButton.OKCancel);
                if (check == MessageBoxResult.OK)
                {
                    reSystem.RemoveJob(reSystem.jobs, selectedJob);
                    LoadListView(ListViewJobLeft, MAINBUTTONS.JOB, false, false);
                    LoadListView(ListViewJobRight, MAINBUTTONS.JOB, true, false);
                }
                MessageBox.Show("Deleted");
                selectedJob = null;
            }
            else if (selectedButton == ButtonViewJobCompleted)
            {
                if (ListViewJobRight.SelectedItem == null)
                {
                    MessageBox.Show("Select Job");

                }
                else
                {
                    reSystem.CompleteJob(reSystem.completed, selectedJob);
                    reSystem.RemoveJob(reSystem.jobs, selectedJob);
                    LoadNotice(ListViewNotice, reSystem.completed[reSystem.completed.Count - 1]);
                    MessageBox.Show("Completed");
                    ClearInformation();

                }
            }
            LoadListView(ListViewJobLeft, MAINBUTTONS.CONTRACTOR, false, false);
            LoadListView(ListViewJobRight, MAINBUTTONS.JOB, true, false);
        }

        private void ButtonSearch_Click(object sender, RoutedEventArgs e)
        {
            Button searchButton = (Button)sender;
            if (searchButton.Name == ButtonContractorSearch.Name)
            {
                reSystem.SortClass(ComboBoxSortContractorViewDetail.SelectedValue.ToString(), MAINBUTTONS.CONTRACTOR);
                LoadAfterSearch(ComboBoxSortContractorView, ListViewContractor, new CONTRACTOR_STATUS(), true);
                LoadLabel(LabelContractorCounter, ListViewContractor, true);

            }
            else if (searchButton.Name == ButtonJobLeftSearch.Name)
            {
                reSystem.SortClass(ComboBoxSortContractorViewDetail.SelectedValue.ToString(), MAINBUTTONS.CONTRACTOR);
                LoadAfterSearch(ComboBoxSortJobLeftView, ListViewJobLeft, new CONTRACTOR_STATUS(), false);
                LoadLabel(LabelJobLeftCounter, ListViewJobLeft, false);
            }
            else if (searchButton.Name == ButtonJobRightSearch.Name)
            {
                reSystem.MinimumSearchCost = int.Parse(ComboBoxMinCost.Text.ToString().Substring(2));
                reSystem.MaximumSearchCost = int.Parse(ComboBoxMaxCost.Text.ToString().Substring(2));
                if (reSystem.MinimumSearchCost > reSystem.MaximumSearchCost)
                {
                    MessageBox.Show("Wrong Cost Range");
                    return;
                };
                reSystem.SortClass(ComboBoxSortJobRightViewCostAndDueDate.SelectedValue.ToString(), MAINBUTTONS.JOB);
                LoadAfterSearch(ComboBoxSortJobRightView, ListViewJobRight, new JOB_STATUS(), true);
                LoadLabel(LabelJobRightCounter, ListViewJobRight, true);

            }
            else if (searchButton.Name == ButtonHistorySearchLeft.Name)
            {
                reSystem.SortClass(ComboBoxHistoryCompleted.SelectedValue.ToString(), MAINBUTTONS.COMPLETED);
                LoadListView(ListViewHistoryCompleted, MAINBUTTONS.HISTORY, false, false);
                LoadLabel(LabelHistoryCounterCompleted, ListViewHistoryCompleted, true);
            }
            else if (searchButton.Name == ButtonHistorySearchRight.Name)
            {
                reSystem.SortClass(ComboBoxHistoryCancel.SelectedValue.ToString(), MAINBUTTONS.CANCEL);
                LoadListView(ListViewHistoryCancel, MAINBUTTONS.HISTORY, false, true);
                LoadLabel(LabelHistoryCounterCancel, ListViewHistoryCancel, true);
            }

        }




        private void SliderWage_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (LabelWageText == null || SliderWage == null) return;
            string decimalUpTo2 = "F2";
            LabelWageText.Content = $"$ {SliderWage.Value.ToString(decimalUpTo2)}";

        }

        private void SliderJobCost_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (LabelJobCostText == null || SliderJobCost == null) return;
            LabelJobCostText.Content = $"$ {SliderJobCost.Value}";
        }




        private void ListViewContractor_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListViewContractor == null || ListViewContractor.SelectedItem == null) return;
            selectedContractor = ListViewContractor.SelectedItem as Contractor;

            if (selectedContractor != null)
            {
                TextBoxFirstName.Text = selectedContractor.FirstName;
                TextBoxLastName.Text = selectedContractor.LastName;
                ComboBoxStartDateDay.Text = selectedContractor.StartDateDay.ToString();
                ComboBoxStartDateMonth.Text = selectedContractor.StartDateMonth.ToString();
                ComboBoxStartDateYear.Text = selectedContractor.StartDateYear.ToString();
                SliderWage.Value = selectedContractor.HourlyWage;
                ComboBoxContractorStatus.Text = selectedContractor.Status.ToString();
            }
        }

        private void ListViewJobLeft_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListViewJobLeft == null || ListViewJobLeft.SelectedItem == null) return;
            string selectedContractor = ListViewJobLeft.SelectedItem.ToString();

            if (selectedContractor != null)
            {
                ComboBoxJobContractor.Text = selectedContractor;

            }
        }

        private void ListViewJobRight_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListViewJobRight == null || ListViewJobRight.SelectedItem == null) return;
            selectedJob = ListViewJobRight.SelectedItem as Job;
            if (selectedJob == null)
            {
                selectedContractor = selectedJob.ContractorAssigned;
            }

            ButtonViewJobCompleted.IsEnabled = false;
            if (selectedJob != null)
            {
                TextBoxJobTitle.Text = selectedJob.Title;



                if (selectedJob.ContractorAssigned != null)
                {
                    ComboBoxJobContractor.Text = $"{selectedJob.ContractorAssigned.Level} {selectedJob.ContractorAssigned.FullName}";
                    ButtonViewJobCompleted.IsEnabled = true;
                }


                ComboBoxDeadLineDay.Text = selectedJob.DeadLineDay.ToString();
                ComboBoxDeadLineMonth.Text = selectedJob.DeadLineMonth.ToString();
                ComboBoxDeadLineYear.Text = selectedJob.DeadLineYear.ToString();



                SliderJobCost.Value = selectedJob.Cost;
                ComboBoxJobStatus.Text = selectedJob.Status.ToString();
            }

            if (ComboBoxJobStatus.Text == JOB_STATUS.CANCEL.ToString())
            {
                MessageBoxResult check = MessageBox.Show($"This is CANCEL job\nAre you sure you want to edit?", "Check Cancel", MessageBoxButton.OKCancel);
                if (check != MessageBoxResult.OK)
                {
                    ClearInformation();

                }
            }

        }

        private void ListViewHistory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListView listViewSelected = sender as ListView;
            if (listViewSelected == null) return;
            if (listViewSelected == ListViewHistoryCompleted)
            {
                if (ListViewHistoryCompleted.SelectedItem == null) return;
                Completed selectedCompleted = ListViewHistoryCompleted.SelectedItem as Completed;
                string message = $""" 
                    ======== Completed By =======

                    AGENCY ID:  {selectedCompleted.CompletedBy.Id} 
                    LEVEL:  {selectedCompleted.CompletedBy.Level}
                    First Name:  {selectedCompleted.CompletedBy.FirstName.ToUpper()} 
                    Last Name:  {selectedCompleted.CompletedBy.LastName.ToUpper()} 
                    Contractor Enrolled Date:  {selectedCompleted.CompletedBy.StartDate.ToString("dd/MM/yy")} 
                    Hourly Wage:  {selectedCompleted.CompletedBy.HourlyWage}
                    Completed Number of Jobs:  {selectedCompleted.CompletedBy.NumberOfCompletedJob}
                    """;
                MessageBox.Show(message);

            }
            else if (listViewSelected == ListViewHistoryCancel)
            {
                if (ListViewHistoryCancel.SelectedItem == null) return;
                Cancel selectedCancelJob = ListViewHistoryCancel.SelectedItem as Cancel;
                string message = $""" 
                    ======== Cancel Job =======

                    Job ID:  {selectedCancelJob.cancelJob.Id} 
                    LEVEL:  {selectedCancelJob.cancelJob.Level}
                    Full Title:  {selectedCancelJob.cancelJob.Title.ToUpper()} 
                    Dead Line:  {selectedCancelJob.cancelJob.DeadLine.ToString("dd/MM/yy")} 
                    Cost:  {selectedCancelJob.cancelJob.Cost}
                    """;
                MessageBox.Show(message);
            }

        }

    }
}
