using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 道を示す
/// </summary>
[DefaultExecutionOrder(-50)]
public class RoutePoint : MonoBehaviour
{
    /// <summary>
    /// 次の候補のルートポイント
    /// </summary>
    [SerializeField]
    protected List<RoutePoint> nextRoutePointCandidates = null;

    float closeTime = 0.0f;

    public bool IsClose { get; private set; } = false;

    void Awake()
    {
        Debug.Log("awake");
        RoutePointManager.Instance.AddRoutePoint(this);
    }

    void Update()
    {
        if(IsClose)
        {
            closeTime += Time.deltaTime;

            if (closeTime >= RoutePointManager.Instance.GetRoutePointCloseDuration)
            {
                closeTime = 0.0f;
                IsClose = false;
                //Debug.Log("UnClose RoutePoint. -RoutePoint; " + name);
            }
        }
    }

    /// <summary>
    /// 通行禁止にする（BranchRoutePointのGetNextRoutePointの対象から外れる）
    /// </summary>
    public void Close()
    {
        IsClose = true;
        //Debug.Log("Close RoutePoint. -RoutePoint; " + name);
    }

    /// <summary>
    /// 次のルートポイントを取得
    /// </summary>
    /// <returns>The next route point.</returns>
    /// <param name="preRoutePoint">Pre route point.</param>
    public virtual RoutePoint GetaNextRoutePoint(RoutePoint preRoutePoint)
    {
        var nextRoutePoint = nextRoutePointCandidates.Find(x => x != preRoutePoint);
        if(nextRoutePoint)
        {
            return nextRoutePoint;
        }

        Debug.LogError("RoutePoint::GetaNextRoutePoint FAIL. Null nextRoutePoint. Please select two nextRotePoints.");
        return null;
    }

    /// <summary>
    /// 距離を返す（sqrMagnitude）
    /// </summary>
    /// <returns>The distance.</returns>
    /// <param name="position">Position.</param>
    public float GetDistance(Vector3 position)
    {
        var diff = position - transform.position;
        return diff.sqrMagnitude;
    }

    private void OnDrawGizmos()
    {
        if(IsClose)
        {
            Gizmos.color = new Color(1f, 0, 0, 0.5f);
            Gizmos.DrawSphere(transform.position, 1f);
        }
    }
}
