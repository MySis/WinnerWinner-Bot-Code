using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FightController : MonoBehaviour
{
    [SerializeField] List<GameObject> prefab;
    [SerializeField] List<Transform> locs;
    [SerializeField] GameObject newDraw;
    public List<GameObject> gos = new List<GameObject>();
    public int total;
    public bool again;
    private bool startend = true;
    // Start is called before the first frame update
    void Start()
    {
        total = GameObject.Find("GameManager").GetComponent<GameManager>().number;
        StartCoroutine("InstantiatePrefab");
    }
    private void Update()
    {
    }
    IEnumerator InstantiatePrefab()
    {
        for (int i = 0; i < total; i++)
        {
            int place = Random.Range(0, locs.Count);
            int fighter = Random.Range(0, prefab.Count);

            GameObject pref = (GameObject)Instantiate(prefab[fighter], locs[place]);
            gos.Add(pref);

        }
        yield return new WaitForSeconds(0.5f);
        int rando = Random.Range(0, total);
        gos[rando].GetComponent<Bandit>().setwinner();
    }
    public void death()
    {
        Bandit[] Bandits = gameObject.GetComponentsInChildren<Bandit>();
        int alive = 0;
        foreach (Bandit bandit in Bandits)
        {
            if (bandit.gameObject.CompareTag("Player"))
            {
                alive += 1;
            }
            
        }
        Debug.Log("Alive count " + alive);
        if (alive == 1)
            StartCoroutine("WinSequence");         
    }
    public IEnumerator WinSequence()
    {
        yield return new WaitForSeconds(5);
        GameManager gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        Text winner = GameObject.Find("Winner").GetComponent<Text>();
        Text name = GameObject.Find("Name").GetComponent<Text>();
        GameObject WinCan = GameObject.Find("Winner");
        name.text = gm.winnerName;

        GameObject.FindGameObjectWithTag("Player").GetComponent<Bandit>().StartCoroutine("WinBan");

        Vector3 originalscale = WinCan.transform.localScale;
        Vector3 targetScale = new Vector3(1, 1, 1);
        Color winTextcolor = winner.color;
        Color nameTextcolor = name.color;
        int originalTime = 4;
        float time = 0;
        while (time < originalTime)
        {
            time += Time.deltaTime;
            {
                WinCan.transform.localScale = Vector3.Lerp(originalscale, targetScale, time / originalTime);
                winner.color = Color.Lerp(winTextcolor, new Color(256, 256, 256, 256), time / originalTime);
                name.color = Color.Lerp(nameTextcolor, new Color(256, 256, 256, 256), time / originalTime);
            }
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForSeconds(3);

        newDraw.SetActive(true);

        if (startend)
        {
            GameObject.Find("SpawnPoints").GetComponent<WinnerScene>().endBattle();
            startend = false;
        }
    }
    public void newdraw()
    {
        GameObject.Find("GameManager").GetComponent<GameManager>().NewDraw();
    }
    public void Redraw()
    {
        GameObject.Find("GameManager").GetComponent<GameManager>().RedrawName();
    }
}
