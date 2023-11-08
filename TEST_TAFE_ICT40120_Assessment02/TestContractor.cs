using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TEST_TAFE_ICT40120_Assessment02
{
    [TestClass]
    public class TestContractor
    {
        [TestMethod]
        [DataRow(LEVEL.L1)]
        [DataRow(LEVEL.L2)]
        [DataRow(LEVEL.L3)]
        public void Contractor_Constructor_ReturnLevelOfType(LEVEL _level)
        {
            Contractor result = new Contractor()
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
        public void Contractor_Constructor_ReturnFirstNameIsNull(string _firstName)
        {
            Contractor result = new Contractor()
            {
                FirstName = _firstName
            };

            Assert.IsNull(result.FirstName);

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
        public void Contractor_Constructor_ReturnLastNameIsNull(string _lastName)
        {
            Contractor result = new Contractor()
            {
                LastName = _lastName
            };

            Assert.IsNull(result.LastName);

        }


        [TestMethod]
        public void Contractor_Constructor_ReturnIDAreNotEqual()
        {
            Contractor test1 = new Contractor() { };
            Contractor test2 = new Contractor() { };

            Assert.AreNotEqual(test1.Id, test2.Id);
        }

        [TestMethod]
        [DataRow(-29193)]
        [DataRow(-55)]
        [DataRow(123)]
        [DataRow(8658)]
        [DataRow(123189)]
        public void Contractor_Constructor_ReturnWageAreNotEqual(double _hourlyWage)
        {
            Contractor testContractor = new Contractor()
            {
                HourlyWage = _hourlyWage
            };

            Assert.AreNotEqual(_hourlyWage, testContractor.HourlyWage);

        }


    }
}
