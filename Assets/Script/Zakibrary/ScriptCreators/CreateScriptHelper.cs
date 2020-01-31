#if UNITY_EDITOR
using UnityEngine;

namespace ScriptCreators
{
    /// <summary>
    /// スクリプトの生成を補助する   
    /// </summary>
    public class CreateScriptHelper
    {
        string _scriptType = "";
        string _nameSpace = "";

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="scriptType"></param>
        /// <param name="nameSpace"></param>
        public CreateScriptHelper(ScriptType scriptType, string nameSpace = "")
        {
            _scriptType = scriptType.ToString().ToLower();
            _nameSpace = nameSpace;
        }

        /// <summary>
        /// <para>テンプレートコード</para>
        /// namespaceは無くても良い
        /// </summary>
        /// <param name="codeType"></param>
        /// <param name="nameSpace"></param>
        /// <returns></returns>
        public string TemplateCode()
        {
            if (string.IsNullOrEmpty(_nameSpace))
            {
                return
                    "/// <summary>\n" +
                    "/// #SUMMARY#\n" +
                    "/// </summary>\n" +
                    $"public {_scriptType} #NAME#\n" +
                    "{" +
                    "#CONTENT#\n" +
                    "}\n";
            }
            else
            {
                return
                    $"namespace {_nameSpace}\n" +
                    "{\n" +
                    "    /// <summary>\n" +
                    "    /// #SUMMARY#\n" +
                    "    /// </summary>\n" +
                    $"    public {_scriptType} #NAME#\n" +
                    "    {#CONTENT#\n" +
                    "    }\n" +
                    "}\n";
            }
        }

        /// <summary>
        /// public static string プロパティ名 { get; private set; } = 値; の文字列を返す
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public string PublicStaticStringGetter(string name, string value = null)
        {
            return (string.IsNullOrEmpty(_nameSpace)) ?
                $"\n    public static string {name} => \"{value ?? name}\";" :
                $"\n        public static string {name} => \"{value ?? name}\";";
        }

        /// <summary>
        /// public static int プロパティ名 => 値; の文字列を返す
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public string PublicStaticIntGetter(string name, int value)
        {
            return (string.IsNullOrEmpty(_nameSpace)) ?
                $"\n    public static int {name} => {value};" :
                $"\n        public static int {name} => {value};";
        }

        /// <summary>
        /// 列挙型名 = 値, の値を返す
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public string EnumContent(string name, int? value = null)
        {
            var enumerator = (value == null) ? "" : $" = {value}";

            return (string.IsNullOrEmpty(_nameSpace)) ?
                $"\n    {name}{enumerator}," :
                $"\n        {name}{enumerator},";
        }
    }
}
#endif // UNITY_EDITOR
