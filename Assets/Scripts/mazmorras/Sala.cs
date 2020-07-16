using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Dynamic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class Sala : MonoBehaviour
{
    public GameObject puntoUnion;
    NavMeshSurface navSur;
    NavMeshSurface[] navMeshSuelo;

    Collection<int> posiblesEnemigosIds = new Collection<int>();
	List<DatosUnidad> datosEnemigos = new List<DatosUnidad>();

    void Start()
    {
        Debug.Log(name +": iniciandome... ");
        navSur = GetComponent<NavMeshSurface>();
        GenerarPosiblesEnemigos();
       // navMeshSuelo = GetComponentsInChildren<NavMeshSurface>();
       //updateNavMesh();


    }

    public void updateNavMesh()
    {
        Debug.Log(name +": Redibujando navegación... ");
        //foreach (NavMeshSurface surfa in navMeshSuelo)
        //{
        //    surfa.BuildNavMesh();
        //}
        navSur.BuildNavMesh();
        
    }

    public Mesh getGeneralMesh()
    {
        MeshFilter[] meses = gameObject.GetComponentsInChildren<MeshFilter>();
        // Debug.Log("tenemos meshes -> " + meses.Length);
        Mesh finalMesh = new Mesh();

        CombineInstance[] combiners = new CombineInstance[meses.Length];

        for (int i = 0; i < meses.Length; i++)
        {
            combiners[i].subMeshIndex = 0;
            combiners[i].mesh = meses[i].sharedMesh;
            combiners[i].transform = meses[i].transform.localToWorldMatrix;
        }

        finalMesh.CombineMeshes(combiners);

        return finalMesh;
    }

     public List<Tile> PuntosInicioEnemigo()
    {
        List<Tile> lista = new List<Tile>();
        foreach (Tile t in gameObject.GetComponentsInChildren<Tile>())
        {
            if (t.spawnEnemigo)
            {
                lista.Add(t);
            }
        }
        return lista;
    }

    public List<Tile> PuntosInicioPlayer()
    {
        List<Tile> lista = new List<Tile>();
        foreach (Tile t in gameObject.GetComponentsInChildren<Tile>())
        {
            if (t.spawnUnidad)
            {
                lista.Add(t);
            }
        }
        return lista;
    }


    void GenerarPosiblesEnemigos()
	{
        //CHAPUZAAAA -> falseo los IDS de los posibles enemigos 
        //-> en un futuro, según el nivel de dificultad, o el numero de salas q lleves...podemos cargar unos enemigos u otros
        posiblesEnemigosIds.Add(1);
		posiblesEnemigosIds.Add(2); //probar con un ID q no exista
        posiblesEnemigosIds.Add(4);
        posiblesEnemigosIds.Add(3);
        posiblesEnemigosIds.Add(5);


        for (int i = 0; i < posiblesEnemigosIds.Count; i++)
		{
            //CHAPUZAAA -> el id debería ser un ID local, pero entonces tb debo cambiarlo en las unidades aliadas
            //-> en un futuro, según el nivel de dificultad....
            DatosUnidad uni = new DatosUnidad(999, GameManager.instance.BDlocal.TiposUnidad.Where(tu => tu.id == posiblesEnemigosIds[i]).First(), "enemigo" + i, 3, 100, Random.Range(1,2));
			datosEnemigos.Add(uni);
		}

	}


    public List<DatosUnidad> dameEnemigos(int n)
    {
        List<DatosUnidad> enemigos = new List<DatosUnidad>();
        for (int i = 0; i<n; i++)
        {
            enemigos.Add(datosEnemigos[Random.Range(0, datosEnemigos.Count )]);
        }
        
        return enemigos;
    }

    public void encontrarCasillasDisponibles()
    {
        foreach (Tile t in PuntosInicioPlayer())
        {
            Debug.DrawRay(t.transform.position, Vector3.up, Color.magenta);

            int lm = ~(1 << 10); 
            if (!Physics.Raycast(t.transform.position - new Vector3(0,0.5f,0), Vector3.up, 4.0f, lm)) //4 por asegurar
            {
                t.selectable = true;
            }
            else
            {
                t.selectable = false;
            }
        }
    }




}
