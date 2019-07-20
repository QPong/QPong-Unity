using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeasureWalls : MonoBehaviour
{
    public bool updateCircuit;
    void OnTriggerEnter2D(Collider2D hitInfo){
        if (hitInfo.name == "Ball"){
            // trigger only if the ball is coming from up to down
            if (GameObject.Find("Ball").GetComponent<Rigidbody2D>().velocity[1] < 0){
                updateCircuit = false;
                GameObject.Find("CircuitGrid").GetComponent<CircuitGridClient>().doMeasurementFlag = true;
                //Debug.Log("Do Measurement!");
            }
        }
    }

    void OnTriggerStay2D(Collider2D hitInfo){
        if (hitInfo.name == "Ball"){
            // trigger only if the ball is coming from up to down
            if (GameObject.Find("Ball").GetComponent<Rigidbody2D>().velocity[1] < 0){
                if (updateCircuit){
                    updateCircuit = false;
                    GameObject.Find("CircuitGrid").GetComponent<CircuitGridClient>().doMeasurementFlag = true;
                    //Debug.Log("Do Measurement Again!");
                }
            }
        }
    }

    void OnTriggerExit2D(Collider2D hitInfo){
        if (hitInfo.name == "Ball"){
            GameObject.Find("CircuitGrid").GetComponent<CircuitGridClient>().getStatevectorFlag = true;
            //Debug.Log("Get Statevector Again!");
        }
    }
}
