using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Main : MonoBehaviour
{
    public static Main Instance;
    [HideInInspector]
    public Transform mainCanvas;
    public Transform sceneObj;
    [HideInInspector]

    public Action OnUpdate;
    public bool IsDemo;
    public bool IsCaptureVedio;
    private void Awake()
    {
        Instance = this;
        mainCanvas = GameObject.Find("Canvas").transform;
        sceneObj = GameObject.Find("Scene").transform;
        GameManager.Instance.Init();
    }
    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.ChangeGameState(GameStateType.Idle);
    }
    private void OnDestroy()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (OnUpdate != null)
            OnUpdate();
#if UNITY_EDITOR
#endif
    }
    public void ShakeMainCamera(float strength)
    {
        Camera.main.transform.DOShakePosition(0.36f, strength);

    }
}
