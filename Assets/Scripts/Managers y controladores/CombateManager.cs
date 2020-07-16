using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

public class CombateManager : MonoBehaviour
{
    string modeloUnidadDefault = "UnidadSRC";
    
    GameManager gameManager;
    [SerializeField]
    TurnManager turnManager;

    [SerializeField]
    DatosUnidad unidadActiva;

    [SerializeField]
    List<DatosUnidad> enemigos;
   
    GameObject unidadSeleccionada;
   
    
    public enum FaseCombate {PAUSA, INICIO, COLOCANDO, INICIO_COMBATE, COMBATE, FIN_COMBATE}
    public FaseCombate fase;

	private void Awake()
	{
        Debug.Log("CM: Awake...");
        if (turnManager == null)
		{
            turnManager = new TurnManager();
        }
        gameManager = GameManager.instance;
    }

    void Start()
    {
        Debug.Log("CM: start....");
        fase = FaseCombate.PAUSA;
    }

    public void Combate(Sala sala)
    {
        fase = FaseCombate.INICIO;
        crearEnemigos(sala);
    }

    private void Update()
    {
        if (fase == FaseCombate.INICIO)
		{
            seleccionarUnidadActiva();
		}
        else if (fase == FaseCombate.COLOCANDO)
        {
            LevelManager.salaActiva.GetComponent<Sala>().encontrarCasillasDisponibles();
            if (gameManager.DatosPlayer.EquipoUnidades.Where(unidad => unidad.isPlaced).Count() <= 0)
            {
                mostrarIniciosDisponibles(LevelManager.salaActiva.GetComponent<Sala>());
                checkMouse();
            }
            else
            {
                gameManager.MostrarTexto("PULSA ESPACIO PARA COMENZAR EL COMBATE");
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    gameManager.MostrarTexto("");
                    fase = FaseCombate.INICIO_COMBATE;
                }
            }

        } 
        else if (fase == FaseCombate.INICIO_COMBATE)
        {
            //desactivamos puerta porque el collider nos jode las casillas
            LevelManager.DesactivarPuerta(LevelManager.salaActiva.GetComponentInChildren<Puerta>());

            fase = FaseCombate.COMBATE;
        } 
        else if (fase == FaseCombate.COMBATE)
        {
            turnManager.Update();
            //mostrar datos enemigos

            //ToDo: decidir las condiciones para que acabe el combate
            //if (se cumplen las condiciones){
            //fase = FaseCombate.FIN_COMBATE;
            //}

        }
        else if (fase == FaseCombate.FIN_COMBATE)
        {
        
        }

    }


    private void crearEnemigos(Sala sala)
    {
        List<Tile> posicionesDisponibles = sala.PuntosInicioEnemigo();
        if (posicionesDisponibles.Count > 0)
        {
            int numeroEnemigos = 0;
            while (numeroEnemigos == 0)
            {
                numeroEnemigos = Random.Range(1, posicionesDisponibles.Count);
            }
            Debug.Log("creando " + numeroEnemigos + " enemigos ");

            foreach (DatosUnidad enemigo in sala.dameEnemigos(numeroEnemigos))
            {
                Tile disponible = posicionesDisponibles[Random.Range(0, posicionesDisponibles.Count()-1)];
                posicionesDisponibles.Remove(disponible);
                GameObject modeloEnemigo = recuperarModeloUnidad(enemigo);
                GameObject nuevoEnemigo = crearUnidad(modeloEnemigo, disponible);
                if (nuevoEnemigo.GetComponent<NPCMove>() == null)
				{
                    nuevoEnemigo.AddComponent<NPCMove>();
                }
                nuevoEnemigo.GetComponent<NPCMove>().setDatos(enemigo);
                nuevoEnemigo.tag = "NPC";
                Debug.Log("Creando enemigo: " + nuevoEnemigo.GetComponent<NPCMove>().datosUnidad.ToString());
            }
        }
        else
        {
            Debug.Log("en " + sala + " No hay seteadas casillas de spawn de enemigos");
        }
           
    }

    private void mostrarIniciosDisponibles(Sala sala)
    {
        List<Tile> puntosInicio = sala.PuntosInicioPlayer();
        foreach (Tile t in puntosInicio)
        {
            t.selectable = true;
            t.target = false;
        }

    }

    GameObject recuperarModeloUnidad(DatosUnidad unidad)
	{
        GameObject modeloUnidad = (GameObject)Resources.Load("Unidades/" + unidad.tipo.nombre);
        if (modeloUnidad == null)
		{
            modeloUnidad = (GameObject)Resources.Load("Unidades/" + modeloUnidadDefault);

        }
        return modeloUnidad;
	}

    private void seleccionarUnidadActiva()
	{
		unidadActiva = gameManager.DatosPlayer.EquipoUnidades.Where(u => u.estoyVivo).First();
		if (unidadActiva != null)
        {
            unidadSeleccionada = recuperarModeloUnidad(unidadActiva);
            gameManager.interfaz.UnidadActiva = unidadActiva;

            if (unidadSeleccionada != null)
			{
                fase = FaseCombate.COLOCANDO;
            }
        }
        else
        {
            Debug.Log("No hay unidades disponibles");
        }
    }

    //ESTA FUNCION DEBERÍA ESTAR EN EL CHEQUEADOR DE MOUSE
    private void checkMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        //DatosUnidad unidadesDisponibles = gameManager.DatosPlayer.EquipoUnidades.Where(datos => datos.estoyVivo).ElementAt(3);
        //DatosUnidad unidadesDisponibles = gameManager.DatosPlayer.EquipoUnidades.Where(datos => datos.estoyVivo).First();
        if (unidadSeleccionada != null)
        {
            gameManager.interfaz.MostrarTexto("Posiciona a tu unidad en una casilla libre");
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.tag == "Tile")
                {
                    Tile c = hit.collider.GetComponent<Tile>();
                    if (c.selectable)
                    {
                        c.target = true;
                        if (Input.GetMouseButton(0))
                        {
                            Debug.Log("Colocando el modelo " + unidadSeleccionada + " en " + c);
                            GameObject nuevaUnidad = crearUnidad(unidadSeleccionada, c);
                            PlayerMove unidadPlayerMove = nuevaUnidad.AddComponent<PlayerMove>();
                            unidadPlayerMove.setDatos(unidadActiva);
                            gameManager.DatosPlayer.EquipoUnidades[0].isPlaced = true;
                            unidadSeleccionada = null;
                        }
                    }
                }
            }
        }
        
    }
    public GameObject crearUnidad(GameObject modeloUnidad, Tile casilla)
    {
        //CHAPUZAAA -> el new Vector es para encajar el modelo NPC, pero me jode otros...
       return  Instantiate(modeloUnidad, casilla.transform.position + new Vector3(0,0.9f,0), Quaternion.identity);
    }
    
}
