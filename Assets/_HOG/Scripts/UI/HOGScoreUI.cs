using TMPro;
using UnityEngine;

namespace HOG.UI
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