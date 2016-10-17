﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PanelLightingColour : MonoBehaviour
{

    public GameObject lightingPanel;
    public GameObject currentLocation;

    public static int curLightMode { get; private set; }
    public static int dayLength { get; private set; }
    public static int[] timeList { get; private set; }
    public static Color[] colorList { get; private set; }
    public static float transitionSpeed { get; private set; }
    public static float transitionSpeedDelay { get; private set; }

    //Local Variables for the Editor Inspector window
    // How fast to move through the Lerp
    public float _transitionSpeed = 0.01f;
    // How many seconds to wait before Lerping further                          
    public float _transitionSpeedDelay = 0.05f;
    // 720 seconds in level 1                   
    public int _dayLength = 720;
    // When to trigger the color switches                                                   
    public int[] _timeList = new int[] { 10, 20, 30, 40 };
    // What color to become
    // The first color is the default color, colour list must be 1 larger than time list.
    public Color[] _colorList = new Color[] { Color.white, Color.white, Color.white, Color.white, Color.white };

    private Image img;

    // Use this for initialization
    void Start()
    {
        img = lightingPanel.GetComponent<Image>();
        img.color = new Color32(0xFF, 0x00, 0x00, 0x2F);

        if (_timeList != null && _colorList != null && _dayLength > 0)
        {
            PanelLightingColour.curLightMode = 0;
            PanelLightingColour.dayLength = _dayLength;
            PanelLightingColour.timeList = _timeList;
            PanelLightingColour.colorList = _colorList;
            PanelLightingColour.transitionSpeed = _transitionSpeed;
            PanelLightingColour.transitionSpeedDelay = _transitionSpeedDelay;
            img.color = _colorList[0]; // Set the default color
        }

        //Debug.Log("CURRENT LOCATION IS: " + currentLocation.GetComponent<Text>().text);

        if (currentLocation.GetComponent<Text>().text.Equals("Office"))
        {
            lightingPanel.SetActive(true);
        }
        else {
            lightingPanel.SetActive(false);
        }
    }


    bool coloursLeft = true;
    //restart coroutine boolean.
    bool colourChanged = false;
    void FixedUpdate()
    {
        if (currentLocation.GetComponent<Text>().text.Equals("Office"))
        {
            lightingPanel.SetActive(true);
        }
        else
        {
            lightingPanel.SetActive(false);
        }

        float curTime = Time.fixedTime % PanelLightingColour.dayLength; // The current time of day is the number of seconds since start mod the day length in seconds

        if (coloursLeft)
        { // We're still within the time list bounds
            if (curTime > PanelLightingColour.timeList[PanelLightingColour.curLightMode])
            { // The current time of day is more than the switch event
                PanelLightingColour.curLightMode++; // So shift to the next light mode
                                             // Set boolean to start clerp coroutine, collur change
                colourChanged = true;
                if (PanelLightingColour.curLightMode >= PanelLightingColour.timeList.Length)
                {
                    // No more times in time list/colours left, all time transitions have been done. Must wait until end of day now
                    coloursLeft = false;
                }
            }
        }
        if (colourChanged)
        { // Something changed our color
            colourChanged = false; // Don't restart the coroutine again
                                   // Stop the current transition
            StopCoroutine("CLerp");
            // Lerp colour at corresponding time transition
            StartCoroutine("CLerp", colorList[curLightMode]);
        }
    }

    IEnumerator CLerp(Color newColor)
    {
        //Debug.Log( "Start Transition to " + newColor.ToString());
        Color oldColor = img.color; // The current colour
        for (float t = 0; t < 1 + PanelLightingColour.transitionSpeed; t += PanelLightingColour.transitionSpeed)
        {
            // Lerp current ambient light colour towards the new colour
            img.color = Color.Lerp(oldColor, newColor, t);
            // Wait for the assigned number of seconds
            yield return new WaitForSeconds(AmbientLight.transitionSpeedDelay);
        }
        Debug.Log("Transition complete now " + img.color.ToString());
    }

}
