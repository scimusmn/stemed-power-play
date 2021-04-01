using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CreateNeptune;

public class ChoiceBounce : MonoBehaviour
{
    [SerializeField] private GameObject choiceBubble;

    private void Start()
    {
        StartCoroutine(Bounce());
    }

    private IEnumerator Bounce()
    {
        Vector2 startPosition = choiceBubble.transform.localPosition;
        Vector2 endPosition = new Vector2(startPosition.x, startPosition.y + 0.15f);

        for(; ;)
        {
            yield return MPAction.MoveObject(choiceBubble, true, 0.5f, startPosition, endPosition, "easeineaseout", false, false, false);

            yield return MPAction.MoveObject(choiceBubble, true, 0.5f, endPosition, startPosition, "easeineaseout", false, false, false);
        }
    }
}
