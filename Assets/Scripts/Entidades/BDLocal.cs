﻿using System.Collections.Generic;
using UnityEngine;
using System;


public class BDLocal
{
	List<TipoUnidad> tiposUnidad = new List<TipoUnidad>();
	List<Habilidad> habilidades = new List<Habilidad>();

	public List<TipoUnidad> TiposUnidad
	{
		get
		{
			return tiposUnidad;
		}
		set
		{
			tiposUnidad = value;
		}
	}

	public void Init()
	{
		Debug.Log("BDLocal: iniciando...");
		CargarUnidades();
		CargarHabilidades();
		
	}

	//CHAPUZAAA -> la string de las peticiones debería guardarla en algún sitio
	void CargarUnidades()
	{
		//llamar a la BD para consultar todas las unidades
		GameManager.instance.serverManager.ObtenerUnidades();
	}



	void CargarHabilidades()
	{
		//llamar a la BD para consultar todas las habilidades
		GameManager.instance.serverManager.PeticionGet("https://resealable-compress.000webhostapp.com/habilidades.php");
	}
}
