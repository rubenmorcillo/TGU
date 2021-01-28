using System;

[Serializable]
public class Habilidad 
{
	public int id;

	public string nombre;

	public int potencia;

	public int esfuerzo;

	public enum TipoHabilidad { FISICO, ESPECIAL, CURA }

	public TipoHabilidad tipo;

	public string desc;

	public int rango;

	public TipoRango tipoRango;

	public override string ToString()
	{
		return nombre + "\n Tipo " + tipo.ToString() + "\n  Potencia: " + potencia + "\n  Coste: " + esfuerzo + "\n  Tipo de ataque: " + tipoRango.ToString() + "\n  Rango: " + rango;
	}

	public enum TipoRango {RECTO = 1,AREA = 2, RANGO = 3}

}
