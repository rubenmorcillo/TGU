using System;

[Serializable]
public class TipoUnidad {

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
	public int id;
    public string nombre;
    public int hp_base, atq_fisico, atq_especial, def_fisico, def_especial, salto_base, movimiento_base, agilidad;

}
