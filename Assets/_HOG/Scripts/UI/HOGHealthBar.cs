using HOG.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace HOG.Screens
{
    
    public class HOGHealthBar : HOGMonoBehaviour
    {
        [SerializeField] Slider slider;

        public void SetHealth(float healthPercentage)
        {
            //slider.value = healthPercentage;

            DOTween.To(() => slider.value, x => slider.value = x, healthPercentage, 0.5f);
        }
    }
}

