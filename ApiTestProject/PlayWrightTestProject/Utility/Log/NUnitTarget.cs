using NLog;
using NLog.Targets;

namespace PlayWrightTestProject.Utility.Log
{

    [Target(nameof(NUnitTarget))]
    public sealed class NUnitTarget : TargetWithLayout
    {
        protected override void Write(LogEventInfo logEvents)
        {
            var logMessage = RenderLogEvent(Layout, logEvents);
            TestContext.Out.WriteLine(logMessage);
        }
    }
}
