using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ルートポイント管理
/// </summary>
public class RoutePointManager : SingletonMonoBehaviour<RoutePointManager>
{
    /// <summary>
    /// ルートポイントを閉鎖する時間
    /// </summary>
    [SerializeField]
    float routePointCloseDuration = 0.0f;

    Dictionary<GameObject, RoutePoint> routePoints = null;

    Dictionary<GameObject, BranchPoint> branchPoints = null;

    /// <summary>
    /// ルートポイントを閉鎖する時間を取得
    /// </summary>
    /// <value>The duration of the get route point close.</value>
    public float GetRoutePointCloseDuration => routePointCloseDuration;

    /// <summary>
    /// ルートポイントとブランチポイントを全て破棄
    /// </summary>
    public void PointsClear()
    {
        routePoints.Clear();
        branchPoints.Clear();
    }

    /// <summary>
    /// ルートポイントを追加
    /// </summary>
    /// <param name="routePoint">Route point.</param>
    public void AddRoutePoint(RoutePoint routePoint)
    {
        routePoints = routePoints ?? new Dictionary<GameObject, RoutePoint>();

        if (!routePoints.ContainsKey(routePoint.gameObject))
        {
            routePoints.Add(routePoint.gameObject, routePoint);
        }

        if (routePoint is BranchPoint branchPoint)
        {
            AddBranchPoint(branchPoint);
        }
    }

    /// <summary>
    /// ブランチポイントを追加
    /// </summary>
    /// <param name="branchPoint">Route point.</param>
    void AddBranchPoint(BranchPoint branchPoint)
    {
        branchPoints = branchPoints ?? new Dictionary<GameObject, BranchPoint>();

        if (!branchPoints.ContainsKey(branchPoint.gameObject))
        {
            branchPoints.Add(branchPoint.gameObject, branchPoint);
        }
    }

    /// <summary>
    /// ルートポイントを取得
    /// </summary>
    /// <returns>The route point.</returns>
    /// <param name="routePointGameObject">Key of game object.</param>
    public RoutePoint GetRoutePoint(GameObject routePointGameObject)
    {
        if ((routePoints == null) || (!routePoints.ContainsKey(routePointGameObject)))
        {
            Debug.LogError($"RoutePointManager::GetRoutePoint FAIL. Not contains {routePointGameObject.name} in routePoints.");
            return null;
        }

        return routePoints[routePointGameObject];
    }

    /// <summary>
    /// ブランチルートポイントを取得
    /// </summary>
    /// <returns>The route point.</returns>
    /// <param name="branchPointGameObject">Key of game object.</param>
    public BranchPoint GetBranchPoint(GameObject branchPointGameObject)
    {
        if ((branchPoints == null) || (!branchPoints.ContainsKey(branchPointGameObject)))
        {
            Debug.LogError($"RoutePointManager::GetBranchPoint FAIL. Not contains {branchPointGameObject.name} in branchPoints.");
            return null;
        }

        return branchPoints[branchPointGameObject];
    }

    /// <summary>
    /// 一番近いルートポイントを取得
    /// </summary>
    /// <returns>The near route point.</returns>
    /// <param name="position">Position.</param>
    public RoutePoint GetNearRoutePoint(Vector3 position)
    {
        RoutePoint nearRoutePoint = null;

        foreach(var routePoint in routePoints.Values)
        {
            if(!nearRoutePoint)
            {
                nearRoutePoint = routePoint;
                continue;
            }

            if (routePoint.GetDistance(position) < nearRoutePoint.GetDistance(position))
            {
                nearRoutePoint = routePoint;
            }
        }

        return nearRoutePoint;
    }
}
