using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    static public GameManager instance;
    public string channelChoice;
    public int number;
    public string winnerName;
    public List<string> names = new List<string>();
    public bool Valid = false;
    void Awake()
    {
        DontDestroyOnLoad(this);

        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }

        else
        {
            instance = this;
        }
    }
    private void Update()
    {
    }
    public void SetChoice()
    {
        InputField choice = GameObject.Find("ChannelChoice").GetComponent<InputField>();
        channelChoice = choice.text.ToLower();
        SceneManager.LoadScene(1);
    }
    public void NewDraw()
    {
        Valid = true;
        number = 0;
        winnerName = null;
        SceneManager.LoadScene(1);
        names.Clear();
    }
    public void AddList(string name, int value)
    {
        InfoHandler info = GameObject.Find("InfoHandler").GetComponent<InfoHandler>();
        if (!info.subsT.isOn && !info.modsT.isOn)
        {
            names.Add(name);
        }
        else if (info.subsT.isOn && !info.modsT.isOn)
        {

            if (value == 0)
            {
                names.Add(name);
            }
            else if (value >= 1)
            {
                names.Add(name);
                names.Add(name);
            }
        }
        else if (info.subsT.isOn && info.modsT.isOn)
        {
            if (value == 0)
            {
                names.Add(name);
            }
            else if (value == 1)
            {
                names.Add(name);
                names.Add(name);
            }
            else if (value == 2)
            {
                names.Add(name);
                names.Add(name);
                names.Add(name);
            }
        }
    }
    public void Pickname()
    {
        int total = names.Count;
        int win;
        if (total == 1)
            win = 0;
        else
            win = Random.Range(0, total);

        string winname = names[win];

        number = names.Count;
        winnerName = winname;
        GameObject.Find("StartBattle").SetActive(true);
    }
    public void RedrawName()
    {
        int total = names.Count;
        int win;
        if (total == 1)
            return;
        else
            win = Random.Range(0, total);
        if (winnerName == names[win])
            RedrawName();
        else
        winnerName = names[win];
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
