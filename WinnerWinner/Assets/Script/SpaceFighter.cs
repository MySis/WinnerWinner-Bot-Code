using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceFighter : MonoBehaviour
{
    private float speed = 0.2f;
    public bool winner;
    [SerializeField] GameObject explosion;
    [SerializeField] List<Sprite> Ships;
    [SerializeField] Transform shootfrom;
    [SerializeField] GameObject Bullet;
    private Rigidbody2D rigidbody2d;
    private GameObject newtarget;
    private GameObject center;
    private float firetimer;
    private float lifetimer;
    private float checktimer = 0.2f;
    public bool only = false;
    private bool die;
    
    private List<GameObject> Fighters = new List<GameObject>();
    

    void Start()
    {
        int ship = Random.Range(0, Ships.Count);
        gameObject.GetComponent<SpriteRenderer>().sprite = Ships[ship];
        if (winner)
            lifetimer = 9;
        else
            lifetimer = Random.Range(3, 8);
        center = GameObject.Find("Center");
        rigidbody2d = GetComponent<Rigidbody2D>();
        newtarget = center;
        GameObject[] fighters = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject fight in fighters)
            if (fight.layer == 3)
                Fighters.Add(fight);
    }
    
    // Update is called once per frame
    void Update()
    {
        if (Mathf.Abs(transform.position.x) > 8 || Mathf.Abs(transform.position.z) > 6 || only)
        newtarget = center;

        else
        {
            if (checktimer <= 0)
            {
                checktimer = 0.5f;
                if (!only)
                    Check();
            }
            else
                checktimer -= Time.deltaTime;
        }

        transform.up = newtarget.transform.position - transform.position;
        Vector3 looking = (newtarget.transform.position - transform.position);
        rigidbody2d.AddForce(looking * speed, ForceMode2D.Force);

        if (newtarget != center)
        {
            if (firetimer <= 0)
            {
                Launch();
                firetimer = 0.3f;
            }
            else
                firetimer -= Time.deltaTime;
        }

        lifetimer -= Time.deltaTime;
        if (lifetimer < 0 && !die)
        {
            die = true;
            StartCoroutine("death");
        }
    }
    void Launch()
    {
        GameObject BULL = Instantiate(Bullet, shootfrom.position, Quaternion.identity);

        randombullet projectile = BULL.GetComponent<randombullet>();
        projectile.Launch(newtarget);
    }
    void Check()
    {                
        int alive = 0;
        for (int i = 0; i < Fighters.Count; i++)
        {
            if (Fighters[i].layer == 3)
                alive += 1;
        }
        if (alive <= 1)
        {
            only = true;
            newtarget = center;
        }
        int target = Random.Range(0, Fighters.Count);
        newtarget = Fighters[target];
    }
    public void setwinner()
    {
        winner = true;
    }
    public IEnumerator death()
    {
        if (!winner)
        {
            GetComponent<AudioSource>().Play();
            explosion.SetActive(true);
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
            yield return new WaitForSeconds(0.5f);
            gameObject.layer = 7;
            gameObject.SetActive(false);
        }
        else
        {
            newtarget = center;
            float time = 4f;
            float timeelapsed = 0;
            GameObject.Find("SpawnPoints").GetComponent<SpaceController>().StartCoroutine("WinSequence");
            while (timeelapsed < time)
            {
                timeelapsed += Time.deltaTime;
                transform.localScale = Vector3.Lerp(new Vector3(1, 1, 1), new Vector3(3, 3, 3), timeelapsed / time);
                yield return new WaitForEndOfFrame();
            }
        }
    }
}
