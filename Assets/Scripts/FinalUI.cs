using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // ���� UI �����ռ�

public class FinalUI : MonoBehaviour
{
    // ������������ Inspector ������
    public GameObject imageBreak;
    public GameObject imageGlitch;
    public GameObject imageBlueScreen;
    public GameObject player;
    public Image RTImage;
    public Material breakMat;
    public Camera mainCamera; // �����������
    public RectTransform canvasRect; // ���� Canvas �� RectTransform

    // �𶯲���
    [Header("Camera Shake Settings")]
    public float shakeDuration = 0.5f; // �𶯳���ʱ��
    public float shakeMagnitude = 0.1f; // �𶯷���

    private RectTransform imageBreakRect; // imageBreak �� RectTransform

    void Start()
    {
        // ȷ����ȡ�˱�Ҫ�����
        if (imageBreak != null)
        {
            imageBreakRect = imageBreak.GetComponent<RectTransform>();
            if (imageBreakRect == null)
            {
                Debug.LogError("imageBreak ȱ�� RectTransform ���.");
            }
        }

        if (mainCamera == null)
        {
            mainCamera = Camera.main; // ���Ի�ȡ�������
            if (mainCamera == null)
            {
                Debug.LogError("�������δ���û򳡾���û�� Tag Ϊ MainCamera ���������");
            }
        }

        // ��ʼʱ���� UI
        if (imageBreak != null)
        {
            imageBreak.SetActive(false);
        }

        Invoke("ShowBreak", 2.2f);
        Invoke("StartPunchScreen", 5.0f);
    }

    public void ShowBreak()
    {
        if (player != null && imageBreak != null && imageBreakRect != null && mainCamera != null)
        {
            // 1. ���� UI λ��
            Vector3 screenPos = mainCamera.WorldToScreenPoint(player.transform.position);


            imageBreak.SetActive(true);
            RTImage.material = breakMat;
            Vector2 offset;
            offset.x = -(screenPos.x / Screen.width - 0.5f) * 1.5f;
            offset.y = (screenPos.y / Screen.height - 0.5f) * 1.5f;
            RTImage.material.SetVector("_BreakOffset", new Vector4(offset.x,offset.y,0.0f,0.0f));

            // WorldToScreenPoint ���ص�����Ļ��������
            // ���� Screen Space - Overlay Canvas������ֱ����������������� RectTransform �� position
            // ������׼�������ǽ���ת��Ϊ RectTransform �� anchoredPosition��
            // ��ͨ����Ҫ���� Canvas ������ģʽ��ê�����á�

            // ��� Canvas ʹ�� Screen Space - Overlay ��ê��Ϊ���� (0.5, 0.5):
            // ����Ҫ����Ļ���� (0,0 �����½�) ת��Ϊ Canvas ���� (0,0 ������)
            if (canvasRect != null)
            {
                // ����Ļ��ת��Ϊ Canvas �ϵľֲ���
                Vector2 localPoint;
                if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    canvasRect,
                    screenPos,
                    null, // Screen Space - Overlay Canvas ���� null
                    out localPoint))
                {
                    imageBreakRect.anchoredPosition = localPoint;
                }
            }
            else
            {
                // ���û�� CanvasRect ���ã��˶������ֱ���� position (������ Screen Space - Camera ������)
                // ע�⣺������ڲ�ͬ�ֱ����±��ֲ�һ�£�ȡ���� Canvas Scaler ����
                imageBreak.transform.position = screenPos;
            }

            // 2. �����������
            StartCoroutine(ShakeCamera(shakeDuration, shakeMagnitude));

        }
    }

    public void ShowGlitch()
    {
        imageBreak.SetActive(false);
        imageGlitch.SetActive(true);
    }

    public void ShowBlueScreen()
    {
        imageGlitch.SetActive(false);
        imageBlueScreen.SetActive(true);
    }

    public IEnumerator PunchScreen()
    {
        ShowGlitch();
        yield return new WaitForSeconds(2.0f);
        ShowBlueScreen();
        yield return new WaitForSeconds(3.0f);
        Application.Quit();
    }

    public void StartPunchScreen()
    {
        StartCoroutine(PunchScreen());
    }

    // ������𶯷��� (��Ҫ using System.Collections)
    // ͨ���������������һ�������� CameraController �ű��л��������Ϊ��������Ĵ��룬�Ұ����������
    private System.Collections.IEnumerator ShakeCamera(float duration, float magnitude)
    {
        if (mainCamera == null) yield break;

        Vector3 originalPos = mainCamera.transform.localPosition;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            // �� x, y �������������ƫ��
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            mainCamera.transform.localPosition = originalPos + new Vector3(x, y, 0f);

            elapsed += Time.deltaTime;

            yield return null; // �ȴ���һ֡
        }

        mainCamera.transform.localPosition = originalPos; // �𶯽����󷵻�ԭλ
    }
}