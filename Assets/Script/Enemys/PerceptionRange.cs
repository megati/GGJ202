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
            var direction = (other.transform.position - transform.position);

            Debug.DrawRay(transform.position, direction);

            if (Physics.Raycast(transform.position, direction, out RaycastHit raycastHit))
            {
                // Rayが当たっている間プレイヤーを捕捉
                if (raycastHit.transform.CompareTag("Player"))
                {
                    enemy.FoundPlayer();
                }
            }
        }
    }
}
