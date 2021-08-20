using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class slowmove : MonoBehaviour
{
    private Vector2 startpos;
    private float timer = 0;
    // Start is called before the first frame update
    void Start()
    {
        startpos = transform.position;
        StartCoroutine("Movemoon");
    }

    // Update is called once per frame
    IEnumerator Movemoon()
    {
        int originalTime = 90;
        while (timer < originalTime)
        {
            timer += Time.deltaTime; 
            transform.position = Vector2.Lerp(startpos, new Vector2(-10, 4.25f), timer/originalTime);
            yield return new WaitForEndOfFrame();
        }
    }
}
