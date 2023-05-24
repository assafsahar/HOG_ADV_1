using HOG.Core;
using UnityEngine;
using DG.Tweening;
using System;

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
            AddListener(HOGEventNames.OnAttackFinish, OnHit);
        }
        private void OnDisable()
        {
            RemoveListener(HOGEventNames.OnAttackFinish, OnHit);
        }
        private void OnHit(object obj)
        {
            if (obj is Tuple<int, int> tupleData)
            {
                if(tupleData.Item2 >= megaHitTreshold)
                {
                    ShakeCamera(tupleData.Item2 * 10);
                }
                
            }
        }

        private void ShakeCamera(int multiplyer)
        {
            transform.DOShakePosition(shakeDuration * multiplyer, baseStrengthShake * multiplyer, shakeVibBase);
        }
    }
}

