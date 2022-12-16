using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;
using Unity.VisualScripting;

public class Bird : MonoBehaviour
{
    [SerializeField]
    private float ForceFactor = 10;
    [SerializeField]
    private TMPro.TextMeshProUGUI scoreGui;
    private Rigidbody2D Rigidbody2D;
    private Vector2 ForceDirection;
    private bool isActiveMenu;   // флаг для проверки на обнуление очков при повторной игре
    private float holdTime;            // время удержание пробела
    private const float holdTimeLimit = 1;       // предельное время пробела
    private const float discrete2continualFactor = 40; // разница в однократном и постоянном дефствии 
    private const float deltaTimeScaler = 100; // множитель при deltaTime для коррекции на быстродействие 
    private const float healthPointCost = 0.333f;  
    private const float energyhPointCost = 0.05f;  
    private GameStat gameStat;     //ссылка на объект класса GameStat, находящийся на холсте "GameStat"
    private MenuCanvas menuCanvas;
    void Start()
    {
        Rigidbody2D = GetComponent<Rigidbody2D>();
        ForceDirection = Vector2.up * ForceFactor;

        gameStat =                          // Объект класса GameStat - это компонент
           GameObject.Find("GameStat")     // GameObject-а "GameStat" (холста)
           .GetComponent<GameStat>();      // 
        menuCanvas = GameObject.Find("MenuCanvas")
            .GetComponent<MenuCanvas>();
    }

    void Update()
    {
        if (gameStat.GameEnergy > 0)
        { 
            if (MenuCanvas.ControlTypeIndex == 0)
            {
                #region Непрерывное управление (постоянное нажатие)
                if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.UpArrow))  // только с клавиатуры
                {
                    Rigidbody2D.AddForce(ForceDirection * Time.deltaTime * deltaTimeScaler);
                    gameStat.GameEnergy -= energyhPointCost * Time.deltaTime;
                }

                float force = Input.GetAxis("Jump");  // с клавиатуры и джойстика
                if (force != 0)
                    Rigidbody2D.AddForce(ForceDirection * Time.deltaTime * deltaTimeScaler * force);
                #endregion
            }
            else if (MenuCanvas.ControlTypeIndex == 1)
            {
                #region Импульсное управление (однократные нажатия)
                if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow))
                {
                    Rigidbody2D.AddForce(ForceDirection * discrete2continualFactor);
                    gameStat.GameEnergy -= energyhPointCost * Time.deltaTime * deltaTimeScaler/2;
                }
                #endregion
            }
            else
            {
                #region
                // Сила растет при удержании пробела, но не дольше 1 секунды
                // дальнейшее удержание пробела игнорируется, требуется повторное нажатие
                //if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow))
                //{
                //    Rigidbody2D.AddForce(ForceDirection * discreate2continualFactor);

                //}

                if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow)) holdTime = holdTimeLimit;
                if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow)) && holdTime > 0) holdTime = Time.deltaTime;
                if (Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.UpArrow))
                {
                    holdTime = 0;
                }

                if (holdTime > 0)
                {
                    Rigidbody2D.AddForce(ForceDirection * Time.deltaTime * deltaTimeScaler);
                    gameStat.GameEnergy -= energyhPointCost * Time.deltaTime;
                }
                #endregion
            }

        this.transform.rotation = Quaternion.Euler(0, 0, 2 * Rigidbody2D.velocity.y);
        }
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Energy"))
        {
            gameStat.GameEnergy += gameStat.GameEnergy + Energy.CountOfEnergy > 1 ? 1 - gameStat.GameEnergy : Energy.CountOfEnergy;
            Destroy(other.gameObject);
        }
        if (other.gameObject.CompareTag("Ranges"))
        {
            gameStat.GameEnergy -= energyhPointCost;
        }
        if (other.gameObject.CompareTag("Pipe") ) 
        {
            gameStat.GameHealth -= healthPointCost;
            if (gameStat.GameEnergy <= 0)       // если при потери здоровья нету полностью енергии
            {
                gameStat.GameEnergy = energyhPointCost * 4;
            }
            if (gameStat.GameHealth < healthPointCost)  // если потрачены все жизни
            {
                Debug.Log(gameStat.GameHealth);
                isActiveMenu = true;
                gameStat.GameScore = 0;
             
               
                menuCanvas.ShowMenu(true, "Again", "Play again. Last time result  " + gameStat.GameTime);

                gameStat.GameTime = 0;
                gameStat.GameHealth = 1;
                gameStat.GameEnergy = 1;
                
                gameStat.WriteStatisticToJSON();         // срабатывает когда только запускаем или при повторной игре
                gameStat.ReadStatisticFromJSON();        // ( затрагивает последнию статистику игры)
            }

            List<GameObject> otherObjects = new List<GameObject>();
            otherObjects.AddRange(GameObject.FindGameObjectsWithTag("Tube"));
            otherObjects.AddRange(GameObject.FindGameObjectsWithTag("Energy"));
            foreach (GameObject obj in otherObjects)
            {
                Destroy(obj);
            }

            this.transform.position = new Vector3(0f, 0f, 0f);
            this.transform.rotation = Quaternion.Euler(0f, 0f, 0f);

            Rigidbody2D.velocity = new Vector2(0f, 0f);
            Rigidbody2D.angularVelocity = 0f;
           
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Tube"))
        {
            gameStat.GameScore += 100;
        }
    }
}
