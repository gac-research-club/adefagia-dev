using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Border : MonoBehaviour
{
    public static bool CanMoveLeft, CanMoveTop, CanMoveRight, CanMoveBottom;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        // Debug.Log(other.tag);
        if (other.CompareTag("Border"))
        {
            if (other.gameObject.name == "BorderLeft")
            {
                Debug.Log("Left");
                CanMoveLeft = false;
            }
            
        }
        else
        {
            Debug.Log("Not Left");
            CanMoveLeft = true;
        }
    }
}
