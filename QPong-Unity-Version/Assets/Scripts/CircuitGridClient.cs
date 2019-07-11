using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json; 

public class CircuitGridClient : MonoBehaviour
{

    private const string API_URL = "http://127.0.0.1:8008/";
    private const string API_VERSION = "api/run/";

    // Start is called before the first frame update
    public bool getStatevectorFlag;
    public bool doMeasurementFlag;
    public int numberOfQubits;
    public int numberOfStates;
    private GameObject CircuitGrid;
    private CircuitGridControl CircuitGridControlScript;
    private string gateArrayString => string.Join(",", GameObject.Find("CircuitGrid").GetComponent<CircuitGridControl>().gateArray);

    public GameObject[] paddleArray;
    
    void Start()
    {
        numberOfQubits = GameObject.Find("CircuitGrid").GetComponent<CircuitGridControl>().rowMax;
        numberOfStates = (int) Math.Pow(2, numberOfQubits);
        GameObject CircuitGrid = GameObject.Find("CircuitGrid");
        CircuitGridControl CircuitGridControlScript = CircuitGrid.GetComponent<CircuitGridControl>();
        // paddleArray = CircuitGridControlScript.paddleArray;
        GetStateVector(gateArrayString);
    }

    //TODO: find out if there is ever an instance of both flags being true for one update, and do we need that to happen? can we optimize to just one?
    void Update()
    {
        if (getStatevectorFlag) {
            getStatevectorFlag = false;
            GetStateVector(gateArrayString);
        }
       
        if (doMeasurementFlag) {
            doMeasurementFlag = false;
            DoMeasurement(gateArrayString);
        }
    }

    private void GetStateVector(string gateString)
    {
        Debug.Log("Send Gate Array: "+ gateString);
        string urlString = API_URL + API_VERSION + "get_statevector";
        StartCoroutine(PostRequest(urlString, gateString, (results) => {

            // Deserialize stateVector from JSON
            //TODO: come up with a better way to abstract this out
            var obj = JsonConvert.DeserializeObject<RootObject>(results);
            Complex[] stateVector = new Complex[numberOfStates];
            double[] stateProbability = new double[numberOfStates];
            paddleArray = GameObject.Find("CircuitGrid").GetComponent<CircuitGridControl>().paddleArray;
            for (int i = 0; i < numberOfStates; i++)
            {
                stateVector[i] = new Complex(obj.__ndarray__[i].__complex__[0], obj.__ndarray__[i].__complex__[1]);
                stateProbability[i] = Complex.Pow(stateVector[i], 2).Magnitude;
                paddleArray[i].GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, (float)stateProbability[i]);
            }
           // Debug.Log("State Probability: [" + string.Join(", ", stateProbability) + "]");
        }));
    }

    IEnumerator DelayStateVector(string gatesString, float delay)
    {
        yield return new WaitForSeconds(delay);

        GetStateVector(gatesString);
    }

    private void DoMeasurement(string gatesString)
    {
        Debug.Log("Send Gate Array: "+ gatesString);
        string urlString = API_URL + API_VERSION + "do_measurement";
        StartCoroutine(PostRequest(urlString, gatesString, (results) =>
        {
            paddleArray = GameObject.Find("CircuitGrid").GetComponent<CircuitGridControl>().paddleArray;
            for (int i = 0; i < 8; i++)
            {
                // make all states invisible and disable colliders
                paddleArray[i].GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
                paddleArray[i].GetComponent<BoxCollider2D>().enabled = false;
            }
            int stateInDecimal = Int32.Parse(results);
            // make the measured state visible and enable collider
            paddleArray[stateInDecimal].GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
            paddleArray[stateInDecimal].GetComponent<BoxCollider2D>().enabled = true;

            //// Show statevector representation again sometime after measurement
            StartCoroutine(DelayStateVector(gatesString, 0.3f));
        }));

    }

    public IEnumerator PostRequest(string uri, string gatesString, Action<string> completionHandler)
    {
        List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        formData.Add(new MultipartFormDataSection("gate_array", gatesString));
        using (UnityWebRequest webRequest = UnityWebRequest.Post(uri, formData))
        {
            print(uri);
            // Request and wait for return
            yield return webRequest.SendWebRequest();

            if (webRequest.isNetworkError)
            {
                // Some Sort of error to be handled
                // Debug.Log(": Error: " + webRequest.error);
            }
            else
            {
                completionHandler(webRequest.downloadHandler.text);

            }
        }
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
