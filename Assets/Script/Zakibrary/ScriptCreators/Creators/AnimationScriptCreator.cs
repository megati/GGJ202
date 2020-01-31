#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;

namespace ScriptCreators
{
    /// <summary>
    /// 指定した AnimationController からステート・パラメータのスクリプトを生成するエディタ
    /// </summary>
    public class AnimationScriptCreator : EditorWindow
    {
        //static AnimatorController _animatorController = null;
        static Animator _animator = null;

        static string _directoryPath = "Assets";

        static string _stateEnumName = "";
        static string _stateSummary = "";
        static string _stateNameSpace = "";

        static string _paramScriptName = "";
        static string _paramSummary = "";
        static string _paramNameSpace = "";

        static string _log = "";

        Object _directory = null;

        [MenuItem("Tool/ScriptCreator/AnimationScript")]
        public static void OpenEditor()
        {
            GetWindow<AnimationScriptCreator>("ScriptCreator -Animationcontroller");
        }

        void OnEnable()
        {

        }

        void OnGUI()
        {
            GUILayout.Label("Animator");
            _animator = EditorGUILayout.ObjectField(_animator, typeof(Animator), true) as Animator;
            GUILayout.Space(10);

            // 出力先のディレクトリ入力欄を表示
            DrawDirectory();
            // アニメーションステートの列挙型情報入力欄を表示
            DrawStateEnumInfo();
            // アニメーションパラメータの構造体情報の入力欄を表示
            DrawParamStructInfo();
            // 自動設定ボタンを表示
            DrawAutoSettingButton();

            // スクリプト生成ボタンを表示
            DrawScriptCreateButton();

            // エラーログを表示
            DrawErrorLog();
        }

        /// <summary>
        /// コードを生成するディレクトリの入力欄を表示する
        /// </summary>
        void DrawDirectory()
        {
            GUI.color = (string.IsNullOrEmpty(_directoryPath)) ? Color.red : Color.white;
            GUILayout.Label("Directory");
            _directoryPath = GUILayout.TextArea(_directoryPath);

            GUI.color = Color.white;
            _directory = EditorGUILayout.ObjectField(_directory, typeof(Object), false);
            if (_directory != null)
            {
                _directoryPath = AssetDatabase.GetAssetPath(_directory);

                //選択されているファイルに拡張子がある場合(ディレクトリでない場合)は一つ上のディレクトリ内に作成する
                if (!string.IsNullOrEmpty(new System.IO.FileInfo(_directoryPath).Extension))
                {
                    var path = _directoryPath.Split('/');
                    _directoryPath = "";
                    for (int i = 0; i < path.Length - 1; i++)
                    {
                        _directoryPath += path[i] + '/';
                    }
                    _directoryPath = _directoryPath.Remove(_directoryPath.Length - 1);
                    //_directoryPath = System.IO.Directory.GetParent(_directoryPath).FullName;
                }

            }
            GUILayout.Space(10);
        }

        /// <summary>
        /// アニメーションステート列挙型情報の入力欄を表示する
        /// </summary>
        void DrawStateEnumInfo()
        {
            GUILayout.Label("--- StateEnum ---------------");
            //GUILayout.Space(5);
            //GUILayout.Label("AnimatorController");
            //_animatorController = EditorGUILayout.ObjectField(_animatorController, typeof(AnimatorController), true) as AnimatorController;
            GUILayout.Space(5);
            GUI.color = (string.IsNullOrEmpty(_stateEnumName)) ? Color.red : Color.white;
            GUILayout.Label("Script Name");
            _stateEnumName = GUILayout.TextField(_stateEnumName);

            GUI.color = Color.white;
            // スクリプトの説明文入力欄
            GUILayout.Label("Summary");
            _stateSummary = GUILayout.TextArea(_stateSummary);

            // スクリプトの名前空間
            GUILayout.Label("namespace");
            _stateNameSpace = GUILayout.TextArea(_stateNameSpace);
            GUILayout.Space(10);
        }

