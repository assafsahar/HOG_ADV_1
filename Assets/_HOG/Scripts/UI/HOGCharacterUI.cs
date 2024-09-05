using HOG.Screens;
using UnityEngine;

public class HOGCharacterUI : MonoBehaviour
{
    [SerializeField] private HOGIntegrityBar integrityBar;

    public void UpdateIntegrityBar(float integrityPercentage)
    {
        integrityBar.SetValue(integrityPercentage);
    }
}
