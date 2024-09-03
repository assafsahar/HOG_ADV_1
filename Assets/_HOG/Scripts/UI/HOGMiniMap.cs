using HOG.Core;
using HOG.Screens;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HOG.UI
{
    public class HOGMiniMap : HOGMonoBehaviour
    {

       [SerializeField] Animator animator;

        private void OnEnable()
        {
            HOGDebug.Log("animator Enabled:" + animator);
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
            HOGDebug.Log("animator:"+ animator);
            animator.SetBool("start", true);

        }

        private void DisableAll()
        {
            
        }
    }
}
///



