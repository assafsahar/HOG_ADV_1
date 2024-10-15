using System.Collections;
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
            if (loadbarComponent == null)
            {
                HOGDebug.LogException("LoadBarComponent is not assigned in HOGGameLoader.");
            }
            else
            {
                HOGDebug.Log("LoadBarComponent is assigned.");
            }
            loadbarComponent.SetTargetAmount(20);
            StartCoroutine(WaitAndExecute(2f, DelayStart));
        }

        private IEnumerator WaitAndExecute(float seconds, System.Action action)
        {
            yield return new WaitForSeconds(seconds);
            action?.Invoke();
        }

        private void DelayStart()
        {
            var manager = new HOGManager();
            loadbarComponent.SetTargetAmount(40);
            manager.LoadManager(() =>
            {
                StartCoroutine(WaitAndExecute(2f, () =>
                {
                    loadbarComponent.SetTargetAmount(98);
                    if (gameLogicLoader == null)
                    {
                        HOGDebug.LogException("gameLogicLoader is not assigned in HOGGameLoader.");
                        return;
                    }
                    HOGDebug.Log("gameLogicLoader is assigned.");
                    gameLogicLoader.StartLoad(() =>
                    {
                        StartCoroutine(WaitAndExecute(2f, () =>
                        {
                            loadbarComponent.SetTargetAmount(100);
                            SceneManager.LoadScene(1);
                            Destroy(this.gameObject);
                        }));
                    });
                }));
            });
        }
    }
}

