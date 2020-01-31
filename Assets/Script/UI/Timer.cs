using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 時間表示
/// </summary>
public class Timer : MonoBehaviour
{
    [SerializeField]
    private List<Image> numberList;

    [SerializeField]
    private List<Sprite> numberSpriteList;

    [SerializeField]
    private GameController gameController;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float time=gameController.GetTime();

        int minute = (int)(time / 60);
        numberList[0].sprite = numberSpriteList[minute / 10];
        numberList[1].sprite = numberSpriteList[minute % 10];

        int second = (int)(time % 60);
        numberList[2].sprite = numberSpriteList[second / 10];
        numberList[3].sprite = numberSpriteList[second % 10];
    }
}
