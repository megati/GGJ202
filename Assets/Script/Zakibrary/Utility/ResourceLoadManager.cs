using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Resourcesパッケージング
/// </summary>
public static class ResourceLoadManager
{
    /// <summary>
    /// 非同期でリソースを読み込む
    /// </summary>
    /// <typeparam name="T">型</typeparam>
    /// <param name="path">Resources/より下から(拡張子無し)</param>
    /// <param name="callback">読み込んだリソースを受け取るコールバック</param>
    public static void AsyncLoadResource<T>(string path, System.Action<T> callback) where T : UnityEngine.Object
    {
        GlobalCoroutine.Instance.StartCoroutine(AsyncLoadResourceCoroutine(path, callback));
    }

    /// <summary>
    /// 非同期でリソースを読み込むコルーチン
    /// </summary>
    /// <typeparam name="T">型</typeparam>
    /// <param name="path">Resources/より下から(拡張子無し)</param>
    /// <param name="callback">読み込んだリソースを受け取るコールバック</param>
    /// <returns></returns>
    static IEnumerator AsyncLoadResourceCoroutine<T>(string path, System.Action<T> callback) where T : UnityEngine.Object
    {
        var async = Resources.LoadAsync<T>(path);

        yield return new WaitWhile(() =>
        {
            return !async.isDone;
        });

        callback(async.asset as T);

        yield return async;
    }

    /// <summary>
    /// 使われていないリソースを破棄する
    /// </summary>
    public static void UnloadResources()
    {
        Resources.UnloadUnusedAssets();
    }
}
