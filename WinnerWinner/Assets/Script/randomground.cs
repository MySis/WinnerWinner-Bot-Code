using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class randomground : MonoBehaviour
{
    [SerializeField] Material[] grounds;
    // Start is called before the first frame update
    void Start()
    {
        int grnd = Random.Range(0, grounds.Length);
        GetComponent<MeshRenderer>().material = grounds[grnd];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
