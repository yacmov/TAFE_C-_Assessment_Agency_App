using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TEST_TAFE_ICT40120_Assessment02
{
    [TestClass]
    public class TestDummyData
    {
        [TestMethod]
        [DataRow(1)]
        [DataRow(100)]
        [DataRow(200)]
        [DataRow(400)]
        [DataRow(-112)]
        public void DummyData_LoadDummyContractors_ReturnLength(int _dummyCreation)
        {
            
            DummyData testDummyData = new DummyData();
            List<Contractor> testContractorList = testDummyData.LoadDummyContractors(_dummyCreation, 0, 0);

            if (_dummyCreation <= 0)
            {
                _dummyCreation = 1;
            }
            Assert.AreEqual(testContractorList.Count -2, _dummyCreation);
        }

        [TestMethod]
        [DataRow(1)]
        [DataRow(100)]
        [DataRow(200)]
        [DataRow(400)]
        [DataRow(-112)]
        public void DummyData_LoadDummyJobs_ReturnLength(int _dummyCreation)
        {

            DummyData testDummyData = new DummyData();
            List<Job> testJobList = testDummyData.LoadDummyJobs(_dummyCreation, 0);

            if (_dummyCreation <= 0)
            {
                _dummyCreation = 1;
            }
            Assert.AreEqual(testJobList.Count - 1, _dummyCreation);
        }
    }
}
