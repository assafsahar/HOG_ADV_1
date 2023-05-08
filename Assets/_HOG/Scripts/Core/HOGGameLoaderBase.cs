using System;

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

