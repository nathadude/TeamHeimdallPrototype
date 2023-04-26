using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using TMPro;
using WorkInstructionManager;

public class WebREQUEST : MonoBehaviour
{
    public string instructionURL; // URL of the web app where the instructions are stored
    public TextMeshPro instructionText; // reference to the TextMesh object in Unity

    void Start()
    {
        StartCoroutine(FetchInstructions());
    }

    IEnumerator FetchInstructions()
    {
        UnityWebRequest www = UnityWebRequest.Get(instructionURL);
        www.downloadHandler = new DownloadHandlerBuffer();

        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log("Error: " + www.error);
        }
        else
        {
            // Parse the JSON data using the JsonConvert class
            string jsonData = www.downloadHandler.text;

            Debug.Log("JsonData before wi: " + jsonData);
            WorkInstruction wi = new WorkInstruction(jsonData);
            Debug.Log("wi b4 manager: " + wi.Start());
            InstructionManager instructionManager = GetComponent<InstructionManager>();
            instructionManager.SetWorkInstruction(wi);
        }
    }
}

[System.Serializable]
public class Instruction
{
    public string text;
}