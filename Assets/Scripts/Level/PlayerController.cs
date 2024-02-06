using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// handles player input
public class PlayerController : MonoBehaviour
{

    private EventHandler eventHandler;
    [SerializeField] private StrengthBar strengthBar;

    private float[] samples = new float[5];
    private int i = 0; // samples index

    private float sum; // for calculating average
    private float shakesPerSecond = 0f; // mean sample

    [SerializeField] private float minimumShakes; // required to trigger a throw

    Vector2 previousMouseAxis; // in last frame
    Vector2 currentMouseAxis; // in current frame
    float previousShakeTime; // time since last registered shake, in seconds
    private float timeElapsed = 0f;
    private bool isShaking;

    [SerializeField] private float drainAmount; // rate at which to drain shakes per second, per second

    private void Awake()
    {

        eventHandler = GetComponent<EventHandler>();

    }

    private void Update()
    {

        // zoom in on first frame of click
        if(Input.GetMouseButtonDown(0))
        {
            eventHandler.TriggerZoomIn();
        }
        // count user's mouse shakes while holding mouse click
        else if(Input.GetMouseButton(0) && eventHandler.isFinished)
        {
            UpdateShakesPerSecond();
        }
        // zoom out/throw egg on first frame of mouse release
        else if(Input.GetMouseButtonUp(0))
        {
            if(shakesPerSecond > minimumShakes)
            {
                eventHandler.TriggerThrowEgg(shakesPerSecond);
                Destroy(this); // shake input is no longer needed after egg throw
            }
            else
            {
                eventHandler.TriggerZoomOut();
                strengthBar.Reset();
            }
        }

    }

    private void UpdateShakesPerSecond()
    {

        CountShakes();
        CalculateShakesPerSecond();
        strengthBar.UpdateFillAmount(shakesPerSecond);
        DrainShakesPerSecond();

    }

    // uses difference in time between mouse shakes to calculate shakes per second for one sample
    private void CountShakes()
    {

        currentMouseAxis = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        if(currentMouseAxis.magnitude > 0.1f) // filters skipped input frames
        {
            isShaking = Mathf.Sign(currentMouseAxis.x) != Mathf.Sign(previousMouseAxis.x) ||
                        Mathf.Sign(currentMouseAxis.y) != Mathf.Sign(previousMouseAxis.y);

            previousMouseAxis = currentMouseAxis;
        }
        else isShaking = false;

        if(isShaking)
        {
            samples[i] = 1 / (timeElapsed - previousShakeTime);
            i = (i == 4) ? 0 : i + 1;

            previousShakeTime = timeElapsed;
        }

        timeElapsed += Time.deltaTime;

    }

    // calculates average over all samples
    private void CalculateShakesPerSecond()
    {

        sum = 0;
        foreach(float sample in samples) sum += sample;
        shakesPerSecond = sum / samples.Length;

    }

    // slowly depletes shakes per second over time
    private void DrainShakesPerSecond()
    {
        for(int j = 0; j < 5; j++)
        {
            samples[j] -= drainAmount * Time.deltaTime;
            if(samples[j] < 0) samples[j] = 0;
        }
    }

}
