using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CreateNeptune;

public class SpecialAnimation : MonoBehaviour
{
    public bool causesAnimation;
    [SerializeField] private int animationNum;

    public void PlayAnimation()
    {
        switch (animationNum)
        {
            case 0:
                StartCoroutine(StepFlash());
                break;
            case 1:
                MoveVehicle(0);
                break;
            case 2:
                MoveVehicle(1);
                break;
            case 3:
                MoveVehicle(2);
                break;
            case 100: // Scene 1 (Home Scene), Choice 0
                BoilWater();
                DisableProgressBubble();
                break;
            case 101:
                DisableProgressBubble();
                break;
            case 110:
                StartCoroutine(GoToSleepAndWakeUp());
                ShowFireworks();
                break;
            case 111: // Scene 1 (Home Scene), Choice 11
                StartCoroutine(TurnOnLights());
                break;
            default:
                break;
        }
    }

    private void DisableProgressBubble()
    {
        GameObject tempProgressBubble = GameObject.FindWithTag("tempprogressbubble");
        StartCoroutine(MPAction.ScaleObject(tempProgressBubble, Vector3.one, Vector3.zero, 0.5f, "easeineaseout", false, true, false, false));
    }

    private IEnumerator GoToSleepAndWakeUp()
    {
        GameObject dimPanel = GameObject.FindWithTag("dimpanelcanvas");
        Canvas dimPanelCanvas = dimPanel.GetComponent<Canvas>();
        GameObject dimPanelImageObject = dimPanel.transform.GetChild(0).gameObject;

        // Enable the canvas, and fade the DimPanel to 100% and back to 0%
        dimPanelCanvas.enabled = true;

        yield return MPAction.FadeObject(dimPanelImageObject, 1.5f, 0f, 1f, false, false, false);
        yield return MPAction.FadeObject(dimPanelImageObject, 1.5f, 1f, 0f, false, false, false);

        // Disable the dim panel canvas when complete
        dimPanelCanvas.enabled = false;
    }

    private void MoveVehicle(int busCarBike)
    {
        GameObject vehicle;
        switch (busCarBike)
        {
            case 0:
                vehicle = GameObject.FindWithTag("bus");
                StartCoroutine(MPAction.MoveObject(vehicle, false, 2f, vehicle.transform.position, new Vector2(27f, -15f), "easein", false, false, false));
                break;
            case 1:
                vehicle = GameObject.FindWithTag("car");
                StartCoroutine(MPAction.MoveObject(vehicle, false, 2f, vehicle.transform.position, new Vector2(-20f, -14f), "easein", false, false, false));
                break;
            case 2:
                vehicle = GameObject.FindWithTag("bike");
                StartCoroutine(MPAction.MoveObject(vehicle, false, 2f, vehicle.transform.position, new Vector2(-20f, -14f), "easein", false, false, false));
                break;
            default:
                break;
        }
    }

    private void ShowFireworks()
    {
        GameObject[] fireworks = GameObject.FindGameObjectsWithTag("fireworks");

        foreach (GameObject firework in fireworks)
            firework.GetComponent<ParticleSystem>().Play();
    }

    private IEnumerator StepFlash()
    {
        GameObject[] steps = GameObject.FindGameObjectsWithTag("step");
        float timeToFlash = 120f;

        foreach(GameObject step in steps)
        {
            SpriteRenderer sr = step.GetComponent<SpriteRenderer>();

            StartCoroutine(MPAction.FlashAnimation(step, 0.5f, timeToFlash, sr.color, Color.white, false, false, false));
        }

        // Change audio for a moment to stairs audio.
        AudioSource camAudio = Camera.main.GetComponent<AudioSource>();
        camAudio.volume = 1f;
        camAudio.clip = Camera.main.GetComponent<SecretSongs>().secretSongs[0];
        camAudio.Play();

        StartCoroutine(FlashSteps(timeToFlash));

        // Change back to original song after stairs song is over.
        yield return new WaitForSeconds(7f);

        camAudio.volume = 0.5f;
        camAudio.clip = Camera.main.GetComponent<SecretSongs>().secretSongs[1];
        camAudio.Play();
    }

    private IEnumerator FlashSteps(float timeToFlash)
    {
        GameObject[] stepLights = GameObject.FindGameObjectsWithTag("steplight");

        foreach(GameObject stepLight in stepLights)
        {
            stepLight.GetComponent<ParticleSystem>().Play();
        }

        yield return new WaitForSeconds(timeToFlash);

        foreach (GameObject stepLight in stepLights)
        {
            stepLight.GetComponent<ParticleSystem>().Stop();
        }
    }

    // When user taps to turn on the lights, fade dim panel to 0%
    private IEnumerator TurnOnLights()
    {
        GameObject dimPanel = GameObject.FindWithTag("dimpanelcanvas");
        Canvas dimPanelCanvas = dimPanel.GetComponent<Canvas>();
        GameObject dimPanelImageObject = dimPanel.transform.GetChild(0).gameObject;

        GameObject[] tempBounceBubbles = GameObject.FindGameObjectsWithTag("tempbubble");
        GameObject tempProgressBubble = GameObject.FindWithTag("tempprogressbubble");
        
        // Fade the DimPanel to 0%
        yield return MPAction.FadeObject(dimPanelImageObject, 1.5f, 0.5f, 0f, false, false, false);

        // Disable the dim panel canvas when complete
        dimPanelCanvas.enabled = false;

        // Deactivate energy and time bouncing bubbles.
        foreach (GameObject tempBubble in tempBounceBubbles)
            StartCoroutine(MPAction.ScaleObject(tempBubble, Vector3.one, Vector3.zero, 0.5f, "easeineaseout", false, true, false, false));

        // Activate progress bouncing bubble image and text.
        tempProgressBubble.GetComponent<Image>().enabled = true;
        tempProgressBubble.transform.GetChild(0).GetComponent<Text>().enabled = true;
    }

    // When user taps the tea kettle, play the Tea Kettle Steam PS
    private void BoilWater()
    {
        // Find the Tea Kettle PS by tag
        GameObject TeaKettleSteamPS = GameObject.FindWithTag("teakettlesteamps");

        // Play the tea kettle PS animation
        TeaKettleSteamPS.GetComponent<ParticleSystem>().Play();
    }
}
