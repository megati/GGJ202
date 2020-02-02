using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

/// <summary>
/// 修理物
/// </summary>
public class Defective : MonoBehaviour
{
    [SerializeField]
    private GameObject UI;
    [SerializeField]
    private GameObject popUp;
    [SerializeField]
    private GameObject mousePlease;
    [SerializeField]
    private Animation popupAnimation;
    [SerializeField]
    private Bar bar;

    [Header("Option")]
    //修復する種類
    [SerializeField]
    private RepairItemType repairItemType = RepairItemType.BOMBE;

    private GameObject camera;
    private GameObject player;

    private bool isAction=false;

    //作業時間
    const float MaxTime = 60 * 5;

    [SerializeField]
    float time = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        camera = GameObject.FindGameObjectWithTag("MainCamera").gameObject;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (popUp.activeSelf)
        {
            popUp.transform.LookAt(new Vector3(camera.transform.position.x * -1, popUp.transform.position.y, camera.transform.position.z * -1), popUp.transform.position);
        }
        //時間中
        if (isAction)
        {
            float percent = (int)((time / MaxTime) * 100);
            bar.SetParameter(percent);
            if (Input.GetMouseButtonDown(1) || time >=  MaxTime)
            {
                UI.SetActive(false);
                isAction = false;
            }
            time += 1.0f*Time.deltaTime;
        }
    }

    /// <summary>
    /// 当たり判定
    /// </summary>
    /// <param name="other">ぶつかったもの</param>
    void OnTriggerEnter(Collider other)
    {
        if (time >= MaxTime) return;

        if (other.tag == "Player")
        {
            popupAnimation.Play();
        }
    }

    /// <summary>
    /// 当たり判定
    /// </summary>
    /// <param name="other">ぶつかったもの</param>
    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            if (time >= MaxTime) return;

            PlayerState playerState = player.GetComponent<PlayerState>();
            if (!playerState.IsHolad() || playerState.GetRepairItemType() != repairItemType) return;

            mousePlease.SetActive(true);
            //道具を使う
            if (Input.GetMouseButtonDown(0))
            {
                UI.SetActive(true);
                UI.GetComponent<SlideGimmick>().Init(this);
                isAction = true;
            }

            //TODO:マウスを出すタイミングだった場合コメントを外す
            //.SemousePleasetActive(true);
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
            UI.SetActive(false);
            popupAnimation.Stop();
            popUp.SetActive(false);
            mousePlease.SetActive(false);
        }
    }

    //加算
    public void TimePlus()
    {
        time += 60.0f;
    }
}
