using HOG.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HOG.Screens
{
    public class HOGScreenBase : HOGMonoBehaviour
    {
        public HOGScreenNames ScreenName { get; set; }

        public virtual void Init()
        {

        }
        public void EnableScreen()
        {
            gameObject.SetActive(true);
        }
        public void DisableScreen()
        {
            gameObject.SetActive(false);
        }
    }
    public enum HOGScreenNames
    {
        OpeningScreen,
        GameScreen,
        SummaryScreen
    }
}
