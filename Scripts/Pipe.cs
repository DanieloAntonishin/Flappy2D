using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pipe : MonoBehaviour
{
    private Rigidbody2D Rigidbody2D;
    [SerializeField]
    private float speed = 1;
    private Vector2 moveDirection;
    void Start()
    {
        Rigidbody2D = GetComponent<Rigidbody2D>();
        moveDirection = Vector2.left*speed;
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.Translate(moveDirection * Time.deltaTime);
    }
}
