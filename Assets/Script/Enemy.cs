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
    NavMeshAgent navMeshAgent = null;

    /// <summary>
    /// 一つ前のポイント
    /// </summary>
    RoutePoint beforRoutePoint = null;

    /// <summary>
    /// 次のポイント
    /// </summary>
    RoutePoint nextRoutePoint = null;

    private void OnEnable()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();

        nextRoutePoint = RoutePointManager.Instance.GetNearRoutePoint(transform.position);
        navMeshAgent.SetDestination(nextRoutePoint.transform.position);
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("RoutePoint"))
        {
            if(RoutePointManager.Instance.GetRoutePoint(other.gameObject) == beforRoutePoint)
            {
                return;
            }

            if(RoutePointManager.Instance.GetRoutePoint(other.gameObject)!= nextRoutePoint)
            {
                return;
            }

            // 到達したポイント
            var routePoint = RoutePointManager.Instance.GetRoutePoint(other.gameObject);
            if(routePoint)
            {
                // 次のポイント
                nextRoutePoint = routePoint.GetaNextRoutePoint(beforRoutePoint);;
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

    private void OnDrawGizmos()
    {
        if(navMeshAgent)
        {
            Gizmos.color = new Color(1f, 0, 1f, 1f);
            Gizmos.DrawLine(transform.position, navMeshAgent.destination);
        }

        if(beforRoutePoint)
        {

            Gizmos.color = new Color(1f, 1f, 1f, 1f);
            Gizmos.DrawLine(transform.position, beforRoutePoint.transform.position);
        }
    }
}
