using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 分岐点に置くポイント
/// </summary>
[DefaultExecutionOrder(-50)]
public class BranchPoint : RoutePoint
{
    public override RoutePoint GetaNextRoutePoint(RoutePoint preRoutePoint)
    {
        if (nextRoutePointCandidates.Count == 0)
        {
            Debug.LogError("BranchPoint::RoutePoint FAIL. Zero routePoints count.");
            return null;
        }

        var notPreRoutePoints = nextRoutePointCandidates.FindAll(x => x != preRoutePoint);
        if(notPreRoutePoints.Count == 0)
        {
            return preRoutePoint;
        }

        int index = 0;
        var notClosedRoutePoints = notPreRoutePoints.FindAll(x => !x.IsClose);

        if (notClosedRoutePoints.Count != 0)
        {
            index = Random.Range(0, notClosedRoutePoints.Count);
            return notClosedRoutePoints[index];
        }

        index = Random.Range(0, notPreRoutePoints.Count);
        return notPreRoutePoints[index];
    }
}
