using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakedObjectsManager : SingletonMonoBehaviour<BreakedObjectsManager>
{
    private float[] repairRates = new float[4];
    
    public void Inc(int index,float rote)
    {
        repairRates[index] += rote;
    }

    public void Set(int index,float rate)
    {
        repairRates[index] = rate;
    }
    public float Get(int index)
    {
        return repairRates[index];
    }
    public bool GetIsComp(int index)
    {
        return repairRates[index]>=100f;
    }
    public bool GetIsComp()
    {
        foreach (var repairRate in repairRates)
        {
           if(repairRate<100)
            {
                return false;
            }
        }
        return true;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
