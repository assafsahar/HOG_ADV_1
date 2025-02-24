using HOG.Core;
using HOG.Screens;
using UnityEngine;

namespace HOG.UI
{

    public class HOGCharacterUI : HOGMonoBehaviour
    {
        [SerializeField] private HOGIntegrityBar integrityBar;

        public void UpdateIntegrityBar(float integrityPercentage)
        {
            integrityBar.SetValue(integrityPercentage);
        }
    }
}
