using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DutyCtrl
{
    Transform dutyTran;
    GameObject dutyPrefab;
    List<DutyItem> dutyObjList = new List<DutyItem>();
    List<DutyInfo> dutyList = new List<DutyInfo>();
    public DutyCtrl(Transform dutyTran)
    {
        this.dutyTran = dutyTran;
        dutyPrefab = dutyTran.Find("DutyObj").gameObject;
        dutyPrefab.SetActive(false);
    }
    public void Show()
    {
        dutyTran.gameObject.SetActive(true);
    }

    public void Hide()
    {
        dutyTran.gameObject.SetActive(false);
    }
    public void AddDuty(DutyType duty, string name)
    {
        Show();
        DutyInfo info = new DutyInfo(duty, name);
        dutyList.Add(info);
        RefreshDuty();
    }
    public void FinishDuty(DutyType duty)
    {
        DutyItem d = dutyObjList.Find(o => o.myInfo.dutyType == duty);
        if (d != null)
        {
            d.Finish();
            TimeManager.Instance.AddTimeOut(3.6f, () =>
            {
                RemoveDuty(duty);
            });
        }
    }
    public void RemoveDuty(DutyType duty)
    {
        DutyInfo d = dutyList.Find(o=>o.dutyType == duty);
        if (d != null)
        {
            dutyList.Remove(d);
        }
        RefreshDuty() ;
    }
    private void RefreshDuty()
    {
        for (int i = 0; i < dutyList.Count; i++)
        {
            DutyItem obj;
            if (i < dutyObjList.Count)
            {
                obj = dutyObjList[i];
            }
            else
            {
                obj = new DutyItem(GameObject.Instantiate(dutyPrefab, dutyTran));
                dutyObjList.Add(obj);
            }
            obj.SetActive(true);
            obj.SetInfo(dutyList[i]);
        }
        for (int i = dutyList.Count; i < dutyObjList.Count; i++)
        {
            dutyObjList[i].SetActive(false);
        }
    }
}
public class DutyItem
{
    public GameObject obj;
    public SuperTextMesh myText;
    public DutyInfo myInfo;
    public DutyItem(GameObject obj)
    {
        this.obj = obj;
        myText = obj.transform.Find("Text").GetComponent<SuperTextMesh>();
    }
    public void SetInfo(DutyInfo info)
    {
        myInfo = info;
        myText.Text = info.dutyName;
        myText.color = Color.white;
    }
    public void Finish()
    {
        myText.color = Color.green;

    }
    public void SetActive(bool v)
    {
        obj.SetActive (v);
    }
}
public class DutyInfo
{
    public bool isFinisher;
    public string dutyName;
    public DutyType dutyType;
    public DutyInfo(DutyType duty, string name) {
        dutyType = duty;
        dutyName = name;
        isFinisher = false;
    }
}
public enum DutyType
{
    OpenValut,//开金库
    Agreed,//点头
}