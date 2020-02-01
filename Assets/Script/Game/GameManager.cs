using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ゲームを管理するシングルトン
/// </summary>
public class GameManager : SingletonMonoBehaviour<GameManager>
{
    //演出中か
    private bool isDirecting = false;

    public bool IsDirecting
    {
        set { isDirecting = value; }
        get { return isDirecting; }
    }
}
