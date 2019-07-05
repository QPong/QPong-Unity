using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CircuitGridClient : MonoBehaviour
{
    // Start is called before the first frame update

    public bool sendGateArray;
    public string gateArrayString;
    private GameObject CircuitGrid;
    private CircuitGridControl CircuitGridControlScript;
    
    void Start()
    {
        GameObject CircuitGrid = GameObject.Find("CircuitGrid");
        CircuitGridControl CircuitGridControlScript = CircuitGrid.GetComponent<CircuitGridControl>();
    }

    void Update()
    {
        var gateArrayString = string.Join(",", GameObject.Find("CircuitGrid").GetComponent<CircuitGridControl>().gateArray);
        Debug.Log(gateArrayString);
        StartCoroutine(SendRequest(gateArrayString));
    }

    IEnumerator SendRequest(string gateArrayString)
    {
        Debug.Log("Send Gate Array: "+ gateArrayString);
        List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        formData.Add(new MultipartFormDataSection("gate_array", gateArrayString));
        UnityWebRequest www = UnityWebRequest.Post("http://127.0.0.1:8888/api/run/gate_array", formData);
        yield return www.SendWebRequest();
        Debug.Log("Response: " + www.downloadHandler.text);
    }
}
