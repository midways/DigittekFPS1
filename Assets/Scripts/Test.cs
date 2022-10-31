using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


namespace Fysiskverden {
public class Test : MonoBehaviour
{
    public string lampeID = "19";
    private string hueURL = "http://192.168.50.206/api/";
    private string brugerID = "J-PTpL58W-gIelwnT8qoSUoLR4tRsbFOg84jdVO5";

    public IEnumerator StartStop(int skiftType){
        string webURLString = hueURL + brugerID + "/lights/" + lampeID + "/state";
        string dataString = "{\"on\":";
        if (skiftType==0) {dataString += "false}";}
        else if (skiftType==1){dataString += "true}";}
        Debug.Log(lampeID + ", " + webURLString +", " + dataString);
        byte[] myData = System.Text.Encoding.UTF8.GetBytes(dataString);
        using (UnityWebRequest www = UnityWebRequest.Put(webURLString, myData))
        {
            yield return www.SendWebRequest();
            if(www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
        }
    }
    // Start is called before the first frame update


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P)) {
            StartCoroutine(StartStop(0));
        }
        if (Input.GetKeyDown(KeyCode.O)) {
            StartCoroutine(StartStop(1));
        }
        
    }
}
}
