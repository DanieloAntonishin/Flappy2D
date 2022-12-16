using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hexagon : MonoBehaviour
{
    private Rigidbody2D Rigidbody2D; // ссылка на компонент Rigidbody2D данного GameObject
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Update()
    {
        // приложение силы
        Rigidbody2D = this.GetComponent<Rigidbody2D>();  // сила прикладывается к твердому телу
        if (Input.GetKey(KeyCode.Space)) // GetKey - Удержание, многократный учет 
        {
            Rigidbody2D.AddForce(Vector2.up * 10); // прикладывем силу 
        }
    }

    // Update is called once per frame
    void UpdateDemo()
    {
        // Script is a component, this mean object instance of GameObject (Hexagon)
        // ref this in scpit point on "parent" GameObject
        // ref = GameObject (Hexagon)
        this.transform  // ref on component Transform in GameObject (Hexagon)
            .Rotate(            
           Vector3.forward,    // around the Z axis (forward)
            (float)0.5);                 // on 1 degree
    }
}
