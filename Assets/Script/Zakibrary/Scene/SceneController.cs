using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// シーンを制御する
/// </summary>
public class SceneController : SingletonMonoBehaviour<SceneController>
{
    /// <summary>
    /// 読み込んだシーンの有効化を待機する
    /// </summary>
    /// <param name="allowSceneActivation">true にすると読み込んだシーンが有効になる（切り替わる）</param>
    public delegate void WaitSceneActivation(ref bool allowSceneActivation);

    /// <summary>
    /// 非同期でシーンを読み込む
    /// </summary>
    /// <param name="scene">読み込むシーン</param>
    /// <param name="onStart">開始時に呼ばれる処理</param>
    /// <param name="onComplete">完了時に呼ばれる処理</param>
    /// <param name="progressCallback">進捗度のコールバック</param>
    /// <param name="setSceneActivationTrigger">シーンを切り替えるトリガー</param>
    public void LoadSceneAsync(SceneName scene,
        System.Action onStart = null, System.Action onComplete = null,
        System.Action<float> progressCallback = null, WaitSceneActivation waitSceneActivation = null)
    {
        StartCoroutine(LoadSceneAsyncCoroutine(SceneManager.LoadSceneAsync((int)scene), onStart, onComplete, progressCallback, waitSceneActivation));

        #region サンプル

        //SceneController.Instance.LoadSceneAsync(SceneName.NewScene,
        //    () =>
        //    {
        //        // 開始時に実行される処理
        //        Debug.Log("開始");
        //    },
        //    () =>
        //    {
        //        // 完了時に実行される処理
        //        Debug.Log("完了");
        //    },
        //    (float progress) =>
        //    {
        //        // プログレスを取得する
        //        Debug.Log("プログレス：" + progress);
        //    },
        //    (ref bool trigger) =>
        //    {
        //        // 読み込み完了後、シーンを切り替えるフラグを設定する
        //        if (Input.GetKeyDown(KeyCode.Return))
        //        {
        //            trigger = true;
        //            Debug.Log("設定");
        //        }
        //    });

        #endregion
    }

    /// <summary>
    /// 非同期でシーンを破棄する
    /// </summary>
    /// <param name="scene">破棄するシーン</param>
    /// <param name="onStart">開始時に呼ばれる処理</param>
    /// <param name="onComplete">完了時に呼ばれる処理</param>
    /// <param name="progressCallback">進捗度のコールバック</param>
    /// <param name="setSceneActivationTrigger">シーンを切り替えるトリガー</param>
    public void UnloadSceneAsync(SceneName scene,
        System.Action onStart = null, System.Action onComplete = null,
        System.Action<float> progressCallback = null, WaitSceneActivation waitSceneActivation = null)
    {
        StartCoroutine(LoadSceneAsyncCoroutine(SceneManager.UnloadSceneAsync((int)scene), onStart, onComplete, progressCallback, waitSceneActivation));

        #region サンプル

        //SceneController.Instance.UnloadSceneAsync(SceneName.NewScene,
        //    () =>
        //    {
        //        // 開始時に実行される処理
        //        Debug.Log("開始");
        //    },
        //    () =>
        //    {
        //        // 完了時に実行される処理
        //        Debug.Log("完了");
        //    },
        //    (float progress) =>
        //    {
        //        // プログレスを取得する
        //        Debug.Log("プログレス：" + progress);
        //    },
        //    (ref bool trigger) =>
        //    {
        //        // 読み込み完了後、シーンを切り替えるフラグを設定する
        //        if (Input.GetKeyDown(KeyCode.Return))
        //        {
        //            trigger = true;
        //            Debug.Log("設定");
        //        }
        //    });

        #endregion
    }

    /// <summary>
    /// 非同期でシーンを扱うコルーチン
    /// </summary>
    /// <param name="async">非同期でシーンを扱う処理</param>
    /// <param name="onStart">開始時に呼ばれる処理</param>
    /// <param name="onComplate">完了時に呼ばれる処理</param>
    /// <param name="progressCallback">進捗度のコールバック</param>
    /// <param name="setSceneActivationTrigger">シーンを切り替えるトリガー</param>
    /// <returns></returns>
    IEnumerator LoadSceneAsyncCoroutine(AsyncOperation async,
        System.Action onStart = null, System.Action onComplate = null,
        System.Action<float> progressCallback = null, WaitSceneActivation waitSceneActivation = null)
    {
        // １フレーム待機
        yield return null;

        // 読み込み開始
        async.allowSceneActivation = false;

        // 成功時処理登録
        async.completed += (asyncOperation) => onComplate?.Invoke();

        // 開始時の処理を実行
        onStart?.Invoke();

        // プログレスが 0.9 未満の間待機（0.9 で止まるらしい）
        yield return new WaitWhile(() =>
        {
            // 進捗を伝える
            progressCallback?.Invoke(async.progress);
            return async.progress < 0.9f;
        });

        /*** 読み込み完了 ***/

        // 進捗完了を伝える
        progressCallback?.Invoke(1.0f);

        // トリガーを設定していなければそのまま完了する
        if (waitSceneActivation == null)
        {
            async.allowSceneActivation = true;
        }
        else
        {
            var allowSceneActivation = false;

            // allowSceneActivation が true になるまで待機
            yield return new WaitWhile(() =>
            {
                // トリガーに渡した引数が true になると完了する
                waitSceneActivation(ref allowSceneActivation);
                async.allowSceneActivation = allowSceneActivation;

                return !(async.allowSceneActivation);
            });
        }
    }
}
