using HOG.Character;
using HOG.Core;
using HOG.Screens;
using System.Collections;
using UnityEngine;

namespace HOG.GameLogic
{
    public class HOGBattleManager : HOGMonoBehaviour
    {
        private int turn;
        

        [SerializeField] HOGCharacter[] characters;
        [SerializeField] HOGScreenManager screenManager;
        [SerializeField] HOGDeckManager deckManager;
        private HOGCharacter character1;
        private HOGCharacter character2;
        private HOGCharacter chosenCharacter;
        private IEnumerator fightCoroutine = null;
        private bool isFightLive = true;
        public int Turn {
             get {return turn; }
            private set { turn = value; }
        }

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

        private void Awake()
        {
            Manager.PoolManager.InitPool("TextToast", 10);
            if (characters[0] != null)
            {
                InitCharacter1();
            }
            if (characters[1] != null)
            {
                InitCharacter2();
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
            isFightLive = true;
            if (obj == null)
            {
                PlayOpponent(2);
                return;
            }
            PlayOpponent((int)obj);
        }

        public void PlayOpponent(object previousPlayedCharacter)
        {
            if (!isFightLive)
            {
                return;
            }
            Turn = (int)previousPlayedCharacter == 1 ? 2 : 1;
            InvokeEvent(HOGEventNames.OnTurnChange, Turn);
            chosenCharacter = (int)previousPlayedCharacter == 1 ? character2 : character1;

            if (chosenCharacter != null)
            {
                fightCoroutine = chosenCharacter.PlayActionSequence();
                StartCoroutine(fightCoroutine);
            }
            if(deckManager == null)
            {
                return;
            }
            deckManager.Turn = turn;
            if (Turn == 1)
            {
                deckManager.DisableAllCards();
            }
            else
            {
                deckManager.EnableAllCards();
            }
        }

        public void StopFight()
        {
            isFightLive = false;
            if (character1.PlayActionSequence() != null)
            {
                StopCoroutine(character1.PlayActionSequence());
            }
            if (character2.PlayActionSequence() != null)
            {
                StopCoroutine(character2.PlayActionSequence());
            }
        }

        private void InitCharacter1()
        {
            if (characters[0].TryGetComponent<HOGCharacter>(out HOGCharacter character))
            {
                character1 = character;
                character1.Init();
            }
        }

        private void InitCharacter2()
        {
            if (characters[1].TryGetComponent<HOGCharacter>(out HOGCharacter character))
            {
                character2 = character;
                character2.Init();
            }
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
        private IEnumerator PlayIdle(object obj, float timer)
        {
            yield return new WaitForSeconds(timer);
            int num = (int)obj;
            characters[num - 1].StartIdle();
        }

        private void KillCharacter(object obj)
        {
            HOGDebug.Log("Character died: " + obj.ToString());
            int num = (int)obj;
            characters[num - 1].Die();
            StopFight();
            StartCoroutine(screenManager.EnableScreen(HOGScreenNames.OpeningScreen, 2f));
        }
    }
}