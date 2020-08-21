using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public bool TouchStart = false;
    private Vector2 StartPoint;
    private Vector2 EndPoint;
    public GameObject InnerCircle;
    private Vector3 mousePos;
    public GameObject[] PlayerDirections;
    private int currentDir = 0;
    private int previousDir = 0;
    private float Direction;
    public GameObject player;
    public LayerMask WallLayer;
    public LayerMask EnemyLayer;
    MapManager mm;
    public GameObject zhangai;

    private void Start()
    {
        mm = FindObjectOfType<MapManager>();
    }
    public void ChangeDirection(int dir)//检测是否可以转向
    {
        previousDir = dir;
        if (dir == -1)
        {
            PlayerDirections[0].SetActive(false);
            PlayerDirections[1].SetActive(false);
            PlayerDirections[2].SetActive(false);
            PlayerDirections[3].SetActive(false);
            PlayerDirections[4].SetActive(false);
            PlayerDirections[5].SetActive(false);
            PlayerDirections[6].SetActive(false);
            PlayerDirections[7].SetActive(false);
            return;
        }
        for (int i = 0; i < 8; i++)
        {
            if (i == dir)
            {
                PlayerDirections[i].SetActive(true);
            }
            else
            {
                PlayerDirections[i].SetActive(false);
            }
        }
    }
    private RaycastHit2D hit;
    void Update()
    {
        //判断是否已经无法移动
        if (Physics2D.Raycast(player.transform.position, new Vector2(-1, 0), 0.75f, WallLayer) && Physics2D.Raycast(player.transform.position, new Vector2(1, 0), 0.75f, WallLayer) && Physics2D.Raycast(player.transform.position, new Vector2(0, 1), 0.75f, WallLayer) && Physics2D.Raycast(player.transform.position, new Vector2(0, -1), 0.75f, WallLayer))
        {
            Destroy(player.gameObject);
            FindObjectOfType<MapManager>().lose();
        }
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        #region Joystick Position
        if (Input.GetMouseButtonDown(0) && mousePos.x > 0 && mousePos.x < 9 && mousePos.y > 0 && mousePos.y < 9) //鼠标按下时(屏幕内)
        {
            transform.position = new Vector2(mousePos.x, mousePos.y);
            StartPoint = new Vector2(mousePos.x, mousePos.y);
            InnerCircle.transform.position = StartPoint;
            InnerCircle.GetComponent<SpriteRenderer>().enabled = true;
            TouchStart = true;
        }
        else if (Input.GetMouseButtonDown(0) && (mousePos.x > 9 || mousePos.x < 0 || mousePos.y > 9 || mousePos.y < 0)) //鼠标按下时(屏幕外)
        {
            transform.position = new Vector2(Mathf.Clamp(mousePos.x, 0, 9), Mathf.Clamp(mousePos.y, 0, 9));
            StartPoint = new Vector2(mousePos.x, mousePos.y);
            InnerCircle.transform.position = StartPoint;
            InnerCircle.GetComponent<SpriteRenderer>().enabled = true;
            TouchStart = true;
        }
        if (Input.GetMouseButton(0) && TouchStart)
        {
            EndPoint = new Vector2(mousePos.x, mousePos.y);
        }
        #endregion
        if (TouchStart)
        {
            Vector2 offset = EndPoint - StartPoint;
            if (previousDir != currentDir)
            {
                ChangeDirection(currentDir);
            }
            if (offset.magnitude > 0.35f)  //当处于外轮盘时选择方向
            {
                Direction = Quaternion.FromToRotation(offset, new Vector2(0, 1)).eulerAngles.z;
                if (Direction > 337.5f || Direction < 22.5f)//上
                {
                    if (!Physics2D.Raycast(player.transform.position, new Vector2(0, 1), 0.75f, WallLayer))
                        currentDir = 0;
                    else
                        currentDir = -1;
                }
                else if (Direction > 22.5f && Direction < 67.5f)//右上
                {
                    if (!Physics2D.Raycast(new Vector2(player.transform.position.x+1, player.transform.position.y+1), new Vector2(1, 1), 0.1f, WallLayer)&&(!Physics2D.Raycast(player.transform.position, new Vector2(0, 1), 0.75f, WallLayer)|| !Physics2D.Raycast(player.transform.position, new Vector2(1, 0), 0.75f, WallLayer)))
                        currentDir = 1;
                    else
                        currentDir = -1;
                }
                else if (Direction > 67.5f && Direction < 112.5f)//右
                {
                    if (!Physics2D.Raycast(player.transform.position, new Vector2(1, 0), 0.75f, WallLayer))
                        currentDir = 2;
                    else
                        currentDir = -1;
                }
                else if (Direction > 112.5f && Direction < 157.5f)//右下
                {
                    if (!Physics2D.Raycast(new Vector2(player.transform.position.x + 1, player.transform.position.y - 1), new Vector2(1, -1), 0.1f, WallLayer) && (!Physics2D.Raycast(player.transform.position, new Vector2(0, -1), 0.75f, WallLayer) || !Physics2D.Raycast(player.transform.position, new Vector2(1, 0), 0.75f, WallLayer)))
                        currentDir = 3;
                    else
                        currentDir = -1;
                }
                else if (Direction > 157.5f && Direction < 202.5f)//下
                {
                    if (!Physics2D.Raycast(player.transform.position, new Vector2(0, -1), 0.75f, WallLayer))
                        currentDir = 4;
                    else
                        currentDir = -1;
                }
                else if (Direction > 202.5f && Direction < 247.5f)//左下
                {
                    if (!Physics2D.Raycast(new Vector2(player.transform.position.x - 1, player.transform.position.y - 1), new Vector2(-1, -1), 0.1f, WallLayer) && (!Physics2D.Raycast(player.transform.position, new Vector2(-1, 0), 0.75f, WallLayer) || !Physics2D.Raycast(player.transform.position, new Vector2(0, -1), 0.75f, WallLayer)))
                        currentDir = 5;
                    else
                        currentDir = -1;
                }
                else if (Direction > 247.5f && Direction < 292.5f)//左
                {
                    if (!Physics2D.Raycast(player.transform.position, new Vector2(-1, 0), 0.75f, WallLayer))
                        currentDir = 6;
                    else
                        currentDir = -1;
                }
                else if (Direction > 292.5f && Direction < 337.5f)//左上
                {
                    if (!Physics2D.Raycast(new Vector2(player.transform.position.x - 1, player.transform.position.y + 1), new Vector2(-1, 1), 0.1f, WallLayer) && (!Physics2D.Raycast(player.transform.position, new Vector2(0, 1), 0.75f, WallLayer) || !Physics2D.Raycast(player.transform.position, new Vector2(-1, 0), 0.75f, WallLayer)))
                        currentDir = 7;
                    else
                        currentDir = -1;
                }
            }
            else if (offset.magnitude < 0.35f) //当处于内轮盘时取消移动
            {
                currentDir = -1;
            }
            Vector2 dir = Vector2.ClampMagnitude(offset, 1.75f);
            InnerCircle.transform.position = new Vector2(StartPoint.x + dir.x, StartPoint.y + dir.y);
            if (offset.magnitude > 1 && Input.GetMouseButtonUp(0) && currentDir != -1)//松开左键时移动
            {
                //移动
                switch (previousDir)
                {
                    case 0://上
                        hit = Physics2D.Raycast(player.transform.position, new Vector2(0, 1), 0.75f, EnemyLayer);
                        if (hit && !mm.Alerted)
                        {
                            if (hit.transform.rotation.eulerAngles.z != 0 && hit.transform.rotation.eulerAngles.z != 315 && hit.transform.rotation.eulerAngles.z != 45)
                            {
                                Destroy(hit.collider.gameObject);
                                mm.goal -= 1;
                            }
                            else
                            {
                                Destroy(player.gameObject);
                                FindObjectOfType<MapManager>().lose();
                            }
                        }
                        else if (hit && mm.Alerted)
                        {
                            Destroy(player.gameObject);
                            FindObjectOfType<MapManager>().lose();
                        }
                        var zhang=Instantiate(zhangai, player.transform);
                        zhang.transform.parent = null;
                        player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y + 1);
                        break;
                    case 1://右上
                        hit = Physics2D.Raycast(player.transform.position, new Vector2(1, 1), 0.75f, EnemyLayer);
                        if (hit && !mm.Alerted)
                        {
                            if (hit.transform.rotation.eulerAngles.z != 270 && hit.transform.rotation.eulerAngles.z != 315 && hit.transform.rotation.eulerAngles.z != 0)
                            {
                                Destroy(hit.collider.gameObject);
                                mm.goal -= 1;
                            }
                            else
                            {
                                Destroy(player.gameObject);
                                FindObjectOfType<MapManager>().lose();
                            }
                        }
                        else if (hit && mm.Alerted)
                        {
                            Destroy(player.gameObject);
                            FindObjectOfType<MapManager>().lose();
                        }
                        var zhang1 = Instantiate(zhangai, player.transform);
                        zhang1.transform.parent = null;
                        player.transform.position = new Vector3(player.transform.position.x + 1, player.transform.position.y + 1);
                        break;
                    case 2://右
                        hit = Physics2D.Raycast(player.transform.position, new Vector2(1, 0), 0.75f, EnemyLayer);
                        if (hit && !mm.Alerted)
                        {
                            if (hit.transform.rotation.eulerAngles.z != 225 && hit.transform.rotation.eulerAngles.z != 315 && hit.transform.rotation.eulerAngles.z != 270)
                            {
                                Destroy(hit.collider.gameObject);
                                mm.goal -= 1;
                            }
                            else
                            {
                                Destroy(player.gameObject);
                                FindObjectOfType<MapManager>().lose();
                            }
                        }
                        else if (hit && mm.Alerted)
                        {
                            Destroy(player.gameObject);
                            FindObjectOfType<MapManager>().lose();
                        }
                        var zhang2 = Instantiate(zhangai, player.transform);
                        zhang2.transform.parent = null;
                        player.transform.position = new Vector3(player.transform.position.x + 1, player.transform.position.y);
                        break;
                    case 3://右下
                        hit = Physics2D.Raycast(player.transform.position, new Vector2(1, -1), 0.75f, EnemyLayer);
                        if (hit && !mm.Alerted)
                        {
                            if (hit.transform.rotation.eulerAngles.z != 180 && hit.transform.rotation.eulerAngles.z != 225 && hit.transform.rotation.eulerAngles.z != 270)
                            {
                                Destroy(hit.collider.gameObject);
                                mm.goal -= 1;
                            }
                            else
                            {
                                Destroy(player.gameObject);
                                FindObjectOfType<MapManager>().lose();
                            }
                        }
                        else if (hit && mm.Alerted)
                        {
                            Destroy(player.gameObject);
                            FindObjectOfType<MapManager>().lose();
                        }
                        var zhang3 = Instantiate(zhangai, player.transform);
                        zhang3.transform.parent = null;
                        player.transform.position = new Vector3(player.transform.position.x + 1, player.transform.position.y - 1);
                        break;
                    case 4://下
                        hit = Physics2D.Raycast(player.transform.position, new Vector2(0, -1), 0.75f, EnemyLayer);
                        if (hit && !mm.Alerted)
                        {
                            if (hit.transform.rotation.eulerAngles.z != 225 && hit.transform.rotation.eulerAngles.z != 180 && hit.transform.rotation.eulerAngles.z != 135)
                            {
                                Destroy(hit.collider.gameObject);
                                mm.goal -= 1;
                            }
                            else
                            {
                                Destroy(player.gameObject);
                                FindObjectOfType<MapManager>().lose();
                            }
                        }
                        else if (hit && mm.Alerted)
                        {
                            Destroy(player.gameObject);
                            FindObjectOfType<MapManager>().lose();
                        }
                        var zhang4 = Instantiate(zhangai, player.transform);
                        zhang4.transform.parent = null;
                        player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y - 1);
                        break;
                    case 5://左下
                        hit = Physics2D.Raycast(player.transform.position, new Vector2(-1, -1), 0.75f, EnemyLayer);
                        if (hit && !mm.Alerted)
                        {
                            if (hit.transform.rotation.eulerAngles.z != 180 && hit.transform.rotation.eulerAngles.z != 135 && hit.transform.rotation.eulerAngles.z != 90)
                            {
                                Destroy(hit.collider.gameObject);
                                mm.goal -= 1;
                            }
                            else
                            {
                                Destroy(player.gameObject);
                                FindObjectOfType<MapManager>().lose();
                            }
                        }
                        else if (hit && mm.Alerted)
                        {
                            Destroy(player.gameObject);
                            FindObjectOfType<MapManager>().lose();
                        }
                        var zhang5 = Instantiate(zhangai, player.transform);
                        zhang5.transform.parent = null;
                        player.transform.position = new Vector3(player.transform.position.x - 1, player.transform.position.y - 1);
                        break;
                    case 6://左
                        hit = Physics2D.Raycast(player.transform.position, new Vector2(-1, 0), 0.75f, EnemyLayer);
                        if (hit && !mm.Alerted)
                        {
                            if (hit.transform.rotation.eulerAngles.z != 135 && hit.transform.rotation.eulerAngles.z != 90 && hit.transform.rotation.eulerAngles.z != 45)
                            {
                                Destroy(hit.collider.gameObject);
                                mm.goal -= 1;
                            }
                            else
                            {
                                Destroy(player.gameObject);
                                FindObjectOfType<MapManager>().lose();
                            }
                        }
                        else if (hit && mm.Alerted)
                        {
                            Destroy(player.gameObject);
                            FindObjectOfType<MapManager>().lose();
                        }
                        var zhang6 = Instantiate(zhangai, player.transform);
                        zhang6.transform.parent = null;
                        player.transform.position = new Vector3(player.transform.position.x - 1, player.transform.position.y);
                        break;
                    case 7://左上
                        hit = Physics2D.Raycast(player.transform.position, new Vector2(-1, 1), 0.75f, EnemyLayer);
                        if (hit && !mm.Alerted)
                        {
                            if (hit.transform.rotation.eulerAngles.z != 0 && hit.transform.rotation.eulerAngles.z != 90 && hit.transform.rotation.eulerAngles.z != 45)
                            {
                                Destroy(hit.collider.gameObject);
                                mm.goal -= 1;
                            }
                            else
                            {
                                Destroy(player.gameObject);
                                FindObjectOfType<MapManager>().lose();
                            }
                        }
                        else if (hit && mm.Alerted)
                        {
                            Destroy(player.gameObject);
                            FindObjectOfType<MapManager>().lose();
                        }
                        var zhang7 = Instantiate(zhangai, player.transform);                        
                        zhang7.transform.parent = null;
                        //mm.blocks.Length += 1;
                        player.transform.position = new Vector3(player.transform.position.x - 1, player.transform.position.y + 1);
                        break;
                }
                TouchStart = false;
                mm.canMoveCounter -= 1;
            }
        }
        else
        {
            InnerCircle.GetComponent<SpriteRenderer>().enabled = false;
        }

    }
}
