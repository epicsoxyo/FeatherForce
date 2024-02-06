using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StrengthBar : MonoBehaviour
{

    private Image bar;

    [SerializeField] private float maxShakes;
    [SerializeField] private float lerpSpeed;
    private float timeElapsed = 0f;

    private float fillAmount = 0;
    private float previousFillAmount;

    private void Start()
    {

        bar = transform.GetChild(0).gameObject.GetComponent<Image>();
        bar.fillAmount = fillAmount;

    }

    private void Update()
    {
        LerpToFillAmount();
    }

    private void LerpToFillAmount()
    {

        bar.fillAmount = Mathf.Lerp(previousFillAmount, fillAmount, lerpSpeed * timeElapsed);
        timeElapsed += Time.deltaTime;

    }

    public void UpdateFillAmount(float shakesPerSecond)
    {

        previousFillAmount = fillAmount;
        fillAmount = (shakesPerSecond < maxShakes)? (shakesPerSecond / maxShakes) : maxShakes;

        timeElapsed = 0f;

    }

    public void Reset()
    {

        fillAmount = 0;

    }
    
}
