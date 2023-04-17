using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spin : MonoBehaviour
{
    private void FixedUpdate()
    {
        transform.Rotate(0, 0, 1);
    }
}
