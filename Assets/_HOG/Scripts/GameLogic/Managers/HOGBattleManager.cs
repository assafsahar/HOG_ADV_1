using HOG.Character;
using HOG.Core;
using HOG.Screens;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.TextCore.Text;

namespace HOG.GameLogic
{
    public class HOGBattleManager : HOGMonoBehaviour
    {
        [SerializeField] HOGCharacter[] characters;
        [SerializeField] HOGScreenManager screenManager;
        [SerializeField] HOGDeckManager deckManager;
        [SerializeField] private float maxDistance = 12f;
        [SerializeField] private float minDistance = 0f;

        public static HOGBattleManager Instance { get; private set; }
        public int Turn
        {
            get { return turn; }
            private set { turn = value; }
        }

        private float distance;
        private HOGCharacter character1;
        private HOGCharacter character2;
        private HOGCharacterStats character1Stats;
        private HOGCharacterStats character2Stats;
        private HOGCharacter chosenCharacter;
        private IEnumerator fightCoroutine = null;
        private bool isFightLive = false;
        private int turn;
        Vector3 character1OriginalPosition;
        Vector3 character2OriginalPosition;
        

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
            if (Instance == null)
            {
                Instance = this;  
            }
            else if (Instance != this)
            {
                Destroy(gameObject);  
            }

            Manager.PoolManager.InitPool("TextToast", 10);
            if (characters[0] != null)
            {
                InitCharacter1();
            }
            if (characters[1] != null)
            {
                InitCharacter2();
            }
            character1OriginalPosition = character1.transform.position;
            character2OriginalPosition = character2.transform.position;
        }

        private void Update()
        {
            if (isFightLive)
            {
                UpdateDistance();
            }
        }

        public void PreFight(object obj)
        {
            character1.PreFight();
            character2.PreFight();
            character1Stats.ResetStats(null);
            character2Stats.ResetStats(null);
            character1.transform.position = character1OriginalPosition;
            character2.transform.position = character2OriginalPosition;
            distance = character2.transform.position.x - character1.transform.position.x;
            InvokeEvent(HOGEventNames.OnPreFightReady);
        }

        public void StartFight(object obj)
        {
            SoundManager.Instance.PlayBackgroundMusic();
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
            SoundManager.Instance.StopBackgroundMusic();
            isFightLive = false;
            if (fightCoroutine != null)
            {
                StopCoroutine(fightCoroutine);
                fightCoroutine = null;
            }
            if (character1.PlayActionSequence() != null)
            {
                StopCoroutine(character1.PlayActionSequence());
            }
            if (character2.PlayActionSequence() != null)
            {
                StopCoroutine(character2.PlayActionSequence());
            }
        }

        public float GetDistance()
        {
            return distance;
        }

        private void InitCharacter1()
        {
            if (characters[0].TryGetComponent<HOGCharacter>(out HOGCharacter character))
            {
                character1 = character;
                character1.Init();
                character1Stats = character1.GetComponent<HOGCharacterStats>();
            }
        }

        private void InitCharacter2()
        {
            if (characters[1].TryGetComponent<HOGCharacter>(out HOGCharacter character))
            {
                character2 = character;
                character2.Init();
                character2Stats = character2.GetComponent<HOGCharacterStats>();
            }
        }

        private void UpdateDistance()
        {
            if (character1Stats != null && character2Stats != null)
            {
                int speed1 = character1Stats.speed;
                int speed2 = character2Stats.speed;
                float speedDifference = speed2 - speed1;
                distance += speedDifference * Time.deltaTime;
                distance = Mathf.Clamp(distance, minDistance, maxDistance);
                character1.transform.position = new Vector3(character2.transform.position.x - distance, character1.transform.position.y, character1.transform.position.z);
                if (distance >= maxDistance)
                {
                    TriggerChasedVictory();
                }
                else if (distance <= minDistance) // reached 0 distance
                {
                    TriggerCloseFighting();
                }
            }
        }

        private void TriggerCloseFighting()
        {
            //HOGDebug.Log("Reached 0 distance, Starting close fighting");
        }

        private void TriggerChasedVictory()
        {
            character2.PlayWin();
            KillCharacter(1);
            StopFight();
            StartCoroutine(screenManager.EnableScreen(HOGScreenNames.OpeningScreen, 2f));
        }

        private void PlayHit(object obj)
        {
            if (obj is Tuple<HOGCharacter, HOGCharacterActionBase> otherCharacterData)
            {
                int num = (int)otherCharacterData.Item1.characterNumber==1?1:0;
                characters[num].PlayHit();
                if (!characters[num].IsDead)
                {
                    StartCoroutine(PlayIdle(num, 0.5f));
                }
            }
        }
        private IEnumerator PlayIdle(int characterIndexNumber, float timer)
        {
            yield return new WaitForSeconds(timer);
            //int num = (int)obj;
            characters[characterIndexNumber].StartIdle();
        }

        private void KillCharacter(object obj)
        {
            //HOGDebug.Log("Character died: " + obj.ToString());
            int num = (int)obj;
            int index = num - 1;

            if (index >= 0 && index < characters.Length)
            {
                characters[index].Die();
            }
            else
            {
                HOGDebug.LogError($"Invalid character number: {num}");
                return;
            }

            int winningCharacterIndex = (num == 1) ? 1 : 0;

            if (winningCharacterIndex >= 0 && winningCharacterIndex < characters.Length)
            {
                characters[winningCharacterIndex].PlayWin();
            }
            else
            {
                HOGDebug.LogError($"Invalid winning character index: {winningCharacterIndex}");
            }

            StopFight();
            StartCoroutine(screenManager.EnableScreen(HOGScreenNames.OpeningScreen, 4f));
        }
    }
}