using System;
using System.Diagnostics;
using Debug = UnityEngine.Debug;

namespace HOG.Core
{
    public class HOGDebug 
    {
        [Conditional("LOGS_ENABLE")]
       public static void Log(object obj)
        {
            Debug.Log(obj.ToString());
        }

        [Conditional("LOGS_ENABLE")]
        public static void LogException(object obj)
        {
            Debug.LogException(new Exception(obj.ToString()));
        }
    }
}
