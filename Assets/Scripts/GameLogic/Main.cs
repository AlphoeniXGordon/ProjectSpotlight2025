using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using Unity.VisualScripting;
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
    public DutyCtrl dutyCtrl;
    public SuperTextMesh talkText;
    public Camera myCamera;
    private GameObject focusPoint;
    private float focusMoveSpeed = 0.06f;
    private bool _isFocusMouse;
    private Vector3 lastMousePos = Vector3.zero;
    public RectTransform arrow;
    /// <summary>
    /// 当前是否以鼠标为焦点
    /// </summary>
    public bool IsFocusMouse
    {
        get {return _isFocusMouse;}
        set { 
            _isFocusMouse = value;
            focusPoint.gameObject.SetActive(value);
            Cursor.visible = !_isFocusMouse;
            lastMousePos = Input.mousePosition;
        }
    }
    private void Awake()
    {
        Application.targetFrameRate = 120;
        Instance = this;
        mainCanvas = GameObject.Find("Canvas").transform;
        sceneObj = GameObject.Find("Scene").transform;
        GameManager.Instance.Init();
        dutyCtrl = new DutyCtrl(mainCanvas.Find("Duty"));
        talkText = mainCanvas.Find("Talk/Text").GetComponent<SuperTextMesh>();
        myCamera = Camera.main;
        focusPoint = mainCanvas.Find("focus").gameObject;
        arrow = mainCanvas.Find("Arrow").GetComponent<RectTransform>();
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

    private void FixedUpdate()
    {
        if (IsFocusMouse)
        {
            Vector3 curMousePos = Input.mousePosition;
            Vector3 detal = curMousePos - lastMousePos;
            myCamera.transform.position += new Vector3(detal.x, 0, detal.y) * focusMoveSpeed;
            lastMousePos = curMousePos;
        }
    }
    public void ShakeMainCamera(float strength)
    {
        Camera.main.transform.DOShakePosition(0.36f, strength);

    }
    public void HideTalk()
    {
        talkText.transform.parent.gameObject.SetActive(false);
    }
    public void ShowTalk(string str)
    {
        talkText.transform.parent.gameObject.SetActive(true);
        talkText.Text = str;
    }

    public void LookAt(GameObject blueNPC)
    {
        Vector3 curCamPos = Camera.main.transform.position;
        float y = Camera.main.transform.position.y;
        Vector3 cameraDir = Camera.main.transform.forward;
        float z = y/cameraDir.y * cameraDir.z;
        Vector3 desPos = blueNPC.transform.position + new Vector3(0, y, z);
        Camera.main.transform.DOMove(desPos, 1.6f);
    }
}
