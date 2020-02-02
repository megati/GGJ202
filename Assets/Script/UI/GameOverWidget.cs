using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverWidget : MonoBehaviour
{
    void Update()
    {

        if(Input.GetMouseButtonDown(0))
        {
            //SceneController.Instance.LoadSceneAsync(SceneName.MasterGame);


            SceneController.Instance.LoadSceneAsync(SceneName.MasterGame,
                () =>
                {
                    DontDestroyOnLoadCanvas.Instance.GetFadeableImage.AlphaFadeIn(0.3f);
                },
                () =>
                {
                    // 完了時に実行される処理
                    Debug.Log("完了");
                });
        }

        if (Input.GetMouseButtonDown(1))
        {

        }
    }
}
