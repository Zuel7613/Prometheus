namespace ApiTestProject.TestCases
{
    [TestFixture]
    public abstract class BaseTest
    {
        [OneTimeSetUp]
        public virtual void OneTimeSetup()
        {
        }

        [OneTimeTearDown]
        public virtual void OneTimeTeardown()
        {
        }
    }
}
