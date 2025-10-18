using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using static UnityEngine.UI.Image;
/// <summary>
/// ȫ�ֹ�����
/// </summary>
public static class ToolUtil
{
    /// <summary>
    /// roll
    /// </summary>
    /// <param name="ratio"></param>
    /// <returns></returns>
    public static bool Roll(int ratio)
    {
        int ran = UnityEngine.Random.Range(0, 100);
        return ran <= ratio;
    }
    public static bool Roll(float ratio)
    {
        int ran = UnityEngine.Random.Range(0, 100);
        return ran <= ratio;
    }

    /// <summary>
    /// ��������ת��Ϊ��Ļ����
    /// </summary>
    /// <param name="worldPoint">��Ļ����</param>
    /// <returns></returns>
    public static Vector2 WorldPointToScreenPoint(Vector3 worldPoint)
    {
        // Camera.main ���������
        Vector2 screenPoint = Camera.main.WorldToScreenPoint(worldPoint);
        return screenPoint;
    }

    /// <summary>
    /// ��Ļ����ת��Ϊ��������
    /// </summary>
    /// <param name="screenPoint">��Ļ����</param>
    /// <param name="planeZ">��������� Z ƽ��ľ���</param>
    /// <returns></returns>
    public static Vector3 ScreenPointToWorldPoint(Vector2 screenPoint, float planeZ)
    {
        // Camera.main ���������
        Vector3 position = new Vector3(screenPoint.x, screenPoint.y, planeZ);
        Vector3 worldPoint = Camera.main.ScreenToWorldPoint(position);
        return worldPoint;
    }

    // RectTransformUtility.WorldToScreenPoint
    // RectTransformUtility.ScreenPointToWorldPointInRectangle
    // RectTransformUtility.ScreenPointToLocalPointInRectangle
    // ������������ת���ķ���ʹ�� Camera �ĵط�
    // �� Canvas renderMode Ϊ RenderMode.ScreenSpaceCamera��RenderMode.WorldSpace ʱ ���ݲ��� canvas.worldCamera
    // �� Canvas renderMode Ϊ RenderMode.ScreenSpaceOverlay ʱ ���ݲ��� null
    
    // UI ����ת��Ϊ��Ļ����
    public static Vector2 UIPointToScreenPoint(Vector3 worldPoint)
    {
        // RectTransform��target
        // worldPoint = target.position;
        Camera uiCamera = GetCurUICamera();

        Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(uiCamera, worldPoint);
        return screenPoint;
    }

    // ��Ļ����ת��Ϊ UGUI ����
    public static Vector3 ScreenPointToUIPoint(RectTransform rt, Vector2 screenPoint)
    {
        Vector3 globalMousePos;
        //UI��Ļ����ת��Ϊ��������
        Camera uiCamera = GetCurUICamera();

        // �� Canvas renderMode Ϊ RenderMode.ScreenSpaceCamera��RenderMode.WorldSpace ʱ uiCamera ����Ϊ��
        // �� Canvas renderMode Ϊ RenderMode.ScreenSpaceOverlay ʱ uiCamera ����Ϊ��
        RectTransformUtility.ScreenPointToWorldPointInRectangle(rt, screenPoint, uiCamera, out globalMousePos);
        // ת����� globalMousePos ʹ�����淽����ֵ
        // target Ϊ��Ҫʹ�õ� UI RectTransform
        // rt ������ target.GetComponent<RectTransform>(), Ҳ������ target.parent.GetComponent<RectTransform>()
        // target.transform.position = globalMousePos;
        return globalMousePos;
    }

