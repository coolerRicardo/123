using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileLine : MonoBehaviour
{
    static public ProjectileLine S; //ќдиночка
    [Header("Set in Inspector")]
    public float minDist = 0.1f;

    private LineRenderer line;
    private GameObject _poi;
    private List<Vector3> points;

    private void Awake()
    {
        S = this; //”становить ссылку на объект одиночку
        //получить ссылку на LineRenderer
        line = GetComponent<LineRenderer>();
        //выключить LineRenderer, пока он не понадобитьс€
        line.enabled = false;
        //инициализировать список точек
        points = new List<Vector3>();
    }
    //Ёто свойство (то есть метод, маскирующийс€ под поле)
    public GameObject poi
    {
        get { return (_poi); }
        set
        {
            _poi = value;
            if (_poi != null)
            {
                //если поле _poi содержит действительную ссылку
                //сбросить все остальные параметры в исходное состо€ние
                line.enabled = false;
                points = new List<Vector3>();
                AddPoint();

            }
        }
    }

    //этот метод можно вызвать непосредственно, чтобы стереть линию
    public void Clear()
    {
        _poi = null;
        line.enabled = false;
        points = new List<Vector3>();

    }

    public void AddPoint()
    {
        //вызываетс€ дл€ добавлени€ точки в линии
        Vector3 pt = _poi.transform.position;
        if (points.Count > 0 && (pt - lastPoint).magnitude < minDist)
        {
            //если точка недостаточно далека от предыдущей, просто выйти
            return;

        }
        if (points.Count == 0)
        {   //если это точка запуска 
            Vector3 launchPosDiff = pt - Slingshot.LAUNCH_POS; //дл€ определени€
            //добавить дополнительный фрагмент линии 
            //чтобы помочь лучше прицелитьс€ в будущем
            points.Add(pt + launchPosDiff);
            points.Add(pt);
            line.positionCount = 2;
            //установить первые две точки
            line.SetPosition(0, points[0]);
            line.SetPosition(1, points[1]);
            //включить LineRender
            line.enabled = true;
        }
        else
        {
            //обычна€ последовательность добавлени€ точки
            points.Add(pt);
            line.positionCount = points.Count;
            line.SetPosition(points.Count - 1, lastPoint);
            line.enabled = true;
        }
    }

    //возвращает местоположение последней добавленной точки
    public Vector3 lastPoint
    {
        get
        {
            if (points == null)
            {
                //если точек нет, вернуть Vector3.zero
                return (Vector3.zero);
            }
            return (points[points.Count - 1]);
        }
    }

    void FixedUpdate()
    {
        if (poi == null)
        {
            //≈сли свойство poi содержит пустое значение, найти интересующий объект
            if (FollowCam.POI != null)
            {
                if (FollowCam.POI.tag == "Projectile")
                {
                    poi = FollowCam.POI;
                }
                else
                {
                    return; // выйти, если интересующий объект не найден
                }
            }
            else
            {
                return; // выйти, если интересующий объект не найден
            }
        }
        //если интересующий объект найден
        //попытатьс€ добавить точку с его координатами в каждом FixedUpdate
        AddPoint();
        if (FollowCam.POI == null)
        {
            //если FollowCam.POI содержить null, записать null в poi
            poi = null;
        }

    }
}



