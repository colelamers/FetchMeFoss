using FetchMeFoss.Models;

namespace TestsFetchMeFoss
{
    [TestClass]
    public class TestFetchMeFoss
    {
        [TestMethod]
        public void TestMethod1()
        {
            Init.Initialization<Configuration> init = new Init.Initialization<Configuration>();
            if (init == null)
            {
                throw new Exception("Xml configuration is not valid. Initialization Failed");
            }
        }

        [TestMethod]
        public void TestMethod2()
        {
            // todo 1; need a test to run through every potential interface and ensure none fail.
            // this means that you cannot forget one as the dynamic casting will fail if one is
            // missing
        }
    }
}