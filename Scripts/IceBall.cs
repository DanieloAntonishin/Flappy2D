using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceBall : MonoBehaviour
{
    [SerializeField]
    private float ForceMagnitude = 10; // public и SerializeField имеет приоритет над другими и доступ к изминениею в редакторе
    [SerializeField]
    private TMPro.TextMeshProUGUI scoreGui;
    private Rigidbody2D Rigidbody2D;
    private Vector2 ForceDirection;
    void Start()
    {
        Rigidbody2D = GetComponent<Rigidbody2D>(); 
    }

    void Update()
    {
        float dx = Input.GetAxis("Horizontal"); // Величина "усилия" по осям:
        float dy = Input.GetAxis("Vertical"); // клавиши, джойтик, "стрелки"
        
        ForceDirection=new Vector2(ForceMagnitude * dx, ForceMagnitude * dy); 
        Rigidbody2D.AddForce(ForceDirection);
    }
    private void OnCollisionEnter2D(Collision2D other)
    {   
        if (other.gameObject.name == "Ranges")
        {
            scoreGui.text = (int.Parse(scoreGui.text) -1 ).ToString();
        }
      
        // Debug.Log("Collision " + other.gameObject.name);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log("Trigger " + other.gameObject.name);
    }
}
