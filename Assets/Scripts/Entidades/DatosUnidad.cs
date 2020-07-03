using System;
using System.Linq;


[Serializable]
public class DatosUnidad
{

    
    public DatosUnidad()
    {
        
    }
	public DatosUnidad(int id, TipoUnidad tipo, string name, int rangoMovimiento, int hp)
	{
		this.id = id;
		this.tipo = tipo;
		alias = name;
		hpMax = hp;
		hpActual = hpMax;
		this.rangoMovimiento = rangoMovimiento;
		modelPrefabName = tipo.nombre;
		exp = 0;
		nivel = 1;
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

    public int rangoMovimiento = 3;

    public int hab1 = 1, hab2, hab3, hab4;

    public bool isPlaced { get; set; }

    public bool estoyVivo = true;

    public Habilidad Hab1
	{
		get
		{
			return GameManager.instance.BDlocal.Habilidades.Where(datos => datos.id == hab1).First();
		}
	}
	public Habilidad Hab2
	{
		get
		{
			return GameManager.instance.BDlocal.Habilidades.Where(datos => datos.id == hab1).First();
		}
	}
	public Habilidad Hab3
	{
		get
		{
			return GameManager.instance.BDlocal.Habilidades.Where(datos => datos.id == hab1).First();
		}
	}
	public Habilidad Hab4
	{
		get
		{
			return GameManager.instance.BDlocal.Habilidades.Where(datos => datos.id == hab1).First();
		}
	}



	public override string ToString()
	{

		return "Soy un "+tipo.nombre + "// level " + nivel + "\n "+
            "hp: "+hpActual+"/" + hpMax + "\n"+
            "hab1: "+Hab1.nombre + " // hab2: "+hab2 + " // hab3: "+hab3+" // hab4: " +hab4;
	}
}
