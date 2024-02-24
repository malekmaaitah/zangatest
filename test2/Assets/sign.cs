using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sign : MonoBehaviour
{
    [SerializeField]
    private GameObject excilimation;
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "player")
        {

         excilimation.SetActive(true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "player")
        {

        excilimation.SetActive(false);
        }
    }
}
