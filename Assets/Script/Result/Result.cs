using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Result : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI timer;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        DataSave save = new DataSave();
        int time = save.LoadLocalData().bestRecord;

        int minute = (int)(time / 60);
        int second = (int)(time % 60);

        timer.text = string.Format("{0}{1}:{2}{3}", minute / 10, minute % 10, second / 10, second % 10);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GoRetry();
        }
        else if(Input.GetMouseButtonDown(1))
        {
            GoTitle();
        }
    }

    public void GoTitle()
    {
        //button.gameObject.SetActive(false);
        //button2.gameObject.SetActive(false);
        SceneTransition.Instance.TransitionScene(SceneName.Title);
    }

    public void GoRetry()
    {
        //button.gameObject.SetActive(false);
        //button2.gameObject.SetActive(false);
        SceneTransition.Instance.TransitionScene(SceneName.MasterGame);
    }
}
