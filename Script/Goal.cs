using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    static public bool goalMet = false;

    private void OnTriggerEnter(Collider other)
    {
        //когда в область действия триггера попадает что то
        //проверить, является ли это что то снарядом
        if(other.gameObject.tag == "Projectile")
        {
            //Если это снаряд, присвоить полю goalMet значение true 
            Goal.goalMet = true;
            //также изменить альфа канал цвета, чтобы увеличить непрозрачность
            Material mat = GetComponent<Renderer>().material;
            Color c = mat.color;
            c.b = 1;
            mat.color = c;
        }
    }

}
