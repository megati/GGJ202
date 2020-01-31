#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Text;

namespace ScriptCreators
{
    /// <summary>
    /// 入力定数構造体を作成する
    /// </summary>
    public class InputAxesStructCreator : EditorWindow
    {
        string _scriptName = "";
        string _summary = "";
        string _nameSpace = "";
        string _directoryPath = "";

        string _log = "";

        /// <summary>
        /// [UnityCall] エディタを開く
        /// </summary>
        [MenuItem("Tool/Script Creator/InputAxes")]
        static void OpenEditor()
        {
            GetWindow<InputAxesStructCreator>("ScriptCreator -InputAxes");
        }

        /// <summary>
        /// [UnityCall] エディタを開いたときに呼ばれる
        /// </summary>
        void OnEnable()
        {
            _scriptName = "InputAxes";
            _summary = "InputAxces";
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

                if (CreateInputAxesStruct())
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
        /// 入力定数構造体を作成する
        /// </summary>
        /// <returns></returns>
        bool CreateInputAxesStruct()
        {
            var filePath = $"{_directoryPath}/{_scriptName}.cs";

            var helper = new CreateScriptHelper(ScriptType.Struct, _nameSpace);
            var code = helper.TemplateCode();

            // 置換
            code = code.Replace("#SUMMARY#", _summary);
            code = code.Replace("#NAME#", _scriptName);
            code = code.Replace("#CONTENT#", InputAxesToString(helper));

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
                Debug.Log($"InputAxesStruct: FAIL. error -{e}");
                return false;
            }

            // 作成したスクリプトを表示する
            FolderController.OpenFolders(filePath);

            Debug.Log($"InputAxesStruct: create -InputAxesStruct / name -{_scriptName}");

            return true;
        }

        /// <summary>
        /// 入力定数の文字列
        /// </summary>
        /// <param name="helper"></param>
        /// <returns></returns>
        string InputAxesToString(CreateScriptHelper helper)
        {
            var content = "";
            var inputManager = AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/InputManager.asset")[0];
            var serializedObject = new SerializedObject(inputManager);
            var axeses = serializedObject.FindProperty("m_Axes");

            if (axeses.arraySize == 0)
            {
                Debug.LogWarning("No Axes");
                return "";
            }

            serializedObject.ApplyModifiedProperties();

            for (int i = 0; i < axeses.arraySize; ++i)
            {
                var axis = axeses.GetArrayElementAtIndex(i);

                var name = GetChildProperty(axis, "m_Name").stringValue;

                if (!content.Contains(name))
                {
                    content += helper.PublicStaticStringGetter(name.Replace(" ", ""), name);
                }
            }

            serializedObject.ApplyModifiedProperties();

            return content;
        }

        /// <summary>
        /// 子要素のプロパティの取得（オーバーテクノロジー）
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        SerializedProperty GetChildProperty(SerializedProperty parent, string name)
        {
            var child = parent.Copy();
            child.Next(true);

            do
            {
                if (child.name == name) return child;
            }
            while (child.Next(false));

            return null;
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
