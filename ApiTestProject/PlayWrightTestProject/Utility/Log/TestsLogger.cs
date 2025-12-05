using NLog;

namespace PlayWrightTestProject.Utility.Log
{
    public class TestsLogger
    {
        public static ILogger Instance
        {
            get
            {
                SetupLogger();

                return LogManager.GetCurrentClassLogger();
            }
        }

        private static void SetupLogger()
        {
            LogManager.Setup().SetupExtensions(ext => ext.RegisterTarget<NUnitTarget>());
            LogManager.Setup().LoadConfigurationFromFile();
        }
    }
}
