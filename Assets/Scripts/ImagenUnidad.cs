using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ImagenUnidad : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	DatosUnidad unidad = null;

	public DatosUnidad Unidad
	{
		get
		{
			return unidad;
		}
		set
		{
			unidad = value;
		}
	}

	public bool isOver = false;

	public void OnPointerEnter(PointerEventData eventData)
	{
		//Debug.Log("Ratón sobre " + unidad.tipo.nombre);
		if (unidad != null)
		{
			isOver = true;
		}

	
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		//Debug.Log("Mouse fuera");
		isOver = false;
	}

}
