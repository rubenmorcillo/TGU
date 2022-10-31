using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ComportamientoNPC
{
	public enum ComportamientoNPCEnum { DEFAULT = 0}

	ComportamientoNPCEnum tipoComportamiento;
	DatosUnidad datosUnidad;
    
	public ComportamientoNPC()
	{
		tipoComportamiento = ComportamientoNPCEnum.DEFAULT;
	}

	public ComportamientoNPC(DatosUnidad datosUnidad)
	{
		this.datosUnidad = datosUnidad;
	}
	
	public ComportamientoNPC(DatosUnidad datosUnidad, ComportamientoNPCEnum tipoComportamiento)
	{
		this.datosUnidad = datosUnidad;
		this.tipoComportamiento = tipoComportamiento;
	}

	public DatosUnidad DatosUnidad
	{
		get
		{
			return datosUnidad;
		}
		set
		{
			datosUnidad = value;
		}
	}

	public ComportamientoNPCEnum TipoComportamiento
	{
		get
		{
			return tipoComportamiento;
		}
		set
		{
			tipoComportamiento = value;
		}
	}


	public void DecidirSiguienteAccion(DatosUnidad datosUnidad)
	{
		this.datosUnidad = datosUnidad;
		switch (tipoComportamiento)
		{
			case ComportamientoNPCEnum.DEFAULT:

				return;

		}
		
	}

	List<Habilidad> ObtenerHabilidadesDisponibles()
	{
		List<Habilidad> habilidades = new List<Habilidad>();
		if (GameManager.instance.mostrarDebug) Debug.Log("Obtener habilidades disponibles para " + datosUnidad.alias);
		if (datosUnidad.Hab1 != null)
		{
			habilidades.Add(datosUnidad.Hab1);
		}
		if (datosUnidad.Hab2 != null)
		{
			habilidades.Add(datosUnidad.Hab2);
		}
		if (datosUnidad.Hab3 != null)
		{
			habilidades.Add(datosUnidad.Hab3);
		}
		if (datosUnidad.Hab4 != null)
		{
			habilidades.Add(datosUnidad.Hab4);
		}

		List<Habilidad> habilidadesDisponibles = new List<Habilidad>();
		habilidadesDisponibles = habilidades;

		return habilidadesDisponibles;
	}

	public Habilidad SeleccionarHabilidad()
	{
		if (GameManager.instance.mostrarDebug) Debug.Log("Voy a seleccionar una habilidad");
		Habilidad habilidadSeleccionada = new Habilidad();
		foreach (Habilidad skill in ObtenerHabilidadesDisponibles()){
			habilidadSeleccionada = skill;
		}
		if (habilidadSeleccionada.id == 0)
		{
			habilidadSeleccionada = GameManager.instance.BDlocal.Habilidades.First();
		}
		if (GameManager.instance.mostrarDebug) Debug.Log("he seleccionado la habilidad " + habilidadSeleccionada.id);
		return habilidadSeleccionada;
	}
}
