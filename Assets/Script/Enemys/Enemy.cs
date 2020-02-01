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
    [SerializeField]
    float walkSpeed = 1.0f;

    [SerializeField]
    float runSpeed = 2.0f;

    [SerializeField]
    Animator surprisedIconAnimator = null;

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

    NavMeshAgent navMeshAgent = null;

    Animator animator = null;

    AnimationEventBinder animationEventBinder = null;

    AnimationEventBinder surprisedIconAnimationEventBinder = null;

    float chaseTime = 0;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        navMeshAgent.speed = walkSpeed;
    }

    private void OnEnable()
    {
        nextRoutePoint = RoutePointManager.Instance.GetNearRoutePoint(transform.position);
        navMeshAgent.SetDestination(nextRoutePoint.transform.position);
    }

    void Update()
    {
        switch (enemyState)
        {
            case EnemyState.Chase:
                {
                    chaseTime += Time.deltaTime;

                    var direction = EnemyManager.Instance.GetPlayerTransform().position - transform.position;
                    if (Physics.Raycast(transform.position, direction, out RaycastHit raycastHit))
                    {
                        // Rayが当たっている間プレイヤーを捕捉
                        if (raycastHit.transform.CompareTag("Player"))
                        {
                            chaseTime = 0.0f;
                        }
                    }

                    navMeshAgent.SetDestination(EnemyManager.Instance.GetPlayerTransform().position);

                    if (chaseTime >= 5.0f)
                    {
                        enemyState = EnemyState.Lost;

                        chaseTime = 0.0f;

                        // プレイヤーを逃す
                        GameCanvas.Instance.GetStatusIcon().Waring(this);

                        navMeshAgent.isStopped = true;

                        animationEventBinder = animationEventBinder ?? new AnimationEventBinder(GetComponent<Animator>());
                        animationEventBinder.SetStateInfo(TitanAnimeState.Idle__L0)
                        .BindCompletedEvent(() =>
                        {
                            enemyState = EnemyState.Patrol;

                            navMeshAgent.speed = walkSpeed;
                            navMeshAgent.isStopped = false;

                            nextRoutePoint = RoutePointManager.Instance.GetNearRoutePoint(transform.position);
                            navMeshAgent.SetDestination(nextRoutePoint.transform.position);

                            animator.Play(TitanAnimeState.Walk__L0);
                            // プレイヤーを逃す
                            GameCanvas.Instance.GetStatusIcon().Escaped(this);

                        }).Play();
                    }
                    break;
                }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("GameOver");

            GamaeManager.Instance.IsDirecting = true;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (enemyState != EnemyState.Patrol)
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
        if (enemyState == EnemyState.Chase)
        {
            return;
        }

        enemyState = EnemyState.Chase;

        navMeshAgent.isStopped = false;
        navMeshAgent.speed = runSpeed;

        // プレイヤーを発見
        GameCanvas.Instance.GetStatusIcon().Waring(this);

        animator.Play(TitanAnimeState.Run__L0);

        surprisedIconAnimationEventBinder = surprisedIconAnimationEventBinder ?? new AnimationEventBinder(surprisedIconAnimator);
        surprisedIconAnimationEventBinder
        .SetStateInfo(SurprisedIconAnimeState.SurprisedIconOn__L0)
        .BindFinishedEvent(() =>
        {
            surprisedIconAnimationEventBinder
            .SetStateInfo(SurprisedIconAnimeState.SurprisedIconOff__L0)
            .BindFinishedEvent(() =>
            {
                    // プレイヤーを追跡
                    GameCanvas.Instance.GetStatusIcon().Danger(this);

            }).Play();

        }).Play();
    }
}
