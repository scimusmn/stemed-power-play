using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChoiceStats : MonoBehaviour
{
    public int energyCost;
    public int timeCost;

    [SerializeField] private TextMeshPro energyAmount;
    [SerializeField] private TextMeshPro timeAmount;

    // Does this choice result in a scene change or moving to a destination within the scene?
    public bool newScene;
    // If it is a scene change, which scene?
    public int newSceneTarget;
    // Represents scenario resulting from choice regardless of scene change or not.
    public int scenarioTarget;

    private void OnEnable()
    {
        // Set text of choice based on underlying energy and time amounts.
        energyAmount.text = energyCost.ToString();
        timeAmount.text = timeCost.ToString();
    }
}
