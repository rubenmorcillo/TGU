using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms;

public class Localizaciones : MonoBehaviour
{
    // Start is called before the first frame update
    private enum Localizacion  {SHOP, CORP, LAB}

    private string localizacion;
    private Localizacion thisLocalizacion;
    void Start()
    {
        localizacion = gameObject.name.ToUpper();
        if (localizacion == Localizacion.CORP.ToString())
		{
            thisLocalizacion = Localizacion.CORP;
		}
        else if (localizacion == Localizacion.SHOP.ToString())
        {
            thisLocalizacion = Localizacion.SHOP;
        } 
        else if (localizacion == Localizacion.LAB.ToString())
        {
            thisLocalizacion = Localizacion.LAB;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	private void OnMouseOver()
	{
        GetComponent<MeshRenderer>().material.color = Color.green;
        // Debug.Log("ir a " +gameObject.name);

        switch (thisLocalizacion)
		{
            case Localizacion.CORP:
                Debug.Log("nos vamos a las CORPOS");
				if (Input.GetMouseButton(0))
				{
                    SceneManager.LoadScene("mazmorra_01");
					GameManager.instance.iniciarMazmorra();
				}
                return;
            case Localizacion.SHOP:
                Debug.Log("nos vamos a la SHOP");
                return;
            case Localizacion.LAB:
                Debug.Log("nos vamos al LAB");
                return;
		}
    }

	private void OnMouseExit()
	{
        GetComponent<MeshRenderer>().material.color = Color.white;
	}
}
