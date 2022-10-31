using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class AccionUnidad
{
	public enum AccionUnidadType { MOVER, SKILL, GUARDIA, VIGILANCIA }
	public AccionUnidadType type;
	public AccionUnidad(){}
	public AccionUnidad(AccionUnidadType type)
	{
		this.type = type;
	}
}
