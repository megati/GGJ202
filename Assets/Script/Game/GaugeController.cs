using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GaugeController : MonoBehaviour
{
    [SerializeField]
    private List<Bar> barList;

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
        }
    }
}
