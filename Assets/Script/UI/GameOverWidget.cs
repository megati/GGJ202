using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverWidget : MonoBehaviour
{
    void Update()
    {

        if(Input.GetMouseButtonDown(0))
        {
            SceneTransition.Instance.TransitionScene(SceneName.MasterGame);
        }

        if (Input.GetMouseButtonDown(1))
        {
            SceneTransition.Instance.TransitionScene(SceneName.Title);
        }
    }
}
