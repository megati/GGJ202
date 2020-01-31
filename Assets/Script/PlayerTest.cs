using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTest : MonoBehaviour
{
    [SerializeField]
    private Vector3 moveAngle;
    [SerializeField]
    private float moveTime;
    // Start is called before the first frame update
    void Start()
    {
        var tween = transform.DOMove
             ( moveAngle, moveTime)
              .SetRelative()
             .SetEase(Ease.OutSine)
             .SetLoops(-1, LoopType.Yoyo);
    }

    // Update is called once per frame

}