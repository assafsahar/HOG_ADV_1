using DG.Tweening;
using HOG.Core;
using System.Collections;
using TMPro;
using UnityEngine;

namespace HOG.Screens
{
    
    public class HOGGameScreen : HOGScreenBase
    {
        [SerializeField] TextMeshProUGUI readyText;
        private RectTransform readyTextRectTransform;
        private Vector3 readyTextOriginalScale;

        private void Awake()
        {
            RectTransform rectTransform;
            var isRectTransform = readyText.TryGetComponent<RectTransform>(out rectTransform);
            readyTextRectTransform = rectTransform;
            readyTextOriginalScale = readyTextRectTransform.localScale;
            //readyTextRectTransform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        }
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

        private IEnumerator EnableReadyText(bool toEnable)
        {
            readyText.CrossFadeAlpha(1, 0.1f, false);
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