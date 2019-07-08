using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeasureWalls : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D hitInfo){
        if (hitInfo.name == "Ball"){
            GameObject.Find("CircuitGrid").GetComponent<CircuitGridClient>().doMeasurementFlag = true;
            Debug.Log("Do Measurement!");
        }
    }

}
