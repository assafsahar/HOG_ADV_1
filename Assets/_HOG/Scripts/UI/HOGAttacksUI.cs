using DG.Tweening;
using HOG.Core;
using System;
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
        [SerializeField] TextMeshProUGUI characterTypeText;
        [SerializeField] RectTransform ActivePanel;
        float[] panelPositions = new float[3] {0,0,0};
        public Dictionary<int, (char,string)> slots = new Dictionary<int, (char, string)>();
        private Vector3 slot1TextOriginalScale;
        private TextMeshProUGUI[,] textArray;
        public void Init(Action onComplete)
        {
            textArray = new TextMeshProUGUI[3,2];
            textArray[0,0] = slot1Text;
            textArray[0,1] = slot1TextStrength;
            textArray[1,0] = slot2Text;
            textArray[1,1] = slot2TextStrength;
            textArray[2,0] = slot3Text;
            textArray[2,1] = slot3TextStrength;
            slot1TextOriginalScale = slot1Text.transform.localScale;
            FillPanelPositions();
            FillDictionary();
            ShowDictionary();
            onComplete.Invoke();
        }

        private void FillPanelPositions()
        {
            panelPositions[0] = slot1Text.transform.position.x;
            panelPositions[1] = slot2Text.transform.position.x;
            panelPositions[2] = slot3Text.transform.position.x;
        }

        private void FillDictionary()
        {
            if (slots.Count == 0)
            {
                slots.Add(1, ('W', "1"));
                slots.Add(2, ('A', "2"));
                slots.Add(3, ('W', "1"));
            }
        }

        public void UpdateAttackText(int slotNumber, char attackText, string attackStrength)
        {
            slots[slotNumber] = (attackText, attackStrength);
            ShowDictionary(slotNumber);
        }
        public void UpdateCharacterTypeText(int type)
        {
            characterTypeText.text = type.ToString();
        }
        public void ShowActiveSlot(int slotNumber)
        {
            ActivePanel.position = new Vector3(panelPositions[slotNumber - 1], ActivePanel.position.y, ActivePanel.position.z);
        }

        private void ShowDictionary(int slotNumber = -1)
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

            if(slotNumber != -1)
            {
                ScaleText(textArray[slotNumber - 1, 0], slot1TextOriginalScale);
                ScaleText(textArray[slotNumber - 1, 1], slot1TextOriginalScale);
                return;
            }
            ScaleText(slot1Text, slot1TextOriginalScale);
                ScaleText(slot1TextStrength, slot1TextOriginalScale);
                ScaleText(slot2Text, slot1TextOriginalScale);
                ScaleText(slot2TextStrength, slot1TextOriginalScale);
                ScaleText(slot3Text, slot1TextOriginalScale);
                ScaleText(slot3TextStrength, slot1TextOriginalScale);
            
        }

        private void ScaleText(TextMeshProUGUI text, Vector3 originalScale)
        {
            text.transform.DOScale(originalScale * 1.7f, 0.5f)
                .SetEase(Ease.OutCubic)
                .OnComplete(() =>
                {
                    text.transform.DOScale(originalScale, 0.5f)
                .SetEase(Ease.InCubic);
                });
        }
    }
}

