using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.Networking;
using UnityEditor;

[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
public class OpenFileName
{
    public int structSize = 0;
    public IntPtr dlgOwner = IntPtr.Zero;
    public IntPtr instance = IntPtr.Zero;
    public String filter = null;
    public String customFilter = null;
    public int maxCustFilter = 0;
    public int filterIndex = 0;
    public String file = null;
    public int maxFile = 0;
    public String fileTitle = null;
    public int maxFileTitle = 0;
    public String initialDir = null;
    public String title = null;
    public int flags = 0;
    public short fileOffset = 0;
    public short fileExtension = 0;
    public String defExt = null;
    public IntPtr custData = IntPtr.Zero;
    public IntPtr hook = IntPtr.Zero;
    public String templateName = null;
    public IntPtr intPtr = IntPtr.Zero;
    public int reservedInt = 0;
    public int floagsEX = 0;
}

[Serializable]
public class Data
{
    public string index;
    public string name;
}
public class DLLTest
{
    [DllImport("Comdlg32.dll", SetLastError = true, ThrowOnUnmappableChar = true, CharSet = CharSet.Auto)]
    public static extern bool GetOpenFileName([In, Out] OpenFileName ofn);
    public static bool GetOpenFileName1([In, Out] OpenFileName ofn)
    {
        return GetOpenFileName(ofn);
    }
}

public class FileManagerUpdate : MonoBehaviour
{ 
    [DllImport("user32.dll")]
    private static extern bool ShowWindow(IntPtr hwnd, int nCmdShow);
    [DllImport("user32.dll")]
    private static extern IntPtr GetActiveWindow();

    [SerializeField]
    string path;

    public void OpenFileDialog()
    {
        Invoke("openfileialogreal", 0.2f);
    }
    private void openfileialogreal()
    {
        OpenFileName ofn = new OpenFileName();
        ofn.structSize = Marshal.SizeOf(ofn);
        ofn.filter = "All Files\0*.*\0\0";
        ofn.file = new string(new char[256]);
        ofn.maxFile = ofn.file.Length;
        ofn.fileTitle = new string(new char[64]);
        ofn.maxFileTitle = ofn.fileTitle.Length;
        ofn.initialDir = @"C:\";//UnityEngine.Application.dataPath;
        ofn.title = "Open Project";
        ofn.defExt = "mp4";
        ofn.flags = 0x00080000 | 0x00001000 | 0x00000800 | 0x00000200 | 0x00000008;

        if (DLLTest.GetOpenFileName(ofn))
        {

            path = ofn.file;
         //   print(path);
            StartCoroutine(TestUnityWebRequest());
        }
    }
    public void OpenFileBrowser()
    {
          openfileialogreal();
        //path = EditorUtility.OpenFilePanel("Daw", "", "mp3");
        //if (path != null)
        //{
            //StartCoroutine(TestUnityWebRequest());
        //}
    }
    IEnumerator TestUnityWebRequest()
    {
        using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip("file://" + path, AudioType.MPEG))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError)
            {
               
            }
            else
            {
                var myClip = DownloadHandlerAudioClip.GetContent(www);
                if (myClip != null)
                {
                    cNodeBuildManager.instance.SetSoundClip(myClip);
                    cNodeBuildManager.instance.SetSoundPath(path);
                    cNodeBuildManager.instance.ChangeScene();
                }
            }
        }
    }
}
