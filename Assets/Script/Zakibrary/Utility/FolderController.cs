using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// フォルダを制御する
/// </summary>
public static class FolderController
{
#if UNITY_EDITOR
    /// <summary>
    /// 指定した全てのパスのフォルダをProjectビュー上で選択・表示する
    /// </summary>
    /// <param name="paths">"Assets/.../*"</param>
    public static void OpenFolders(params string[] paths)
    {
        //メソッド検索オプションを設定
        var flag = BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;

        //UnityEditor.dllを取得
        var asm = Assembly.Load("UnityEditor");

        //ProjectBrowserクラスを取得
        var projectwindowtype = asm.GetType("UnityEditor.ProjectBrowser");

        //列挙体 ProjectBrowser.ViewMode を取得
        var viewmodetype = asm.GetType("UnityEditor.ProjectBrowser+ViewMode");

        //フォルダIDを取得するメソッドを取得
        var GetFolderInstanceIDs = projectwindowtype.GetMethod("GetFolderInstanceIDs", flag);

        //任意IDのフォルダを選択するメソッドを取得
        var SetFolderSelection = projectwindowtype.GetMethod("SetFolderSelection", flag);

        //ビューモードを設定するメソッドを取得
        var InitViewMode = projectwindowtype.GetMethod("InitViewMode", flag);

        //プロジェクトウィンドウを取得
        var projectwindow = EditorWindow.GetWindow(projectwindowtype, false, "Project", false);

        //プロジェクトウィンドウにフォーカス
        projectwindow.Focus();

        //プロジェクトウィンドウを２カラム表示に変更
        InitViewMode.Invoke(projectwindow, new[] { System.Enum.GetValues(viewmodetype).GetValue(1) });

        //渡されたパスのフォルダIDを取得
        int[] folderids = (int[])GetFolderInstanceIDs.Invoke(null, new[] { paths });

        try
        {
            //取得したIDのフォルダを選択（第二引数はとりあえずfalse）
            SetFolderSelection.Invoke(projectwindow, new object[] { folderids, false });
        }
        catch (System.Exception e)
        {
            Debug.Log(e);
        }
    }
#endif // UNITY_EDITOR
}
