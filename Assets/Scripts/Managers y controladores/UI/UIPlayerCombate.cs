using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayerCombate : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    TestTacticsMove unidadActiva;
    
    Habilidad hab1, hab2, hab3, hab4;
    

    [SerializeField]
    GameObject btnHab1, btnHab2, btnHab3, btnHab4;
    [SerializeField]
    Text txtMovimiento, txtEsfuerzo;
    [SerializeField]
    Text txtHab1, txtHab2, txtHab3, txtHab4;

    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        if (unidadActiva != null)
		{
            MostrarStats();
            MostrarSkills();
		}
       
    }
    public void DeseleccionarUnidad()
	{
        unidadActiva = null;
	}
    public void SeleccionarUnidad(TestTacticsMove unidad)
	{
        Debug.Log("UI BATALLA: seleccionando: " + unidadActiva.datosUnidad.alias);
        unidadActiva = unidad;
	}

    public void MostrarStats()
	{
        txtMovimiento.text = unidadActiva.datosUnidad.puntosMovimientoActual.ToString();
        txtEsfuerzo.text = unidadActiva.datosUnidad.puntosEsfuerzoActual.ToString();
	}

    public void MostrarSkills()
	{
        if (unidadActiva.datosUnidad.Hab1 != null)
		{
            txtHab1.text = unidadActiva.datosUnidad.Hab1.nombre;

        }
        if (unidadActiva.datosUnidad.Hab2 != null)
		{
            txtHab2.text = unidadActiva.datosUnidad.Hab2.nombre;

        }
        if (unidadActiva.datosUnidad.Hab2 != null)
		{
            txtHab3.text = unidadActiva.datosUnidad.Hab3.nombre;

        }
        if (unidadActiva.datosUnidad.Hab2 != null)
		{
            txtHab4.text = unidadActiva.datosUnidad.Hab4.nombre;
        }
    }

}
