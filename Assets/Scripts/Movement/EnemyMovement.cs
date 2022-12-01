using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public enum MovementOptions
    {
        Plus,
        Circle,
        Random
    };

    public MovementOptions movementStyle = MovementOptions.Plus;
    private Transform origin;
    
    void Start()
    {
        origin = transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (movementStyle==MovementOptions.Plus) PlusMovement();
    }

    private void PlusMovement()
    {
    }
}
