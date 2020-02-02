using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GaugeController : MonoBehaviour
{
    [SerializeField]
    private List<Bar> barList;

    [SerializeField]
    private GameController gameController;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        bool isPerfect = true;
        foreach (Bar bar in barList)
        {
            if (!bar.GetIsClear()) isPerfect = false;
        }

        if (isPerfect || Input.GetKeyDown(KeyCode.P))
        {
            GameManager.Instance.IsDirecting = true;
            DataSave save = new DataSave();
            BestTime bestTime;
            bestTime.bestRecord = (int)gameController.GetTime();
            save.SaveLocalDataToJson(bestTime);
            SceneTransition.Instance.TransitionScene(SceneName.Result);
        }
    }
}
