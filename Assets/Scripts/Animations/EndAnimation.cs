using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// coordinates score + linerender animations at the end of the game
public class EndAnimation : MonoBehaviour
{

    private RectTransform rect;
    private Vector3 scoreUIStartPos;
    [SerializeField] private Vector3 scoreUIEndPos;
    
    private float lerpTime = 1f;
    private float timeElapsed = 0f;

    private LineRenderer endMarker;
    [SerializeField] private float endMarkerLength;

    private AudioSource eggTimerSFX;


    private void Awake()
    {

        rect = GetComponent<RectTransform>();
        scoreUIStartPos = rect.anchoredPosition;

        eggTimerSFX = GetComponent<AudioSource>();

    }

    private void Start()
    {

        endMarker = GameObject.FindWithTag("EndMarker").GetComponent<LineRenderer>();
        endMarker.positionCount = 2;

    }

    public void TriggerAnimation(Vector3 lineBegin)
    {

        StartCoroutine(Animation(lineBegin));

    }

    private IEnumerator Animation(Vector3 lineBegin)
    {

        Vector3 lineEnd;
        timeElapsed = 0f;

        while(timeElapsed < lerpTime)
        {
            lineEnd = Vector3.Lerp(lineBegin, lineBegin + Vector3.up * endMarkerLength, timeElapsed / lerpTime);
            endMarker.SetPositions(new Vector3[] {lineBegin, lineEnd});

            rect.anchoredPosition = Vector3.Lerp(scoreUIStartPos, scoreUIEndPos, timeElapsed / lerpTime);

            timeElapsed += Time.deltaTime;

            yield return null;
        }

        eggTimerSFX.Play();

    }

}
