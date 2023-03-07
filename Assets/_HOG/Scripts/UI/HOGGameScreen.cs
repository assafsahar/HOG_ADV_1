using HOG.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace HOG.Screens
{
    
    public class HOGGameScreen : HOGScreenBase
    {
        [SerializeField] TextMeshProUGUI readyText;
        public override void Init()
        {
            ScreenName = HOGScreenNames.GameScreen;
            
        }
       
        public override void EnableScreen()
        {
            
            base.EnableScreen();
            InvokeEvent(Core.HOGEventNames.OnGameReset);
            StartCoroutine(EnableReadyText(true));
        }

        IEnumerator EnableReadyText(bool toEnable)
        {
            if (toEnable)
            {
                readyText.enabled = toEnable;
                StartCoroutine(EnableReadyText(false));
                yield break;
            }
            yield return new WaitForSeconds(2f);
            readyText.enabled = toEnable;
            InvokeEvent(HOGEventNames.OnFightReady);
            yield break;
        }
     

    }

}
