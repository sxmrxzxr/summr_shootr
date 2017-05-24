using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class CaptureZone : NetworkBehaviour {

    [SerializeField]
    private string ControllingTeam = "None";
    [SyncVar]
    private int ControlValue = 0;
    [SerializeField]
    private bool Locked = true;

    private const int RED_CAP = 50;
    private const int BLUE_CAP = -50;

    void RpcPlayerCapzone(string name, bool direction)
    {
        string strDirection;

        if (direction)
        {
            strDirection = "entered";
        }
        else
        {
            strDirection = "exited";
        }

        Debug.Log(name + " has " + strDirection + " capture zone controlled by " + ControllingTeam);
    }
 
    void OnTriggerEnter(Collider c)
    {
        if (c.tag == "RedTeam")
        {
            Debug.Log(c.name);
            RpcPlayerCapzone(c.name, true);
        }
        else if (c.tag == "BlueTeam")
        {
            Debug.Log(c.name);

        }       
    }
    
    void OnTriggerStay(Collider c)
    {
        //Debug.Log("Object remains in trigger");

        if (ControlValue == RED_CAP && c.tag == "RedTeam")
        {
            ControllingTeam = "Red Team";
            return;
        }
        else if (ControlValue == BLUE_CAP && c.tag == "BlueTeam")
        {
            ControllingTeam = "Blue Team";
            return;
        }

        if (c.tag == "RedTeam")
        {
            ControlValue++;
            Debug.Log(ControlValue);
        }
        else if (c.tag == "BlueTeam")
        {
            ControlValue--;
            Debug.Log(ControlValue);
        }
    }

    void OnTriggerExit(Collider c)
    {
        if (c.tag == "Player")
        {
            Debug.Log(c.name);
            RpcPlayerCapzone(c.name, false);
        }        
    }

    public void ToggleLocked()
    {
        Locked = !Locked;
    }
}
