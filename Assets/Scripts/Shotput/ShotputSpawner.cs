using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// spawns shotputs with force proportional to shakes per second at the location of the musclebird's animatedEgg
public class ShotputSpawner : MonoBehaviour
{

    private GameObject animatedEgg; // to get shotput spawn position
    [SerializeField] private float delay; // time in seconds to wait during animation until shotput release

    [SerializeField] private GameObject shotputPrefab; // shotput to spawn
    private GameObject shotput; // instanced shotput

    [SerializeField] private Vector3 forceDirection; // will be normalized
    [SerializeField] private float forceMultiplier; // will be multiplied by shakesPerSecond to get magnitude

    private void Awake()
    {

        forceDirection /= forceDirection.magnitude;

    }

    private void Start()
    {

        animatedEgg = GameObject.FindWithTag("AnimatedEgg");

    }

    public void TriggerThrowEgg(float forceMagnitude, CameraManager cameraManager, float lerpTime, float normalizedSplitDistance)
    {

        StartCoroutine(ThrowEgg(forceMagnitude, cameraManager, lerpTime, normalizedSplitDistance));

    }

    private IEnumerator ThrowEgg(float shakesPerSecond, CameraManager cameraManager, float lerpTime, float normalizedSplitDistance)
    {

        yield return new WaitForSeconds(delay);

        animatedEgg.GetComponent<MeshRenderer>().enabled = false;
        shotput = Instantiate(shotputPrefab, animatedEgg.transform.position, Quaternion.identity);

        forceDirection *= shakesPerSecond * forceMultiplier;
        shotput.GetComponent<Rigidbody>().AddForce(forceDirection, ForceMode.VelocityChange);

        cameraManager.TriggerObjectTracking(shotput, lerpTime, normalizedSplitDistance);

    }

}
