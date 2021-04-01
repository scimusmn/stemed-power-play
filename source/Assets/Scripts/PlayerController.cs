using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CreateNeptune;

public class PlayerController : MonoBehaviour
{
    private RectTransform rt;
    [SerializeField] private GameObject speechBubble;
    [SerializeField] private Text speechText;

    [SerializeField] Image avatarImage;
    [SerializeField] Sprite[] avatars;

    private void Awake()
    {
        rt = GetComponent<RectTransform>();
    }

    private void Start()
    {
        int randAvatar = Random.Range(0, avatars.Length);
        avatarImage.sprite = avatars[randAvatar];

        StartCoroutine(SlideIn());
    }

    private IEnumerator SlideIn()
    {
        Vector2 startPosition = rt.anchoredPosition;
        Vector2 endPosition1 = new Vector2(startPosition.x + 250f, startPosition.y);
        Vector2 endPosition2 = new Vector2(startPosition.x + 200f, startPosition.y);

        yield return MPAction.MoveCanvasObject(gameObject, 0.75f, startPosition, endPosition1, "easeineaseout", false, false, false);

        yield return MPAction.MoveCanvasObject(gameObject, 0.25f, endPosition1, endPosition2, "easeineaseout", false, false, false);
    }

    public IEnumerator SetAvatarText(int scenario, bool shrinkFirst)
    {
        speechText.enabled = false;

        float scaleTime = 1f;

        if (shrinkFirst)
            yield return MPAction.ScaleObject(speechBubble, Vector3.one, Vector3.zero, scaleTime, "easeineaseout", false, false, false, false);

        yield return MPAction.ScaleObject(speechBubble, Vector3.zero, Vector3.one, scaleTime, "easeineaseout", false, false, false, false);

        // Change text.
        speechText.text = StaticVariables.scenarios[scenario].NarratorText;

        speechText.enabled = true;
    }
}
