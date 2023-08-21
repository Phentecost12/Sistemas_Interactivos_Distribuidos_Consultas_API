using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Manager : MonoBehaviour
{
    [SerializeField] private List<GameObject> panels;

    public void ActivatePanel(int i)
    {
        if(panels[i].activeInHierarchy)
        {
            panels[i].SetActive(false);
        }
        else
        {
            panels[i].SetActive(true);
        }
    }
}