    // ��Ļ����ת��Ϊ UGUI RectTransform �� anchoredPosition
    public static Vector2 ScreenPointToUILocalPoint(RectTransform parentRT, Vector2 screenPoint)
    {
        Vector2 localPos;
        Camera uiCamera = GetCurUICamera();

        RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRT, screenPoint, uiCamera, out localPos);
        // ת����� localPos ʹ�����淽����ֵ
        // target Ϊ��Ҫʹ�õ� UI RectTransform
        // parentRT �� target.parent.GetComponent<RectTransform>()
        // ���ֵ target.anchoredPosition = localPos;
        return localPos;
    }
    public static Vector2 WorldPointToUILocalPoint(Vector3 worldPoint, RectTransform parentRT)
    {
        Vector2 screenPoint = UIPointToScreenPoint(worldPoint);
        Vector2 localPos = ScreenPointToUILocalPoint(parentRT, screenPoint);
        return localPos;
    }
    public static Vector3 UIPointToWorldPoint(Vector3 worldPoint)
    {
        Vector2 screenPoint = UIPointToScreenPoint(worldPoint);
        Vector2 worldPos = ScreenPointToWorldPoint(screenPoint, 0);
        return worldPos;
    }
    static Camera GetCurUICamera()
    {
        Canvas canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        if (canvas.renderMode == RenderMode.ScreenSpaceCamera
            || canvas.renderMode == RenderMode.WorldSpace)
        {
            return canvas.worldCamera;
        }
        else if (canvas.renderMode == RenderMode.ScreenSpaceOverlay)
        {
            return null;
        }
        else
        {
            return null;
        }
    }
    public static string Sec2Min(int sec)
    {
        int shi;
        int fen;
        int miao;
        if (sec < 0)
            sec = 0;
        miao = sec % 60;
        sec = sec - miao;
        sec /= 60;
        fen = sec % 60;
        sec -= fen;
        shi = sec / 60;
        return string.Format("{0:00}:{1:00}:{2:00}", shi, fen, miao);
    }
    /// <summary>
    /// ���б��л�ȡ�����������
    /// </summary>
    /// <param name="count"></param>
    /// <returns></returns>
    public static List<T> GetRandomInList<T>(int count, List<T> list)
    {
        List<T> newList = new List<T>();
        if (count <= 0)
        {
            Debug.LogError("�����������");
            return newList;
        }
        if (count >= list.Count)
        {
            //Debug.LogError("����������ڵ��ڿ�����������");
            return list;
        }
        List<int> indexList = new List<int>();
        while (indexList.Count < count)
        {
            int index = UnityEngine.Random.Range(0, list.Count);
            if (!indexList.Contains(index))
            {
                indexList.Add(index);
            }
        }
        foreach (var index in indexList)
        {
            T item = list[index];
            newList.Add(item);
        }
        return newList;
    }
    /// <summary>
    /// ���б��л�ȡһ���������
    /// </summary>
    /// <param name="count"></param>
    /// <returns></returns>
    public static T GetRandomInList<T>(List<T> list)
    {
        if(list.Count > 0)
        {
            return list[UnityEngine.Random.Range(0, list.Count)];
        }
        else
        {
            return default;
        }
    }
    public static List<T> GetRandomInList<T>(int count, List<T>deslist, Func<T, bool> condition = null)
    {
        List<T> list = new List<T>();
        List<T> itemList = deslist;
        for (int i = 0; i < itemList.Count; i++)
        {
            T item = itemList[i];
            if (condition == null || condition(item))
            {
                list.Add(item);
            }
        }
        if (list.Count <= 0)
            return list;
        else
        {
            return GetRandomInList(count, list);
        }
    }
    public static T GetRandomInList<T>(List<T> deslist, Func<T, bool> condition = null)
    {
        List<T> list = new List<T>();
        List<T> itemList = deslist;
        for (int i = 0; i < itemList.Count; i++)
        {
            T item = itemList[i];
            if (condition == null || condition(item))
            {
                list.Add(item);
            }
        }
        return GetRandomInList(list);
    }
    public static float GetAnimLenByName(Animator anim, string name)
    {
        float length = 0;
        AnimationClip[] clips = anim.runtimeAnimatorController.animationClips;
        foreach (AnimationClip clip in clips)
        {
            if (clip.name.Equals(name))
            {
                length = clip.length;
                break;
            }
        }
        return length;
    }

    // �ݹ�������
    public static T GetComponentInChildernRecursive<T>(Transform parent) where T: Component
    {
        // ����ҵ���ƥ������壬ֱ�ӷ���
        T t = parent.GetComponent<T>();
        if (t != null)
            return t;

        // ���򣬱������������岢�ݹ����
        foreach (Transform child in parent)
        {
            T found = GetComponentInChildernRecursive<T>(child);
            if (found != null)
                return found;
        }

        // ���û���ҵ�������null
        return null;
    }
    // �ݹ��������GameObject�ϵ�ָ���������
    public static List<T> GetAllComponentsInChildren<T>(GameObject parent) where T : Component
    {
        List<T> components = new List<T>();
        Stack<GameObject> stack = new Stack<GameObject>();
        stack.Push(parent);

        while (stack.Count > 0)
        {
            GameObject go = stack.Pop();
            T component = go.GetComponent<T>();
            if (component != null)
            {
                components.Add(component);
            }

            foreach (Transform child in go.transform)
            {
                stack.Push(child.gameObject);
            }
        }

        return components;
    }
    public static bool JudgmentUiInScreen(GameObject obj)
    {
        RectTransform rect = obj.GetComponent<RectTransform>();
        RectTransform uiRoot = rect.root.GetComponent<RectTransform>();
        Vector2 pivotVec = rect.pivot;
        Vector3 pos = uiRoot.InverseTransformPoint(obj.transform.position); //ȡ�õ�ǰUI����Ļ���е�λ��
        float upY, downY, leftX, rightX;
        downY = pos.y - rect.sizeDelta.y * pivotVec.y;
        upY = pos.y + rect.sizeDelta.y * (1 - pivotVec.y);
        leftX = pos.x - rect.sizeDelta.x * pivotVec.x;
        rightX = pos.x + rect.sizeDelta.x * (1 - pivotVec.x);
        Vector3 moveDistance = new Vector3(0, 0, 0);
        int offset = 50;
        //�жϵ�ǰ��λ�� ����Ļ����Ĺ�ϵ
        bool isInScreen = true;

        float ScreenWidthHalf = uiRoot.sizeDelta.x / 2;
        float ScreenHeightHalf = uiRoot.sizeDelta.y / 2;
        if (rightX > ScreenWidthHalf)
        {
            isInScreen = false;
            moveDistance.x = ScreenWidthHalf - rightX - offset;
        }
        else if (leftX < -ScreenWidthHalf)
        {
            isInScreen = false;
            moveDistance.x = -ScreenWidthHalf - leftX + offset;
        }

        if (upY > ScreenHeightHalf)
        {
            isInScreen = false;
            moveDistance.y = ScreenHeightHalf - upY - offset;
        }
        else if (downY < -ScreenHeightHalf)
        {
            isInScreen = false;
            moveDistance.y = -ScreenHeightHalf - downY + offset;
        }
        return isInScreen;
    }
    /// <summary>
    /// �б�ϴ��
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    public static List<T> Shuffle<T>(List<T> original)
    {
        System.Random randomNum = new System.Random();
        int index = 0;
        T temp;
        for (int i = 0; i < original.Count; i++)
        {
            index = randomNum.Next(0, original.Count - 1);
            if (index != i)
            {
                temp = original[i];
                original[i] = original[index];
                original[index] = temp;
            }
        }
        return original;
    }
    /// <summary>
    /// ǿ��ˢ�²���
    /// </summary>
    /// <param name="tf"></param>
    public static void ForcedLayout(this Transform tf)
    {
        if (tf.TryGetComponent<VerticalLayoutGroup>(out var verticalLayoutGroup))
        {
            verticalLayoutGroup.SetLayoutVertical();
        }

        if (tf.TryGetComponent<HorizontalLayoutGroup>(out var horizontalLayoutGroup))
        {
            horizontalLayoutGroup.SetLayoutHorizontal();
        }

        if (tf.TryGetComponent<ContentSizeFitter>(out var contentSizeFitter))
        {
            contentSizeFitter.SetLayoutHorizontal();
            contentSizeFitter.SetLayoutVertical();
        }
        LayoutRebuilder.ForceRebuildLayoutImmediate(tf.GetComponent<RectTransform>());
    }
    /// <summary>
    /// �ж�2������֮���Ƿ����ڵ�
    /// </summary>
    /// <param name="viewer"></param>
    /// <param name="target"></param>
    /// <returns></returns>
    public static bool IsVisible(Transform viewer, Transform target)
    {
        Vector3 direction = target.position - viewer.position;
        float distance = direction.magnitude;
        direction.Normalize();

        RaycastHit hit;
        if (Physics.Raycast(viewer.position, direction, out hit, distance))
        {
            // ���hit.collider����null�����ʾ���ڵ�
            return hit.collider.gameObject != target.gameObject;
        }
        return true; // û���ڵ�
    }
    /// <summary>
    /// ��ȡ��������
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    public static Vector3 GetTargetCenter(Transform target)
    {
        if (target.GetComponent<SkinnedMeshRenderer>() != null) return target.GetComponent<SkinnedMeshRenderer>().bounds.center;
        if (target.GetComponent<Renderer>() != null) return target.GetComponent<Renderer>().bounds.center;
        if (target.GetComponent<Collider>() != null) return target.GetComponent<Collider>().bounds.center;
        return target.position;
    }
    public static bool IsInCameraView(Camera camera, Vector3 point)
    {
        Vector3 pointInCameraSpace = camera.WorldToViewportPoint(point);
        return pointInCameraSpace.z > 0 && pointInCameraSpace.x > 0 && pointInCameraSpace.x < 1 && pointInCameraSpace.y > 0 && pointInCameraSpace.y < 1;
    }
    /// <summary>
    /// Get point of Target relative to Screen
    /// </summary>
    /// <param name="canvas"></param>
    /// <param name="targetPoint"></param>
    /// <returns></returns>
    public static Vector2 GetScreenPointOffBoundsCenter(this Transform target, Canvas canvas, Camera cam, float _heightOffset)
    {
        var bounds = target.GetComponent<Collider>().bounds;
        var middle = bounds.center;
        var height = Vector3.Distance(bounds.min, bounds.max);

        var point = middle + new Vector3(0, height * _heightOffset, 0);
        var rectTransform = canvas.transform as RectTransform;
        Vector2 ViewportPosition = cam.WorldToViewportPoint(point);
        Vector2 WorldObject_ScreenPosition = new Vector2(
         ((ViewportPosition.x * rectTransform.sizeDelta.x) - (rectTransform.sizeDelta.x * 0.5f)),
         ((ViewportPosition.y * rectTransform.sizeDelta.y) - (rectTransform.sizeDelta.y * 0.5f)));
        return WorldObject_ScreenPosition;
    }
    public static Vector2 GetScreenPointOfBoundsTop(this Transform target, Canvas canvas, Camera cam, float _heightOffset)
    {
        var bounds = target.GetComponent<Collider>().bounds;
        var top = bounds.max;
        top.x = bounds.center.x;
        top.z = bounds.center.z;
        var height = Vector3.Distance(bounds.min, bounds.max);

        var point = top + new Vector3(0, height * _heightOffset, 0);
        var rectTransform = canvas.transform as RectTransform;
        Vector2 ViewportPosition = cam.WorldToViewportPoint(point);
        Vector2 WorldObject_ScreenPosition = new Vector2(
         ((ViewportPosition.x * rectTransform.sizeDelta.x) - (rectTransform.sizeDelta.x * 0.5f)),
         ((ViewportPosition.y * rectTransform.sizeDelta.y) - (rectTransform.sizeDelta.y * 0.5f)));
        return WorldObject_ScreenPosition;
    }

}
