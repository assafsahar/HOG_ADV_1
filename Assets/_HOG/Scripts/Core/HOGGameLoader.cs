using UnityEngine;
using UnityEngine.SceneManagement;

namespace HOG.Core
{
    public class HOGGameLoader : HOGMonoBehaviour
    {
        [SerializeField] private HOGGameLoaderBase gameLogicLoader;
        private void Start()
        {
            Invoke("DelayStart", 0.1f);
        }

        private void DelayStart()
        {
            var manager = new HOGManager();
            manager.LoadManager(() =>
            {
                gameLogicLoader.StartLoad(() =>
                {
                    SceneManager.LoadScene(1);
                    Destroy(this.gameObject);
                });
            });
            
        }
    }
}

