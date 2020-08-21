using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class AStarAlgorithm
{
    private const int mGridWidth = 9;
    private const int mGridHeight = 9;
    //使用二维数组存储点网格
    public AStarPoint[,] mPointGrid = new AStarPoint[mGridWidth, mGridHeight];
    //存储路径方格子
    public List<AStarPoint> mPathPosList = new List<AStarPoint>();
    private static AStarAlgorithm _instance;
    public static AStarAlgorithm GetInstance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new AStarAlgorithm();
            }
            return _instance;
        }
    }
    public AStarAlgorithm()
    {
        InitPoint();
    }
    //在网格上设置点的信息  
    private void InitPoint()
    {
        for (int i = 0; i < mGridWidth; i++)
        {
            for (int j = 0; j < mGridHeight; j++)
            {
                mPointGrid[i, j] = new AStarPoint(i, j);
            }
        }
        //设置障碍物  
        mPointGrid[1, 1].mIsObstacle = true;
        mPointGrid[1, 2].mIsObstacle = true;
        mPointGrid[1, 5].mIsObstacle = true;
        mPointGrid[2, 5].mIsObstacle = true;
        mPointGrid[4, 3].mIsObstacle = true;
        mPointGrid[5, 3].mIsObstacle = true;
        mPointGrid[6, 3].mIsObstacle = true;
        mPointGrid[7, 3].mIsObstacle = true;
        mPointGrid[5, 5].mIsObstacle = true;
        mPointGrid[5, 6].mIsObstacle = true;
        mPointGrid[5, 7].mIsObstacle = true;
    }
    public void ClearGrid()
    {
        for (int x = 0; x < mGridWidth; x++)
        {
            for (int y = 0; y < mGridHeight; y++)
            {
                if (!mPointGrid[x, y].mIsObstacle)
                {
                        mPointGrid[x, y].mParentPoint = null;
                }
            }
        }
    }
    //寻路  
    public List<AStarPoint> FindPath(AStarPoint mStartPoint, AStarPoint mEndPoint)
    {
        if (mEndPoint.mIsObstacle || mStartPoint.mPosition == mEndPoint.mPosition)
        {
            //Debug.Log("failed");
            return null;
        }
        //开启列表  
        List<AStarPoint> openPointList = new List<AStarPoint>();
        //关闭列表  
        List<AStarPoint> closePointList = new List<AStarPoint>();
        openPointList.Add(mStartPoint);
        while (openPointList.Count > 0)
        {            
            //寻找开启列表中最小预算值的表格  
            AStarPoint minFPoint = FindPointWithMinF(openPointList);
            //将当前表格从开启列表移除 在关闭列表添加  
            //Debug.Log("minf"+minFPoint.mPositionX+" "+minFPoint.mPositionY);
            openPointList.Remove(minFPoint);
            closePointList.Add(minFPoint);
            //找到当前点周围的全部点  
            List<AStarPoint> surroundPoints = FindSurroundPoint(minFPoint);
            //在周围的点中，将关闭列表里的点移除掉  
            SurroundPointsFilter(surroundPoints, closePointList);
            //Debug.Log("opencount" + openPointList.Count);
            //寻路逻辑  
            foreach (var surroundPoint in surroundPoints)
            {
                bool ifContains = false;
                foreach (AStarPoint ap in openPointList)
                {
                    if (ap.mPositionX == surroundPoint.mPositionX && ap.mPositionY == surroundPoint.mPositionY)
                    {
                        ifContains = true;
                        break;
                    }
                    else ifContains = false;
                }
                if (ifContains)
                {
                    //计算下新路径下的G值（H值不变的，比较G相当于比较F值）  
                    float newPathG = CalcG(surroundPoint, minFPoint);
                    if (newPathG < surroundPoint.mG)
                    {                        
                        surroundPoint.mG = newPathG;
                        surroundPoint.mF = surroundPoint.mG + surroundPoint.mH;
                        surroundPoint.mParentPoint = minFPoint;
                        //Debug.Log("parent"+surroundPoint.mParentPoint);
                    }
                }
                else
                {
                    //将点之间的  
                    surroundPoint.mParentPoint = minFPoint;
                    //Debug.Log("parent" + surroundPoint.mParentPoint);
                    CalcF(surroundPoint, mEndPoint);
                    openPointList.Add(surroundPoint);
                    //Debug.Log(surroundPoint.mPositionX + " " + surroundPoint.mPositionY);
                }
            }
            bool canBreak = false;
            //如果开始列表中包含了终点，说明找到路径  
            foreach (AStarPoint ap in openPointList)
            {
                if (ap.mPositionX == mEndPoint.mPositionX && ap.mPositionY == mEndPoint.mPositionY)
                {
                    mEndPoint.mParentPoint = minFPoint;
                    //Debug.Log("break");
                    //Debug.Log(mEndPoint.mPositionX + " " + mEndPoint.mPositionY);
                    //Debug.Log(openPointList.Count);
                    canBreak = true;
                }
            }
            if (canBreak) { break; }
        }

        mPathPosList.Clear();
        AStarPoint temp = mEndPoint;
        while (true)
        {
            mPathPosList.Add(temp);
            //Debug.Log("added");
            if (temp.mParentPoint == null)
            {
                //Debug.Log("ParentNull");
                break;
            }
            temp = temp.mParentPoint;
        }
        //Debug.Log(mPathPosList.Count);
        return mPathPosList;
    }
    //寻找预计值最小的格子  
    private AStarPoint FindPointWithMinF(List<AStarPoint> openPointList)
    {
        float f = float.MaxValue;
        AStarPoint temp = null;
        foreach (AStarPoint p in openPointList)
        {
            if (p.mF < f)
            {
                temp = p;
                f = p.mF;
            }
        }
        return temp;
    }
    //寻找周围的全部点  
    private List<AStarPoint> FindSurroundPoint(AStarPoint point)
    {
        bool canUp = false, canDown = false, canLeft = false, canRight = false;
        bool canUL = false, canUR = false, canLL = false, canLR = false;
        List<AStarPoint> list = new List<AStarPoint>();
        ////////////判断周围的八个点是否在网格内/////////////  
        AStarPoint up = null, down = null, left = null, right = null;
        AStarPoint lu = null, ru = null, ld = null, rd = null;
        if (point.mPositionY < mGridHeight - 2)
        {
            up = mPointGrid[point.mPositionX, point.mPositionY + 1];
            canUp = true;
        }
        if (point.mPositionY > 1)
        {
            down = mPointGrid[point.mPositionX, point.mPositionY - 1];
            canDown = true;
        }
        if (point.mPositionX > 1)
        {
            left = mPointGrid[point.mPositionX - 1, point.mPositionY];
            canLeft = true;
        }
        if (point.mPositionX < mGridWidth - 2)
        {
            right = mPointGrid[point.mPositionX + 1, point.mPositionY];
            canRight = true;
        }
        if (canUp && canLeft)
        {
            lu = mPointGrid[point.mPositionX - 1, point.mPositionY + 1];
            canUL = true;
        }
        if (canUp && canRight)
        {
            ru = mPointGrid[point.mPositionX + 1, point.mPositionY + 1];
            canUR = true;
        }
        if (canDown && canLeft)
        {
            ld = mPointGrid[point.mPositionX - 1, point.mPositionY - 1];
            canLL = true;
        }
        if (canDown && canRight)
        {
            rd = mPointGrid[point.mPositionX + 1, point.mPositionY - 1];
            canLR = true;
        }
        /////////////将可以经过的表格添加到开启列表中/////////////  
        if (canDown && down.mIsObstacle == false)
        {
            list.Add(down);
        }
        if (canUp && up.mIsObstacle == false)
        {
            list.Add(up);
        }
        if (canLeft && left.mIsObstacle == false)
        {
            list.Add(left);
        }
        if (canRight && right.mIsObstacle == false)
        {
            list.Add(right);
        }
        if (canUL && lu.mIsObstacle == false && left.mIsObstacle == false && up.mIsObstacle == false)
        {
            list.Add(lu);
        }
        if (canLL && ld.mIsObstacle == false && left.mIsObstacle == false && down.mIsObstacle == false)
        {
            list.Add(ld);
        }
        if (canUR && ru.mIsObstacle == false && right.mIsObstacle == false && up.mIsObstacle == false)
        {
            list.Add(ru);
        }
        if (canLR && rd.mIsObstacle == false && right.mIsObstacle == false && down.mIsObstacle == false)
        {
            list.Add(rd);
        }
        return list;
    }
    //将关闭带你从周围点列表中关闭  
    private void SurroundPointsFilter(List<AStarPoint> surroundPoints, List<AStarPoint> closePoints)
    {
        foreach (AStarPoint closePoint in closePoints)
        {
            bool ifContainclose = false;
            foreach (var pa in surroundPoints)
            {
                if (pa.mPositionX == closePoint.mPositionX && pa.mPositionY == closePoint.mPositionY)
                {
                    ifContainclose = true;
                    break;
                }
                else ifContainclose = false;
            }
            if (ifContainclose)
            {
                    //Debug.Log("将关闭列表的点移除"+ closePoint.mPositionX + " " + closePoint.mPositionY);
                    surroundPoints.Remove(closePoint);
                    //Debug.Log("surrendcount" + surroundPoints.Count);
                    //Debug.Log(surroundPoints[0].mPositionX + " " + surroundPoints[0].mPositionY);
                    //Debug.Log(surroundPoints[1].mPositionX + " " + surroundPoints[1].mPositionY);
                    //Debug.Log(surroundPoints[2].mPositionX + " " + surroundPoints[2].mPositionY);
                    //Debug.Log(surroundPoints[3].mPositionX + " " + surroundPoints[3].mPositionY);
                    //Debug.Log(surroundPoints[4].mPositionX + " " + surroundPoints[4].mPositionY);
                    //Debug.Log(surroundPoints[5].mPositionX + " " + surroundPoints[5].mPositionY);
                    //Debug.Log(surroundPoints[6].mPositionX + " " + surroundPoints[6].mPositionY);
            }
        }
    }
    //计算最小预算值点G值  
    private float CalcG(AStarPoint surround, AStarPoint minFPoint)
    {
        return Vector3.Distance(surround.mPosition, minFPoint.mPosition) + minFPoint.mG;
    }
    //计算该点到终点的F值  
    private void CalcF(AStarPoint now, AStarPoint end)
    {
        //F = G + H  
        float h = Mathf.Abs(end.mPositionX - now.mPositionX) + Mathf.Abs(end.mPositionY - now.mPositionY);
        float g = 0;
        if (now.mParentPoint == null)
        {
            g = 0;
        }
        else
        {
            g = Vector2.Distance(new Vector2(now.mPositionX, now.mPositionY), new Vector2(now.mParentPoint.mPositionX, now.mParentPoint.mPositionY)) + now.mParentPoint.mG;
        }
        float f = g + h;
        now.mF = f;
        now.mG = g;
        now.mH = h;
    }
}