using HOG.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HOG.Menus
{
    public class HOGScreenBase : HOGMonoBehaviour
    {

        public void EnableScreen()
        {
            gameObject.SetActive(true);
        }
        public void DisableScreen()
        {
            gameObject.SetActive(false);
        }
    }
}
