using System;
using TMPro; // Text Mesh Pro
using UnityEngine;

public class Clock : MonoBehaviour
{
    private TextMeshProUGUI clock;  // ссылка на компонент TextMeshProUGUI
    private float time;
    void Start()
    {
        clock = GetComponent<TextMeshProUGUI>();
        time = 4;
    }

    void Update()
    {
        if (time < 0.0) time = 4;
        time -= Time.deltaTime;
    }
    private void LateUpdate()  // метод ЖЦ, вызывается позже всех 
    {
        int t = (int)time;
        clock.text = String.Format("{0:0}.{1}", t % 60, (int)((time - t) * 10)
            );
    }
}
