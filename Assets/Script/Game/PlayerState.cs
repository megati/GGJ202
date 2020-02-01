using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤーの状態
/// </summary>
public class PlayerState : MonoBehaviour
{
    [SerializeField]
    private Transform RuckSackTransform;
    [SerializeField]
    private Transform BagTransform;

    //持っているかどうか
    bool isHold=false;
    //なにを持っているか
    private RepairItemType repairItemType;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Transform GetRuckSackTransform() { return RuckSackTransform; }
    public Transform GetBagTransform() { return BagTransform; }

    /// <summary>
    /// 所持しているものを登録
    /// </summary>
    public void PossessionRegister(RepairItemType type)
    {
        repairItemType = type;
        isHold = true;
    }

    /// <summary>
    /// 所持物を解除
    /// </summary>
    public void PossessionRelease()
    {
        isHold = false;
    }
    
    /// <summary>
    /// アイテムを保持して運んでいるか
    /// </summary>
    /// <returns></returns>
    public bool IsHolad() { return isHold; }
}
