using HOG.Core;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


namespace UI
{
    public class HOGAttacksUI : HOGMonoBehaviour
    {
        [SerializeField] TextMeshProUGUI slot1Text;
        [SerializeField] TextMeshProUGUI slot2Text;
        [SerializeField] TextMeshProUGUI slot3Text;
        [SerializeField] TextMeshProUGUI slot1TextStrength;
        [SerializeField] TextMeshProUGUI slot2TextStrength;
        [SerializeField] TextMeshProUGUI slot3TextStrength;
        [SerializeField] RectTransform ActivePanel;
        [SerializeField] float[] panelPositions = new float[3] {0,0,0};
        public Dictionary<int, (char,string)> slots = new Dictionary<int, (char, string)>();


        public void Init()
        {
            FillDictionary();
            ShowDictionary();
        }

        private void FillDictionary()
        {
            slots.Add(1, ('W', "1"));
            slots.Add(2, ('A', "2"));
            slots.Add(3, ('W', "1"));
        }

        public void UpdateAttackText(int slotNumber, char attackText, string attackStrength)
        {
            slots[slotNumber] = (attackText, attackStrength);
            ShowDictionary();
        }
        public void ShowActiveSlot(int slotNumber)
        {
            ActivePanel.position = new Vector3(panelPositions[slotNumber - 1], ActivePanel.position.y, ActivePanel.position.z);
        }

        private void ShowDictionary()
        {
            if(slot1Text == null)
            {
                return;
            }
            slot1Text.text = "" + slots[1].Item1;
            slot1TextStrength.text = slots[1].Item2;
            slot2Text.text = "" + slots[2].Item1;
            slot2TextStrength.text= slots[2].Item2;
            slot3Text.text = "" + slots[3].Item1;
            slot3TextStrength.text= slots[3].Item2;
        }
    }
}

