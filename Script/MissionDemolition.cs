using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum GameMode
{
    idle,
    playing,
    levelEnd
}
public class MissionDemolition : MonoBehaviour
{
    static private MissionDemolition S; //скрытый объект одиночка

    [Header("Set in inspector")]
    public Text uiLevel; //ссылка на объект UIText_Level
    public Text uiShots; //ссылка на объект UIText_Shots
    public Text uiButton; //ссылка на дочерний объект Text в UIButton_View
    public Vector3 castlePos; //местоположение замка
    public GameObject[] castles;//массив замков

    [Header("Set Dynamically")]
    public int level;//текущий уровень
    public int levelMax;//кол-во уровней
    public int shotsTaken;
    public GameObject castle;//текущий замок
    public GameMode mode = GameMode.idle;
    public string showing = "Show Slingshot";//режим FollowCam


    void Start()
    {
        S = this; // определить объект одиночку

        level = 0;
        levelMax = castles.Length;
        StartLevel();
        
    }

    void StartLevel()
    {
        //уничтожить прежний замок, если он существует
        if (castle!=null) Destroy(castle);
        
        //уничтожить прежние снаряды, если они существуют
        GameObject[] gos = GameObject.FindGameObjectsWithTag("Projectile");
        foreach (GameObject pTemp in gos)
            Destroy(pTemp);

        //создать новый замок
        castle = Instantiate<GameObject>(castles[level]);
        castle.transform.position = castlePos;
        shotsTaken = 0;

        //переустановить камеру в начальную позицию
        SwitchView("Show Both");
        ProjectileLine.S.Clear();

        //сбросить цель
        Goal.goalMet = false;

        UpdateGUI();

        mode = GameMode.playing;

    }

    void UpdateGUI()
    {
        //показать данные в элемента ПИ
        uiLevel.text = "Level: " + (level + 1) + " of " + levelMax;
        uiShots.text = "Shots Taken: " + shotsTaken;
    }

    void Update()
    {
        UpdateGUI();

        //проверить завершение уровня
        if((mode == GameMode.playing) && Goal.goalMet)
        {
            //изменить режим, чтобы прекратить проверку завершения уровня
            mode = GameMode.levelEnd;
            //уменьшить масштаб
            SwitchView("Show Both");
            //начать новый уровень через 2 секунды
            Invoke("NextLevel", 2f);

        }
    }

    void NextLevel()
    {
        level++;
        if (level == levelMax) level = 0;
        StartLevel();
    }

    public void SwitchView(string eview = "")
    {
        if (eview == "") eview = uiButton.text;
        showing = eview;
        switch (showing)
        {
            case "Show Slingshot":
                FollowCam.POI = null;
                uiButton.text = "Show Castle";
                break;
            case "Show Castle":
                FollowCam.POI = S.castle;
                uiButton.text = "Show Both";
                break;
            case "Show Both":
                FollowCam.POI = GameObject.Find("ViewBoth");
                uiButton.text = "Show Slingshot";
                break;
        }
    }

    //статический метод позволяющий из любого кода увеличить shotsTaken
    public static void ShotFired()
    {
        S.shotsTaken++;
    }





}
