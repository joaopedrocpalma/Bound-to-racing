using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenPanel : MonoBehaviour
{
    public GameObject pickMenu;
    public GameObject hidePanel;
    public GameObject hidePanel2;
    public GameObject hidePanel3;

    public void Open_Panel()
    {
        if(pickMenu != null)
        {
            pickMenu.SetActive(true);
        }
    }

    public void Close_Panel()
    {
        if (pickMenu == null)
        {
            pickMenu.SetActive(false);
        }
    }

    public void Hide_Panel()
    {
        hidePanel.SetActive(false);
    }

    public void Hide_Panel2()
    {
        hidePanel2.SetActive(false);
    }

    public void Hide_Panel3()
    {
        hidePanel3.SetActive(false);
    }
}
