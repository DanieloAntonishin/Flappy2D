using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameStat : MonoBehaviour
{

    [SerializeField]
    private TMPro.TextMeshProUGUI clock;
    [SerializeField]
    private TMPro.TextMeshProUGUI score;
    [SerializeField]
    private UnityEngine.UI.Image energy; // EnergyIndicator - field-tye image
    [SerializeField]
    private UnityEngine.UI.Image health; // HealthIndicator - field-tye image

    private MenuCanvas menuCanvas;

    private string bestDataFileName = "Assets/Files/bestdata.sav";
    private string bestDataJsonName = "Assets/Files/bestdata.json";

    private float _score;
    private float _lastScore; // предидущия кол-во очков
    private float _bestScore; // лучшее значение, храним в файле
    private float _gameTime;
    private float _lastTime;  // предидущие время в игре
    private float _bestTime;  // максимальное время проведенное в игре
    private float _gameEnergy; // [0..1] value
    private float _gameHealth; // [0..1] value
    public float GameScore
    {
        get => _score;
        set
        {
            _score = value;
            UpdateUIScore();
        }
    }
    public float GameTime
    {
        get => _gameTime;
        set
        {
            _gameTime = value;
            UpdateUITime();
        }
    }
    public float GameEnergy
    {
        get => _gameEnergy;
        set
        {
            _gameEnergy = value;
            UpdateUIEnergy();
        }
    }
    public float GameHealth
    {
        get => _gameHealth;
        set
        {
            _gameHealth = value;
            UpdateUIHealth();
        }
    }
    void Start()
    {
        GameTime = 0;
        GameEnergy = energy.fillAmount;  // TODO: зависимость от сложности 
        GameHealth = health.fillAmount; // Выставленние hp

        menuCanvas = menuCanvas = GameObject.Find("MenuCanvas")
            .GetComponent<MenuCanvas>();

        ReadStatisticFromJSON(); // чтение данных из JSON и установка на холст

    }

    void LateUpdate()
    {
        GameTime += Time.deltaTime;
    }

    private void OnDestroy()  // метод жизненого цикла, запускается перед разрушением объекта
    {
        // JSON file
        WriteStatisticToJSON();
    }

    private void UpdateUITime()
    {
        int t = (int)_gameTime;
        clock.text = $"{t / 3600 % 24:00}:{t / 60 % 60:00}:{t % 60:00}.{(int)((_gameTime - t) * 10):0}";
        if (_gameTime > _bestTime)
        {
            score.fontStyle = TMPro.FontStyles.Bold;
            _bestTime = _gameTime; // новый рекорд
        }
        else
        {
            score.fontStyle = TMPro.FontStyles.Normal;
        }
        _lastTime = _gameTime <= 1 ? _lastTime : _gameTime; 
    }
    private void UpdateUIEnergy()
    {
        if (_gameEnergy >= 0 && _gameEnergy <= 1)
        {
            energy.fillAmount = _gameEnergy;
        }
        else
        {
            Debug.LogError("Game Energy out of range");
        }
    }
    private void UpdateUIHealth()
    {
        if (_gameHealth >= 0 && _gameHealth <= 1)
        {
            health.fillAmount = _gameHealth;
        }
    }
    private void UpdateUIScore()
    {
        //score.text = _score.ToString();
        score.text = $"{_score:0000}";
        if (_score > _bestScore)
        {
            score.fontStyle = TMPro.FontStyles.Bold;
            _bestScore = _score;
        }
        else
        {
            score.fontStyle = TMPro.FontStyles.Normal;
        }
        _lastScore = _score != 0 ? _score : _lastScore;
    }

    class BestData  // для сериализации в JSON
    {
        public float Score;
        public float Time;
        public float LastScore;
        public float LastTime;
    }

    public void WriteStatisticToJSON()
    {
        Debug.Log("Write");
        BestData data = new BestData()
        {
            Score = (_score >= _bestScore ? _score : _bestScore),
            Time = (_gameTime >= _bestTime ? _gameTime : _bestTime),
            LastScore = _lastScore,
            LastTime = _lastTime
        };
        System.IO.File.WriteAllText(bestDataJsonName, JsonUtility.ToJson(data, true));
    }

    public void ReadStatisticFromJSON()
    {
        // JSON file 
        if (System.IO.File.Exists(bestDataFileName))
        {
            BestData data = JsonUtility.FromJson<BestData>(
                System.IO.File.ReadAllText(bestDataJsonName));
            _bestScore = data.Score;
            _bestTime = data.Time;
            _lastScore = data.LastScore;
            _lastTime = data.LastTime;
        }
        else
        {
            _bestScore = 0;
        }

        menuCanvas.GameRecord = $"Best score = {_bestScore} \n Best time = {_bestTime}\n\nLast score = {_lastScore} \n Last time = {_lastTime}";

        _lastScore = 0;
        _lastTime = 0;
    }
}
