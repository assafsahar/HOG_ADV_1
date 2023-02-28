using HOG.Character;
using HOG.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HOGBattleManager : HOGMonoBehaviour
{
    [SerializeField] HOGCharacter[] characters;
    private HOGCharacter character1;
    private HOGCharacter character2;
    private HOGCharacter chosenCharacter;
    private Coroutine fightCoroutine = null;

    private void OnEnable()
    {
        AddListener(HOGEventNames.OnAttacksFinish, PlayOpponent);
        AddListener(HOGEventNames.OnGameStart, StartFight);
        AddListener(HOGEventNames.OnCharacterDied, KillCharacter);
    }

    private void OnDisable()
    {
        RemoveListener(HOGEventNames.OnAttacksFinish, PlayOpponent);
        RemoveListener(HOGEventNames.OnGameStart, StartFight);
        RemoveListener(HOGEventNames.OnCharacterDied, KillCharacter);
    }
    private void Awake()
    {
        
        if(characters[0] != null)
        {
            if(characters[0].TryGetComponent<HOGCharacter>(out HOGCharacter character))
            {
                character1 = character;
            }
        }
        if (characters[1] != null)
        {
            if (characters[1].TryGetComponent<HOGCharacter>(out HOGCharacter character))
            {
                character2 = character;
            }
        }
    }

    public void StartFight(object obj)
    {
        if(obj == null)
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
        /*if ((int)obj == 1)
        {
            character1.Die();
            
        }
        else if ((int)obj == 2)
        {
            character2.Die();
            StopFight();
        }*/
    }

}
