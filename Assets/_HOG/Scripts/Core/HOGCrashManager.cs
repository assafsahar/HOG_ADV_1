using Firebase.Crashlytics;
using System;

namespace HOG.Core
{
    public class HOGCrashManager
    {
        public HOGCrashManager() {
            HOGDebug.Log("HOGCrashManager");
            Crashlytics.ReportUncaughtExceptionsAsFatal = true;
        }
        public void LogExceptionHandling(string message)
        {
            Crashlytics.LogException(new Exception(message));
            HOGDebug.LogException(message);
        }
        public void LogBreadcrumb(string message)
        {
            Crashlytics.Log(message);
            HOGDebug.Log(message);
        }
    }
}
