using UnityEngine;
using UnityEngine.SceneManagement;

namespace HOG.Core
{
    public class HOGGameLoader : HOGMonoBehaviour
    {
        [SerializeField] private HOGGameLoaderBase gameLogicLoader;
        [SerializeField] private HOGLoadBarComponent loadbarComponent;
        private void Start()
        {
            //Invoke("DelayStart", 0.1f);
            loadbarComponent.SetTargetAmount(20);
            WaitForSeconds(2, DelayStart);
        }

        private void DelayStart()
        {
            var manager = new HOGManager();
            loadbarComponent.SetTargetAmount(40);
            manager.LoadManager(() =>
            {
                WaitForSeconds(2, () =>
                {
                    loadbarComponent.SetTargetAmount(98);
                    gameLogicLoader.StartLoad(() =>
                    {
                        WaitForSeconds(2, () =>
                        {
                            loadbarComponent.SetTargetAmount(100);
                            SceneManager.LoadScene(1);
                            Destroy(this.gameObject);
                        });
                    });
                });
                    
                
                
            });
            
        }
    }
}

