using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState_Idle : GameStateBase
{
    public override GameStateType State => GameStateType.Idle;
    List<Transform> wanderPosList = new List<Transform>();
    GameObject startNotice;
    Sequence se;
    public override GameStateBase Init()
    {
        startNotice = Main.Instance.mainCanvas.Find("StartNotice").gameObject;
        Transform posTran = Main.Instance.sceneObj.Find("CameraWanderPos");
        for (int i = 0; i < posTran.childCount; i++)
        {
            wanderPosList.Add(posTran.GetChild(i));
        }
        return base.Init();
    }
    public override void OnGameStateUpdate()
    {
        base.OnGameStateUpdate();
        if (Input.anyKey)
        {
            GameManager.Instance.ChangeGameState(GameStateType.Chapter01);
        }
    }
    public override void OnGameStateEnter()
    {
        base.OnGameStateEnter();
        startNotice.SetActive(true);
        Camera.main.transform.position = wanderPosList[wanderPosList.Count - 1].position;
        se = DOTween.Sequence();
        for (int i = 0; i < wanderPosList.Count; i++)
        {
            se.Append(Camera.main.transform.DOMove(wanderPosList[i].position, 3.6f));
        }
        se.SetLoops(-1, LoopType.Restart);
        se.Play();

    }
    public override void OnGameStateExit()
    {
        base.OnGameStateExit();
        startNotice.SetActive(false);
        if(se != null)se.Kill();
    }
}
