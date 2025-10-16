using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class FakeWindow : MonoBehaviour
{
    [DllImport("user32.dll")]
    public static extern bool ShowWindow(IntPtr hwnd, int nCmdShow);

    [DllImport("user32.dll")]
    private static extern IntPtr GetActiveWindow();

    private const int SW_SHOWMINIMIZED = 2;

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void MinimizedGame()
    {
#if !UNITY_Editor
        ShowWindow(GetActiveWindow(), SW_SHOWMINIMIZED);
#endif
    }
}
