using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class QiskitSession : MonoBehaviour
{
    public bool launchQASM;
    public string qasmString;
    
    void Start()
    {
        
    }

    void Update()
    {
        if (launchQASM)
        {
            launchQASM = false;
            StartCoroutine(SendRequest(qasmString));
        }
    }
    
    IEnumerator SendRequest(string qasmString)
    {
        // Example with the Hadamard gate
        // string qasmString = "include \"qelib1.inc\"; qreg q[1]; creg c[1]; h q[0]; measure q[0] -> c[0];";
        Debug.Log("Input QASM String: "+ qasmString);
        List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        formData.Add(new MultipartFormDataSection("qasm", qasmString));
        UnityWebRequest www = UnityWebRequest.Post("http://127.0.0.1:8001/api/run/qasm", formData);
        yield return www.SendWebRequest();
        Debug.Log("Response: " + www.downloadHandler.text);
    }

}
