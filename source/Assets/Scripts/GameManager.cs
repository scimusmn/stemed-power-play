using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using CreateNeptune;

public class GameManager : MonoBehaviour
{
    public GameState gameState = GameState.gameplay;

    private Camera cam;
    private CameraController camController;
    private PlayerController pc;

    private float openingScenarioChoiceTime = 4f;
    [SerializeField] private float timeBeforeChoices;
    [SerializeField] private GameObject energyScoreText;
    [SerializeField] private GameObject timeScoreText;
    [SerializeField] private GameObject overallScoreText;
    [SerializeField] private ParticleSystem energyPS;
    [SerializeField] private ParticleSystem timePS;
    [SerializeField] private GameObject dimPanel;
    [SerializeField] private GameObject energyMeter;
    [SerializeField] private GameObject overallMeter;
    [SerializeField] private GameObject timeMeter;
    [SerializeField] private Canvas gpsCanvas;
    [SerializeField] private Text gpsText;
    [SerializeField] private GameObject zip;
    [SerializeField] private GameObject zap;
    [SerializeField] private GameObject techBubble;
    [SerializeField] private Text techBubbleText;
    [SerializeField] private GameObject[] tempBubbles;
    [SerializeField] private GameObject tempProgressBubble;

    // Available objects to click on / choices in this scene.
    private List<GameObject> choices = new List<GameObject>();

    // Keep track of previous scenario to shrink those bubbles down after transitions.
    private int previousScenario = -1;

    public enum GameState
    {
        gameplay, transition, endgame
    }

    private void Start()
    {
        cam = Camera.main;
        camController = cam.GetComponent<CameraController>();
        pc = GameObject.FindWithTag("Player").GetComponent<PlayerController>();

        // Get all the available choices in the scene.
        Transform choicesParentT = GameObject.Find("Choices").transform;

        int numChoices = choicesParentT.transform.childCount;

        for (int i = 0; i < numChoices; i++)
        {
            choices.Add(choicesParentT.Find("Choice" + i).gameObject);
        }

        // Set energy, time, and score based on current stats.
        energyScoreText.GetComponent<Text>().text = StaticVariables.energyScore.ToString();
        timeScoreText.GetComponent<Text>().text = StaticVariables.timeScore.ToString();
        overallScoreText.GetComponent<Text>().text = ((int)((float)StaticVariables.overallScore / StaticVariables.maxOverallScore * 100f) + "%").ToString();

        // Set corresponding meters.
        energyMeter.GetComponent<Image>().fillAmount = (float)StaticVariables.energyScore / StaticVariables.maxEnergyScore;
        timeMeter.GetComponent<Image>().fillAmount = (float)StaticVariables.timeScore / StaticVariables.maxTimeScore;
        overallMeter.GetComponent<Image>().fillAmount = (float)StaticVariables.overallScore / StaticVariables.maxOverallScore;

        // Start camera at the right spot.
        cam.transform.position = StaticVariables.scenarios[StaticVariables.scenario].CameraLocation;

        // Set up initial scenario for scene.
        SetScenario(StaticVariables.scenario);

        // Lights darkened for opening scene. Zoom in from building view. Show temp bubbles.
        if (StaticVariables.scenario == StaticVariables.startGameSceneNum)
            StartCoroutine(ZoomInFromBuilding());
        else
        {
            // If it's not the first scene, deactivate temp bubbles.
            foreach (GameObject tempBubble in tempBubbles)
            {
                tempBubble.SetActive(false);
            }

            tempProgressBubble.SetActive(false);
        }
    }

    private IEnumerator ZoomInFromBuilding()
    {
        GameObject dimPanel = GameObject.FindWithTag("dimpanelcanvas");
        Canvas dimPanelCanvas = dimPanel.GetComponent<Canvas>();
        GameObject dimPanelImageObject = dimPanel.transform.GetChild(0).gameObject;

        GameObject apartment = GameObject.FindWithTag("apartment");
        SpriteRenderer apartmentSR = apartment.GetComponent<SpriteRenderer>();

        apartmentSR.enabled = true;

        yield return new WaitForSeconds(openingScenarioChoiceTime);

        dimPanelCanvas.enabled = true;

        StartCoroutine(MPAction.FadeObject(dimPanelImageObject, 2f, 0f, 0.7f, false, false, false));
        yield return MPAction.FadeObject(apartment, 2f, 1f, 0f, false, false, false);

        apartmentSR.enabled = false;
    }

