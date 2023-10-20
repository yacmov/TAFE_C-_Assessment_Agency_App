using System.Security.Cryptography.Xml;

namespace TEST_TAFE_ICT40120_Assessment02
{
    [TestClass]
    public class TestJob
    {
        [TestMethod]
        [DataRow(LEVEL.L1)]
        [DataRow(LEVEL.L2)]
        [DataRow(LEVEL.L3)]
        public void Job_Constructor_ReturnLevelOfType(LEVEL _level)
        {
            Job result = new Job()
            {
                Level = _level
            };

            Assert.IsInstanceOfType(result.Level, typeof(Enum));
        }



        [TestMethod]
        [DataRow("TEST%%^")]
        [DataRow("**%%^")]
        [DataRow(">>%%^")]
        [DataRow("e^")]
        [DataRow(">^")]
        [DataRow("'")]
        [DataRow("m")]
        [DataRow("++")]
        public void Job_Constructor_ReturnTitleIsNull(string _title)
        {
            Job result = new Job()
            {
                Title = _title
            };

            Assert.IsNull(result.Title);

        }



        [TestMethod]
        [DataRow(19, 2, 31)]
        [DataRow(20, 2, 30)]
        [DataRow(21, 2, 29)]
        [DataRow(22445, 4, 31)]
        [DataRow(0, -1, -6)]
        [DataRow(-10, 0, -10)]
        [DataRow(-10, 01, -10)]
        [DataRow(-10, 59868, -101218283)]
        public void Job_Constructor_ReturnDateIsNull(int _year, int _month, int _day)
        {
            Job result = null;
            if (ErrorCheck.CheckValidDate(_year, _month, _day))
            {
                result = new Job()
                {


                    DeadLine = new DateTime(_year, _month, _day)

                };
            }

            Assert.IsNull(result);
        }

        [TestMethod]
        public void Job_Constructor_ReturnIDAreNotEqual()
        {
            Job test1 = new Job() { };
            Job test2 = new Job() { };

            Assert.AreNotEqual(test1.Id, test2.Id);
        }

        [TestMethod]
        [DataRow(-29193)]
        [DataRow(-55)]
        [DataRow(123)]
        [DataRow(8658)]
        [DataRow(123189)]
        public void Job_Constructor_ReturnCostAreNotEqual(double _cost)
        {
            Job testJob = new Job() 
            {
                Cost = _cost
            };

            Assert.AreNotEqual(_cost, testJob.Cost);

        }

        [TestMethod]
        public void Job_Constructor_ReturnContractorAssignedIsNull()
        {
            Job test = new Job() { };
            Assert.IsNull(test.ContractorAssigned);
        }



    }
}
