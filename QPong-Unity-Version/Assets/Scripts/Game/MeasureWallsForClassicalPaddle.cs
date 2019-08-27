using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeasureWallsForClassicalPaddle : MonoBehaviour
{
    public bool updateCircuit;
    public bool measureFlag=true;
    private Rigidbody2D rb2d;
    CircuitGridClient circuitGridClientScript;
    void Start(){
        circuitGridClientScript = GameObject.Find("CircuitGrid").GetComponent<CircuitGridClient>();
    }

    void Update(){
        rb2d = GameObject.FindGameObjectWithTag("Ball").GetComponent<Rigidbody2D>();
    }
    void OnTriggerEnter2D(Collider2D hitInfo){
        if (hitInfo.tag == "Ball"){
            Debug.Log("Trigger!");
            // trigger only if the ball is going up
            if ((rb2d.velocity[1] > 0) && measureFlag) {
                updateCircuit = false;
                measureFlag = false;
                circuitGridClientScript.doMeasurementFlag = true;
                Debug.Log("Do Measurement!");
            }
        }
    }

    void OnTriggerExit2D(Collider2D hitInfo){
        if (hitInfo.tag == "Ball"){
            circuitGridClientScript.getStatevectorFlag = true;
            measureFlag = true;
            Debug.Log("Get Statevector Again!");
        }
    }
}
