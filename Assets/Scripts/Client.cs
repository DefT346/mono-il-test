using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Client : MonoBehaviour
{
    private void Start()
    {
        Command();
    }

    public void Command()
    {
        Debug.Log("Command Code on client");
    }
}
