using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using UnityEngine.Experimental.Rendering.Universal;

public class DayNightCycle : MonoBehaviour
{
    [Header("Time")]
    [SerializeField] [Range(0,24)]private float timeOfDay = 7;
    public int daysPassed = 0;
    [Range(0.05f, 240)] public float dayNightCycleMin = 2;
    [Range(0, 24)] public float startNight = 20;
    [Range(0, 24)] public float endNight = 6;
    [Space]
    [Header("Color")]
    [SerializeField] private AnimationCurve darknessTime;
    [SerializeField] private AnimationCurve redColorTime;
    [SerializeField] private AnimationCurve greenColorTime;
    [SerializeField] private AnimationCurve blueColorTime;
    [Space]
    [Header("Shadows")]
    [SerializeField] private AnimationCurve shadowsXPosition;
    [SerializeField] private AnimationCurve shadowsOpacity;
    [Space]
    [Header("References")]
    [SerializeField] private Light2D globalLight;
    [SerializeField] private Transform shadowsTilemap;
    [SerializeField] private Tilemap shadowImage;
    [SerializeField] private Text dayCounterText;
    [SerializeField] private RectTransform clock;
    public List<SpriteRenderer> movingShadows = new List<SpriteRenderer>();

    private void Start()
    {
        SetDayCounterText();
    }

    // Update is called once per frame
    void Update()
    {
        if (OpenPauseMenu.pauseMenuOpen) return;
        ChangeTimeOfDay();
        ChangeColorsDarknessShadows();
        clockRotation();
    }

    private void ChangeTimeOfDay()
    {
        // Changes the time of day.
        if (timeOfDay >= 24)
        {
            timeOfDay = 0;
            daysPassed++;
            SetDayCounterText();
        }
        else
        {
            timeOfDay += 24 / dayNightCycleMin / 60 * Time.deltaTime;
        }
    }
     
    private void ChangeColorsDarknessShadows()
    {
        // changes the color and opacity of a canvas panel to make it look darker or a different color.
        globalLight.color = new Color(redColorTime.Evaluate(timeOfDay), greenColorTime.Evaluate(timeOfDay), blueColorTime.Evaluate(timeOfDay), 1);
        globalLight.intensity = darknessTime.Evaluate(timeOfDay);
        // Changes the X position of the tile map with shadows so it look like time passes
        shadowsTilemap.position = new Vector3(shadowsXPosition.Evaluate(timeOfDay), 0, 0);
        // changes the opicaty of the shadows so you don't have shadows in the night.
        shadowImage.color = new Color(0, 0, 0, shadowsOpacity.Evaluate(timeOfDay));

        MoveMovingShadows();
    }

    public bool IsNight()
    {
        if (timeOfDay < endNight || timeOfDay > startNight)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void MoveMovingShadows()
    {
        foreach (var movingShadow in movingShadows)
        {
            movingShadow.transform.position = movingShadow.transform.parent.position + new Vector3(shadowsXPosition.Evaluate(timeOfDay), 0, 0);
            movingShadow.color = new Color(0, 0, 0, shadowsOpacity.Evaluate(timeOfDay));
        }
    }

    private void SetDayCounterText()
    {
        dayCounterText.text = "Day " + daysPassed;
    }

    private void clockRotation()
    {
        clock.eulerAngles = new Vector3(0,0, -360 / 24 * timeOfDay);
    }
}
