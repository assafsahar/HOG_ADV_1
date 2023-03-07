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
        public int Turn { get; private set; }
        

    private void OnEnable()
        {
            AddListener(HOGEventNames.OnAttacksFinish, PlayOpponent);
            AddListener(HOGEventNames.OnGameStart, PreFight);
            AddListener(HOGEventNames.OnCharacterDied, KillCharacter);
            AddListener(HOGEventNames.OnGetHit, PlayHit);
            AddListener(HOGEventNames.OnFightReady, StartFight);
        }
        private void OnDisable()
        {
            RemoveListener(HOGEventNames.OnAttacksFinish, PlayOpponent);
            RemoveListener(HOGEventNames.OnGameStart, PreFight);
            RemoveListener(HOGEventNames.OnCharacterDied, KillCharacter);
            RemoveListener(HOGEventNames.OnGetHit, PlayHit);
            RemoveListener(HOGEventNames.OnFightReady, StartFight);
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

        public void PreFight(object obj)
        {
            character1.PreFight();
            character2.PreFight();
            InvokeEvent(HOGEventNames.OnPreFightReady);
        }

        public void StartFight(object obj)
        {
            if (obj == null)
            {
                PlayOpponent(2);
                return;
            }
            PlayOpponent((int)obj);


        }

        public void PlayOpponent(object previousPlayedCharacter)
        {
            Turn = (int)previousPlayedCharacter == 1 ? 2 : 1;
            InvokeEvent(HOGEventNames.OnTurnChange,Turn);
            chosenCharacter = (int)previousPlayedCharacter == 1 ? character2 : character1;
            
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

