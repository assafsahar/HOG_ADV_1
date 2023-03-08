using HOG.GameLogic;
using System.Collections.Generic;
using UnityEngine;

namespace HOG.Character
{
    [CreateAssetMenu(fileName = "characterAttacks")]
    public class HOGCharacterAttacksScriptable : ScriptableObject
    {
        
        public List<HOGCharacterActionsConfig> AttacksConfig;
    }
}
