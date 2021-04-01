using UnityEngine;

public class Scenario
{
    public int ScenarioNum;
    public Vector3 CameraLocation;
    public float CameraZoom;
    public int[] Choices;
    public DisabledChoice[] PossibleDisabledChoices;
    public (int, float, string) PossibleNewTech;
    public string NarratorText;

    // Every scenario has a unique numerical ID, a location, a specified zoom, certain base choices,
    // certain choices that may be disabled with P(x), a possible new technology which appears with P(y),
    // and associated narration.
    public Scenario(int scenarioNum, Vector3 cameraLocation, float cameraZoom,
        int[] choices, DisabledChoice[] possibleDisabledChoices,  (int, float, string) possibleNewTech, string narratorText)
    {
        ScenarioNum = scenarioNum;
        CameraLocation = cameraLocation;
        CameraZoom = cameraZoom;
        Choices = choices;
        PossibleDisabledChoices = possibleDisabledChoices;
        PossibleNewTech = possibleNewTech;
        NarratorText = narratorText;
    }
}