        /// <summary>
        /// アニメーションパラメータ構造体情報の入力欄を表示する
        /// </summary>
        void DrawParamStructInfo()
        {
            GUILayout.Label("--- ParamStruct ---------------");
            //GUILayout.Space(5);
            //GUILayout.Label("Animator");
            //_animator = EditorGUILayout.ObjectField(_animator, typeof(Animator), true) as Animator;
            GUILayout.Space(5);
            GUI.color = (string.IsNullOrEmpty(_paramScriptName)) ? Color.red : Color.white;
            GUILayout.Label("Script Name");
            _paramScriptName = GUILayout.TextField(_paramScriptName);

            GUI.color = Color.white;
            // スクリプトの説明文入力欄
            GUILayout.Label("Summary");
            _paramSummary = GUILayout.TextArea(_paramSummary);

            // スクリプトの名前空間
            GUILayout.Label("namespace");
            _paramNameSpace = GUILayout.TextArea(_paramNameSpace);
            GUILayout.Space(10);
        }

        /// <summary>
        /// 自動設定ボタンを表示する
        /// </summary>
        void DrawAutoSettingButton()
        {
            if (GUILayout.Button("AutoSetting"))
            {
                //var name = _animatorController.name.Replace(" ", "");
                //_stateEnumName = $"{name}AnimeState";
                //_stateSummary = $"{name}`s Animation States.";
                //_paramScriptName = $"{name}AnimeParam";
                //_paramSummary = $"{name}`s Animation Parameters.";

                var name = _animator.name.Replace(" ", "");
                _stateEnumName = $"{name}AnimeState";
                _stateSummary = $"{name}`s Animation States.";
                _paramScriptName = $"{name}AnimeParam";
                _paramSummary = $"{name}`s Animation Parameters.";
            }
        }

        /// <summary>
        /// スクリプト生成ボタンを表示する
        /// </summary>
        void DrawScriptCreateButton()
        {
            if (GUILayout.Button("Create"))
            {
                WriteLogIfParamError();

                if (!string.IsNullOrEmpty(_log))
                {
                    return;
                }

                if (CreateAnimeScripts())
                {
                    // Projectビューで作成したファイルを表示する
                    FolderController.OpenFolders(_directoryPath);
                    Close();
                }
            }
        }

        /// <summary>
        /// エラーログを表示する
        /// </summary>
        void DrawErrorLog()
        {
            GUILayout.Space(10);
            GUILayout.Label("--- Error Log ---");
            var style = new GUIStyle(EditorStyles.textField);
            style.normal.textColor = Color.red;
            GUILayout.TextArea(_log, style);
        }

        /// <summary>
        /// エラーのログを吐く
        /// </summary>
        void WriteLogIfParamError()
        {
            _log = "";

            if (string.IsNullOrEmpty(_stateEnumName))
            {
                _log += "Null or empty [StateEnum Name]\n";
            }
            if (string.IsNullOrEmpty(_paramScriptName))
            {
                _log += "Null or empty [ParamScript Name]\n";
            }
            if (string.IsNullOrEmpty(_stateEnumName))
            {
                _log += "Null or empty [StateEnum Name]\n";
            }
            if (string.IsNullOrEmpty(_directoryPath))
            {
                _log += "Null or empty [Directory Path]";
            }
        }

        /// <summary>
        /// Animatorから情報を取得しスクリプトを作成する
        /// </summary>
        bool CreateAnimeScripts()
        {
            try
            {
                // ステートの列挙型を作成
                CreateState();
            }
            catch (System.Exception e)
            {
                Debug.Log(e);
                return false;
            }

            try
            {
                // パラメータの構造体を作成
                CreateParam();
            }
            catch (System.Exception e)
            {
                Debug.Log(e);
                return false;
            }

            return true;
        }

