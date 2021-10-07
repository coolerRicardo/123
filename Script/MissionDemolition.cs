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
    static private MissionDemolition S; //������� ������ ��������

    [Header("Set in inspector")]
    public Text uiLevel; //������ �� ������ UIText_Level
    public Text uiShots; //������ �� ������ UIText_Shots
    public Text uiButton; //������ �� �������� ������ Text � UIButton_View
    public Vector3 castlePos; //�������������� �����
    public GameObject[] castles;//������ ������

    [Header("Set Dynamically")]
    public int level;//������� �������
    public int levelMax;//���-�� �������
    public int shotsTaken;
    public GameObject castle;//������� �����
    public GameMode mode = GameMode.idle;
    public string showing = "Show Slingshot";//����� FollowCam


    void Start()
    {
        S = this; // ���������� ������ ��������

        level = 0;
        levelMax = castles.Length;
        StartLevel();
        
    }

    void StartLevel()
    {
        //���������� ������� �����, ���� �� ����������
        if (castle!=null) Destroy(castle);
        
        //���������� ������� �������, ���� ��� ����������
        GameObject[] gos = GameObject.FindGameObjectsWithTag("Projectile");
        foreach (GameObject pTemp in gos)
            Destroy(pTemp);

        //������� ����� �����
        castle = Instantiate<GameObject>(castles[level]);
        castle.transform.position = castlePos;
        shotsTaken = 0;

        //�������������� ������ � ��������� �������
        SwitchView("Show Both");
        ProjectileLine.S.Clear();

        //�������� ����
        Goal.goalMet = false;

        UpdateGUI();

        mode = GameMode.playing;

    }

    void UpdateGUI()
    {
        //�������� ������ � �������� ��
        uiLevel.text = "Level: " + (level + 1) + " of " + levelMax;
        uiShots.text = "Shots Taken: " + shotsTaken;
    }

    void Update()
    {
        UpdateGUI();

        //��������� ���������� ������
        if((mode == GameMode.playing) && Goal.goalMet)
        {
            //�������� �����, ����� ���������� �������� ���������� ������
            mode = GameMode.levelEnd;
            //��������� �������
            SwitchView("Show Both");
            //������ ����� ������� ����� 2 �������
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

    //����������� ����� ����������� �� ������ ���� ��������� shotsTaken
    public static void ShotFired()
    {
        S.shotsTaken++;
    }





}
