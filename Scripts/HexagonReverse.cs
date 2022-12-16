using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexagonReverse : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.transform  // ref on component Transform in GameObject (Hexagon)
            .Rotate(
           Vector3.back,    // around the Z axis (forward)
            (float)0.5);                 // on 1 degree
    }
}
