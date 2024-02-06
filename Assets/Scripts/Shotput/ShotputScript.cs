using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// calculates and sets score from shotput position
public class ShotputScript : MonoBehaviour
{

    private GameObject scoreUI;
    private TextMeshProUGUI scoreUIText;
    private EndAnimation endAnimation;

    private float score = 0f;
    private float scoreLastFrame = 0f;
    private float threshold = 0.9f;

    private float waitTime = 3f;
    private float timeElapsed = 0f;

    private AudioSource crackingSFX;

    private void Start()
    {
        
        scoreUI = GameObject.FindWithTag("ScoreUI");
        scoreUIText = scoreUI.GetComponent<TextMeshProUGUI>();
        endAnimation = scoreUI.GetComponent<EndAnimation>();

        crackingSFX = GetComponent<AudioSource>();

    }

    private void Update() {
        
        score = transform.position.x;

        if(Mathf.Abs(score - scoreLastFrame) > threshold)
        {
            scoreUIText.SetText((int)score + "m");
            scoreLastFrame = score;
            timeElapsed = 0f;
        }
        else if(timeElapsed < waitTime)
        {
            timeElapsed += Time.deltaTime;
        }
        else
        {
            endAnimation.TriggerAnimation(transform.position);
            Destroy(this);
        }

    }

    private void OnCollisionEnter(Collision other)
    {

        if(other.gameObject.CompareTag("Floor"))
        {
            crackingSFX.Stop();
            crackingSFX.Play();
        }

    }

}
