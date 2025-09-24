using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
    public Transform end;
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, end.position, Time.time);
    }
}
