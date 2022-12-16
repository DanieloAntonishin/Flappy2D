using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LogPipe : MonoBehaviour
{
    private int startPointScaler = 30;
    private GameObject SpawnPoint; // ссылка на объект, задающий позицию появления
    void Start()
    {
        SpawnPoint = GameObject.Find("SpawnPoint"); // поиск по имени ( в иерархии )
    }

    void Update()
    {
        
    }
    /* private void OnTriggerEnter2D(Collider2D other) //old version 
    {

        if (other.gameObject.CompareTag("Pipe"))
        {
            GameObject pipe = other.transform.parent.gameObject;  // GameObject родительский 
            Debug.Log(pipe.name);
            pipe.transform.Translate(Vector2.right * startPointScaler);
        }
    }*/
    private void OnTriggerEnter2D(Collider2D other)
    {
        GameObject.Destroy(other.gameObject);

        //if (other.gameObject.CompareTag("Tube"))
        //{
        //    other.gameObject.transform.position =  // переносим позицию объекта
        //        SpawnPoint.transform.position      // в точку появления 
        //        + Vector3.up * Random.Range(-2f, 2f);
        //}

    }

}
