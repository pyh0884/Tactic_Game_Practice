using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class EnemyAI : MonoBehaviour
{
    //存储路径点  
    private List<AStarPoint> mPathPosList;
    private List<Vector2> NextStep;
    public const int mGridWidth = 9;//网格大小  
    public const int mGridHeight = 9;
    private AStarPoint[,] mPointGrid;
    private AStarPoint mStartPos;
    public Vector2[] poss;
    public bool Alerted;
    public bool OutOfAlert;
    public GameObject player;
    private bool done = false;
    public GameObject vision;
    private AStarPoint mEndPos { get; set; }
    //目标点  
    private int mTargetX { get; set; }
    private int mTargetY { get; set; }

    private void Awake()
    {
        mPointGrid = AStarAlgorithm.GetInstance.mPointGrid;
        mStartPos = mPointGrid[Mathf.FloorToInt(transform.position.x), Mathf.FloorToInt(transform.position.y)];
    }
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
    private void Update()
    {
        vision.SetActive(!Alerted); //暴露时是否隐藏视野
        if (Alerted && !done)
        {
            FindPath(Mathf.FloorToInt(player.transform.position.x), Mathf.FloorToInt(player.transform.position.y));
            done = true;
        }
        if (OutOfAlert)
        {
            Alerted = false;
            FindPath((int)poss[0].x, (int)poss[0].y);
            OutOfAlert = false;
        }
        if (Mathf.FloorToInt(transform.position.x) == (int)poss[1].x && Mathf.FloorToInt(transform.position.y) == (int)poss[1].y && !Alerted&&!done)
        {
            FindPath((int)poss[0].x, (int)poss[0].y);
            done = true;
        }
        else if (Mathf.FloorToInt(transform.position.x) == (int)poss[0].x && Mathf.FloorToInt(transform.position.y) == (int)poss[0].y && !Alerted&&!done)
        {
            FindPath((int)poss[1].x, (int)poss[1].y);
            done = true;
        }
    }
    public void Move()//MovePosition at next turn
    {
        if (NextStep != null && NextStep.Count > 1)
        {
            NextStep.Remove(NextStep[NextStep.Count - 1]);
            transform.rotation = Quaternion.Euler(0, 0, -Quaternion.FromToRotation(new Vector3(NextStep[NextStep.Count - 1].x + 0.5f, NextStep[NextStep.Count - 1].y + 0.5f) - transform.position, new Vector2(0, -1)).eulerAngles.z);
            transform.position = new Vector3(NextStep[NextStep.Count - 1].x + 0.5f, NextStep[NextStep.Count - 1].y + 0.5f);
            if (player.transform.position == transform.position)
        {
            Destroy(player.gameObject);
            FindObjectOfType<MapManager>().lose();
        }
            done = false;
        }
        
    }
    public void FindPath(int mTargetX, int mTargetY)
    {
        if (mPathPosList != null)
        {
            mPathPosList.Clear();
        }
        AStarAlgorithm.GetInstance.ClearGrid();
        mEndPos = mPointGrid[mTargetX, mTargetY];
        mStartPos = mPointGrid[Mathf.FloorToInt(transform.position.x), Mathf.FloorToInt(transform.position.y)];
        if (mTargetX == mStartPos.mPositionX && mTargetY == mStartPos.mPositionY)
            return;
        mPathPosList = AStarAlgorithm.GetInstance.FindPath(mStartPos, mEndPos);
        NextStep = new List<Vector2>();
        for (int i = 0; i < mPathPosList.Count; i++)
        {
            NextStep.Add(new Vector2(mPathPosList[i].mPositionX, mPathPosList[i].mPositionY));
        }
    }
}