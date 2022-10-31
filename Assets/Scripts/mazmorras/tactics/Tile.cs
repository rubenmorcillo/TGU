using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static DatosUnidad.CoverageUnidad;


public class Tile : MonoBehaviour 
{
    public bool walkable = true;
    public bool current = false;
    public bool target = false;
    public bool selectable = false;
    public bool spawnEnemigo = false;
    public bool spawnUnidad = false;
    public bool showDebugTile = false;

    public List<Tile> adjacencyList = new List<Tile>();
    public List<directionName> coveragesDirections;
    public List<directionName> coveragesValidDirections;
    public Dictionary<directionName, Coverage.CoverageType> currentCoverage = new Dictionary<directionName, Coverage.CoverageType>();


    public List<directionName> enemiesInverseDirections;

    //Needed BFS (breadth first search)
    public bool visited = false;
    public Tile parent = null;
    public int distance = 0;

    //For A*
    public float f = 0;
    public float g = 0;
    public float h = 0;

	// Use this for initialization
	void Start () 
	{
        gameObject.AddComponent<NavMeshSurface>();
        coveragesDirections = new List<directionName>();
        coveragesValidDirections = new List<directionName>();
        enemiesInverseDirections = new List<directionName>();
	}
	
	// Update is called once per frame
	void Update () 
	{
       
            if (current)
            {
                GetComponent<Renderer>().material.color = Color.cyan;
            }
            else if (target)
            {
                GetComponent<Renderer>().material.color = Color.green;
            }
            else if (selectable && walkable)
            {
                GetComponent<Renderer>().material.color = Color.blue;
            }
            else
            {
                GetComponent<Renderer>().material.color = Color.white;
            }
        if (GameManager.instance.estadoJuego == EstadosJuego.Estado.COMBATE)
		{
            CheckEnemiesRelativeOrientation();
            if (showDebugTile) DibujarCoberturasTemp(); //CHAPUZAAA esto es temporal para ver si funciona, reemplazar por gui.
        }

    }

    public void Reset()
    {
        adjacencyList.Clear();
        coveragesDirections.Clear();
        coveragesValidDirections.Clear();
        current = false;
        target = false;
        selectable = false;

        visited = false;
        parent = null;
        distance = 0;

        f = g = h = 0;
    }

    public void FindNeighbors(float jumpHeight, Tile target, Habilidad habilidad, bool move)
    {
        Reset();
		//antes de comprobar vecinos debo comprobar colisiones, y solo buscar tiles si no hay pared en medio
        if (!CheckWall(Vector3.forward, jumpHeight, target))
		{
            CheckTile(Vector3.forward, jumpHeight, target, habilidad, move);
        }
        if (!CheckWall(-Vector3.forward, jumpHeight, target))
        {
            CheckTile(-Vector3.forward, jumpHeight, target, habilidad, move);
        }
        if (!CheckWall(Vector3.right, jumpHeight, target))
        {
            CheckTile(Vector3.right, jumpHeight, target, habilidad, move);
        }
        if (!CheckWall(-Vector3.right, jumpHeight, target))
        {
            CheckTile(-Vector3.right, jumpHeight, target, habilidad, move);

        }
    }


    public void CheckTile(Vector3 direction, float jumpHeight, Tile target, Habilidad habilidad, bool move)
    {
        Vector3 halfExtents = new Vector3(0.25f, (1 + jumpHeight) / 2.0f, 0.25f);
        Collider[] colliders = Physics.OverlapBox(transform.position + direction, halfExtents);
        foreach (Collider item in colliders)
        {
            Tile tile = item.GetComponent<Tile>();
            //CHAPUZAAA: con la última condicion...los ataques de los enemigos no pueden traspasar muros??? y eso está MAL
            if ((tile != null && tile.walkable) || (tile != null && !move))
            {
                if (move)
				{
                    RaycastHit hit;
                    if (!Physics.Raycast(tile.transform.position, Vector3.up, out hit, 1) || (tile == target))
                    {
                        adjacencyList.Add(tile);
                    }
					else
					{
                        //TODO: si quiero disparar a los obstaculos tengo que ver qué hay encima del Tile
                    }
                }
				else
				{
                    adjacencyList.Add(tile);
				}
               
            }
        }
    }

