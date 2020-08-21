using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public EnemyAI[] enemies;
    public bool canMove;
    public int canMoveCounter = 2;
    public GameObject player;
    public bool isInside;
    public bool Alerted;
    public float Timer = 1;
    private float timer;
    public int goal;
    public GameObject Win;
    public GameObject Lose;
    public GameObject[] blocks;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
    public void lose()
    {
        Lose.SetActive(true);
        foreach (EnemyAI ea in enemies)
        {
            if (ea != null)
            Destroy(ea.gameObject);
        }
        Destroy(gameObject, 0.2f);
    }
    private void Update()
    {
        if (goal <= 0)
        {
            Win.SetActive(true);
            Destroy(GameObject.FindGameObjectWithTag("Player"));
            foreach (EnemyAI ea in enemies)
            {
                if (ea != null)
                    Destroy(ea.gameObject);
            }
            Destroy(gameObject, 0.2f);
        }
        //判断是否在房间内
        if (Mathf.FloorToInt(player.transform.position.x) > 0 && Mathf.FloorToInt(player.transform.position.x) < 8 && Mathf.FloorToInt(player.transform.position.y) > 0 && Mathf.FloorToInt(player.transform.position.y) < 8)
        {
            isInside = true;
        }
        else
        {
            foreach (GameObject go in GameObject.FindGameObjectsWithTag("Obstacle"))
            {
                Destroy(go);
            }
            isInside = false;
            timer += Time.deltaTime;
        }
        if (timer >= Timer)
        {
            foreach (EnemyAI ea in enemies)
            {
                if (ea != null)
                    ea.Move();
            }
            timer = 0;
        }
        if (Alerted)
        {
            foreach (EnemyAI ea in enemies)
            {
                if (ea != null)
                    ea.Alerted = true;
            }
            if (!isInside)
            {
                Alerted = false;
                foreach (EnemyAI ea in enemies)
                {
                    if (ea != null)
                        ea.OutOfAlert = true;
                }
            }
        }
            if (canMoveCounter <= 0)
        {
            canMove = true;
        }
        if (canMove)
        {
            foreach (EnemyAI ea in enemies)
            {
                if (ea != null)
                {

                    ea.Move();
                    if (Alerted)
                        ea.Move();
                }
            }
            canMove = false;
            canMoveCounter = Alerted ? 1 : 2;
        }
    }

}
