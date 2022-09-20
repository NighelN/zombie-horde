using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolTipSystem : MonoBehaviour
{
    [System.Serializable]
    public class ToolTipData
    {
        public ToolTip toolTip;
        [HideInInspector] public bool shown = false;
    }

    [Header("Reference")]
    [SerializeField] private GameObject toolTipHolder;
    [SerializeField] private Text toolTipNameText;
    [SerializeField] private Text toolTipDescriptionText;
    [Space]
    [Header("Tooltips")]
    [SerializeField] private ToolTipData reloadGun;
    [SerializeField] private ToolTipData openInventory;
    [SerializeField] private ToolTipData openCrafting;
    [SerializeField] private ToolTipData movement;
    [Space]
    [Header("Settings")]
    [SerializeField] private float toolTipCooldownSec = 5;
    [SerializeField] private float toolTipShowTimeSec = 15;

    private float tooltipDelay = 0;
    private ToolTip currentToolTip;
    public static bool showReload = false;

    private void Start()
    {
        ShowToolTip(movement);
    }

    // Update is called once per frame
    void Update()
    {
        //Checks if the pause ment isn't open and the player is alive
        if (!OpenPauseMenu.pauseMenuOpen && !ConsoleHandler.instance.consoleOpened && PlayerHealth.playerAlive)
        {
            //Checks if the current tooltip isn't null and has activated himself
            if (currentToolTip && toolTipHolder.activeSelf)
            {
                currentToolTip.ToolTipComplete(this);
            }
            //Checks if the inventory one hasn't been shown yet
            if (!openInventory.shown)
            {
                openInventory.shown = openInventory.toolTip.ButtonAlreadyPressed();
            }
            //Checks if the crafting one hasn't been shown yet
            if (!openCrafting.shown)
            {
                openCrafting.shown = openCrafting.toolTip.ButtonAlreadyPressed();
            }
            //Checks if the reload gun one hasn't been shown yet
            if (!reloadGun.shown)
            {
                reloadGun.shown = reloadGun.toolTip.ButtonAlreadyPressed();
            }

            //Checks if the time exceeded the tooltip delay
            if (Time.time > tooltipDelay && !toolTipHolder.activeSelf)
            {
                //Calculates a random range
                float chance = Random.Range(0, 100);
                //Chance is lower then 50
                if (chance < 50 && !openInventory.shown)
                {
                    ShowToolTip(openInventory);
                }
                //Chance lower then 100
                else if (chance < 100 && !openCrafting.shown)
                {
                    ShowToolTip(openCrafting);
                }
            }
            if (showReload && !reloadGun.shown)
            {
                ShowToolTip(reloadGun);
            }
        }
    }

    public void ShowToolTip(ToolTipData tooltip)
    {
        toolTipHolder.SetActive(true);
        tooltip.shown = true;
        tooltip.toolTip.LoadToolTip(toolTipNameText, toolTipDescriptionText);
        currentToolTip = tooltip.toolTip;
        StartCoroutine(HideTimer());
    }

    private IEnumerator HideTimer()
    {
        yield return new WaitForSeconds(toolTipShowTimeSec);
        HideToolTip();
    }

    public void HideToolTip()
    {
        toolTipHolder.SetActive(false);
        tooltipDelay = Time.time + toolTipCooldownSec;
    }
}
