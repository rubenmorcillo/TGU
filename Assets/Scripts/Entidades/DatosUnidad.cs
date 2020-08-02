using System;
using System.Linq;


[Serializable]
public class DatosUnidad
{

    
    public DatosUnidad()
    {
        
    }
	public DatosUnidad(int id, TipoUnidad tipo, string name, int puntosMovimientoTotal, int hp, int iniciativa)
	{
		this.id = id;
		this.tipo = tipo;
		alias = name;
		hpMax = hp;
		hpActual = hpMax;
		this.puntosMovimientoTotal = puntosMovimientoTotal;
		modelPrefabName = tipo.nombre;
		exp = 0;
		nivel = 1;
		this.iniciativa = iniciativa;
	}

	public int id;

    public string alias;

    public TipoUnidad tipo;

    public int nivel;

    public int exp;

    public int hpMax;

    public int hpActual;

    public string modelPrefabName;

    public int iniciativa;

    public int defensaCerca;

    public int defensaLejos;

    public int puntosMovimientoTotal = 3;

	public int puntosMovimientoActual;

	public int puntosEsfuerzoTotal = 3;

	public int puntosEsfuerzoActual;

    public int hab1 = 1, hab2, hab3, hab4;

    public bool isPlaced { get; set; }

    public bool estoyVivo = true;

    public Habilidad Hab1
	{
		get
		{
			if (hab1 > 0)
			{
				return GameManager.instance.BDlocal.Habilidades.Where(datos => datos.id == hab1).First();
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

	public void RestorePoints()
	{
		puntosMovimientoActual = puntosMovimientoTotal;
		puntosEsfuerzoActual = puntosEsfuerzoTotal;
	}

	public void SubstractMovementPoints(int n)
	{
		puntosMovimientoActual -= n;
	}

	public void SubstractEffortPoints(int n)
	{
		puntosEsfuerzoActual -= n;
	}

	public override string ToString()
	{

		return "Soy un "+tipo.nombre + "// level " + nivel + "\n "+
            "hp: "+hpActual+"/" + hpMax + "\n"+
            "hab1: "+Hab1.nombre + " // hab2: "+hab2 + " // hab3: "+hab3+" // hab4: " +hab4;
	}
}
