using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    private GameObject selectedGameObject ;

    private void Awake()
    {
        selectedGameObject = transform.Find("Selected").gameObject;
        setSelectedVisible(false);
    }


    //methods
    public void setSelectedVisible(bool visible)
    {
        selectedGameObject.SetActive(visible);
    }

    
}
 