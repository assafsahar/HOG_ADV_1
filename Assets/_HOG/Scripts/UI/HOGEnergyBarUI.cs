using HOG.Core;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;

namespace HOG.UI
{
    public class HOGEnergyBarUI : HOGMonoBehaviour
    {
        [SerializeField] private Slider slider;
        [SerializeField] private float fillDuration = 0.5f;
        [SerializeField] private GameObject missingEnergyIndicator;
        [SerializeField] private RectTransform energyBar;
        public float maxValue = 1;
        GameObject indicatorClone;
        public void SetEnergy(float value)
        {
            value = Mathf.Clamp(value, 0f, maxValue);
            DOTween.To(() => slider.value, x => slider.value = x, value, fillDuration)
                .OnUpdate(() => UpdateFillAmount(slider.value))
                .SetEase(Ease.Linear);
        }

        public void ShowMissingEnergyIndicator(float missingEnergy)
        {
            
            if (missingEnergyIndicator != null)
            {
                if(indicatorClone != null)
                {
                    Destroy(indicatorClone.gameObject);
                }
                indicatorClone = Instantiate(missingEnergyIndicator, new Vector2(energyBar.transform.position.x - energyBar.rect.width + slider.value * energyBar.rect.width / 10, energyBar.transform.position.y), Quaternion.identity, energyBar.transform);
                RectTransform indicatorRect = indicatorClone.GetComponent<RectTransform>();
                float newWidth = energyBar.rect.width / 10 * missingEnergy;
                indicatorRect.sizeDelta = new Vector2(newWidth, indicatorRect.sizeDelta.y);

                StartCoroutine(BlinkAndDestroy(indicatorClone, 3));
            }
        }

        private IEnumerator BlinkAndDestroy(GameObject indicator, int blinkCount)
        {
            Image indicatorImage = indicator.GetComponent<Image>();
            if (indicatorImage != null)
            {
                Color originalColor = indicatorImage.color;
                Color transparentColor = new Color(originalColor.r, originalColor.g, originalColor.b, 0);

                for (int i = 0; i < blinkCount; i++)
                {
                    indicatorImage.color = transparentColor;
                    yield return new WaitForSeconds(0.2f);

                    indicatorImage.color = originalColor;
                    yield return new WaitForSeconds(0.3f);
                }
            }
            Destroy(indicator);
        }

        private void UpdateFillAmount(float currentValue)
        {
            //HOGDebug.Log($"currentValue={currentValue}");
        }

        
    }
}
