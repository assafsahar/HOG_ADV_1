using HOG.Character;
using HOG.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HOGBattleManager : HOGMonoBehaviour
{
    [SerializeField] GameObject[] characters;
    private HOGCharacter character1;
    private HOGCharacter character2;

    private void OnEnable()
    {
        AddListener(HOGEventNames.OnAttacksFinish, PlayCharacter);
        AddListener(HOGEventNames.OnGameStart, StartFight);
    }
    private void OnDisable()
    {
        RemoveListener(HOGEventNames.OnAttacksFinish, PlayCharacter);
        RemoveListener(HOGEventNames.OnGameStart, StartFight);
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
            PlayCharacter(2);
            return;
        }
        PlayCharacter((int)obj);

        
    }

    public void PlayCharacter(object previousPlayedCharacter)
    {
        HOGCharacter chosenCharacter;
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
            StartCoroutine(chosenCharacter.PlayActionSequence());
        }
    }

}
