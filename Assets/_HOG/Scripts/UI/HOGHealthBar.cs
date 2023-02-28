using HOG.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HOG.Screens
{
    
    public class HOGHealthBar : HOGMonoBehaviour
    {
        [SerializeField] Slider slider;

        public void SetHealth(float healthPercentage)
        {
            slider.value = healthPercentage;
        }
    }
}

