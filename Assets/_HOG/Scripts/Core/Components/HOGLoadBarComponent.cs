
using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace HOG.Core
{
    public class HOGLoadBarComponent : HOGMonoBehaviour
    {
        [SerializeField] private Image loadingImage;
        [SerializeField] private TMP_Text loaderNumber;
        [SerializeField] private float fillSpeed = 1;
        [SerializeField] float animationDuration = 1.0f;


        private float targetAmount = 0;
        private int currentAmount = 0;
        private bool isAnimating = false;

        private void Awake()
        {
            loadingImage.fillAmount = 0;
            UpdateView();
        }

        public void SetTargetAmount(float amount)
        {
            HOGDebug.Log($"SetTargetAmount called with amount: {amount}");
            DOTween.KillAll();
            if (isAnimating)
            {
                StopAllCoroutines();
            }
            targetAmount = amount;
            UpdateView();
        }

        private void UpdateView()
        {
            loadingImage.DOFillAmount(targetAmount / 100, fillSpeed).SetEase(Ease.Linear);
            StartCoroutine(AnimateToTargetAmount((int)targetAmount));
        }

        private IEnumerator AnimateToTargetAmount(int targetAmount)
        {
            isAnimating = true;
            float elapsedTime = 0.0f;
            int startAmount = currentAmount;

            while (elapsedTime < animationDuration)
            {
                elapsedTime += Time.deltaTime;

                float t = Mathf.Clamp01(elapsedTime / animationDuration);
                currentAmount = Mathf.RoundToInt(Mathf.Lerp(startAmount, targetAmount, t));
                loaderNumber.text = currentAmount.ToString("N0") + "%"; ;
                //HOGDebug.Log($"from while loop, currentAmount={currentAmount}");
                yield return null;
            }

            currentAmount = targetAmount;
            loaderNumber.text = currentAmount.ToString("N0") + "%";
            isAnimating = false;
        }

    }
}
