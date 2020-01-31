#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using System.IO;
using System.Text;

namespace ScriptCreators
{
    /// <summary>
    /// シーンの列挙型を作成する
    /// </summary>
    public class SceneEnumCreator : EditorWindow
    {
        string _scriptName = "";
        string _summary = "";
        string _nameSpace = "";
        string _directoryPath = "";

        string _log = "";

        /// <summary>
        /// [UnityCall]エディタを開く
        /// </summary>
        [MenuItem("Tool/Script Creator/SceneEnum")]
        static void OpenEditor()
        {
            GetWindow<SceneEnumCreator>("ScriptCreator -SceneEnum");
        }

        /// <summary>
        /// [UnityCall] エディタを開いたときに呼ばれる
        /// </summary>
        void OnEnable()
        {
            _scriptName = "SceneName";
            _summary = "Scenes";
            _nameSpace = "";
            _directoryPath = "Assets/Script/SystemEnums";

            WriteLogIfParamError();
        }

        /// <summary>
        /// [UnityMethod] エディタで表示する設定
        /// </summary>
        void OnGUI()
        {
            // 出力先のディレクトリ入力欄
            GUI.color = (string.IsNullOrEmpty(_directoryPath)) ? Color.red : Color.white;
            GUILayout.Label("Directory");
            _directoryPath = GUILayout.TextArea(_directoryPath);
            GUILayout.Space(10);

            // 新規作成するスクリプト及びクラス名の入力欄
            GUI.color = (string.IsNullOrEmpty(_scriptName)) ? Color.red : Color.white;
            GUILayout.Label("Script Name");
            _scriptName = GUILayout.TextField(_scriptName);
            GUILayout.Space(10);

            GUI.color = Color.white;
            GUILayout.Label("--- Opsitons ---");
            GUILayout.Space(10);

            // スクリプトの説明文入力欄
            GUILayout.Label("Summary");
            _summary = GUILayout.TextArea(_summary);
            GUILayout.Space(10);

            // スクリプトの名前空間
            GUILayout.Label("namespace");
            _nameSpace = GUILayout.TextArea(_nameSpace);
            GUILayout.Space(10);

            // 押すとスクリプトの生成を行うボタン
            if (GUILayout.Button("Create"))
            {
                WriteLogIfParamError();

                if (!string.IsNullOrEmpty(_log))
                {
                    return;
                }

                if (CreateSceneEnum())
                {
                    Close();
                }
            }

            // エラーログ
            GUILayout.Space(10);
            GUILayout.Label("--- Error Log ---");
            var style = new GUIStyle(EditorStyles.textField);
            style.normal.textColor = Color.red;
            GUILayout.TextArea(_log, style);
        }

        /// <summary>
        /// シーンの列挙型を生成する
        /// </summary>
        /// <returns></returns>
        bool CreateSceneEnum()
        {
            var filePath = $"{_directoryPath}/{_scriptName}.cs";

            var helper = new CreateScriptHelper(ScriptType.Enum, _nameSpace);
            var code = helper.TemplateCode();

            // 置換
            code = code.Replace("#SUMMARY#", _summary);
            code = code.Replace("#NAME#", _scriptName);
            code = code.Replace("#CONTENT#", ScenesToString(helper));

            try
            {
                // ディレクトリが無ければ生成
                if (!Directory.Exists(_directoryPath))
                {
                    Directory.CreateDirectory(_directoryPath);
                }

                File.WriteAllText(filePath, code, Encoding.UTF8);
                AssetDatabase.Refresh(ImportAssetOptions.ImportRecursive);
            }
            catch (System.Exception e)
            {
                Debug.Log($"SceneEnumCreator: FAIL. error -{e}");
                return false;
            }

            // 作成したスクリプトを表示する
            FolderController.OpenFolders(filePath);

            Debug.Log($"SceneEnumCreator: create -SceneEnum / name -{_scriptName}");

            return true;
        }

        /// <summary> ビルド設定されている全シーンを文字列で取得 </summary>
        string ScenesToString(CreateScriptHelper helper)
        {
            var content = "";
            // ビルドセッティングされている全シーンのパスの取得
            var scenePaths = EditorBuildSettingsScene.GetActiveSceneList(EditorBuildSettings.scenes);
            for (int i = 0; i < scenePaths.Length; i++)
            {
                var fileName = scenePaths[i].Split('/').ToList().Last();
                // 拡張子の削除
                var name = fileName.Replace(".unity", "").Replace(" ", "");
                content += helper.EnumContent(name, i);
            }

            return content;
        }

        /// <summary>
        /// パラメータにエラーがあるならログを吐く
        /// </summary>
        void WriteLogIfParamError()
        {
            _log = "";

            if (string.IsNullOrEmpty(_scriptName))
            {
                _log += "Null or empty [Script Name]\n";
            }
            if (string.IsNullOrEmpty(_directoryPath))
            {
                _log += "Null or empty [Directory Path]";
            }
        }
    }
}
#endif // UNITY_EDITOR