    bool CheckWall(Vector3 direction, float jumpHeight, Tile target)
    {
        bool wall = false;
        //Vector3 halfExtents = new Vector3(0.25f, (0.5f + jumpHeight) / 2.0f, 0.25f);
        Vector3 halfExtents = new Vector3(0.01f, 0.01f, 0.01f);
        //Debug.DrawRay(transform.position + direction/2, Vector3.up, Color.green);
        //Collider[] colliders = Physics.OverlapBox(transform.position + direction / 2, halfExtents);
        RaycastHit hiteo;
        Vector3 correccionAltura = new Vector3(0, 0.25f, 0);
        
        if (showDebugTile) Debug.DrawRay(transform.position + correccionAltura, direction, Color.red, 10);

        
        if (Physics.Raycast(transform.position + correccionAltura, direction, out hiteo,0.5f))
		{
            if (hiteo.collider.CompareTag("Muro"))
			{
                wall = true;
                CheckRelativeCoverages(hiteo.collider.gameObject);
            }
            
        }
		
        return wall;
    }

    void CheckEnemiesRelativeOrientation()
	{
        enemiesInverseDirections.Clear();
        if (GameManager.instance.combateManager.unidadActiva.CompareTag("NPC"))
		{
            CheckRelativeOrientation(GameManager.instance.combateManager.aliados);
		}
		else
		{
            CheckRelativeOrientation(GameManager.instance.combateManager.enemigos);
        }
    }
    void CheckRelativeOrientation(List<TestTacticsMove> unidades)
	{
        if (showDebugTile) Debug.Log("tengo que buscar entre " + unidades.Count + " enemigos");
        foreach(TestTacticsMove unidad in unidades)
		{
           
            if (unidad.transform.position.x > transform.position.x)
			{
                enemiesInverseDirections.Add(directionName.OESTE);
				//hay una unidad a mi derecha
			}
			
            if (unidad.transform.position.x < transform.position.x)
			{
                //unidad a la izquierda
                enemiesInverseDirections.Add(directionName.ESTE);

            }

            if (unidad.transform.position.z > transform.position.z)
			{
                //unidad arriba
                enemiesInverseDirections.Add(directionName.SUR);

            }
            if (unidad.transform.position.z < transform.position.z)
            {
                enemiesInverseDirections.Add(directionName.NORTE);
            }

		}
	}
    directionName InvertirDireccion(directionName direccion)
    {
        directionName direccionInvertida = new directionName();
        switch (direccion)
        {
            case directionName.ESTE:
                direccionInvertida = directionName.OESTE;
                break;
            case directionName.OESTE:
                direccionInvertida = directionName.ESTE;
                break;
            case directionName.NORTE:
                direccionInvertida = directionName.SUR;
                break;
            case directionName.SUR:
                direccionInvertida = directionName.NORTE;
                break;
        }
        return direccionInvertida;
    }

	void CheckRelativeCoverages(GameObject wall)
	{
        if (wall.transform.position.x > transform.position.x)
        {
            
            coveragesDirections.Add(directionName.ESTE);
            if (!currentCoverage.ContainsKey(directionName.ESTE))
			{
                currentCoverage.Add(directionName.ESTE, wall.GetComponent<Coverage>().type);

            }
            if (showDebugTile) Debug.Log("hay un muro a mi derecha porque "+wall.transform.position.x +" es mayor que "+ transform.position.x);
        }
        if (wall.transform.position.x < transform.position.x)
        {
            if (showDebugTile) Debug.Log("muro a la izquierda porque " + wall.transform.position.x + " es menor que " + transform.position.x);
            if (!currentCoverage.ContainsKey(directionName.OESTE))
			{
                currentCoverage.Add(directionName.OESTE, wall.GetComponent<Coverage>().type);
            }

            coveragesDirections.Add(directionName.OESTE);

        }
        if (wall.transform.position.z > transform.position.z)
        {
            if (showDebugTile) Debug.Log("hay muro arriba porque " + wall.transform.position.z + " es mayor que " + transform.position.z);
            coveragesDirections.Add(directionName.NORTE);
            if (!currentCoverage.ContainsKey(directionName.NORTE))
            {
                currentCoverage.Add(directionName.NORTE, wall.GetComponent<Coverage>().type);
            }

        }
        if (wall.transform.position.z < transform.position.z)
        {
            if (showDebugTile) Debug.Log("hay muro abajo porque " + wall.transform.position.z + " es menor que " + transform.position.z);
            coveragesDirections.Add(directionName.SUR);
            if (!currentCoverage.ContainsKey(directionName.SUR)) { 
                currentCoverage.Add(directionName.SUR, wall.GetComponent<Coverage>().type);
            }
        }
    }
    void FillValidCoverages()
	{
        foreach (directionName posibleCobertura in coveragesDirections)
        {
            coveragesValidDirections.Add(posibleCobertura);
        }
    }

