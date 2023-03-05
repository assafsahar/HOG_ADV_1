using HOG.Character;
using HOG.Core;
using HOG.Screens;
using System;
using System.Collections;
using UnityEngine;

namespace HOG.GameLogic
{
    public class HOGBattleManager : HOGMonoBehaviour
    {
        [SerializeField] HOGCharacter[] characters;
        [SerializeField] HOGScreenManager screenManager;
        private HOGCharacter character1;
        private HOGCharacter character2;
        private HOGCharacter chosenCharacter;
        private Coroutine fightCoroutine = null;
        

    private void OnEnable()
        {
            AddListener(HOGEventNames.OnAttacksFinish, PlayOpponent);
            AddListener(HOGEventNames.OnGameStart, StartFight);
            AddListener(HOGEventNames.OnCharacterDied, KillCharacter);
            AddListener(HOGEventNames.OnGetHit, PlayHit);
        }

        private void PlayHit(object obj)
        {
            int num = (int)obj;
            characters[num - 1].PlayHit();
            if(!characters[num - 1].IsDead)
            {
                StartCoroutine(PlayIdle(obj, 0.5f));
            }
            
        }
        IEnumerator PlayIdle(object obj, float timer)
        {
            yield return new WaitForSeconds(timer);
            int num = (int)obj;
            characters[num - 1].StartIdle();
        }

        private void OnDisable()
        {
            RemoveListener(HOGEventNames.OnAttacksFinish, PlayOpponent);
            RemoveListener(HOGEventNames.OnGameStart, StartFight);
            RemoveListener(HOGEventNames.OnCharacterDied, KillCharacter);
            RemoveListener(HOGEventNames.OnGetHit, PlayHit);
        }
        private void Awake()
        {
            if (characters[0] != null)
            {
                if (characters[0].TryGetComponent<HOGCharacter>(out HOGCharacter character))
                {
                    character1 = character;
                    character1.Init();
                }
            }
            if (characters[1] != null)
            {
                if (characters[1].TryGetComponent<HOGCharacter>(out HOGCharacter character))
                {
                    character2 = character;
                    character2.Init();
                }
            }
        }

        public void StartFight(object obj)
        {

            character1.PreFight();
            character2.PreFight();
            if (obj == null)
            {
                PlayOpponent(2);
                return;
            }
            PlayOpponent((int)obj);


        }

        public void PlayOpponent(object previousPlayedCharacter)
        {

            if ((int)previousPlayedCharacter == 2)
            {
                chosenCharacter = character1;
            }
            else
            {
                chosenCharacter = character2;
            }
            if (chosenCharacter != null)
            {
                fightCoroutine = StartCoroutine(chosenCharacter.PlayActionSequence());
            }
        }

        public void StopFight()
        {
            StopCoroutine(fightCoroutine);
        }

        private void KillCharacter(object obj)
        {
            Debug.Log("Character died: " + obj.ToString());
            int num = (int)obj;
            characters[num - 1].Die();
            StopFight();
            StartCoroutine(screenManager.EnableScreen(HOGScreenNames.OpeningScreen, 2f));
        }

    }
}

