using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// handles player character transition animations
public class MuscleBirdAnimation : MonoBehaviour
{

    private Animator playerAnim;
    private Animator eyebrowAnim;

    private AudioSource fartSFX;
    private AudioSource whooshSFX;

    private ParticleSystem eggBirthVFX; // feather explosion

    private GameObject animatedEgg; // facade egg held by musclebird during animation
    private MeshRenderer eggMesh; // render mesh of animated egg
    private MeshRenderer eggHologramVFX; // render mesh of egg hologram effect

    [SerializeField] private GameObject eggPrefab; // generated when musclebird drops the egg


    private void Start()
    {

        playerAnim = GetComponent<Animator>();
        eyebrowAnim = GameObject.FindWithTag("Eyebrows").GetComponent<Animator>();

        fartSFX = transform.GetChild(2).gameObject.GetComponent<AudioSource>();
        whooshSFX = transform.GetChild(3).gameObject.GetComponent<AudioSource>();

        eggBirthVFX = GameObject.FindWithTag("EggBirthVFX").GetComponent<ParticleSystem>();

        animatedEgg = GameObject.FindWithTag("AnimatedEgg");
        eggMesh = animatedEgg.GetComponent<MeshRenderer>();
        eggHologramVFX = animatedEgg.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>();

        eggMesh.enabled = false;
        eggHologramVFX.enabled = false;

    }

    private IEnumerator ZoomIn()
    {

        playerAnim.SetTrigger("MouseDown");
        eyebrowAnim.SetTrigger("Frown");

        fartSFX.Play();

        eggBirthVFX.Play();
        eggMesh.enabled = true;

        while(!playerAnim.GetCurrentAnimatorStateInfo(0).IsName("ShotputHold")) yield return null;

        eggHologramVFX.enabled = true;

    }

    public void ZoomOut()
    {

        StopAllCoroutines();

        playerAnim.SetTrigger("MouseUp");
        eyebrowAnim.SetTrigger("Unfrown");

        eggBirthVFX.Clear();

        Instantiate(eggPrefab, animatedEgg.transform.position, animatedEgg.transform.rotation);

        eggMesh.enabled = false;
        eggHologramVFX.enabled = false;

    }

    public void Throw()
    {

        whooshSFX.Play();

        playerAnim.SetTrigger("Throw");
        eyebrowAnim.SetTrigger("Unfrown");

        eggHologramVFX.enabled = false;

    }


}