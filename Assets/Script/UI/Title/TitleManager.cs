using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{
    [SerializeField]
    private Text Timetext=null;
    [SerializeField]
    private GameObject Tutorial= null;
    // Update is called once per frame
    private void Awake()
    {
        var bestTime = new DataSave().LoadLocalData();
        bestTime.bestRecord = 200;
        var sec = bestTime.bestRecord % 60;
        var min = bestTime.bestRecord / 60;
        Timetext.text = "BEST TIME: "+min.ToString()+":"+sec;
        Tutorial.SetActive(false);
    }
    void Update()
    {
     
        if (Input.GetMouseButtonDown(0))
        {
            SceneTransition.Instance.TransitionScene(SceneName.MasterGame);
        }
        if(Input.GetMouseButtonDown(1))
        {
            if(Tutorial.activeSelf)
            {
                Tutorial.SetActive(false);
            }
            else
            {
                Tutorial.SetActive(true);
            }
        }
    }
}
