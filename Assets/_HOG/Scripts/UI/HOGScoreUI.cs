using HOG.Core;
using TMPro;
using UnityEngine;

namespace HOG.UI
{
    public class HOGScoreUI : HOGMonoBehaviour
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