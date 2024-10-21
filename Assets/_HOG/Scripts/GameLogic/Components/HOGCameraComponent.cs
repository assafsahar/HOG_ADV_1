using HOG.Core;
using UnityEngine;
using DG.Tweening;
using System;
using HOG.Character;

namespace HOG.GameLogic
{
    public class HOGCameraComponent : HOGMonoBehaviour
    {
        [SerializeField] int megaHitTreshold = 3;
        private float shakeDuration = 0.01f;
        private float baseStrengthShake = 0.01f;
        private int shakeVibBase = 1;

        private void OnEnable()
        {
            AddListener(HOGEventNames.OnGetHit, OnHit);
        }
        private void OnDisable()
        {
            RemoveListener(HOGEventNames.OnGetHit, OnHit);
        }
        private void OnHit(object obj)
        {
            if (obj is Tuple<HOGCharacter, HOGCharacterActionBase> tupleData)
            {
                Debug.Log($"OnHit, ShakeCamera amount={tupleData.Item2.ActionStrength}");
                ShakeCamera(tupleData.Item2.ActionStrength);
            }
        }

        private void ShakeCamera(int multiplyer)
        {
            Debug.Log("ShakeCamera");
            transform.DOShakePosition(shakeDuration * multiplyer, baseStrengthShake * multiplyer, shakeVibBase);
        }
    }
}

