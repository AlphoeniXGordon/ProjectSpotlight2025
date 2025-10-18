using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class GameManager
{
    private static GameManager instance;
    public static GameManager Instance
    {
        get { 
            if(instance == null)
                instance = new GameManager();
            return instance; 
        }
    }

    public GameStateBase curState;
    private Dictionary<GameStateType, GameStateBase> gameStateDic = new Dictionary<GameStateType, GameStateBase>();

    public void Init()
    {
        gameStateDic.Add(GameStateType.Idle, new GameState_Idle().Init());
        gameStateDic.Add(GameStateType.Chapter01, new GameState_Chapter01().Init());
        gameStateDic.Add(GameStateType.Chapter02, new GameState_Chapter02().Init());
        gameStateDic.Add(GameStateType.GameEnd, new GameState_GameEnd().Init());
    }
    public void ChangeGameState(GameStateType state)
    {
        if (curState != null) { curState.OnGameStateExit(); }
        if (gameStateDic.ContainsKey(state))
        {
            curState = gameStateDic[state];
            curState.OnGameStateEnter();
        }
        else
        {
            Debug.LogError("GameStateŒ¥≥ı ºªØ£∫ " + state);
        }

    }

}
