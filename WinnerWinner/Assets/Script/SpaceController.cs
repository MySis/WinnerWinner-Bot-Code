using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpaceController : MonoBehaviour
{
    [SerializeField] GameObject prefab;
    [SerializeField] GameObject newDraw;
    public List<GameObject> gos = new List<GameObject>();
    public int total;
    public bool again;
    private bool startend = true;
    // Start is called before the first frame update
    void Start()
    {
       total = GameObject.Find("GameManager").GetComponent<GameManager>().number;
        Debug.Log("Calling Instantiate");
        for (int i = 0; i< total; i++)
            InstantiatePrefab();
        int rando = Random.Range(0, total);
        gos[rando].GetComponent<SpaceFighter>().setwinner();
    }
    private void Update()
    {
    }
    void InstantiatePrefab()
    {

            Vector2 spawnpos = findRandom();
            if (gos.Count > 1)
            {
                for (int ai = 0; ai < gos.Count; ai++)
                {
                    if (Physics2D.OverlapCircle(spawnpos, 2, 6))
                    {
                        InstantiatePrefab();
                    }

                    else
                    {
                        GameObject pref = (GameObject)Instantiate(prefab, spawnpos, Quaternion.identity);
                        pref.transform.LookAt(transform);
                        gos.Add(pref);
                        return;
                    }
                }
            }
            else
            {
                GameObject pref = (GameObject)Instantiate(prefab, spawnpos, Quaternion.identity);
                pref.transform.LookAt(transform);
                gos.Add(pref);
            }   
    }
    Vector2 findRandom()
    {
        float Horizontal = Random.Range(-5, 5);
        float Vertical = Random.Range(-5, 5);
        Vector2 spawnpos = new Vector2(Horizontal, Vertical);
        return spawnpos;
    }
    public void death()
    {
        SpaceFighter[] SF = gameObject.GetComponentsInChildren<SpaceFighter>();
        int alive = 0;
        foreach (SpaceFighter sf in SF)
        {
            if (sf.gameObject.CompareTag("Player"))
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
            GameObject.Find("TwitchIRC").GetComponent<WinnerScene>().endBattle();
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