        /// <summary>
        /// アニメーションステートの構造体を生成する
        /// </summary>
        void CreateState()
        {
            var helper = new CreateScriptHelper(ScriptType.Enum, _stateNameSpace);
            var animeStateToString = AnimeStateToString(helper);
            var statePath = _directoryPath + $"/{_stateEnumName}.cs";
            if (!string.IsNullOrEmpty(animeStateToString))
            {
                // 置換
                var templateCode = helper.TemplateCode();
                templateCode = templateCode.Replace("#NAME#", _stateEnumName);
                templateCode = templateCode.Replace("#SUMMARY#", _stateSummary);
                templateCode = templateCode.Replace("#CONTENT#", animeStateToString);

                // スクリプトを書き出す
                File.WriteAllText(statePath, templateCode, Encoding.UTF8);
                AssetDatabase.Refresh(ImportAssetOptions.ImportRecursive);

                Debug.Log($"AnimeScriptCreator: CreateState: SUCCESS. Created {_stateEnumName}.");
            }
            else
            {
                var error = $"AnimeScriptCreator: CreateState: FAIL. Not AnimeState {_stateEnumName}.";
                // ステートの列挙型が作られていたら削除する
                if (File.Exists(statePath))
                {
                    File.Delete(statePath);
                    error += $" Auto delete {_stateEnumName}.";
                }
                Debug.Log(error);
            }
        }

        /// <summary>
        /// アニメーションパラメータの構造体を作成する
        /// </summary>
        void CreateParam()
        {
            var helper = new CreateScriptHelper(ScriptType.Struct, _paramNameSpace);
            var animeParamToString = AnimeParamToString(helper);
            var paramPath = _directoryPath + $"/{_paramScriptName}.cs";
            if (!string.IsNullOrEmpty(animeParamToString))
            {
                // 置換
                var templateCode = helper.TemplateCode();
                templateCode = templateCode.Replace("#NAME#", _paramScriptName);
                templateCode = templateCode.Replace("#SUMMARY#", _paramSummary);
                templateCode = templateCode.Replace("#CONTENT#", animeParamToString);

                // スクリプトを書き出す
                File.WriteAllText(paramPath, templateCode, Encoding.UTF8);
                AssetDatabase.Refresh(ImportAssetOptions.ImportRecursive);

                Debug.Log($"AnimeScriptCreator: CreateParam: SUCCESS. Created {_paramScriptName}.");
            }
            else
            {
                var error = $"AnimeScriptCreator: CreateParam: FAIL. Not AnimeParam {_paramScriptName}.";
                // パラメータの構造体が作られていたら削除する
                if (File.Exists(paramPath))
                {
                    File.Delete(paramPath);
                    error += $" Auto delete {_paramScriptName}.";
                }
                Debug.Log(error);
            }
        }

        /// <summary>
        /// アニメーションステートを文字列で取得
        /// </summary>
        string AnimeStateToString(CreateScriptHelper helper)
        {
            string animeStateToString = "";
            //var layers = _animatorController.layers;

            var controller = _animator.runtimeAnimatorController as AnimatorController;
            var layers = controller.layers;

            for (int i = 0; i < layers.Length; i++)
            {
                foreach (var animatorState in layers[i].stateMachine.states)
                {
                    var state = animatorState.state;
                    var hash = Animator.StringToHash($"{layers[i].name}.{state.name}");
                    var name = $"{state.name}__L{i}".Replace(" ", "");
                    animeStateToString += helper.EnumContent(name, hash);
                }
            }
            return (animeStateToString);
        }

        /// <summary>
        /// アニメーションパラメータを文字列で取得
        /// </summary>
        string AnimeParamToString(CreateScriptHelper helper)
        {
            string animeParamToString = "";
            var parameters = _animator.parameters;

            for (int i = 0; i < parameters.Length; i++)
            {
                // 書き込み
                animeParamToString += helper.PublicStaticStringGetter(parameters[i].name.Replace(" ", ""));
            }

            return animeParamToString;
        }
    }
}
#endif
