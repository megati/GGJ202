using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤーの持たせ方
/// </summary>
public enum HoldType
{
    BAG,
    RUCKSACK
}

/// <summary>
/// 回収するアイテム
/// </summary>
public enum RepairItemType
{
    BOMBE,   //ボンベ
    FUSE,       //ヒューズ
    HANDLE,     //ハンドル
    PACK
}

public class RepairItem : MonoBehaviour
{
    [SerializeField]
    private GameObject Point;

    [Header("Option")]
    //プレイヤーの持ち方
    [SerializeField]
    private HoldType holdType= HoldType.BAG;
    //回収するアイテム
    [SerializeField]
    private RepairItemType repairItemType = RepairItemType.BOMBE;

    private GameObject Player;
    private bool isHolad = false;

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (isHolad && Input.GetMouseButtonDown(1))
        {
            PlayerState playerState = Player.GetComponent<PlayerState>();
            //所有権を放棄
            playerState.PossessionRelease();
            //地上に返す
            isHolad = false;
        }
    }

    /// <summary>
    /// 当たり判定
    /// </summary>
    /// <param name="other">ぶつかったもの</param>
    void OnTriggerStay(Collider other)
    {
        if (isHolad) return;

        if (other.tag == "Player")
        {
            if (Input.GetMouseButtonDown(0))
            {
                PlayerState playerState= Player.GetComponent<PlayerState>();
                if (playerState.IsHolad()) return;

                //登録
                playerState.PossessionRegister(repairItemType);
                Point.SetActive(false);
                if (holdType == HoldType.BAG)
                {
                    transform.SetParent(playerState.GetBagTransform(), false);
                }
                else
                {
                    transform.SetParent(playerState.GetRuckSackTransform(), false);

                }
                isHolad = true;
            }
        }
    }
}
