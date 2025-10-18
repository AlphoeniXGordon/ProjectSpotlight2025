using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState_Chapter01 : GameStateBase
{
    public override GameStateType State => GameStateType.Chapter01;
    RectTransform NPCTalk;
    SuperTextMesh superNPCTalkText;
    GameObject blueNPC;
    /// <summary>
    /// ��ͷ���ʱ��
    /// </summary>
    float agreedDetectTime = 0.6f;
    float LastAgreedCameraPosZ;
    float AgreedCameraZValue = 0.02f;
    bool isDetectingAgreed;
    bool hasAgreedUp;
    float agreedUpTime;
    public override GameStateBase Init()
    {
        NPCTalk = Main.Instance.mainCanvas.Find("NPCTalk").GetComponent<RectTransform>();
        NPCTalk.gameObject.SetActive(false);
        superNPCTalkText = NPCTalk.Find("Text").GetComponent<SuperTextMesh>();
        blueNPC = Main.Instance.sceneObj.Find("npc").gameObject;
        return base.Init();
    }
    public override void OnGameStateEnter()
    {
        base.OnGameStateEnter();
        Main.Instance.IsFocusMouse = true;
        Main.Instance.dutyCtrl.AddDuty(DutyType.OpenValut, "�������н��");
        Main.Instance.ShowTalk("��ӭ�ص���֮�ǣ�������������Ǯ��");
        TargetCamera();
        Main.Instance.arrow.gameObject.SetActive(true);

        hasAgreedUp = false;
        isDetectingAgreed = false;
    }

    public override void OnGameStateUpdate()
    {
        base.OnGameStateUpdate();
        if (curTarget != null)
        {
            Main.Instance.arrow.anchoredPosition = curTarget.GetScreenPointOfBoundsTop(Main.Instance.mainCanvas.GetComponent<Canvas>(), Main.Instance.myCamera, 0.05f);
            NPCTalk.anchoredPosition = blueNPC.transform.GetScreenPointOfBoundsTop(Main.Instance.mainCanvas.GetComponent<Canvas>(), Main.Instance.myCamera, 0.15f);
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hit;
                if (Physics.Raycast(Main.Instance.myCamera.transform.position, Main.Instance.myCamera.transform.forward, out hit, 1000))
                {
                    if (hit.transform == curTarget)
                    {
                        if (curTarget.name == "camera")//�ݻ�����ͷ
                        {
                            curTarget.gameObject.SetActive(false);
                            TargetSecurity();
                        }
                        else if (curTarget.name == "security")//�ݻٱ���
                        {
                            curTarget.gameObject.SetActive(false);
                            TargetNPC();
                        }
                        else if (curTarget.name == "npc")//�ݻ�NPC
                        {
                            isDetectingAgreed = false;
                            Main.Instance.arrow.gameObject.SetActive(false);
                            curTarget.gameObject.SetActive(false);
                            //��Ϸ����
                            NPCTalk.gameObject.SetActive(false);
                            Main.Instance.HideTalk();
                        }
                    }
                }
            }
        }
        //��ͷ���
        if (isDetectingAgreed)
        {
            float curPosZ = Main.Instance.myCamera.transform.position.z;
            if (!hasAgreedUp)
            {
                if (curPosZ - LastAgreedCameraPosZ > AgreedCameraZValue)
                {
                    hasAgreedUp = true;
                    agreedUpTime = Time.time;
                }
            }
            else
            {
                if (curPosZ - LastAgreedCameraPosZ < -AgreedCameraZValue)
                {
                    if (Time.time - agreedUpTime <= agreedDetectTime)//��ͷ�ɹ�
                    {
                        OnAgreed();
                    }
                    hasAgreedUp = false;
                }
            }
            LastAgreedCameraPosZ = curPosZ;
        }
    }

    Transform curTarget;
    void TargetCamera()
    {
        curTarget = Main.Instance.sceneObj.Find("camera");
    }
    void TargetSecurity()
    {
        curTarget = Main.Instance.sceneObj.Find("security");
    }
    void TargetNPC()
    {
        curTarget = Main.Instance.sceneObj.Find("npc");
        NPCTalk.gameObject.SetActive(true);
        superNPCTalkText.Text = "<w>ס�֣�";

        //NPC�߳���̨����
        TimeManager.Instance.AddTimeOut(2.6f, () =>
        {
            superNPCTalkText.Text = "���ܹ����ո�һ�հ���ϵͳ�涨ȥ�����Ը�������?";
            superNPCTalkText.OnCompleteEvent += ShowHelpDuty;
        });
    }
    void ShowHelpDuty()
    {
        superNPCTalkText.OnCompleteEvent -= ShowHelpDuty;
        Main.Instance.dutyCtrl.AddDuty(DutyType.Agreed, "���Բ�Ҫ<c=rainbow><w>��ͷ</c></w>");
        isDetectingAgreed = true;
        
    }

    /// <summary>
    /// ��ͷͬ��
    /// </summary>
    void OnAgreed()
    {
        isDetectingAgreed = false;
        Main.Instance.arrow.gameObject.SetActive(false);
        curTarget = null;
        Main.Instance.dutyCtrl.FinishDuty(DutyType.Agreed);
        Main.Instance.dutyCtrl.RemoveDuty(DutyType.OpenValut);
        GameManager.Instance.ChangeGameState(GameStateType.Chapter02);
        Main.Instance.HideTalk();
    }
}