using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

// Скрипт меню пользователя
public class MenuCanvas : MonoBehaviour
{
    public static int ControlTypeIndex;   // стат. поле позволят обращаться к классу 
                                          // (не искать объект из других скриптов)
    public static float Difficulty;

    [SerializeField]
    private GameObject UserMenu;

    [SerializeField]
    private TMPro.TextMeshProUGUI StartButtonText;

    [SerializeField]
    private TMPro.TMP_Dropdown ChangeTypeDropdown;

    [SerializeField]
    private TMPro.TextMeshProUGUI MessageUGUI;

    [SerializeField]
    private TMPro.TextMeshProUGUI RecordUGUI;

    [SerializeField]
    private Camera Camera;

    private string _record;

    private bool bgDarkThemeEnabled;

    private Color32 LightTheme;

    private Color32 DarkTheme;

    public string GameRecord
    {
        set
        {
            _record = value;
            UpdateRecordMsg();
        }
    }

    private GameStat gameStat;     //ссылка на объект класса GameStat, находящийся на холсте "GameStat"

    void Start()
    {
        bgDarkThemeEnabled = GameObject.Find("DarkThemeToggle").GetComponent<Toggle>().isOn;  
        LightTheme = new Color32(49, 121, 121,0);
        DarkTheme = new Color32(44, 51, 51,0);
        
        ControlTypeIndex = 0;
        ShowMenu(true, "Start",message: "Start play");
        gameStat =                          // Объект класса GameStat - это компонент
            GameObject.Find("GameStat")     // GameObject-а "GameStat" (холста)
            .GetComponent<GameStat>();      

    }

    void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            int t = (int)gameStat.GameTime;
            string timeInfo = $"{t / 3600 % 24:00}:{t / 60 % 60:00}:{t % 60:00}.{(int)((gameStat.GameTime - t) * 10):0}";
            string msg = $"Game time: {timeInfo}\n Score: {gameStat.GameScore}\n Energy left: {gameStat.GameEnergy:F2}";
            ShowMenu(true, message: msg); 
        }
    }

    public void ResumeButtonClick()
    {
        ShowMenu(false);
    }

    public void ControlTypeChanged(int value)
    {
       MenuCanvas.ControlTypeIndex = ChangeTypeDropdown.value;   // сохраняем стат. поле
    }

    public void DifficultyChanged(System.Single value)
    {
       MenuCanvas.Difficulty = value;
    }

    public void GameThemeChanged()
    {
        Camera.backgroundColor = bgDarkThemeEnabled ? DarkTheme : LightTheme;
    }

    public void ShowMenu(bool mode, string buttonText = "Resume",string message="")
    {
        if (mode)   // режим отображения меню
        {
            UserMenu.SetActive(true);            // Отображаем контейнер меню (все эл-ты)
            Time.timeScale = 0;                  // Останавливаем физическое время
            StartButtonText.text = buttonText;   // Устанавливаем надпись кнопке
            MessageUGUI.text = message;
        }
        else
        {
            UserMenu.SetActive(false);           // Скрываем контейнер меню
            Time.timeScale = 1;                  // Запускаем физическое время
        }
    }

    public void UpdateRecordMsg()
    {
        if (!(File.Exists("Assets/Files/bestdata.json") || File.Exists("Assets/Files/bestdata.sav")))
            RecordUGUI.text = "Play to get first record";
        else
            RecordUGUI.text = _record;
    }

    public void MusicToggleChanged(bool isChecked)
    {
        bgDarkThemeEnabled = isChecked;
        GameThemeChanged();
    }
}
