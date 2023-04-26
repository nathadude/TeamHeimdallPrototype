using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using WorkInstructionManager;

public class InstructionManager : MonoBehaviour
{
    public TextMeshPro textMesh;

    private WorkInstruction wi;
    public int index;


    //// Start is called before the first frame update
    //void Start()
    //{
    //    string json = WorkInstruction.SampleWorkInstruction1();

    //    wi = new WorkInstruction(json);

    //    textMesh.text = wi.Start();
    //}
    public void SetWorkInstruction(WorkInstruction workInstruction)
    {
        wi = workInstruction;
        Debug.Log("wi after manager: " + workInstruction.Start());
        textMesh.text = wi.Start();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.P))
        {
            textMesh.text = wi.GetNextInstruction();
        }
        else if (Input.GetKeyUp(KeyCode.O))
        {
            textMesh.text = wi.GetPrevInstruction();
        }

        if (Input.GetKeyUp(KeyCode.I))
        {
            textMesh.text = wi.Reset();
        }

    }

    public void GetNext()
    {
        textMesh.text = wi.GetNextInstruction();
    }

    public void GetPrev()
    {
        textMesh.text = wi.GetPrevInstruction();
    }
}