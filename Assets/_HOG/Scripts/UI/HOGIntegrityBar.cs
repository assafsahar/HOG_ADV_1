using HOG.Core;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace HOG.Screens
{
    public class HOGIntegrityBar : HOGMonoBehaviour
    {
        [SerializeField] Slider slider;

        public void SetValue(float percentage)
        {
            DOTween.To(() => slider.value, x => slider.value = x, percentage, 0.5f);
        }
    }
}