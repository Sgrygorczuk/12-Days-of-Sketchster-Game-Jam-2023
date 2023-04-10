using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{

    private PlatformEffector2D effector2D;
    public float waitTime;
    void Start()
    {
        effector2D = GetComponent<PlatformEffector2D>()
        }
    void Update()
    {
        if (Input.GetKeyup(KeyCode.DownArrow))
        {
            waitTime = 0.5f;
        }
        if (Input.Getkey(KeyCode.DownArrow))
        {
            if (waitTime <= 0)
            {
                effector2D.rotationalOffset = 180f;
                waitTime = 0.5f;
            }
            else { waitTime -= waitTime.deltaTime; }


        }
        if (Input.Input.GetKeyup(KeyCode.UpArrow)) { effector2D.rotationlOffeset = 0; }



    }
}