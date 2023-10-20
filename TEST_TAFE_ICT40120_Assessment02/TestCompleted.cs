using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TEST_TAFE_ICT40120_Assessment02
{
    [TestClass]
    public class TestCompleted
    {
        [TestMethod]
        public void Completed_Constructor_ReturnNotNull()
        {
            Contractor contractor = new Contractor();
            Job job = new Job();
            Completed testCompleted = new Completed(contractor, job);
            Assert.IsNotNull(testCompleted);
        }
    }
}
