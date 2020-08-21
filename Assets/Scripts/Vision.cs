using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vision : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player"&& Mathf.FloorToInt(collision.transform.position.x) > 0 && Mathf.FloorToInt(collision.transform.position.x) < 8 && Mathf.FloorToInt(collision.transform.position.y) > 0 && Mathf.FloorToInt(collision.transform.position.y) < 8)
        {
            FindObjectOfType<MapManager>().Alerted = true;
        }
    }
}