    private IEnumerator SetScores(int energyCost, int timeCost, bool advance)
    {
        float timeForStuff = 0.75f;

        // Energy
        if (advance)
            energyPS.Play();

        StartCoroutine(MPAction.FillBar(energyMeter, timeForStuff, (float)(StaticVariables.energyScore + energyCost)
            / StaticVariables.maxEnergyScore, (float)StaticVariables.energyScore / StaticVariables.maxEnergyScore, false, false, false));

        // Play buzz sound.
        energyMeter.GetComponent<AudioSource>().Play();

        yield return MPAction.CountUpObject(energyScoreText, StaticVariables.energyScore + energyCost,
            StaticVariables.energyScore, timeForStuff, "", "", false, false, false);

        // Time
        if (advance)
            timePS.Play();

        StartCoroutine(MPAction.FillBar(timeMeter, timeForStuff, (float)(StaticVariables.timeScore + timeCost)
            / StaticVariables.maxTimeScore, (float)StaticVariables.timeScore / StaticVariables.maxTimeScore, false, false, false));

        // Play buzz sound.
        timeMeter.GetComponent<AudioSource>().Play();

        yield return MPAction.CountUpObject(timeScoreText, StaticVariables.timeScore + timeCost,
            StaticVariables.timeScore, timeForStuff, "", "", false, false, false);

        // Overall score
        if (advance)
        {
            // If it's the last scenario make sure you reach 100% as long as you aren't out of energy or time.
            if (StaticVariables.scenario == StaticVariables.endGameSceneNum && StaticVariables.energyScore > 0
                && StaticVariables.timeScore > 0)
                StaticVariables.overallScore = StaticVariables.maxOverallScore;
            else
                StaticVariables.overallScore = Mathf.Min(StaticVariables.overallScore, StaticVariables.maxOverallScore - StaticVariables.scorePerTurn);

            StartCoroutine(MPAction.FillBar(overallMeter, timeForStuff, (float)(StaticVariables.overallScore - StaticVariables.scorePerTurn)
            / StaticVariables.maxOverallScore, (float)StaticVariables.overallScore / StaticVariables.maxOverallScore, false, false, false));

            yield return MPAction.CountUpObject(overallScoreText, (int)((float)(StaticVariables.overallScore - StaticVariables.scorePerTurn) / StaticVariables.maxOverallScore * 100f),
                (int)((float)StaticVariables.overallScore / StaticVariables.maxOverallScore * 100f), timeForStuff, "", "%", false, false, false);
        }
    }

    private void Update()
    {
        // Only allow clicks if not in transition.
        if (gameState == GameState.gameplay)
            GetInput();
    }

    private void GetInput()
    {
        // Using mouse up allows player to move off of choice to take it back if necessary.
        if (Input.GetMouseButtonUp(0))
            CheckClick();
    }

    private void CheckClick()
    {
        Vector2 worldPoint = cam.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit2D = Physics2D.Raycast(worldPoint, Vector2.zero);

        if (hit2D.collider == null)
            return;

        if (hit2D.collider.TryGetComponent(out ChoiceStats choiceStats))
        {
            // Do not allow player to click until scene transitions or changes.
            gameState = GameState.transition;

            // Click sound.
            hit2D.collider.GetComponent<AudioSource>().Play();

            // Play any special animation.
            SpecialAnimation special = hit2D.collider.GetComponent<SpecialAnimation>();
            if (special.causesAnimation)
                special.PlayAnimation();

            // Subtract from energy and time, and increment score. If out of either energy or time, end game.
            StaticVariables.energyScore = Mathf.Max(0, StaticVariables.energyScore - choiceStats.energyCost);
            StaticVariables.timeScore = Mathf.Max(0, StaticVariables.timeScore - choiceStats.timeCost);
            StaticVariables.overallScore += StaticVariables.scorePerTurn;

            // Show score changes in view if not advancing scenes.
            if (!choiceStats.newScene)
                StartCoroutine(SetScores(choiceStats.energyCost, choiceStats.timeCost, true));

            // Change scenario based on choice.
            previousScenario = StaticVariables.scenario;
            StaticVariables.scenario = choiceStats.scenarioTarget;

            if (StaticVariables.energyScore <= 0 || StaticVariables.timeScore <= 0)
                StartCoroutine(EndGame());
            else
                GoToNextDestination(choiceStats.newScene, choiceStats.newSceneTarget);
        }
    }

