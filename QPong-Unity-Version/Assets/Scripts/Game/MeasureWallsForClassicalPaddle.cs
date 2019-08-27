using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeasureWallsForClassicalPaddle : MonoBehaviour
{
    public bool updateCircuit;
    public bool measureFlag=true;
    private Rigidbody2D rb2d;
    CircuitGridClient circuitGridClientScript;
    private string gateString;
    void Start(){
        circuitGridClientScript = GameObject.Find("CircuitGrid").GetComponent<CircuitGridClient>();
    }

    void Update(){
        rb2d = GameObject.FindGameObjectWithTag("Ball").GetComponent<Rigidbody2D>();
    }
    void OnTriggerEnter2D(Collider2D hitInfo){
        if (hitInfo.tag == "Ball"){
            Debug.Log("Trigger!");
            Debug.Log(hitInfo.gameObject.GetComponent<SuperposedBallControl>().ballType);
            gateString = hitInfo.gameObject.GetComponent<SuperposedBallControl>().gateString;
            Debug.Log("Gate string in wall:"+gateString);
            
            // trigger only if the ball type is quantum ball
            if (hitInfo.gameObject.GetComponent<SuperposedBallControl>().ballType == "QuantumBall") {
                // trigger only if the ball is going up
                Debug.Log("Quantum Ball!");
                if ((rb2d.velocity[1] > 0) && measureFlag) {
                    updateCircuit = false;
                    measureFlag = false;
                    //circuitGridClientScript.doMeasurementFlag = true;
                    circuitGridClientScript.DoMeasurement(gateString);
                    Debug.Log("Do Measurement!");
                }
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
