using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonBehave : MonoBehaviour
{
    [SerializeField] List<InputField> Inputs;
    [SerializeField] Toggle Message;
    [SerializeField] Button StartButt;
    private int fields;

    // Start is called before the first frame update
    void Start()
    {
        StartButt.interactable = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Message.isOn)
            fields = 4;
        else
            fields = 2;
        setchange();
    }
    public void setchange()
    {
        int total = 0;
        if (!Message.isOn)
        {
            if (Inputs[0].text != "")
                total += 1;
            if (Inputs[1].text != "")
                total += 1;
        }
        else
        {
            for (int i = 0; i < Inputs.Count; i++)
            {
                if (Inputs[i].text != "")
                    total += 1;
            }
        }       
        if (total == fields)
            StartButt.interactable = true;
        else
            StartButt.interactable = false;
    }

}

