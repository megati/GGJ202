using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// シーン遷移クラス
/// </summary>
public class SceneTransition : Singleton<SceneTransition>
{
    bool aaa = false;

    /// <summary>
    /// フェードとともにシーン遷移を行う
    /// </summary>
    /// <param name="sceneName">Scene name.</param>
    public void TransitionScene(SceneName sceneName)
    {
        DontDestroyOnLoadCanvas.Instance.GetFadeableImage.AlphaFadeOut(0.5f, () =>
        {
            SceneController.Instance.LoadSceneAsync(sceneName, null, () =>
            {
                DontDestroyOnLoadCanvas.Instance.GetFadeableImage.AlphaFadeIn(0.5f);
            });
        });
    }
}
