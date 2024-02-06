using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{

    private GameObject mainCamera;
    private GameObject faceCamera;

    private Vector3[] cameraTargetPositions = new Vector3[4];
    private Quaternion[] cameraTargetRotations = new Quaternion[4];

    private float timeElapsed = 1000f;

    private bool isTrackingObject = false;
    private Transform trackedObject;
    [SerializeField] private Vector3 trackingPositionDisplacement;
    [SerializeField] private Quaternion trackingRotation;

    [SerializeField] private FloorManager floorManager;


    private void Start()
    {

        mainCamera = gameObject.transform.GetChild(0).gameObject;
        faceCamera = gameObject.transform.GetChild(1).gameObject;

        GetCameraTargets();

    }

    // get camera target locations and destroy the markers
    private void GetCameraTargets()
    {

        Transform cameraTargetParent = gameObject.transform.GetChild(2);

        for(int i = 3; i >= 0; i--)
        {
            cameraTargetPositions[i] = cameraTargetParent.GetChild(i).position;
            cameraTargetRotations[i] = cameraTargetParent.GetChild(i).rotation;
            Destroy(cameraTargetParent.GetChild(i).gameObject);
        }

        Destroy(cameraTargetParent.gameObject);

    }

    public void TriggerZoomIn(float lerpTime, float normalizedSplitDistance)
    {

        StopAllCoroutines();
        StartCoroutine(ZoomIn(lerpTime, normalizedSplitDistance));

    }

    private IEnumerator ZoomIn(float lerpTime, float normalizedSplitDistance)
    {

        timeElapsed = lerpTime - timeElapsed;
        if(timeElapsed < 0) timeElapsed = 0;

        // merged camera animation
        while(timeElapsed < lerpTime * normalizedSplitDistance)
        {
            mainCamera.GetComponent<Camera>().rect = new Rect(0f, 0f, 1f, 1f);

            mainCamera.transform.position = Vector3.Lerp
                (
                    cameraTargetPositions[0],
                    cameraTargetPositions[1],
                    timeElapsed / (lerpTime * normalizedSplitDistance)
                );
            mainCamera.transform.rotation = Quaternion.Lerp
                (
                    cameraTargetRotations[0],
                    cameraTargetRotations[1],
                    timeElapsed / (lerpTime * normalizedSplitDistance)
                );

            timeElapsed += Time.deltaTime;
            yield return null;
        }
        // splitscreen animation
        while(timeElapsed < lerpTime)
        {
            mainCamera.GetComponent<Camera>().rect = new Rect(0f, 0f, normalizedSplitDistance, 1f);
            faceCamera.GetComponent<Camera>().rect = new Rect(normalizedSplitDistance, 0f, (1 - normalizedSplitDistance), 1f);

            mainCamera.transform.position = Vector3.Lerp
                (
                    cameraTargetPositions[1],
                    cameraTargetPositions[3],
                    (timeElapsed - (lerpTime * normalizedSplitDistance)) / (lerpTime * (1 - normalizedSplitDistance))
                );
            mainCamera.transform.rotation = Quaternion.Lerp
                (
                    cameraTargetRotations[1],
                    cameraTargetRotations[3],
                    (timeElapsed - (lerpTime * normalizedSplitDistance)) / (lerpTime * (1 - normalizedSplitDistance))
                );
            
            faceCamera.transform.position = Vector3.Lerp
                (
                    cameraTargetPositions[1],
                    cameraTargetPositions[2],
                    (timeElapsed - (lerpTime * normalizedSplitDistance)) / (lerpTime * (1 - normalizedSplitDistance))
                );
            faceCamera.transform.rotation = Quaternion.Lerp
                (
                    cameraTargetRotations[1],
                    cameraTargetRotations[2],
                    (timeElapsed - (lerpTime * normalizedSplitDistance)) / (lerpTime * (1 - normalizedSplitDistance))
                );

            timeElapsed += Time.deltaTime;
            yield return null;
        }

    }

    public void TriggerZoomOut(float lerpTime, float normalizedSplitDistance)
    {

        StopAllCoroutines();
        StartCoroutine(ZoomOut(lerpTime, normalizedSplitDistance));

    }

    private IEnumerator ZoomOut(float lerpTime, float normalizedSplitDistance)
    {

        timeElapsed = lerpTime - timeElapsed;
        if(timeElapsed < 0) timeElapsed = 0;

        while(timeElapsed < lerpTime * (1 - normalizedSplitDistance))
        {
            mainCamera.GetComponent<Camera>().rect = new Rect(0f, 0f, 0.5f, 1f);
            faceCamera.GetComponent<Camera>().rect = new Rect(0.5f, 0f, 0.5f, 1f);

            mainCamera.transform.position = Vector3.Lerp
                (
                    cameraTargetPositions[3],
                    cameraTargetPositions[1],
                    timeElapsed / (lerpTime * (1 - normalizedSplitDistance))
                );
            mainCamera.transform.rotation = Quaternion.Lerp
                (
                    cameraTargetRotations[3],
                    cameraTargetRotations[1],
                    timeElapsed / (lerpTime * (1 - normalizedSplitDistance))
                );

            faceCamera.transform.position = Vector3.Lerp
                (
                    cameraTargetPositions[2],
                    cameraTargetPositions[1],
                    timeElapsed / (lerpTime * (1 - normalizedSplitDistance))
                );
            faceCamera.transform.rotation = Quaternion.Lerp
                (
                    cameraTargetRotations[2],
                    cameraTargetRotations[1],
                    timeElapsed / (lerpTime * (1 - normalizedSplitDistance))
                );

            timeElapsed += Time.deltaTime;
            yield return null;
        }
        while(timeElapsed < lerpTime)
        {
            mainCamera.GetComponent<Camera>().rect = new Rect(0f, 0f, 1f, 1f);

            mainCamera.transform.position = Vector3.Lerp
                (
                    cameraTargetPositions[1],
                    cameraTargetPositions[0],
                    (timeElapsed - (lerpTime * (1 - normalizedSplitDistance))) / (lerpTime * normalizedSplitDistance)
                );
            mainCamera.transform.rotation = Quaternion.Lerp
                (
                    cameraTargetRotations[1],
                    cameraTargetRotations[0],
                    (timeElapsed - (lerpTime * (1 - normalizedSplitDistance))) / (lerpTime * normalizedSplitDistance)
                );

            timeElapsed += Time.deltaTime;
            yield return null;
        }

    }

    public void TriggerObjectTracking(GameObject objectToTrack, float lerpTime, float normalizedSplitDistance)
    {

        StopAllCoroutines();

        trackedObject = objectToTrack.transform;

        StartCoroutine(BeginTrackingObject(lerpTime, normalizedSplitDistance));

    }

    private IEnumerator BeginTrackingObject(float lerpTime, float normalizedSplitDistance)
    {

        Vector3 destinationPosition = trackedObject.position + trackingPositionDisplacement;

        timeElapsed = 0;

        while(timeElapsed < lerpTime)
        {
            mainCamera.GetComponent<Camera>().rect = new Rect(0f, 0f, 1f, 1f);

            mainCamera.transform.position = Vector3.Lerp
                (
                    cameraTargetPositions[1],
                    destinationPosition,
                    timeElapsed / lerpTime
                );
            mainCamera.transform.rotation = Quaternion.Lerp
                (
                    cameraTargetRotations[1],
                    trackingRotation,
                    timeElapsed / lerpTime
                );

            timeElapsed += Time.deltaTime;
            yield return null;
        }

        isTrackingObject = true;

    }

    private void Update()
    {

        if(isTrackingObject)
        {
            MoveToObjectPosition();
            floorManager.UpdateFloor(mainCamera.transform.position.x);
        }

    }

    private void MoveToObjectPosition()
    {

        mainCamera.transform.position = trackedObject.transform.position + trackingPositionDisplacement;

    }

}
