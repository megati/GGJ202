using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 修理物
/// </summary>
public class Defective : MonoBehaviour
{
    [SerializeField]
    private GameObject popUp;
    [SerializeField]
    private GameObject mousePlease;
    [SerializeField]
    private Animation popupAnimation;

    //プレイヤーの座標
    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("MainCamera").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (popUp.activeSelf)
        {
            popUp.transform.LookAt(new Vector3(player.transform.position.x * -1, popUp.transform.position.y, player.transform.position.z * -1), popUp.transform.position);
        }
    }

    /// <summary>
    /// 当たり判定
    /// </summary>
    /// <param name="other">ぶつかったもの</param>
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            popupAnimation.Play();
            //TODO:マウスを出すタイミングだった場合コメントを外す
            //mousePlease.SetActive(true);
        }
    }

    /// <summary>
    /// 当たり判定から離れたら
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            popupAnimation.Stop();
            popUp.SetActive(false);
            mousePlease.SetActive(false);
        }
    }
}
