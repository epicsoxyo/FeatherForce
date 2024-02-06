using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

// orchestrates level events
public class EventHandler : MonoBehaviour
{

    [Header("Player Animation")]
    [SerializeField] private MuscleBirdAnimation muscleBird;
    private bool isHoldingEgg = false; // true if currently in shotput hold position
    public bool isFinished // public accessor/mutator for isHoldingEgg
    {
        get {return isHoldingEgg;}
        set {isHoldingEgg = value;}
    }



    [Header("Camera Transition")]
    [SerializeField] private CameraManager cameraManager;
    [SerializeField] private float lerpTime = 0.2f;
    [SerializeField] private float normalizedSplitDistance = 0.6f; // for distributing lerp time (Splitscreen:Unsplitscreen)



    [Header("UI Transition")]
    [SerializeField] private UISlideAnimation cameraDivision;
    [SerializeField] private UISlideAnimation strengthBar;
    [SerializeField] private TextMeshProUGUI scoreUI;



    [Header("Game Start")]
    [SerializeField] private ShotputSpawner shotputSpawner;



    private void Start()
    {

        scoreUI.CrossFadeAlpha(0f, 0f, false);

    }

    public void TriggerZoomIn()
    {
        StopAllCoroutines();
        StartCoroutine("ZoomIn");
    }

    private IEnumerator ZoomIn()
    {

        yield return muscleBird.StartCoroutine("ZoomIn");

        cameraDivision.TriggerShow(lerpTime);
        strengthBar.TriggerShow(lerpTime);

        cameraManager.TriggerZoomIn(lerpTime, normalizedSplitDistance);

        isHoldingEgg = true;    

    }

    public void TriggerZoomOut()
    {

        muscleBird.ZoomOut();

        cameraDivision.TriggerHide(lerpTime);
        strengthBar.TriggerHide(lerpTime);

        cameraManager.TriggerZoomOut(lerpTime, normalizedSplitDistance);

        isHoldingEgg = false;

    }

    public void TriggerThrowEgg(float shakesPerSecond)
    {

        scoreUI.CrossFadeAlpha(1.0f, lerpTime, false);

        muscleBird.Throw();

        cameraDivision.TriggerHide(lerpTime);
        strengthBar.TriggerHide(lerpTime);

        shotputSpawner.TriggerThrowEgg(shakesPerSecond, cameraManager, lerpTime, normalizedSplitDistance);

    }

    public void TriggerLevelReset()
    {

        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);

    }

}
