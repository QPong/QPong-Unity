using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json; 

public class CircuitGridClient : MonoBehaviour
{
    // Start is called before the first frame update

    public bool sendGateArray;
    public string gateArrayString;
    private GameObject CircuitGrid;
    private CircuitGridControl CircuitGridControlScript;

    public GameObject[] paddleArray;
    
    void Start()
    {
        GameObject CircuitGrid = GameObject.Find("CircuitGrid");
        CircuitGridControl CircuitGridControlScript = CircuitGrid.GetComponent<CircuitGridControl>();
        // paddleArray = CircuitGridControlScript.paddleArray;
        var gateArrayString = string.Join(",", GameObject.Find("CircuitGrid").GetComponent<CircuitGridControl>().gateArray);
        StartCoroutine(SendRequest(gateArrayString));
    }

    void Update()
    {
        if (sendGateArray) {
            sendGateArray = false;
            var gateArrayString = string.Join(",", GameObject.Find("CircuitGrid").GetComponent<CircuitGridControl>().gateArray);
            StartCoroutine(SendRequest(gateArrayString));
        }
    }

    IEnumerator SendRequest(string gateArrayString)
    {
        Debug.Log("Send Gate Array: "+ gateArrayString);
        List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        formData.Add(new MultipartFormDataSection("gate_array", gateArrayString));
        UnityWebRequest www = UnityWebRequest.Post("http://127.0.0.1:8008/api/run/gate_array", formData);
        yield return www.SendWebRequest();
        Debug.Log("Response: " + www.downloadHandler.text);
        
        // Deserialize stateVector from JSON
        var obj = JsonConvert.DeserializeObject<RootObject>(www.downloadHandler.text);
        var numberOfState = obj.shape[0];
        Complex[] stateVector = new Complex[numberOfState];
        double[] stateProbability = new double[numberOfState];
        paddleArray = GameObject.Find("CircuitGrid").GetComponent<CircuitGridControl>().paddleArray;
        for (int i = 0; i < numberOfState; i++){
            stateVector[i] = new Complex(obj.__ndarray__[i].__complex__[0],obj.__ndarray__[i].__complex__[1]);
            stateProbability[i] = Complex.Pow(stateVector[i],2).Magnitude;
            paddleArray[i].GetComponent<SpriteRenderer> ().color = new Color (1,1,1,(float)stateProbability[i]);
        }
        Debug.Log("State Probability: ["+string.Join(", ", stateProbability)+"]");


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
