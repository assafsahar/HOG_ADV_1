using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace UI
{
    public class HOGScoreUI : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI score1;

       
        void Start()
        {
            score1.text = "000";
        }

        public void UpdateText(string scoreText)
        {
            score1.text = scoreText;
        }

    }
}

