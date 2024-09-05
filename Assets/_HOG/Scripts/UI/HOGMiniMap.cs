using Codice.CM.Common;
using HOG.Core;
using HOG.Screens;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace HOG.UI
{
    public class HOGMiniMap : HOGMonoBehaviour
    {

        //[SerializeField] Animator animator;
        [SerializeField] PlayableDirector playableDirector;  // Reference to the PlayableDirector component

        private void OnEnable()
        {
            AddListener(HOGEventNames.OnPreFightReady, StartMiniMap);
        }


        private void OnDisable()
        {
           
            RemoveListener(HOGEventNames.OnPreFightReady, StartMiniMap);
        }

        private void Awake()
        {
          
        }

        private void Start()
        {
            
        }
       

        private void StartMiniMap(object obj)
        {
            playableDirector.Play();

        }

        private void DisableAll()
        {
            
        }
    }
}
///



