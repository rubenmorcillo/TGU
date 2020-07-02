using System;

[Serializable]
public class Habilidad 
{
	public enum TipoHabilidad {FISICO, ESPECIAL }

	public int id;

	public string nombre;

	public int potencia;

	public TipoHabilidad tipo;

}
