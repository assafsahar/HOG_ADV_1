using HOG.Core;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

namespace HOG.UI
{
    public class HOGEnergyBarUI : HOGMonoBehaviour
    {
        [SerializeField] private Slider slider;
        [SerializeField] private float fillDuration = 0.5f;
        public float maxValue = 1;

        public void SetEnergy(float value)
        {
            value = Mathf.Clamp(value, 0f, maxValue);
            DOTween.To(() => slider.value, x => slider.value = x, value, fillDuration)
                .OnUpdate(() => UpdateFillAmount(slider.value))
                .SetEase(Ease.Linear);
        }

        private void UpdateFillAmount(float currentValue)
        {
            HOGDebug.Log($"currentValue={currentValue}");
        }
    }
}
