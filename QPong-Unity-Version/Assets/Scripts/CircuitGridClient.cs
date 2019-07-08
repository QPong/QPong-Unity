using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json; 

public class CircuitGridClient : MonoBehaviour
{
    // Start is called before the first frame update

    public bool getStatevectorFlag;
    public bool doMeasurementFlag;
    public string gateArrayString;
    public int numberOfQubits;
    public int numberOfStates;
    private GameObject CircuitGrid;
    private CircuitGridControl CircuitGridControlScript;

    public GameObject[] paddleArray;
    
    void Start()
    {
        numberOfQubits = GameObject.Find("CircuitGrid").GetComponent<CircuitGridControl>().rowMax;
        numberOfStates = (int) Math.Pow(2, numberOfQubits);
        GameObject CircuitGrid = GameObject.Find("CircuitGrid");
        CircuitGridControl CircuitGridControlScript = CircuitGrid.GetComponent<CircuitGridControl>();
        // paddleArray = CircuitGridControlScript.paddleArray;
        var gateArrayString = string.Join(",", GameObject.Find("CircuitGrid").GetComponent<CircuitGridControl>().gateArray);
        StartCoroutine(getStatevector(gateArrayString));
    }

    void Update()
    {
        if (getStatevectorFlag) {
            getStatevectorFlag = false;
            var gateArrayString = string.Join(",", GameObject.Find("CircuitGrid").GetComponent<CircuitGridControl>().gateArray);
            StartCoroutine(getStatevector(gateArrayString));
        }
        if (doMeasurementFlag) {
            doMeasurementFlag = false;
            var gateArrayString = string.Join(",", GameObject.Find("CircuitGrid").GetComponent<CircuitGridControl>().gateArray);
            StartCoroutine(doMeasurement(gateArrayString));
        }
    }

    IEnumerator getStatevector(string gateArrayString)
    {
        Debug.Log("Send Gate Array: "+ gateArrayString);
        List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        formData.Add(new MultipartFormDataSection("gate_array", gateArrayString));
        UnityWebRequest www = UnityWebRequest.Post("http://127.0.0.1:8008/api/run/get_statevector", formData);
        yield return www.SendWebRequest();
        Debug.Log("Response: " + www.downloadHandler.text);
        
        // Deserialize stateVector from JSON
        var obj = JsonConvert.DeserializeObject<RootObject>(www.downloadHandler.text);
        Complex[] stateVector = new Complex[numberOfStates];
        double[] stateProbability = new double[numberOfStates];
        paddleArray = GameObject.Find("CircuitGrid").GetComponent<CircuitGridControl>().paddleArray;
        for (int i = 0; i < numberOfStates; i++){
            stateVector[i] = new Complex(obj.__ndarray__[i].__complex__[0],obj.__ndarray__[i].__complex__[1]);
            stateProbability[i] = Complex.Pow(stateVector[i], 2).Magnitude;
            paddleArray[i].GetComponent<SpriteRenderer> ().color = new Color (1,1,1,(float)stateProbability[i]);
        }
        Debug.Log("State Probability: ["+string.Join(", ", stateProbability)+"]");
    }

    IEnumerator doMeasurement(string gateArrayString)
    {
        Debug.Log("Send Gate Array: "+ gateArrayString);
        List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        formData.Add(new MultipartFormDataSection("gate_array", gateArrayString));
        UnityWebRequest www = UnityWebRequest.Post("http://127.0.0.1:8008/api/run/do_measurement", formData);
        yield return www.SendWebRequest();
        Debug.Log("State in decimal: " + www.downloadHandler.text);

        paddleArray = GameObject.Find("CircuitGrid").GetComponent<CircuitGridControl>().paddleArray;
        for (int i = 0; i < 8; i++){
            // make all states invisible and disable colliders
            paddleArray[i].GetComponent<SpriteRenderer> ().color = new Color (1,1,1,0);
            paddleArray[i].GetComponent<BoxCollider2D> ().enabled = false;
        }
        int stateInDecimal = Int32.Parse(www.downloadHandler.text);
        // make the measured state visible and enable collider
        paddleArray[stateInDecimal].GetComponent<SpriteRenderer> ().color = new Color (1,1,1,1);
        paddleArray[stateInDecimal].GetComponent<BoxCollider2D> ().enabled = true;
    }

    public class DataObject{
        public double[] __complex__ { get; set; }
    }

    public class RootObject{
        public DataObject[] __ndarray__ { get; set; }
        public string dtype { get; set; }
        public int[] shape { get; set; }
    }


}
