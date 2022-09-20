using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Tooltip", menuName = "Tooltip")]
public class ToolTip : ScriptableObject
{
    [SerializeField] private string toolTipName;
    [SerializeField] private string toolTipDescription;
    [SerializeField] private KeyCode keyToCloseTip;

    public void LoadToolTip(Text toolTipNameText, Text toolTipDescriptionText)
    {
        toolTipNameText.text = toolTipName;
        toolTipDescriptionText.text = toolTipDescription;
    }

    public void ToolTipComplete(ToolTipSystem toolTipSystem)
    {
        if (Input.GetKeyDown(keyToCloseTip))
        {
            toolTipSystem.HideToolTip();
        }
    }

    public bool ButtonAlreadyPressed()
    {
        if (Input.GetKeyDown(keyToCloseTip))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
