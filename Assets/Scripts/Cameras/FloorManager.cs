using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorManager : MonoBehaviour
{

    [SerializeField] private GameObject floorPrefab;
    private Queue<GameObject> activeFloors = new Queue<GameObject>();
    private Transform currentFloorTransform;
    private float spacing;
    [SerializeField] private int maxFloors;

    private void Start()
    {

        GameObject startingPlatform = transform.GetChild(0).gameObject;
        GameObject firstFloor = transform.GetChild(1).gameObject;

        activeFloors.Enqueue(startingPlatform);
        activeFloors.Enqueue(firstFloor);

        currentFloorTransform = firstFloor.transform;
        spacing = currentFloorTransform.localScale.x;

    }

    public void UpdateFloor(float cameraX)
    {

        if(cameraX > currentFloorTransform.position.x)
        {
            Vector3 newPosition = new Vector3
            (
                currentFloorTransform.position.x + spacing,
                currentFloorTransform.position.y,
                currentFloorTransform.position.z
            );

            GameObject nextFloor = Instantiate(floorPrefab, newPosition, currentFloorTransform.rotation);

            activeFloors.Enqueue(nextFloor);

            currentFloorTransform = nextFloor.GetComponent<Transform>();

        }
        if(activeFloors.Count > maxFloors) Destroy(activeFloors.Dequeue());

    }


}
