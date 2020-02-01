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

    float LostedChaseTime = 0.0f;

    NavMeshAgent navMeshAgent = null;

    Animator animator = null;

    AnimationEventBinder animationEventBinder = null;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        nextRoutePoint = RoutePointManager.Instance.GetNearRoutePoint(transform.position);
        navMeshAgent.SetDestination(nextRoutePoint.transform.position);
    }

    void Update()
    {
        switch(enemyState)
        {
            case EnemyState.Chase:
                {
                    navMeshAgent.SetDestination(EnemyManager.Instance.GetPlayerTransform().position);

                    break;
                }
            case EnemyState.Lost:
                {
                    if(navMeshAgent.isStopped)
                    {
                        return;
                    }

                    navMeshAgent.SetDestination(EnemyManager.Instance.GetPlayerTransform().position);

                    LostedChaseTime += Time.deltaTime;
                    if (LostedChaseTime >= 2.0f)
                    {
                        LostedChaseTime = 0.0f;
                        navMeshAgent.isStopped = true;

                        //enemyState = EnemyState.Patrol;
                        //navMeshAgent.isStopped = false;
                        //nextRoutePoint = RoutePointManager.Instance.GetNearRoutePoint(transform.position);
                        //navMeshAgent.SetDestination(nextRoutePoint.transform.position);
                        //animator.Play(TitanAnimeState.Walk__L0);

                        animationEventBinder = animationEventBinder ?? new AnimationEventBinder(GetComponent<Animator>());
                        animationEventBinder.SetStateInfo(TitanAnimeState.Idle__L0)
                            .BindCompletedEvent(() =>
                            {
                                enemyState = EnemyState.Patrol;
                                navMeshAgent.isStopped = false;
                                nextRoutePoint = RoutePointManager.Instance.GetNearRoutePoint(transform.position);
                                navMeshAgent.SetDestination(nextRoutePoint.transform.position);

                                animator.Play(TitanAnimeState.Walk__L0);

                            }).Play();

                        //Debug.Log("on");
                    }
                    break;
                }
        //    case EnemyState.Patrol:
        //        {
        //            break;
        //        }
        }
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
    public void FoundPlayer()
    {
        //if(enemyState != EnemyState.Chase)
        //{
        //    enemyState = EnemyState.Chase;
        //    navMeshAgent.autoBraking = true;
        //    navMeshAgent.isStopped = false;
        //}

        if (enemyState != EnemyState.Chase)
        {
            enemyState = EnemyState.Chase;
            navMeshAgent.isStopped = false;

            //animator.SetTrigger(TitanAnimeParam.RunTrigger);
            animator.Play(TitanAnimeState.Run__L0);
        
        }
    }

    /// <summary>
    /// 追跡中にプレイヤーを見失ったらよ呼ぶ
    /// </summary>
    public void LostPlayer()
    {
        //if(enemyState == EnemyState.Chase)
        //{
        //    enemyState = EnemyState.Lost;
        //    Player = player;
        //}

        if (enemyState == EnemyState.Chase)
        {
            enemyState = EnemyState.Lost;
            navMeshAgent.isStopped = false;
        }
    }
}
