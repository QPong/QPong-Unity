using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeasureWalls : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D hitInfo){
        if (hitInfo.name == "Ball"){
            // trigger only if the ball is coming from up to down
            if (GameObject.Find("Ball").GetComponent<Rigidbody2D>().velocity[1] < 0){
                GameObject.Find("CircuitGrid").GetComponent<CircuitGridClient>().doMeasurementFlag = true;
                Debug.Log("Do Measurement!");
            }
        }
    }
}
