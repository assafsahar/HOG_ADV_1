using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace HOG.Core
{
    public class HOGDebug 
    {
        [Conditional("LOGS_ENABLE")]
       public static void Log(object obj)
        {
            //Debug.Log(obj.ToString());
        }

        [Conditional("LOGS_ENABLE")]
        public static void LogException(object obj)
        {
            Debug.LogException(new Exception(obj.ToString()));
        }
    }
}
