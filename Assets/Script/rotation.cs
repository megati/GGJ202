﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour
{
    [SerializeField]
    private  Vector3 debugAngle;
    private void FixedUpdate()
    {
        transform.Rotate(debugAngle);
    }

}
