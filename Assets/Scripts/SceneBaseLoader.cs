using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneBaseLoader : MonoBehaviour
{
    public GameObject menuEsc;
    void Start()
    {
        Debug.Log("SL_Base: START...");
        TecladoController teclado = GameManager.instance.gameObject.AddComponent<TecladoController>();
       
        menuEsc = GameObject.Find("menuEsc");
        teclado.menuEsc = menuEsc;
        menuEsc.SetActive(false);
        
    }

}
