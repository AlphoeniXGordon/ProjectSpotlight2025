using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameState_Chapter02 : GameStateBase
{
    public override GameStateType State => GameStateType.Chapter02;
    RectTransform NPCTalk;
    SuperTextMesh superNPCTalkText;
    GameObject blueNPC;
    Button settingBtn;
    GameObject settingPanel;
    Button settingCloseBtn;
    public override GameStateBase Init()
    {
        NPCTalk = Main.Instance.mainCanvas.Find("NPCTalk").GetComponent<RectTransform>();
        superNPCTalkText = NPCTalk.Find("Text").GetComponent<SuperTextMesh>();
        blueNPC = Main.Instance.sceneObj.Find("npc").gameObject;

        settingBtn = Main.Instance.mainCanvas.Find("SettingBtn").GetComponent<Button>();
        settingBtn.transform.gameObject.SetActive(false);
        settingPanel = Main.Instance.mainCanvas.Find("SettingPanel").gameObject;
        settingPanel.gameObject.SetActive(false);
        settingCloseBtn = settingPanel.transform.Find("CloseBtn").GetComponent<Button>();
        return base.Init();
    }
    public override void OnGameStateUpdate()
    {
        base.OnGameStateUpdate();
        NPCTalk.anchoredPosition = blueNPC.transform.GetScreenPointOfBoundsTop(Main.Instance.mainCanvas.GetComponent<Canvas>(), Main.Instance.myCamera, 0.15f);

    }
    public override void OnGameStateEnter()
    {
        base.OnGameStateEnter();
        superNPCTalkText.Text = "系统一直在告诉我该干什么，如果你能让他<c=rainbow><w>闭嘴</c></w>，我会很感激";
        Main.Instance.IsFocusMouse= false;
        Main.Instance.LookAt(blueNPC);
    }
    public override void OnGameStateExit()
    {
        base.OnGameStateExit();
    }

}
