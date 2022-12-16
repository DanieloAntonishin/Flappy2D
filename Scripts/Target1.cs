using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Target1 : MonoBehaviour
{
    private Sprite sprite;
    [SerializeField]
    private TMPro.TextMeshProUGUI scoreGui;
    [SerializeField]
    private TextMeshProUGUI clock;
    [SerializeField]
    private GameObject player;
    private float coordX = 20f;
    private float coordY = 20f;
    private void Start()
    {
        sprite = GetComponent<Sprite>();
        Debug.Log(player.transform.position.x);
    }
    private void Update()
    {
        if (clock.text == "0.0")
        {
            ChangePosition();
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Target"))
        {
            scoreGui.text = (int.Parse(scoreGui.text) + 2).ToString();
        }
        if (other.gameObject.CompareTag("Player"))
        {
            scoreGui.text = (int.Parse(scoreGui.text) + 1).ToString();
        }
        ChangePosition();
    }
    private void ChangePosition()
    {
       
        float rxFirst = Random.Range(coordX * -1.0f, player.transform.position.x-3f);
        float rxSecond = Random.Range(player.transform.position.x + 3f,coordX);
        float ryFirst = Random.Range(coordY * -1.0f, player.transform.position.y - 3f);
        float rySecond = Random.Range(player.transform.position.y + 3f, coordY);

        float rx= Random.Range(0, 2) == 0 ?rxFirst:rxSecond;
        float ry= Random.Range(0, 2) == 0 ?ryFirst:rySecond;

        this.transform.position = new Vector3(rx, ry, 0);
    }
}
