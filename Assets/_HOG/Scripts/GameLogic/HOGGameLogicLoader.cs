using HOG.Core;
using System;

namespace HOG.GameLogic
{
    public class HOGGameLogicLoader : HOGGameLoaderBase
    {
        public override void StartLoad(Action onComplete)
        {
            HOGGameLogicManager hogGameLogicManager = new HOGGameLogicManager();
            hogGameLogicManager.LoadManager( () =>
            {
                base.StartLoad(onComplete);
            });
        }
    }
}
