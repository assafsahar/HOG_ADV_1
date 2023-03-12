using DG.Tweening;
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
        private RectTransform readyTextRectTransform;
        private Vector3 readyTextOriginalScale;
        public override void Init()
        {
            ScreenName = HOGScreenNames.GameScreen;
            
        }

        private void Awake()
        {
            RectTransform rectTransform;
            var isRectTransform = readyText.TryGetComponent<RectTransform>(out rectTransform);
            readyTextRectTransform = rectTransform;
            readyTextOriginalScale = readyTextRectTransform.localScale;
            //readyTextRectTransform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
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
                readyTextRectTransform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                readyTextRectTransform.DOScale(readyTextOriginalScale, 0.5f);
                readyText.enabled = toEnable;
                StartCoroutine(EnableReadyText(false));
                yield break;
            }
            yield return new WaitForSeconds(2f);
            readyText.CrossFadeAlpha(0, 0.5f, false);
            yield return new WaitForSeconds(0.5f);
            readyText.enabled = toEnable;
            InvokeEvent(HOGEventNames.OnFightReady);
            yield break;
        }
     

    }

}
