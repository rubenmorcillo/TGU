
using System;
using System.Data;

[Serializable]
public class TipoUnidad {

    public TipoUnidad()
	{

	}

    public TipoUnidad(int id, string nombre, int hp_base, int movimiento_base, int atq1, int atq2, int def1, int def2, int agilidad)
	{
        this.id = id;
        this.nombre = nombre;
        this.hp_base = hp_base;
        this.atq_fisico = atq1;
        this.atq_especial = atq2;
        this.def_fisico = def1;
        this.def_especial = def2;
        this.agilidad = agilidad;
        this.movimiento_base = movimiento_base;
	}
    public int id { get; set; }
    public string nombre { get; set; }
    public int hp_base { get; set; }
    public int movimiento_base { get; set; }
    public int salto_base { get; set; }
    public int atq_fisico { get; set; }
    public int atq_especial { get; set; }
    public int def_fisico { get; set; }
    public int def_especial { get; set; }
    public int agilidad { get; set; }

}
