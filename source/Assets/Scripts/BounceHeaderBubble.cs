using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CreateNeptune;

public class BounceHeaderBubble : MonoBehaviour
{
    private RectTransform rt;
    
    private void Awake()
    {
        rt = GetComponent<RectTransform>();
    }

    private void OnEnable()
    {
        StartCoroutine(BounceBubbles());
    }

    private IEnumerator BounceBubbles()
    {
        Vector2 startPosition = rt.anchoredPosition;
        Vector2 endPosition = new Vector2(startPosition.x, startPosition.y - 5f);

        yield return new WaitForSeconds(Random.Range(0.05f, 0.5f));

        for (; ;)
        {
            yield return MPAction.MoveCanvasObject(gameObject, 0.75f, startPosition, endPosition, "easeineaseout", false, false, false);

            yield return MPAction.MoveCanvasObject(gameObject, 0.75f, endPosition, startPosition, "easeineaseout", false, false, false);
        }
    }
}
