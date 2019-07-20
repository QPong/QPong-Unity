using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;

public enum Endpoint { get_statevector, do_measurement }

public class CircuitGridClient : MonoBehaviour
{

    private const string API_URL = "http://127.0.0.1:8008/";
    private const string API_VERSION = "api/run/";
    private string circuitDimensionString;

    // Start is called before the first frame update
    public bool getStatevectorFlag;
    public bool doMeasurementFlag;
    public int qubitNumber;
    public int circuitDepth;
    public int stateNumber;
    private GameObject CircuitGrid;
    private CircuitGridControl CircuitGridControlScript;
    private string gateString => string.Join(",", GameObject.Find("CircuitGrid").GetComponent<CircuitGridControl>().gateArray);

    public GameObject[] paddleArray;
    
    void Start()
    {
        qubitNumber = GameObject.Find("CircuitGrid").GetComponent<CircuitGridControl>().qubitNumber;
        circuitDepth = GameObject.Find("CircuitGrid").GetComponent<CircuitGridControl>().circuitDepth;
        stateNumber = (int) Math.Pow(2, qubitNumber);
        circuitDimensionString = string.Join(",", qubitNumber, circuitDepth);
        GameObject CircuitGrid = GameObject.Find("CircuitGrid");
        CircuitGridControl CircuitGridControlScript = CircuitGrid.GetComponent<CircuitGridControl>();
        // paddleArray = CircuitGridControlScript.paddleArray;
        GetStateVector(gateString);
    }

    //TODO: find out if there is ever an instance of both flags being true for one update, and do we need that to happen? can we optimize to just one?
    void Update()
    {
        if (getStatevectorFlag) {
            getStatevectorFlag = false;
            GetStateVector(gateString);
        }
       
        if (doMeasurementFlag) {
            doMeasurementFlag = false;
            DoMeasurement(gateString);
        }
    }

    private void GetStateVector(string gateString)
    {
        string urlString = API_URL + API_VERSION + Endpoint.get_statevector;
        StartCoroutine(PostRequest(urlString, circuitDimensionString, gateString, (results) => {

            // Deserialize stateVector from JSON
            //TODO: come up with a better way to abstract this out
            var obj = JsonConvert.DeserializeObject<RootObject>(results);
            Complex[] stateVector = new Complex[stateNumber];
            double[] stateProbability = new double[stateNumber];
            paddleArray = GameObject.Find("CircuitGrid").GetComponent<CircuitGridControl>().paddleArray;
            for (int i = 0; i < stateNumber; i++)
            {
                stateVector[i] = new Complex(obj.__ndarray__[i].__complex__[0], obj.__ndarray__[i].__complex__[1]);
                stateProbability[i] = Complex.Pow(stateVector[i], 2).Magnitude;
                paddleArray[i].GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, (float)stateProbability[i]);
            }
           // Debug.Log("State Probability: [" + string.Join(", ", stateProbability) + "]");
        }));
    }

    IEnumerator DelayStateVector(string gateString, float delay)
    {
        yield return new WaitForSeconds(delay);

        GetStateVector(gateString);
    }

    private void DoMeasurement(string gateString)
    {
        Debug.Log("Send Gate Array: "+ gateString);
        string urlString = API_URL + API_VERSION + Endpoint.do_measurement;
        StartCoroutine(PostRequest(urlString, circuitDimensionString, gateString, (results) =>
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
            //StartCoroutine(DelayStateVector(gateString, 0.3f));
        }));

    }

    public IEnumerator PostRequest(string url, string circuitDimensionString, string gateString, Action<string> completionHandler)
    {
        List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        formData.Add(new MultipartFormDataSection("circuit_dimension", circuitDimensionString));
        formData.Add(new MultipartFormDataSection("gate_array", gateString));
        using (UnityWebRequest webRequest = UnityWebRequest.Post(url, formData))
        {
            print(url);
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
