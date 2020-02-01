using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 知覚範囲
/// </summary>
public class PerceptionRange : MonoBehaviour
{
    [SerializeField]
    Enemy enemy = null;

    void OnTriggerStay(Collider other)
    {
        // コライダーでプレイヤーを検知
        if(other.CompareTag("Player"))
        {
            Debug.DrawRay(transform.position, (other.transform.position - transform.position));

            //if (Physics.Raycast(transform.position, (other.transform.position - transform.position), out RaycastHit raycastHit))
            //{
            //    // Rayが当たっている間プレイヤーを捕捉
            //    if (raycastHit.transform.CompareTag("Player"))
            //    {
            //        enemy.FoundPlayer(other.transform.position);
            //    }
            //    else
            //    {
            //        enemy.LostPlayer(other.transform);
            //    }
            //}
            //else
            //{
            //    enemy.LostPlayer(other.transform);
            //}
        }
        //else
        //{
        //    //enemy.LostPlayer(other.transform);
        //}
    }
}
