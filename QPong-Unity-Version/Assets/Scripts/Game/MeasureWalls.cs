using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeasureWalls : MonoBehaviour
{
    public bool updateCircuit;
    private Rigidbody2D rb2d;
    CircuitGridClient circuitGridClientScript;
    void Start(){
        rb2d = GameObject.Find("Ball").GetComponent<Rigidbody2D>();
        circuitGridClientScript = GameObject.Find("CircuitGrid").GetComponent<CircuitGridClient>();
    }
    void OnTriggerEnter2D(Collider2D hitInfo){
        if (hitInfo.name == "Ball"){
            // trigger only if the ball is coming from up to down
            if (rb2d.velocity[1] < 0){
                updateCircuit = false;
                circuitGridClientScript.doMeasurementFlag = true;
                //Debug.Log("Do Measurement!");
            }
        }
    }

    void OnTriggerStay2D(Collider2D hitInfo){
        if (hitInfo.name == "Ball"){
            // trigger only if the ball is coming from up to down
            if (rb2d.velocity[1] < 0){
                if (updateCircuit){
                    updateCircuit = false;
                    circuitGridClientScript.doMeasurementFlag = true;
                    //Debug.Log("Do Measurement Again!");
                }
            }
        }
    }

    void OnTriggerExit2D(Collider2D hitInfo){
        if (hitInfo.name == "Ball"){
            circuitGridClientScript.getStatevectorFlag = true;
            //Debug.Log("Get Statevector Again!");
        }
    }
}
