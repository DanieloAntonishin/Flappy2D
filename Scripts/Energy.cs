using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Energy : MonoBehaviour
{
    private Rigidbody2D Rigidbody2D;
    [SerializeField]
    private float speed = 1;
    public static float CountOfEnergy = 0.3f;
    private Vector2 moveDirection;
    void Start()
    {
        Rigidbody2D = GetComponent<Rigidbody2D>();
        moveDirection = Vector2.left * speed;
    }

    void Update()
    {
        this.transform.Translate(moveDirection * Time.deltaTime);
    }
}
