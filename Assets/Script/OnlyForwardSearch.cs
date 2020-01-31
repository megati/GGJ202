using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class OnlyForwardSearch : MonoBehaviour
{
    [SerializeField]
    private SphereCollider searchArea;

    [Tooltip("敵の視野角の設定")]
    public float searchAngle = 130f;

    
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            //　主人公の方向
            var playerDirection = other.transform.position - transform.position;
            //　敵の前方からの主人公の方向
            var angle = Vector3.Angle(transform.forward, playerDirection);
            //　サーチする角度内だったら発見
            if (angle <= searchAngle)
            {
                //Rayの作成　　　　　　　↓Rayを飛ばす原点　　　↓Rayを飛ばす方向
                Ray ray = new Ray(transform.position, playerDirection);

                //Rayが当たったオブジェクトの情報を入れる箱
                RaycastHit hit;

                //Rayの飛ばせる距離
                int distance = 10;

                //  Debug.DrawRay(transform.position, playerDirection, Color.red);
                //もしRayにオブジェクトが衝突したら
                //                  ↓Ray  ↓Rayが当たったオブジェクト ↓距離
                if (Physics.Raycast(ray, out hit, distance))
                {
                    //Rayが当たったオブジェクトのtagがPlayerだったら
                    if (hit.collider.tag == "Player")
                        Debug.Log("RayがPlayerに当たった");
                }
                //Debug.Log("主人公発見: " + angle);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
        }
    }
#if UNITY_EDITOR
    //　サーチする角度表示
    private void OnDrawGizmos()
    {
        Handles.color = Color.red;
        Handles.DrawSolidArc(transform.position, Vector3.up, Quaternion.Euler(0f, -searchAngle, 0f) * transform.forward, searchAngle * 2f, searchArea.radius);
    }
#endif
}
