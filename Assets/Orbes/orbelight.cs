using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class orbelight : MonoBehaviour
{


     // Add this script to Cube(2)  
     [Header("Add your turret")]
     public GameObject Turret;//to get the position in worldspace to which this gameObject will rotate around.
 
     [Header("The axis by which it will rotate around")]
     public Vector3 axis;//by which axis it will rotate. x,y or z.
 
     [Header("Angle covered per update")]
     public float angle; //or the speed of rotation.

    // Update is called once per frame
    void Update()
    {
        transform.RotateAround(Turret.transform.position, axis, angle);
    }
}