    private void GoToNextDestination(bool newScene, int newSceneTarget)
    {
        // Make a scene change or transition to destination in current scene.
        if (newScene)
            StartCoroutine(TransitionToNewScene(newSceneTarget));
        else
        {
            // Will reset to gameplay after pan.
            StartCoroutine(camController.CameraPan(StaticVariables.scenarios[StaticVariables.scenario].CameraLocation,
                StaticVariables.scenarios[StaticVariables.scenario].CameraZoom));

            // Set up player text and current choices being shown on screen.
            SetScenario(StaticVariables.scenario);
        }
    }

    private IEnumerator TransitionToNewScene(int newSceneTarget)
    {
        dimPanel.GetComponent<Canvas>().enabled = true;

        yield return MPAction.FadeObject(dimPanel.transform.GetChild(0).gameObject, 1.5f, 0f, 1f, false, false, false);

        SceneManager.LoadScene(newSceneTarget);
    }

    private void SetScenario(int scenario)
    {
        // Set avatar text based on scenario.
        if (previousScenario == -1)
            StartCoroutine(pc.SetAvatarText(scenario, false));
        else
            StartCoroutine(pc.SetAvatarText(scenario, true));

        // Set appropriate choices based on scenario.
        StartCoroutine(DisplayScenarioChoices(scenario));

        // End game if this is the end-game scene.
        if (scenario == StaticVariables.endGameSceneNum)
            StartCoroutine(EndGame());
    }

    private IEnumerator DisplayScenarioChoices(int scenario)
    {
        // Shrink previous scenario's choices if there is one. Otherwise there should be no choices shown.
        if (previousScenario == -1)
        {
            foreach (GameObject choice in choices)
            {
                choice.GetComponent<Collider2D>().enabled = false;

                choice.transform.localScale = Vector3.zero;
            }
        }
        else
        {
            int[] previousScenarioChoices = StaticVariables.scenarios[previousScenario].Choices;

            for (int i = 0; i < previousScenarioChoices.Length; i++)
            {
                choices[previousScenarioChoices[i]].GetComponent<Collider2D>().enabled = false;

                StartCoroutine(MPAction.ScaleObject(choices[previousScenarioChoices[i]], Vector3.one, Vector3.zero, 0.5f, "easeineaseout", false, false, false, false));
            }
        }

        // Scale up current scenario's choices.
        int[] currentScenarioChoices = StaticVariables.scenarios[scenario].Choices;

        // Wait before displaying choices so player can process instructions.
        // If this is the first choice, wait a little longer so player can process instructions.
        if (StaticVariables.scenario == StaticVariables.startGameSceneNum)
            yield return new WaitForSeconds(openingScenarioChoiceTime);
        else
            yield return new WaitForSeconds(timeBeforeChoices);

        for (int i = 0; i < currentScenarioChoices.Length; i++)
        {
            // Enable collider on choice so it can be chosen now if it isn't disabled.
            choices[currentScenarioChoices[i]].GetComponent<Collider2D>().enabled = true;

            StartCoroutine(MPAction.ScaleObject(choices[currentScenarioChoices[i]], Vector3.zero, Vector3.one, 0.5f, "easeineaseout", false, false, false, false));
        }

        // X out and disable any choices randomly based on possible disabled choice and show text if appropriate.
        DisabledChoice[] disabledChoices = StaticVariables.scenarios[scenario].PossibleDisabledChoices;
        (int, float, string) newTech = StaticVariables.scenarios[scenario].PossibleNewTech;
        float spinner = Random.Range(0f, 1f);

        // Test to determine if a choice should be disabled.
        (bool, int, string) disableChoiceTest = DisabledChoiceTest(disabledChoices, spinner);

        // If a choice should be disabled
        if (disableChoiceTest.Item1)
        {
            // Choice to disable.
            GameObject choiceToDisable = choices[disableChoiceTest.Item2];

            // Disable collider so you can't choose this choice anymore.
            choiceToDisable.GetComponent<Collider2D>().enabled = false;

            // Show X image over choice.
            choiceToDisable.transform.GetChild(0).Find("ChoiceX").GetComponent<SpriteRenderer>().enabled = true;

            // Set text on tech bubble.
            techBubbleText.text = disableChoiceTest.Item3;

            // Slide in tech bubble.
            Vector2 startPosition = techBubble.GetComponent<RectTransform>().anchoredPosition;
            Vector2 endPosition = new Vector2(170f, startPosition.y);

            StartCoroutine(MPAction.MoveCanvasObject(techBubble, 0.5f, startPosition, endPosition, "easeineaseout", false, false, false));
        }
        // If you got new tech
        else if (spinner < newTech.Item2)
        {
            // Set text on tech bubble.
            techBubbleText.text = newTech.Item3;

            // Slide in tech bubble.
            Vector2 startPosition = techBubble.GetComponent<RectTransform>().anchoredPosition;
            Vector2 endPosition = new Vector2(170f, startPosition.y);

            StartCoroutine(MPAction.MoveCanvasObject(techBubble, 0.5f, startPosition, endPosition, "easeineaseout", false, false, false));
        }
        else
        {
            // Cancel new tech choice if it exists.
            if (newTech.Item1 != 999)
            {
                // Choice to disable.
                GameObject techToDisable = choices[newTech.Item1];

                // Disable collider so you can't choose this choice anymore.
                techToDisable.GetComponent<Collider2D>().enabled = false;

                // Show X image over choice.
                techToDisable.transform.GetChild(0).Find("ChoiceX").GetComponent<SpriteRenderer>().enabled = true;
            }

            // Slide out tech bubble.
            Vector2 startPosition = techBubble.GetComponent<RectTransform>().anchoredPosition;
            Vector2 endPosition = new Vector2(800f, startPosition.y);

            StartCoroutine(MPAction.MoveCanvasObject(techBubble, 0.5f, startPosition, endPosition, "easeineaseout", false, false, false));
        }
    }

