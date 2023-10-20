using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace TEST_TAFE_ICT40120_Assessment02
{
    [TestClass]
    public class TestRecruitmentSystem
    {
        RecruitmentSystem testRS = new RecruitmentSystem();
        List<Job> testListJobs = new List<Job>();
        List<Contractor> testListContractors = new List<Contractor>();
        List<Completed> testListCompleted = new List<Completed>();
        DummyData dummyData = new DummyData();

        [TestMethod]
        public void RecruitmentSystem_Constructor_ReturnDummyDataIsNull()
        {
            RecruitmentSystem testRsSystem = new RecruitmentSystem(false);
            Assert.IsNull(testRsSystem.jobs);
            Assert.IsNull(testRsSystem.contractors);
            Assert.IsNull(testRsSystem.completed);

        }

        [TestMethod]
        public void RecruitmentSystem_Constructor_ReturnDummyDataIsNotNull()
        {
            RecruitmentSystem testRsSystem = new RecruitmentSystem(true);
            Assert.IsNotNull(testRsSystem.jobs);
            Assert.IsNotNull(testRsSystem.contractors);
            Assert.IsNotNull(testRsSystem.completed);
        }

        [TestMethod]
        public void RecruitmentSystem_AddContractor_ReturnIsNotNull()
        {
            
            List<Contractor> dummyList = dummyData.LoadDummyContractors(1, 0, 0);
            
            Contractor contractor = dummyList[0];
            testListContractors.Clear();
            testRS.AddContractor(testListContractors, contractor);

            Assert.IsNotNull(testListContractors[0]);
            
        }

        [TestMethod]
        public void RecruitmentSystem_RemoveContractor_ThrowException()
        {
            List<Contractor> dummyContractor = dummyData.LoadDummyContractors(1, 0, 0);
            Contractor testContractor = dummyContractor[0];
            testListContractors.Clear();
            testRS.AddContractor(testListContractors, testContractor);
            testRS.RemoveContractor(testListContractors, testContractor);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
            {
                Contractor testContractor1 = testListContractors[0]; 
            });
        }

        [TestMethod]
        public void RecruitmentSystem_AddJob_ReturnIsNotNull()
        {
            List<Job> dummyList = dummyData.LoadDummyJobs(1, 0);

            Job testJob = dummyList[0];
            testListJobs.Clear();
            testRS.AddJob(testListJobs, testJob);
            Assert.IsNotNull(testListJobs[0]);

        }

        [TestMethod]
        public void RecruitmentSystem_RemoveJob_ThrowException()
        {
            List<Job> dummyJob = dummyData.LoadDummyJobs(1, 0);
            Job testJob = dummyJob[0];
            testListContractors.Clear();
            testRS.AddJob(testListJobs, testJob);
            testRS.RemoveJob(testListJobs, testJob);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
            {
                Job testJob1 = testListJobs[0];
            });
        }

        [TestMethod]
        [DataRow("jal&jdlf_jasd")]
        [DataRow("j4jn4$$3")]
        [DataRow("&**#*#@$")]

        public void RecruitmentSystem_AssignJob_ReturnAreNotEqual(string _target)
        {
            List<Job> dummyjob = dummyData.LoadDummyJobs(1, 0);
            Job testJob = dummyjob[0]; 
            List<Contractor> dummyContractor = dummyData.LoadDummyContractors(1, 0, 0);
            Contractor  testContractor = dummyContractor[0];
            testListContractors.Clear();
            testListContractors.Add(testContractor);
            testListContractors[0].Level = LEVEL.L3;
            testListContractors[0].FirstName = "TEST";
            testListContractors[0].LastName = _target;
            testListContractors[0].FullName = testListContractors[0].FirstName + " " + testListContractors[0].LastName;
            testRS.AssignJob(testListContractors[0], testJob, JOB_STATUS.Assigned.ToString());
            Assert.AreNotEqual(_target, testJob.ContractorAssigned.LastName.ToString());
        }


        [TestMethod]
        [DataRow("Hello")]
        [DataRow("World")]
        [DataRow("Test")]
        public void RecruitmentSystem_AssignJob_ReturnAreEqual(string _target)
        {
            List<Job> dummyjob = dummyData.LoadDummyJobs(1, 0);
            Job testJob = dummyjob[0];
            List<Contractor> dummyContractor = dummyData.LoadDummyContractors(1, 0, 0);
            Contractor testContractor = dummyContractor[0];
            testListContractors.Clear();
            testListContractors.Add(testContractor);
            testListContractors[0].Level = LEVEL.L3;
            testListContractors[0].FirstName = "TEST";
            testListContractors[0].LastName = _target;
            testListContractors[0].FullName = testListContractors[0].FirstName + " " + testListContractors[0].LastName;
            testRS.AssignJob(testListContractors[0], testJob, JOB_STATUS.Assigned.ToString());
            Assert.AreEqual(_target, testJob.ContractorAssigned.LastName.ToString());
        }

        [TestMethod]
        [DataRow(1, MAINBUTTONS.CONTRACTOR)]
        [DataRow(31, MAINBUTTONS.CONTRACTOR)]
        [DataRow(-1, MAINBUTTONS.CONTRACTOR)]
        public void RecruitmentSystem_LevelCalculator_ReturnContractorLevel1(int _int, MAINBUTTONS _mainButtons)
        {
            Assert.AreEqual(testRS.LevelCalculator(_int, _mainButtons), LEVEL.L1);
        }


        [TestMethod]
        [DataRow(50, MAINBUTTONS.CONTRACTOR)]
        [DataRow(99, MAINBUTTONS.CONTRACTOR)]
        [DataRow(10000, MAINBUTTONS.CONTRACTOR)]
        public void RecruitmentSystem_LevelCalculator_ReturnContractorLevel3(int _int, MAINBUTTONS _mainButtons)
        {

            Assert.AreEqual(testRS.LevelCalculator(_int, _mainButtons), LEVEL.L3);

        }


        [TestMethod]
        [DataRow(-100, MAINBUTTONS.JOB)]
        [DataRow(300, MAINBUTTONS.JOB)]
        [DataRow(999, MAINBUTTONS.JOB   )]
        public void RecruitmentSystem_LevelCalculator_ReturnJobLevel1(int _int, MAINBUTTONS _mainButtons)
        {
            Assert.AreEqual(testRS.LevelCalculator(_int, _mainButtons), LEVEL.L1);
        }


        [TestMethod]
        [DataRow(2000, MAINBUTTONS.JOB)]
        [DataRow(5000, MAINBUTTONS.JOB)]
        [DataRow(10000, MAINBUTTONS.JOB)]
        public void RecruitmentSystem_LevelCalculator_ReturnJobLevel3(int _int, MAINBUTTONS _mainButtons)
        {

            Assert.AreEqual(testRS.LevelCalculator(_int, _mainButtons), LEVEL.L3);

        }


        [TestMethod]
        [DataRow(1)]
        [DataRow(-14450)]
        [DataRow(2)]
        [DataRow(7)]
        [DataRow(-1)]
        public void RecruitmentSystem_CompleteJob_ReturnAreEqual(int _create)
        {
            testListContractors.Clear();
            testListContractors = dummyData.LoadDummyContractors(_create, 0, 0);
            testListJobs.Clear();
            testListJobs = dummyData.LoadDummyJobs(_create, 0);
            testListCompleted.Clear();
            testListContractors[0].Level = LEVEL.L3;
            testListContractors[0].FullName = "Test";
            testRS.AssignJob(testListContractors[0], testListJobs[0], JOB_STATUS.Assigned.ToString());
            testRS.CompleteJob(testListCompleted, testListJobs[0]);

            Assert.AreEqual(testListCompleted[0].CompletedJob.Title, testListJobs[0].Title);
        }

        

    }
}