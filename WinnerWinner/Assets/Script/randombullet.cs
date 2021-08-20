using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class randombullet : MonoBehaviour
{
    Rigidbody2D rigidbody2d;
    public float force;
    private Vector3 aim;
    private float timer = 3f;

    void Awake()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
    }
    void Start()
    {
        int number = Random.Range(0, 5);
        GetComponent<Animator>().SetInteger("Bullet", number);        
    }
    public void Update()
    {
            timer -= Time.deltaTime;

            rigidbody2d.AddForce(aim * force);

            if (Vector2.Distance(new Vector2(0, 0), transform.position) >= 20)
                Destroy(gameObject, 0);

            if (timer < 0)
                Destroy(gameObject);
    }
    public void Launch(GameObject target)
    {
        aim = (target.transform.position - transform.position);
    }
}