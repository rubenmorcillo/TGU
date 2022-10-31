using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DatosUnidad
{
	public DatosUnidad()
	{

	}
	public DatosUnidad(int id, TipoUnidad tipo, string name, int rangoMovimiento, int hp, int iniciativa)
	{
		this.id = id;
		this.tipo = tipo;
		alias = name;
		hpMax = hp;
		hpActual = hpMax;
		this.rangoMovimiento = rangoMovimiento;
		accionRealizada = false;
		movimientoRealizado = false;
		modelPrefabName = tipo.nombre;
		exp = 0;
		nivel = 1;
		this.iniciativa = iniciativa;
		this.agilidad = 3;
		this.velocidad = 10;
		IniciarCoberturas();
	}

	public int id;

    public string alias;

    public TipoUnidad tipo;

    public int nivel;

    public int exp;

    public int hpMax;

    public int hpActual;

	public int poder;

	//public int atqEspecial;

    public string modelPrefabName;

    public int iniciativa;

	public int agilidad;

	public float punteria;

	public int velocidad;

    public int defensa;

	public int rangoMovimiento;

	public bool movimientoRealizado;

	public bool accionRealizada;

    public int hab1, hab2, hab3, hab4;

    public bool isPlaced { get; set; }

    public bool estoyVivo = true;

	public List<CoverageUnidad> coberturas;

    public Habilidad Hab1
	{
		get
		{
			if (hab1 > 0)
			{
				Habilidad h = GameManager.instance.BDlocal.Habilidades.First(datos => datos.id == hab1);
				return h;
			}
			else
			{
				return null;
			}
			
		}
	}
	public Habilidad Hab2
	{
		get
		{
			if(hab2 > 0)
			{
				return GameManager.instance.BDlocal.Habilidades.Where(datos => datos.id == hab2).First();
			}
			else
			{
				return null;
			}
			
		}
	}
	public Habilidad Hab3
	{
		get
		{
			if (hab3 > 0)
			{
				return GameManager.instance.BDlocal.Habilidades.Where(datos => datos.id == hab3).First();
			}
			else
			{
				return null;
			}
		}
	}
	public Habilidad Hab4
	{
		get
		{
			if (hab4 > 0)
			{
				return GameManager.instance.BDlocal.Habilidades.Where(datos => datos.id == hab4).First();
			}
			else
			{
				return null;
			}
		}
	}
	public List<EstadoAlterado> estados_alterados;
	//lista de estados alterados

	void IniciarCoberturas()
	{
		coberturas = new List<CoverageUnidad>();
		coberturas.Add(new CoverageUnidad(Vector3.forward, CoverageUnidad.CoverageType.NADA));
		coberturas.Add(new CoverageUnidad(-Vector3.forward, CoverageUnidad.CoverageType.NADA));
		coberturas.Add(new CoverageUnidad(Vector3.right, CoverageUnidad.CoverageType.NADA));
		coberturas.Add(new CoverageUnidad(-Vector3.right, CoverageUnidad.CoverageType.NADA));

	}
	public CoverageUnidad.CoverageType TraducirCobertura(Coverage.CoverageType type)
	{
		switch (type)
		{
			case Coverage.CoverageType.COBERTURA_MEDIA:
				return CoverageUnidad.CoverageType.COBERTURA_MEDIA;
			case Coverage.CoverageType.COBERTURA_COMPLETA:
				return CoverageUnidad.CoverageType.COBERTURA_COMPLETA;
			default:
				return CoverageUnidad.CoverageType.NADA;
		}
	}

	[Serializable]
	public class EstadoAlterado
	{
		public enum Estado { }

		public Estado estado;
		public int turnos_efectividad;
		public EstadoAlterado(Estado estado, int turnos)
		{
			this.estado = estado;
			turnos_efectividad = turnos;
			//TODO: comprobar que se acaba el efecto, cada unidad debe restarle y comprobar cada vez que finalice turno(?)
		}
	}
	[Serializable]
	public class CoverageUnidad
	{

		public enum directionName {NORTE, SUR, ESTE, OESTE }
		public Vector3 direction;
		public CoverageType type;
		public enum CoverageType { COBERTURA_MEDIA, COBERTURA_COMPLETA, NADA}
		public CoverageUnidad(Vector3 direction, CoverageType type)
		{
			this.direction = direction;
			this.type = type;
		}
		public directionName InvertirDireccion(directionName direccion)
		{
			directionName direccionInvertida = new directionName();
			switch (direccion)
			{
				case directionName.ESTE:
					direccionInvertida = directionName.OESTE;
					break;
				case directionName.OESTE:
					direccionInvertida = directionName.ESTE;
					break;
				case directionName.NORTE:
					direccionInvertida = directionName.SUR;
					break;
				case directionName.SUR:
					direccionInvertida = directionName.NORTE;
					break;
			}
			return direccionInvertida;
		}
	}
	public void RestorePoints()
	{
		movimientoRealizado = false;
		accionRealizada = false;
	}

	public void PerderVida(int cantidad)
	{
		hpActual -= cantidad;
		if (hpActual <= 0)
		{
			estoyVivo = false;
		}
	}

	public override string ToString()
	{
		return "Soy un "+tipo.nombre + "// level " + nivel + "\n "+
            "hp: "+hpActual+"/" + hpMax + "\n"+
            "hab1: "+Hab1 + " // hab2: "+hab2 + " // hab3: "+hab3+" // hab4: " +hab4;
	}
}
