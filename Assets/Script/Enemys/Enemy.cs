using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// 敵
/// </summary>
[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : MonoBehaviour
{
    /// <summary>
    /// 敵の状態
    /// </summary>
    EnemyState enemyState = EnemyState.Patrol;

    /// <summary>
    /// 一つ前のポイント
    /// </summary>
    RoutePoint beforRoutePoint = null;

    /// <summary>
    /// 次のポイント
    /// </summary>
    RoutePoint nextRoutePoint = null;

    Transform Player = null;

    float LostedChaseTime = 0.0f;

    NavMeshAgent navMeshAgent = null;

    AnimationEventBinder animationEventBinder = null;

    private void OnEnable()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();

        nextRoutePoint = RoutePointManager.Instance.GetNearRoutePoint(transform.position);
        navMeshAgent.SetDestination(nextRoutePoint.transform.position);
    }

    void Update()
    {
        //switch(enemyState)
        //{
        //    case EnemyState.Chase:
        //        {
        //            break;
        //        }
        //    case EnemyState.Lost:
        //        {
        //            if(navMeshAgent.isStopped)
        //            {
        //                return;
        //            }

        //            navMeshAgent.SetDestination(Player.transform.position);

        //            LostedChaseTime += Time.deltaTime;
        //            if (LostedChaseTime >= 2.0f)
        //            {
        //                LostedChaseTime = 0.0f;
        //                navMeshAgent.isStopped = true;

        //                animationEventBinder = animationEventBinder ?? new AnimationEventBinder(GetComponent<Animator>());
        //                animationEventBinder.SetStateInfo(TitanAnimeState.Idle__L0)
        //                    .BindFinishedEvent(() =>
        //                    {
        //                        enemyState = EnemyState.Patrol;
        //                        navMeshAgent.isStopped = false;
        //                        nextRoutePoint = RoutePointManager.Instance.GetNearRoutePoint(transform.position);
        //                        navMeshAgent.SetDestination(nextRoutePoint.transform.position);

        //                        animationEventBinder.SetStateInfo(TitanAnimeState.Walk__L0).Play();

        //                    }).Play();
        //            }
        //            break;
        //        }
        //    case EnemyState.Patrol:
        //        {
        //            break;
        //        }
        //}
    }

    void OnTriggerEnter(Collider other)
    {
        if(enemyState != EnemyState.Patrol)
        {
            return;
        }

        if (other.CompareTag("RoutePoint"))
        {
            if (RoutePointManager.Instance.GetRoutePoint(other.gameObject) == beforRoutePoint)
            {
                return;
            }

            if (RoutePointManager.Instance.GetRoutePoint(other.gameObject) != nextRoutePoint)
            {
                return;
            }

            // 到達したポイント
            var routePoint = RoutePointManager.Instance.GetRoutePoint(other.gameObject);
            if (routePoint)
            {
                // 次のポイント
                nextRoutePoint = routePoint.GetaNextRoutePoint(beforRoutePoint); ;
                if (nextRoutePoint)
                {
                    navMeshAgent.SetDestination(nextRoutePoint.transform.position);
                }
                beforRoutePoint = routePoint;
            }
        }
        else if (other.CompareTag("BranchPoint"))
        {
            if (RoutePointManager.Instance.GetRoutePoint(other.gameObject) == beforRoutePoint)
            {
                return;
            }

            if (RoutePointManager.Instance.GetRoutePoint(other.gameObject) != nextRoutePoint)
            {
                return;
            }

            // 到達したポイント
            var branchPoint = RoutePointManager.Instance.GetBranchPoint(other.gameObject);
            if (branchPoint)
            {
                // 次のポイント
                nextRoutePoint = branchPoint.GetaNextRoutePoint(beforRoutePoint);
                if (nextRoutePoint)
                {
                    navMeshAgent.SetDestination(nextRoutePoint.transform.position);
                    nextRoutePoint.Close();
                }
                beforRoutePoint = branchPoint;
            }
        }
    }

    void OnDrawGizmos()
    {
        if (navMeshAgent)
        {
            Gizmos.color = new Color(1f, 0, 1f, 1f);
            Gizmos.DrawLine(transform.position, navMeshAgent.destination);
        }

        if (beforRoutePoint)
        {
            Gizmos.color = new Color(1f, 1f, 1f, 1f);
            Gizmos.DrawLine(transform.position, beforRoutePoint.transform.position);
        }
    }

    /// <summary>
    /// プレイヤーを発見した際に呼ぶ
    /// </summary>
    /// <param name="position">Position.</param>
    public void FoundPlayer(Vector3 position)
    {
        //if(enemyState != EnemyState.Chase)
        //{
        //    enemyState = EnemyState.Chase;
        //    navMeshAgent.autoBraking = true;
        //    navMeshAgent.isStopped = false;
        //}

        //navMeshAgent.SetDestination(position);
    }

    /// <summary>
    /// 追跡中にプレイヤーを見失ったらよ呼ぶ
    /// </summary>
    /// <param name="player">Player.</param>
    public void LostPlayer(Transform player)
    {
        //if(enemyState == EnemyState.Chase)
        //{
        //    enemyState = EnemyState.Lost;
        //    Player = player;
        //}
    }
}
