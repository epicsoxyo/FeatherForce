using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOutOfBounds : MonoBehaviour
{
    
    private void Update()
    {

        if(transform.position.y <= -10) Destroy(gameObject);

    }

}
