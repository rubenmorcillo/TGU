using System;

[Serializable]
public class Habilidad 
{
	public enum TipoHabilidad {FISICO, ESPECIAL }

	public int id;

	public string nombre;

	public int potencia;

	public int esfuerzo;

	public TipoHabilidad tipo;

	public string desc;

	public override string ToString()
	{
		return nombre + "\n Tipo " + tipo.ToString() + "\n  Potencia: " + potencia + "\n  Coste: " + esfuerzo;
	}

}
