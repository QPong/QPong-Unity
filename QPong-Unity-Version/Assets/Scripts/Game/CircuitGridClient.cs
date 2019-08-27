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
    public string gateString => string.Join(",", GameObject.Find("CircuitGrid").GetComponent<CircuitGridControl>().gateArray);

    public GameObject[] paddleArray;
    public GameObject[] ballArray;
    GameObject circuitGrid;
    CircuitGridControl circuitGridControlScript;
    public Sprite classicalBallSprite;
    private GameManager gameManager;

    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();

        circuitGrid = GameObject.Find("CircuitGrid");
        circuitGridControlScript = circuitGrid.GetComponent<CircuitGridControl>();

        qubitNumber = circuitGridControlScript.qubitNumber;
        circuitDepth = circuitGridControlScript.circuitDepth;
        stateNumber = (int) Math.Pow(2, qubitNumber);
        circuitDimensionString = string.Join(",", qubitNumber, circuitDepth);
        paddleArray = circuitGridControlScript.paddleArray;

        ballArray = gameManager.ballArray;
        GetStateVector(gateString);
    }

    //TODO: find out if there is ever an instance of both flags being true for one update, and do we need that to happen? can we optimize to just one?
    void Update()
    {
        if (getStatevectorFlag) {
            getStatevectorFlag = false;
            GetStateVector(gateString);
        }
        // DoMeasurement directly from measurement wall
        //if (doMeasurementFlag) {
        //    doMeasurementFlag = false;
        //    DoMeasurement(gateString);
        //}
    }

    private void GetStateVector(string gateString)
    {
        string urlString = API_URL + API_VERSION + Endpoint.get_statevector;
        StartCoroutine(PostRequest(urlString, circuitDimensionString, gateString, (results) => {

            // Deserialize stateVector from JSON
            // TODO: come up with a better way to abstract this out
            var obj = JsonConvert.DeserializeObject<RootObject>(results);
            Complex[] stateVector = new Complex[stateNumber];
            double[] stateProbability = new double[stateNumber];
            for (int i = 0; i < stateNumber; i++)
            {
                stateVector[i] = new Complex(obj.__ndarray__[i].__complex__[0], obj.__ndarray__[i].__complex__[1]);
                stateProbability[i] = Complex.Pow(stateVector[i], 2).Magnitude;
                paddleArray[i].GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, (float)stateProbability[i]);
                // Disable collider for paddles with 0 probability
                if (stateProbability[i] == 0f) {
                    paddleArray[i].GetComponent<BoxCollider2D>().enabled = false;
                } else {
                    paddleArray[i].GetComponent<BoxCollider2D>().enabled = true;
                }

            }
           // Debug.Log("State Probability: [" + string.Join(", ", stateProbability) + "]");
        }));
    }

    public void DoMeasurement(string gateString)
    {
        Debug.Log("Send Gate Array: "+ gateString);
        string urlString = API_URL + API_VERSION + Endpoint.do_measurement;
        StartCoroutine(PostRequest(urlString, circuitDimensionString, gateString, (results) =>
        {
            int stateInDecimal = Int32.Parse(results);

            for (int i = 0; i < 8; i++)
            {
                if (i==stateInDecimal) {
                    // make the measured state visible and enable collider
                    ballArray[i].GetComponent<SuperposedBallControl>().ballType = "ClassicalBall";
                    ballArray[i].GetComponent<SpriteRenderer>().color = new Color(1f, 0.2f, 1f, 1f);
                    ballArray[i].GetComponent<BoxCollider2D>().enabled = true;
                }
                else {
                    // make the other states invisible and disable collider
                    ballArray[i].GetComponent<SuperposedBallControl>().ballType = "HiddenBall";
                    ballArray[i].GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 0.2f, 0.3f);
                    ballArray[i].GetComponent<BoxCollider2D>().enabled = false;
                }
            }
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
