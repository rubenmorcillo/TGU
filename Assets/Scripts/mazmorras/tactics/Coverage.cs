using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static DatosUnidad.CoverageUnidad;

public class Coverage : MonoBehaviour
{
    public enum CoverageType { COBERTURA_MEDIA, COBERTURA_COMPLETA, NADA }
    public CoverageType type;

	public HashSet<directionName> currentOrientationCoverage;

	public bool covering;

	public void Start()
	{
		currentOrientationCoverage = new HashSet<directionName>();
	}

	void UpdateTileCoverage(Vector3 direccion,Vector3 halfExtents)
	{
		Collider[] colliders = Physics.OverlapBox(gameObject.transform.position + new Vector3(0, 0.5f, 0) + direccion / 2, halfExtents);
		foreach (Collider item in colliders)
		{
			Debug.Log("soy el muro  "+name +" y tengo al lado " + item.name);
			if (item.GetComponent<Tile>() != null)
			{

				//item.GetComponent<Tile>()
			}
		}
	}
	private void Update()
	{
	}

	public void ClearOrientationCoverages()
	{
		//se debe llamar cada vez que se cambia de personaje? NOP
		currentOrientationCoverage.Clear();
	}

	void CheckOrientationCoverage(int xWall, int zWall, int xObjetivo, int zObjetivo)
	{
		if (transform.localScale.z >= 1)
		{
			if (xObjetivo > xWall)
			{
					currentOrientationCoverage.Add(directionName.OESTE);
			}
			else
			{
					currentOrientationCoverage.Add(directionName.ESTE);
			}
			
		}
		
		if (transform.localScale.x >= 1)
		{
			if (zObjetivo > zWall)
			{
				//muro abajo, objetivo arriba
					currentOrientationCoverage.Add(directionName.SUR);
			}
			else
			{
					currentOrientationCoverage.Add(directionName.NORTE);
			}
			
		}
		
	}

	public void UpdateOrientationCoverages(GameObject unidadActiva, List<TestTacticsMove> visibleTargets)
	{
		ClearOrientationCoverages();
		int xWall = (int)transform.position.x;
		int zWall = (int)transform.position.z;
		if (unidadActiva.gameObject.transform != null)
		{
			//Debug.Log("tenemos " + visibleTargets.Count + " visibles targets");
			foreach (TestTacticsMove visibleTarget in visibleTargets)
			{
				int xObjetivo = (int)visibleTarget.transform.position.x;
				int zObjetivo = (int)visibleTarget.transform.position.z;
				CheckOrientationCoverage(xWall, zWall, xObjetivo, zObjetivo);
			}
		}
	}
}


