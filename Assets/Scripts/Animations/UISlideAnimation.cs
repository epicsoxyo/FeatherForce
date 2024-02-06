using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// UI strength bar show/hide animations
public class UISlideAnimation : MonoBehaviour
{

    private RectTransform position;

    [SerializeField] private Vector3 hiddenPosition; // off-screen local position
    [SerializeField] private Vector3 visiblePosition; // on-screen local position

    private float timeElapsed = 1e10f;

    private void Awake()
    {

        position = GetComponent<RectTransform>();
        position.anchoredPosition = hiddenPosition;

    }

    public void TriggerShow(float lerpTime)
    {

        StopAllCoroutines();
        StartCoroutine(Show(lerpTime));

    }

    private IEnumerator Show(float lerpTime)
    {

        timeElapsed = lerpTime - timeElapsed;
        if(timeElapsed < 0) timeElapsed = 0;

        while(true)
        {
            if(timeElapsed < lerpTime)
            {
                position.anchoredPosition = Vector3.Lerp(hiddenPosition, visiblePosition, timeElapsed / lerpTime);
                timeElapsed += Time.deltaTime;
            }
            else yield break;

            yield return null;
        }

    }

    public void TriggerHide(float lerpTime)
    {

        StopAllCoroutines();
        StartCoroutine(Hide(lerpTime));

    }

    private IEnumerator Hide(float lerpTime)
    {

        timeElapsed = lerpTime - timeElapsed;
        if(timeElapsed < 0) timeElapsed = 0;

        while(true)
        {
            if(timeElapsed < lerpTime)
            {
                position.localPosition = Vector3.Lerp(visiblePosition, hiddenPosition, timeElapsed / lerpTime);
                timeElapsed += Time.deltaTime;
            }
            else yield break;

            yield return null;
        }

    }

}
