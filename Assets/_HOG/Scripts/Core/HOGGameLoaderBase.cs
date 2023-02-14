using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HOG.Core
{
    public class HOGGameLoaderBase : HOGMonoBehaviour
    {
        public virtual void StartLoad(Action onComplete)
        {
            onComplete?.Invoke();
        }

    }
}

