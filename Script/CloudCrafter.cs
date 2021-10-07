using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudCrafter : MonoBehaviour
{

    [Header("Set in Inspector")]
    public int numClouds = 40; // число облаков
    public GameObject cloudPrefab; // шаблон для облаков
    public Vector3 cloudPosMin = new Vector3(-40, -5, 10);
    public Vector3 cloudPosMax = new Vector3(150, 100, 10);
    public float cloudScaleMin = 1; //Мин. масштаб каждого облака
    public float cloudScaleMax = 3; //Макс. масштаб каждого облака
    public float cloudSpeedMult = 0.1f; //Коэф. скорости облаков

    private GameObject[] cloudInstances;

    private void Awake()
    {
        //Создать массив для хранения всех экземпляров облаков
        cloudInstances = new GameObject[numClouds];
        //Найти родительский игровой объект CloudAnchor 
        GameObject anchor = GameObject.Find("CloudAnchor");
        //Создать в цикле заданное количество облаков
        GameObject cloud;
        for (int i = 0; i < numClouds; i++)
        {
            //Создать экземпляр cloudPrefab
            cloud = Instantiate<GameObject>(cloudPrefab);
            //Выбрать местоположение для облака
            Vector3 cPos = Vector3.zero;
            cPos.x = Random.Range(cloudPosMin.x, cloudPosMin.y);
            cPos.y = Random.Range(cloudPosMin.x, cloudPosMin.y);
            //Масштабировать облако
            float scaleU = Random.value;
            float scaleVal = Mathf.Lerp(cloudScaleMin, cloudScaleMax, scaleU);
            //Меньшие облака (с меньшим значением scaleU) должны быть ближе к земле
            cPos.y = Mathf.Lerp(cloudPosMin.y, cPos.y, scaleU);
            //Меньшие облака должны быть дальше
            cPos.z = 100 - 90 * scaleU;
            //Применить полученные значения координат и масштаба к облаку
            cloud.transform.position = cPos;
            cloud.transform.localScale = Vector3.one * scaleVal;
            //Сделать облако дочерним по отношению к anchor
            cloud.transform.SetParent(anchor.transform);
            //добавить облако в массив cloudInstances
            cloudInstances[i] = cloud;

        }
    }

    void Update()
    {
        //обойти в цикле все созданные облака
        foreach(GameObject cloud in cloudInstances)
        {
            //Получить масштаб и координаты облака
            float scaleVal = cloud.transform.localScale.x;
            Vector3 cPos = cloud.transform.position;
            //увеличить скорость для ближних облаков
            cPos.x -= scaleVal * Time.deltaTime * cloudSpeedMult;
            //Если облако сместилось слишком далеко влево
            //то переместить его далеко вправо
            if (cPos.x <= cloudPosMin.x) cPos.x = cloudPosMax.x;
            //применить новые координаты
            cloud.transform.position = cPos;

        }
        
    }
}
