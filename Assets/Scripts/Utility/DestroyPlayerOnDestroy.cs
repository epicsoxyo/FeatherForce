using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyPlayerOnDestroy : MonoBehaviour
{

    private GameObject player;

    private void Start()
    {
        
        player = GameObject.FindWithTag("Player");

    }

    private void OnDestroy()
    {

        Destroy(player);

    }

}