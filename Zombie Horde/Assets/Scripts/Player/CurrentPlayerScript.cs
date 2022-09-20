using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurrentPlayerScript : MonoBehaviour
{
    [SerializeField] private InputField playerNameInputField;

    public void SetCurrentPlayer()
    {
        PlayerPrefs.SetString("CurrentPlayer", playerNameInputField.text);
    }
}
