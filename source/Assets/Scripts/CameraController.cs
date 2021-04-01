using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CreateNeptune;

public class CameraController : MonoBehaviour
{
    private GameManager gm;

    [SerializeField] private float panTime;
    private Camera cam;
    private Transform t;

    private void Awake()
    {
        cam = GetComponent<Camera>();
        t = transform;
    }

    private void Start()
    {
        gm = GameObject.FindWithTag("gm").GetComponent<GameManager>();
    }

    public IEnumerator CameraPan(Vector3 targetPosition, float targetSize)
    {
        yield return CNExtensions.CameraPan(cam, panTime, t.localPosition, targetPosition, cam.orthographicSize, targetSize, "easeineaseout", false);

        // Transition to gameplay again after camera completes its pan.
        gm.gameState = GameManager.GameState.gameplay;
    }
}
