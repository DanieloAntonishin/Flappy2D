using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Периодическая генерация новых объектов
public class SpawnPoint : MonoBehaviour
{
    // ссылка на префаб 
    [SerializeField]    
    private GameObject Pipe;
    private float PipeTime;
    [SerializeField]
    private float PipeSpawnTime = 2; // секунды между трубами
    [SerializeField]
    private GameObject Energy;
    private float EnergyTime;
    [SerializeField]
    private float EnergySpawnTime = 10;
    private float TimeRange = 4;     // диапазон изминения в зависимости от сложности
    void Start()
    {
        PipeTime = 0;
        EnergyTime = EnergySpawnTime*2;
    }

    void Update()
    {
        PipeTime -= Time.deltaTime;
        EnergyTime -= Time.deltaTime;
        if(PipeTime < 0 )
        {
            PipeTime = PipeSpawnTime + TimeRange * (1 - MenuCanvas.Difficulty);
            SpawnPipe();
        }
        if (EnergyTime < 0)
        {
            EnergyTime = EnergySpawnTime + TimeRange * (1 + MenuCanvas.Difficulty);
            SpawnEnergy();
        }
    }
    void SpawnPipe()
    {
        GameObject obj = GameObject.Instantiate(Pipe,this.transform.position, Quaternion.identity);
        obj.transform.position = obj.transform.position + Vector3.up * Random.Range(-2f, 2f);
    }
    void SpawnEnergy()
    {
        GameObject obj = GameObject.Instantiate(Energy, this.transform.position, Quaternion.identity);
        obj.transform.position = obj.transform.position + Vector3.up * Random.Range(-5f, 5f);
    }

}
