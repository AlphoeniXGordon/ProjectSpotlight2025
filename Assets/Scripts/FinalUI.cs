using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // 引入 UI 命名空间

public class FinalUI : MonoBehaviour
{
    // 公开变量，在 Inspector 中拖入
    public GameObject imageBreak;
    public GameObject imageGlitch;
    public GameObject imageBlueScreen;
    public GameObject player;
    public Image RTImage;
    public Material breakMat;
    public Camera mainCamera; // 引用主摄像机
    public RectTransform canvasRect; // 引用 Canvas 的 RectTransform

    // 震动参数
    [Header("Camera Shake Settings")]
    public float shakeDuration = 0.5f; // 震动持续时间
    public float shakeMagnitude = 0.1f; // 震动幅度

    private RectTransform imageBreakRect; // imageBreak 的 RectTransform

    void Start()
    {
        // 确保获取了必要的组件
        if (imageBreak != null)
        {
            imageBreakRect = imageBreak.GetComponent<RectTransform>();
            if (imageBreakRect == null)
            {
                Debug.LogError("imageBreak 缺少 RectTransform 组件.");
            }
        }

        if (mainCamera == null)
        {
            mainCamera = Camera.main; // 尝试获取主摄像机
            if (mainCamera == null)
            {
                Debug.LogError("主摄像机未设置或场景中没有 Tag 为 MainCamera 的摄像机。");
            }
        }

        // 初始时隐藏 UI
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
            // 1. 设置 UI 位置
            Vector3 screenPos = mainCamera.WorldToScreenPoint(player.transform.position);


            imageBreak.SetActive(true);
            RTImage.material = breakMat;
            Vector2 offset;
            offset.x = -(screenPos.x / Screen.width - 0.5f) * 1.5f;
            offset.y = (screenPos.y / Screen.height - 0.5f) * 1.5f;
            RTImage.material.SetVector("_BreakOffset", new Vector4(offset.x,offset.y,0.0f,0.0f));

            // WorldToScreenPoint 返回的是屏幕像素坐标
            // 对于 Screen Space - Overlay Canvas，可以直接用这个坐标来设置 RectTransform 的 position
            // 但更标准的做法是将其转换为 RectTransform 的 anchoredPosition，
            // 这通常需要考虑 Canvas 的缩放模式和锚点设置。

            // 如果 Canvas 使用 Screen Space - Overlay 且锚点为中心 (0.5, 0.5):
            // 你需要将屏幕坐标 (0,0 在左下角) 转换为 Canvas 坐标 (0,0 在中心)
            if (canvasRect != null)
            {
                // 将屏幕点转换为 Canvas 上的局部点
                Vector2 localPoint;
                if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    canvasRect,
                    screenPos,
                    null, // Screen Space - Overlay Canvas 传递 null
                    out localPoint))
                {
                    imageBreakRect.anchoredPosition = localPoint;
                }
            }
            else
            {
                // 如果没有 CanvasRect 引用，退而求其次直接用 position (适用于 Screen Space - Camera 或简单情况)
                // 注意：这可能在不同分辨率下表现不一致，取决于 Canvas Scaler 设置
                imageBreak.transform.position = screenPos;
            }

            // 2. 启动摄像机震动
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

    // 摄像机震动方法 (需要 using System.Collections)
    // 通常将这个方法放在一个单独的 CameraController 脚本中会更合理，但为了完善你的代码，我把它放在这里。
    private System.Collections.IEnumerator ShakeCamera(float duration, float magnitude)
    {
        if (mainCamera == null) yield break;

        Vector3 originalPos = mainCamera.transform.localPosition;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            // 在 x, y 轴上生成随机震动偏移
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            mainCamera.transform.localPosition = originalPos + new Vector3(x, y, 0f);

            elapsed += Time.deltaTime;

            yield return null; // 等待下一帧
        }

        mainCamera.transform.localPosition = originalPos; // 震动结束后返回原位
    }
}