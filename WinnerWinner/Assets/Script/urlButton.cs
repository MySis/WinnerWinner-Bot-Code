using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class urlButton : MonoBehaviour  
{
    public void OpenURL()
    {
        Application.OpenURL("https://discord.gg/ZHm6tGUUGb");
        Debug.Log("is this working?");
    }

}
