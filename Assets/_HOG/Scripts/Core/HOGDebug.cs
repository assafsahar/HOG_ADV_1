using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace HOG.Core
{
    public class HOGDebug 
    {
        [Conditional("LOGS_ENABLE")]
       public static void Log(object obj)
        {
            UnityEngine.Debug.Log(obj.ToString());
        }
    }
}
