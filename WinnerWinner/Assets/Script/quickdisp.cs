using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class quickdisp : MonoBehaviour
{
    float timer = 5f;
    // Start is called before the first frame update
    void Start()
    {
        timer = 5f;
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
            gameObject.SetActive(false);
    }
}
