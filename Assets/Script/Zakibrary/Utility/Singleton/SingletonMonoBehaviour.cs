using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// MonoBehaviourを継承したシングルトン基底クラス
/// </summary>
/// <typeparam name="T"></typeparam>
public class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
{
    /// <summary>
    /// インスタンス
    /// </summary>
    static T _instance = null;

    /// <summary>
    /// インスタンスを取得
    /// </summary>
    public static T Instance
    {
        get
        {
            // まずは検索
            _instance = _instance ?? FindObjectOfType<T>();
            // 存在しなければ生成
            if (_instance == null)
            {
                var type = typeof(T);
                new GameObject(type.Name, type);

                _instance = FindObjectOfType<T>();
            }

            return _instance;
        }
    }

    /// <summary>
    /// DontDestroyOnLoadに設定したときの親
    /// </summary>
    static Transform _singletonMonoBehaviourParent = null;

    /// <summary>
    /// DontDestroyOnLoadに設定したときの親を取得
    /// </summary>
    static Transform SingletonMonoBehaviourParent
    {
        get
        {
            // 作成
            var parent = new GameObject("--- SingletonMonoBehaviours ---").transform;
            // シーンが切り替わっても破棄されないようにする
            DontDestroyOnLoad(parent);

            return parent;
        }
    }

    /// <summary>
    /// シーンが切り替わっても破棄されないようにする
    /// </summary>
    public static void DontDestroyOnLoad()
    {
        // DontDestroyOnLoadに設定した親の子にする
        _singletonMonoBehaviourParent = _singletonMonoBehaviourParent ?? SingletonMonoBehaviourParent;
        Instance.transform.SetParent(_singletonMonoBehaviourParent);
    }

    /// <summary>
    /// インスタンスを破棄する
    /// </summary>
    public static void Destroy()
    {
        Destroy(Instance.gameObject);
    }
}
