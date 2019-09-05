using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ArcadeAPIController : MonoBehaviour
{

    private const string API_URL = "http://qarcade-controls-2.local:5000/";
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    public void ButtonPressed(ArcadeButtonGates gate)
    {
        //we have to know which button was pressed, so use the Gate Enums
        //public enum ArcadeButtonGates { None, iz, zi, ih, hi, ix, xi, cz };

        string gateName = gate.ToString();
        string urlString = API_URL + "pressed?gate=" + gateName;
        // print("reaching out to tell the arcacde controls to go " + urlString);
        StartCoroutine(GetRequest(urlString));
    }

    public void SetupPuzzle(ArcadeButtonGates[] gates)
    {
        string gatesToDisable = string.Join(", ", gates);
        string urlString = API_URL + "puzzle/setup?gatesDisabled=" + UnityWebRequest.EscapeURL(gatesToDisable);
        // print("gates disabled " + urlString);
        StartCoroutine(GetRequest(urlString));
    }

    public void InitGame(string gameName)
    {
        string urlString = API_URL + "init?game=" + UnityWebRequest.EscapeURL(gameName);
        StartCoroutine(GetRequest(urlString));
    }

    public void PuzzleSolved()
    {
        string urlString = API_URL + "solved";
        // print("arcade controls solved " + urlString);
        StartCoroutine(GetRequest(urlString));
    }

    public void LostPoint()
    {
        string urlString = API_URL + "lostpoint";
        StartCoroutine(GetRequest(urlString));
    }

    public void GameLost()
    {
        string urlString = API_URL + "gamelost";
        StartCoroutine(GetRequest(urlString));
    }


    public IEnumerator GetRequest(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            print(uri);
            // Request and wait for return
            yield return webRequest.SendWebRequest();

            if (webRequest.isNetworkError)
            {
                //   Debug.Log(pages[page] + ": Error: " + webRequest.error);
            }
            else
            {
                // Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
            }
        }
    }
}
