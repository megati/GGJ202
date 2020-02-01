using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 敵を管理する
/// </summary>
public class EnemyManager : SingletonMonoBehaviour<EnemyManager>
{
    GameObject player = null;

    /// <summary>
    /// プレイヤーのTransformを取得
    /// </summary>
    /// <returns>The player transform.</returns>
    public Transform GetPlayerTransform()
    {
        if(player = player?? GameObject.FindGameObjectWithTag("Player"))
        {
            return player.transform;
        }

        return null;
    }
}
