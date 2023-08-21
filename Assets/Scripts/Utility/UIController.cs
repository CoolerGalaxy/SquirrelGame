using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using GameProperties;

public class UIController : MonoBehaviour
{
    public TextMeshProUGUI acorn_text;
    public AcornCollector acornCollector;
    public GameStateTracker gameStateTracker;

    /// <summary>
    /// Lockout variable to prevent HUD updates during text animation
    /// </summary>
    private bool pulseAnimation = false;
    private int totalMainPulses = 3;
    private float pulseScaleMin = 3f;
    private float pulseScaleMax = 3.5f;
    private float pulseSpeed = 3f;
    /// <summary>
    /// Scaling oscillation control variable.
    /// </summary>
    private bool scaleUp = true;
    private Color levelChangeColor = Color.green;

    private Levels lastLevel;
    private int collectedAcorns = 0;
    private int totalAcorns = 0;

    private void Start()
    {
        lastLevel = gameStateTracker.currentLevel;
        collectedAcorns = GameConstants.sideYardTotalAcorns;
        totalAcorns = GameConstants.sideYardTotalAcorns;
    }

    void LateUpdate()
    {
        UpdateAcorns();
    }

    public void UpdateAcorns()
    {
        if (lastLevel != gameStateTracker.currentLevel && !pulseAnimation) // if there was a level transition
        {
            pulseAnimation = true;

            switch (gameStateTracker.currentLevel)
            { // these cases need to lead the actual level because of how the game state works
                case Levels.yardOne:
                    collectedAcorns = GameConstants.sideYardTotalAcorns;
                    totalAcorns = GameConstants.sideYardTotalAcorns;
                    acorn_text.text = "Acorns: " + collectedAcorns.ToString() + "/" + totalAcorns.ToString();
                    break;
                case Levels.yardTwo:
                    collectedAcorns = GameConstants.yardOneTotalAcorns;
                    totalAcorns = GameConstants.yardOneTotalAcorns;
                    acorn_text.text = "Acorns: " + collectedAcorns.ToString() + "/" + totalAcorns.ToString();
                    break;
                case Levels.yardThree:
                    collectedAcorns = GameConstants.yardTwoTotalAcorns;
                    totalAcorns = GameConstants.yardTwoTotalAcorns;
                    acorn_text.text = "Acorns: " + collectedAcorns.ToString() + "/" + totalAcorns.ToString();
                    break;
                    // TODO: add more if more levels are added
            }
            StartCoroutine(FlourishText()); // https://docs.unity3d.com/ScriptReference/MonoBehaviour.StartCoroutine.html
        }

        if (!pulseAnimation)
        {
            switch (gameStateTracker.currentLevel)
            {
                case Levels.sideYard:
                    collectedAcorns = acornCollector.sideYardCollectedAcorns;
                    totalAcorns = GameConstants.sideYardTotalAcorns;
                    acorn_text.text = "Acorns: " + collectedAcorns.ToString() + "/" + totalAcorns.ToString();
                    break;
                case Levels.yardOne:
                    collectedAcorns = acornCollector.yardOneCollectedAcorns;
                    totalAcorns = GameConstants.yardOneTotalAcorns;
                    acorn_text.text = "Acorns: " + collectedAcorns.ToString() + "/" + totalAcorns.ToString();
                    break;
                case Levels.yardTwo:
                    collectedAcorns = acornCollector.yardTwoCollectedAcorns;
                    totalAcorns = GameConstants.yardTwoTotalAcorns;
                    acorn_text.text = "Acorns: " + collectedAcorns.ToString() + "/" + totalAcorns.ToString();
                    break;
                case Levels.yardThree:
                    acorn_text.text = "Go to the Tree-O-Plenty!";
                    break;
            }
        }

    }

    private IEnumerator FlourishText() // asynchronously creates a pulsing effect 
    {
        Vector3 scaleMin = new Vector3(pulseScaleMin, pulseScaleMin, pulseScaleMin);
        Vector3 scaleMax = new Vector3(pulseScaleMax, pulseScaleMax, pulseScaleMax);

        acorn_text.color = levelChangeColor;

        for (int i = 0; i < totalMainPulses; i++)
        {
            while (scaleUp)
            {
                acorn_text.transform.localScale =
                    Vector3.MoveTowards(acorn_text.transform.localScale, scaleMax, pulseSpeed * Time.deltaTime);
                //Debug.Log("BIGGER - " + acorn_text.transform.localScale.x + " - " + pulseScaleMax);
                if (acorn_text.transform.localScale.x >= pulseScaleMax)
                {
                    scaleUp = false;
                    break;
                }
                yield return null; // wait for next frame to be generated
            }

            while (!scaleUp)
            {
                acorn_text.transform.localScale =
                    Vector3.MoveTowards(acorn_text.transform.localScale, scaleMin, pulseSpeed * Time.deltaTime);
                if (acorn_text.transform.localScale.x <= pulseScaleMin)
                {
                    scaleUp = true;
                    break;
                }
                yield return null;
            }
        }
        acorn_text.color = Color.white;
        lastLevel = gameStateTracker.currentLevel;
        pulseAnimation = false;
    }

}

