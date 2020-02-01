using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepairItem : MonoBehaviour
{
    [SerializeField]
    private GameObject Point;

    private GameObject Player;

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// 当たり判定
    /// </summary>
    /// <param name="other">ぶつかったもの</param>
    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            if (Input.GetMouseButtonDown(0))
            {
                Point.SetActive(false);
                transform.SetParent(Player.GetComponent<PlayerMove>().GetCarryTransform(), false);
            }
        }
    }
}
