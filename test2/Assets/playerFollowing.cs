using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerFollowing : MonoBehaviour
{
    public GameObject pl ;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = pl.transform.position + 10*Vector3.back;
    }
}
