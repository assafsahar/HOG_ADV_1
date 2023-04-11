using HOG.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HOGCardClicker : HOGMonoBehaviour
{
    // the following methods are called from the UI buttons (editor)
    public void ChangeAttack(int amount)
    {
        InvokeEvent(HOGEventNames.OnAbilityChange, new Tuple<HOGCharacterState.CharacterStates, int>(HOGCharacterState.CharacterStates.Attack, amount));
    }

    public void changeCharacter(int characterNumber)
    {
        InvokeEvent(HOGEventNames.OnCharacterChange, characterNumber - 1);
    }
}
