using DG.Tweening;
using HOG.Character;
using HOG.Core;
using System;
using System.Collections;
using UnityEngine;

namespace HOG.GameLogic.VFX
{
    public class HOGVFXController : HOGMonoBehaviour
    {
        [Header("Particle Effect")]
        [SerializeField] private ParticleSystem[] particleEffects; 
        [SerializeField] private float effectDuration = 0.4f;
        [Header("Movement Settings")]
        [SerializeField] private Transform pointA;
        [SerializeField] private Transform pointB;
        [SerializeField] private float moveDuration = 0.5f;
        [SerializeField] private Ease moveEase = Ease.Linear;
        [SerializeField] private float effectDelay = 0.2f;

        private int characterNumber = 1;
        private ParticleSystem activeEffect;
        private Coroutine vfxCoroutine;
        private bool movingToB = false;

        private void Awake()
        {
            if (particleEffects.Length == 0)
            {
                Debug.LogError("HOGVFXController: No ParticleSystems assigned!");
                return;
            }
            StopAllEffects();
        }

        private void OnEnable()
        {
            AddListener(HOGEventNames.OnEffectTriggered, OnEffectTriggered);
        }
        private void OnDisable()
        {
            RemoveListener(HOGEventNames.OnEffectTriggered, OnEffectTriggered);
        }

        private void OnEffectTriggered(object obj)
        {
            if (obj is Tuple<HOGCharacter, HOGCharacterActionBase> tupleData)
            {
                int triggeringCharacterNumber = tupleData.Item1.characterNumber;

                /*if (triggeringCharacterNumber != characterNumber)
                {
                    return;
                }*/

                SetStartingPosition(triggeringCharacterNumber);

                StartCoroutine(PlayEffectWithDelay());
            }
        }

        private void StopAllEffects()
        {
            foreach (var effect in particleEffects)
            {
                if (effect.isPlaying)
                {
                    effect.Stop();
                }
            }
        }

        private IEnumerator PlayEffectWithDelay()
        {
            yield return new WaitForSeconds(effectDelay);
            PlayEffect();
        }

        private void PlayEffect()
        {
            if (vfxCoroutine != null)
            {
                StopCoroutine(vfxCoroutine);
            }
            vfxCoroutine = StartCoroutine(EffectRoutine());

            MoveEffect();
        }

        private IEnumerator EffectRoutine()
        {
            if (particleEffects.Length == 0)
            {
                yield break;
            }

            activeEffect = particleEffects[UnityEngine.Random.Range(0, particleEffects.Length)];
            activeEffect.Play(); 

            yield return new WaitForSeconds(effectDuration);

            activeEffect.Stop();
            InvokeEvent(HOGEventNames.OnEffectEnded, this);
        }

        private void MoveEffect()
        {
            if (pointA == null || pointB == null)
            {
                Debug.LogError("HOGVFXController: Move points not assigned!");
                return;
            }

            Transform target = movingToB ? pointB : pointA;
            transform.DOMove(target.position, moveDuration).SetEase(moveEase).OnComplete(() =>
            {
                movingToB = !movingToB;

            });
        }

        private void StopEffect()
        {
            if (vfxCoroutine != null)
            {
                StopCoroutine(vfxCoroutine);
                vfxCoroutine = null;
            }

            if (activeEffect != null)
            {
                activeEffect.Stop();
            }
            transform.DOKill();
        }

        private void SetStartingPosition(int triggeringCharacterNumber)
        {
            characterNumber = triggeringCharacterNumber;

            if (characterNumber == 1)
            {
                transform.position = pointB.position;
                movingToB = false;
            }
            else if (characterNumber == 2)
            {
                transform.position = pointA.position;
                movingToB = true;
            }
            else
            {
                Debug.LogError("HOGVFXController: Invalid character number assigned!");
            }
        }
    }
}
