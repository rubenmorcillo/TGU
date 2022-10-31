using System;
using System.Collections.Generic;

[Serializable]
public class TipoUnidad {

	public TipoUnidad(int id, string nombre, int hp_base, int rangoMovimiento_base, int poder, int defensa, int agilidad, int iniciativa, int velocidad)
	{
		this.id = id;
		this.nombre = nombre;
		this.hp_base = hp_base;
		this.poder_base = poder;
		this.defensa_base = defensa;
		this.agilidad_base = agilidad;
		this.iniciativa_base = iniciativa;
		this.velocidad_base = velocidad;
		this.rangoMovimiento_base = rangoMovimiento_base;

	}
	public int id;
    public string nombre;
    public int hp_base, poder_base, defensa_base, salto_base, rangoMovimiento_base, agilidad_base, iniciativa_base, velocidad_base;
	public float punteria_base;
	

}
