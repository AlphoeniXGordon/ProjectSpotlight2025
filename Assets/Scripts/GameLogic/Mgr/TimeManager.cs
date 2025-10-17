using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 计时器
/// </summary>
public class TimeManager{
    private List<TimeoutDir> timeoutList = new List<TimeoutDir>();
    private TimeManager() {
        Main.Instance.OnUpdate += OnUpdate;
    }
    private static TimeManager _instance;

    public static TimeManager Instance
    {
        get{
            if (_instance == null)
            {
                _instance = new TimeManager();
            }
            return _instance;
        }
        
    }
    private void OnUpdate()
    {
        if (timeoutList.Count <= 0)
            return;
        for (int i = 0; i < timeoutList.Count; i++)
        {
            timeoutList[i].OnUpdate();
        }
    }
    /// <summary>
    /// 延时触发
    /// </summary>
    /// <param name="时间"></param>
    /// <param name="回调"></param>
    /// <returns></returns>
    public TimeoutDir AddTimeOut(float time, Action callback)
    {
        TimeoutDir dir = new TimeoutDir(time, callback);
        dir.AddOverCallback(() => { RemoveTimeout(dir); });
        timeoutList.Add(dir);
        return dir;
    }
    /// <summary>
    /// 延时、间隔触发
    /// </summary>
    /// <param name="间隔时间"></param>
    /// <param name="间隔回调"></param>
    /// <param name="结束时间"></param>
    /// <param name="结束回调"></param>
    /// <returns></returns>
    public TimeoutDir AddTimeOut(float interval, Action intervalCallback, float time, Action callback)
    {
        TimeoutDir dir = new TimeoutDir(interval, intervalCallback, time, callback);
        dir.AddOverCallback(() => { RemoveTimeout(dir); });
        timeoutList.Add(dir);
        return dir;
    }
    /// <summary>
    /// 持续间隔触发
    /// </summary>
    /// <param name="间隔时间"></param>
    /// <param name="间隔回调"></param>
    /// <param name="是否持续"></param>
    /// <returns></returns>
    public TimeoutDir AddTimeOut(float interval, Action intervalCallback, bool isEndLess)
    {
        TimeoutDir dir = new TimeoutDir(interval, intervalCallback, isEndLess);
        dir.AddOverCallback(() => { RemoveTimeout(dir); });
        timeoutList.Add(dir);
        return dir;
    }
    public void RemoveTimeout(TimeoutDir dir)
    {
        timeoutList.Remove(dir);
        dir = null;
    }

}

public class TimeoutDir
{
    float countTime;
    Action myCallback;
    /// <summary>
    /// 结束回调
    /// </summary>
    Action OverCallback;
    /// <summary>
    /// 间隔回调
    /// </summary>
    Action IntervalCallback;
    /// <summary>
    /// 结束时间
    /// </summary>
    float deltalTime;
    float myInterval;
    /// <summary>
    /// 触发间隔
    /// </summary>
    float intervalDetal;
    /// <summary>
    /// 是否为持续计时器
    /// </summary>
    bool _isEndLess = false;
    /// <summary>
    /// 延时触发
    /// </summary>
    /// <param name="时间"></param>
    /// <param name="回调"></param>
    public TimeoutDir(float time, Action callback)
    {
        countTime = time;
        myCallback = callback;
        deltalTime = time;
    }
    /// <summary>
    /// 间隔触发，延时回调
    /// </summary>
    /// <param name="间隔时间"></param>
    /// <param name="间隔回调"></param>
    /// <param name="结束时间"></param>
    /// <param name="结束回调"></param>
    public TimeoutDir(float interval, Action intervalCB, float time, Action callback)
    {
        countTime = time;
        myCallback = callback;
        deltalTime = time;
        myInterval = interval;
        IntervalCallback = intervalCB;
    }
    /// <summary>
    /// 持续间隔
    /// </summary>
    /// <param name="interval"></param>
    /// <param name="intervalCB"></param>
    /// <param name="isEndLess"></param>
    public TimeoutDir(float interval, Action intervalCB, bool isEndLess)
    {
        deltalTime = 10f;
        myInterval = interval;
        IntervalCallback = intervalCB;
        _isEndLess = isEndLess;
    }
    public void AddOverCallback(Action callback)
    {
        OverCallback = callback;
    }
    public void OnUpdate()
    {
        if (deltalTime > 0)
        {
            if(!_isEndLess)
                deltalTime -= Time.deltaTime;

            if (IntervalCallback!=null)
            {
                if(intervalDetal < myInterval)
                {
                    intervalDetal += Time.deltaTime;
                }
                else
                {
                    intervalDetal = 0;
                    IntervalCallback();
                }
            }
        }
        else
        {
            if(myCallback != null)
                myCallback();
            OverCallback();
        }
    }
}
