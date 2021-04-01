using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisabledChoice
{
    public int ChoiceNum;
    public float ChoiceDisabledProbability;
    public string ChoiceDisabledStatement;
    public bool ShouldAnimate;
    public int AnimationNum;

    // A disabled choice specifies the choice ID to be disabled, the cumulative P(x) with which this happens,
    // a statement for when it is disabled, whether there should be an animation associated,
    // and which animation ID should be associated if there is one.
    public DisabledChoice(int choiceNum, float choiceDisabledProbability,
        string choiceDisabledStatement, bool shouldAnimate, int animationNum)
    {
        ChoiceNum = choiceNum;
        ChoiceDisabledProbability = choiceDisabledProbability;
        ChoiceDisabledStatement = choiceDisabledStatement;
        ShouldAnimate = shouldAnimate;
        AnimationNum = animationNum;
    }
}
