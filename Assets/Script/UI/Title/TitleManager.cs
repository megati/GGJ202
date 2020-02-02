using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{
    [SerializeField]
    private Text Timetext=null;

    // Update is called once per frame
    private void Awake()
    {
        var bestTime = new DataSave().LoadLocalData();
        bestTime.bestRecord = 200;
        var sec = bestTime.bestRecord % 60;
        var min = bestTime.bestRecord / 60;
        Timetext.text = "BEST TIME: "+min.ToString()+":"+sec;
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {

        }
        if(Input.GetMouseButtonDown(1))
        {

        }
    }
}
