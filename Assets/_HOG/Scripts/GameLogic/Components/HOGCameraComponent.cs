using HOG.Core;
using UnityEngine;
using DG.Tweening;
using System;
using HOG.Character;

namespace HOG.GameLogic
{
    public class HOGCameraComponent : HOGMonoBehaviour
    {
        [Header("Camera Settings")]
        [SerializeField] private float zoomedInSize = 2.8f;
        [SerializeField] private float zoomedOutSize = 5f;
        [SerializeField] private float zoomDuration = 0.5f;
        [SerializeField] private Ease zoomEase = Ease.InOutSine;
        [SerializeField] int megaHitTreshold = 3;

        private float shakeDuration = 0.01f;
        private float baseStrengthShake = 0.01f;
        private int shakeVibBase = 1;
        private Camera mainCamera;


        private void Awake()
        {
            mainCamera = GetComponent<Camera>();
        }
        private void OnEnable()
        {
            AddListener(HOGEventNames.OnGetHit, OnHit);
            AddListener(HOGEventNames.OnFightReady, OnFightStarted);
            AddListener(HOGEventNames.OnFightEnded, OnFightEnded);
        }
        private void OnDisable()
        {
            RemoveListener(HOGEventNames.OnGetHit, OnHit);
            RemoveListener(HOGEventNames.OnFightReady, OnFightStarted);
            RemoveListener(HOGEventNames.OnFightEnded, OnFightEnded);
        }

        private void OnFightStarted(object obj)
        {
            ZoomCamera(zoomedInSize);
        }
        private void OnFightEnded(object obj)
        {
            ZoomCamera(zoomedOutSize);
        }

        private void ZoomCamera(float targetSize)
        {
            if (mainCamera != null)
            {
                mainCamera.DOOrthoSize(targetSize, zoomDuration).SetEase(zoomEase);
            }
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

