using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public Transform PlayerYippei;

    void Update()
    {
        transform.position = PlayerYippei.transform.position;
    }
}