    void IgnoreCoverages()
	{
        coveragesValidDirections.Clear();
	}
    public void ValidateCoveragesDirections()
	{

        FillValidCoverages();
        List<TestTacticsMove> enemigos = new List<TestTacticsMove>();
        if (GameManager.instance.combateManager.unidadActiva.CompareTag("Player"))
        {
            enemigos = GameManager.instance.combateManager.enemigos;
        }
        else if (GameManager.instance.combateManager.unidadActiva.CompareTag("NPC"))
        {
            enemigos = GameManager.instance.combateManager.aliados;
        }
        
        foreach(TestTacticsMove unit in enemigos)
		{
            if (unit.GetComponent<FieldOfView>().visibleTiles.Contains(transform))
			{
                IgnoreCoverages();
                break;
            }
		}

   //     foreach (directionName posibleCobertura in coveragesValidDirections)
   //     {
   //         //TODO: si estoy en el campo de visión de un "enemigo", qué orientación tiene el enemigo
   //         //los enemigos son todo el equipo contrario a la unidad actual.
   //         foreach (directionName inverseEnemyDirection in enemiesInverseDirections)
			//{

                
   //             if (posibleCobertura == inverseEnemyDirection)
   //             {

   //                 while (coveragesValidDirections.Contains(posibleCobertura))
   //                 {
   //                     //Debug.Log("lo borro [" + posibleCobertura + "]");
   //                     coveragesValidDirections.Remove(posibleCobertura);
   //                 }
   //             }
   //         }
   //     }
    }

    void DibujarCoberturasTemp()
	{
        foreach(KeyValuePair<directionName,Coverage.CoverageType> cobertura in currentCoverage)
		{

            Vector3 correccionAltura = new Vector3(0, 0.25f, 0);
            Vector3 direction = Vector3.zero;
            switch (cobertura.Key)
            {
                case directionName.ESTE:
                    direction = Vector3.right;
                    break;
                case directionName.OESTE:
                case directionName.NORTE:
                    direction = Vector3.forward;
                    break;
                case directionName.SUR:
                    break;
            }
            if (cobertura.Value == Coverage.CoverageType.COBERTURA_MEDIA)
			{
               
                Debug.DrawRay(transform.position + correccionAltura, direction, Color.green, 10);
			}else if(cobertura.Value == Coverage.CoverageType.COBERTURA_MEDIA)
			{
                Debug.DrawRay(transform.position + correccionAltura, direction, Color.yellow, 10);

            }
        }
	}
    public void DoDamage(Damage damage)
	{
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.up, out hit, 1))
        {
            TestTacticsMove posibleObjetivo = hit.collider.gameObject.GetComponent<TestTacticsMove>();
            if (posibleObjetivo)
			{
                //CHAPUZAAA según la habilidad habrá que aplicar damage o hacer otro efecto
                posibleObjetivo.AplicarDamage(damage);
                if (GameManager.instance.mostrarDebug) Debug.Log("Le han dado a " + posibleObjetivo.datosUnidad.alias + " con " + damage);
			}
			else
			{
                if (GameManager.instance.mostrarDebug) Debug.Log("No le has dado a nada");
			}
        }
    }
}
