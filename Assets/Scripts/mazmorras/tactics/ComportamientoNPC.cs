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
	
	public ComportamientoNPC(ComportamientoNPCEnum tipoComportamiento)
	{
		this.tipoComportamiento = tipoComportamiento;
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
		if (habilidades.Count > 0)
		{
			habilidadesDisponibles = habilidades.Where(h => h.esfuerzo < datosUnidad.puntosEsfuerzoActual).ToList<Habilidad>();
		}

		return habilidadesDisponibles;
	}

	public Habilidad SeleccionarHabilidad()
	{
		Habilidad habilidadSeleccionada = new Habilidad();
		foreach (Habilidad skill in ObtenerHabilidadesDisponibles()){
			habilidadSeleccionada = skill;
		}

		return habilidadSeleccionada;
	}
}