    private (bool, int, string) DisabledChoiceTest(DisabledChoice[] disabledChoices, float spinner)
    {
        for (int i = 0; i < disabledChoices.Length; i++)
        {
            if (spinner < disabledChoices[i].ChoiceDisabledProbability)
            {
                if (disabledChoices[i].ShouldAnimate)
                    PlayDisabledChoiceAnimation(disabledChoices[i].AnimationNum);

                return (true, disabledChoices[i].ChoiceNum, disabledChoices[i].ChoiceDisabledStatement);
            }
        }

        return (false, 999, "");
    }

    private void PlayDisabledChoiceAnimation(int animationToPlay)
    {
        // Show relevant disabled choice animation in scene.
        switch(animationToPlay)
        {
            // Rain
            case 0:
                GameObject[] rains = GameObject.FindGameObjectsWithTag("rain");
                foreach (GameObject rain in rains)
                    rain.GetComponent<ParticleSystem>().Play();
                break;
            default:
                break;
        }
    }

    private IEnumerator EndGame()
    {
        gameState = GameState.endgame;

        yield return new WaitForSeconds(1.5f);

        // Show victory text if got to final scenario and not out of energy and time.
        if (StaticVariables.scenario == StaticVariables.endGameSceneNum && StaticVariables.energyScore > 0
            && StaticVariables.timeScore > 0)
        {
            gpsText.text = "Congratulations! You balanced your time and energy! Try again along a different path.";
            StartCoroutine(ZipDance());
            StartCoroutine(ZapDance());
        }
        else if (StaticVariables.energyScore <= 0)
        {
            gpsText.text = "Your energy got zapped! Try again to see if you can reach 100%.";
        }
        else
        {
            gpsText.text = "Time zipped away! Try again to see if you can reach 100%.";
        }

        // Show gameplay summary screen.
        gpsCanvas.enabled = true;
    }

    private IEnumerator ZipDance()
    {
        Vector2 startScale = Vector2.one;
        Vector2 endScale = Vector2.one * 3f;

        for (; ;)
        {
            yield return MPAction.ScaleObject(zip, startScale, endScale, 0.75f, "easeineaseout", false, false, false, false);

            yield return MPAction.ScaleObject(zip, endScale, startScale, 0.75f, "easeineaseout", false, false, false, false);
        }
    }

    private IEnumerator ZapDance()
    {
        Vector2 startScale = Vector2.one;
        Vector2 endScale = Vector2.one * 3f;

        yield return new WaitForSeconds(0.75f);

        for (; ;)
        {
            yield return MPAction.ScaleObject(zap, startScale, endScale, 0.75f, "easeineaseout", false, false, false, false);

            yield return MPAction.ScaleObject(zap, endScale, startScale, 0.75f, "easeineaseout", false, false, false, false);
        }
    }

    public void RestartGame()
    {
        StaticVariables.energyScore = 100;
        StaticVariables.timeScore = 100;
        StaticVariables.overallScore = 0;
        StaticVariables.scenario = StaticVariables.startGameSceneNum;

        SceneManager.LoadScene(0);
    }
}
