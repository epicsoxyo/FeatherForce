using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// handles audio toggle input
public class AudioToggle : MonoBehaviour
{

    private AudioSource music;

    [SerializeField] private Sprite onSymbol;
    [SerializeField] private Sprite offSymbol;
    private Toggle toggle;
    private Image currentSymbol;

    private void Start()
    {

        music = GameObject.FindWithTag("Audio").GetComponent<AudioSource>();

        toggle = GetComponent<Toggle>();

        currentSymbol = transform.GetChild(0).gameObject.GetComponent<Image>();

        if(music.isPlaying)
        {
            toggle.isOn = true;
            currentSymbol.sprite = onSymbol;
        }
        else
        {
            toggle.isOn = false;
            currentSymbol.sprite = offSymbol;
        }

    }

    public void OnValueChanged(bool isOn)
    {
        if(isOn)
        {
            if(!music.isPlaying) music.Play();
            currentSymbol.sprite = onSymbol;
        }
        else
        {
            if(music.isPlaying) music.Pause();
            currentSymbol.sprite = offSymbol;
        }
    }

}
