using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ゲームの状態をコントロールする
/// </summary>
public class GameController : MonoBehaviour
{
    //ゲームの制限時間
    private float time = 0.0f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //演出中なら処理しない
        if (GamaeManager.Instance.IsDirecting) return;

        time += 1.0f * Time.deltaTime;
        //99分59秒で表示を止める
        if (time > 5999) time = 5999;
    }

    /// <summary>
    /// ゲームの進行時間を返す
    /// </summary>
    /// <returns>何秒か</returns>
    public float GetTime(){ return time; }
}
