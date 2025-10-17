using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum GameStateType
{
    Idle,
    Chapter01,
    GameEnd,
}
public class GameStateBase
{
    public virtual GameStateType State => GameStateType.Idle;
    public virtual GameStateBase Init()
    {
        return this;
    }
    public virtual void OnGameStateEnter()
    {
        Main.Instance.OnUpdate += OnGameStateUpdate;
    }
    public virtual void OnGameStateUpdate()
    {
    }
    public virtual void OnGameStateExit()
    {
        Main.Instance.OnUpdate -= OnGameStateUpdate;
    }
}
