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

        } else if (fase == FaseCombate.INICIO_COMBATE)
        {
            //desactivamos puerta porque el collider nos jode las casillas
            LevelManager.DesactivarPuerta(LevelManager.salaActiva.GetComponentInChildren<Puerta>());

            fase = FaseCombate.COMBATE;
        } else if (fase == FaseCombate.COMBATE)
        {
            turnManager.Update();

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
        System.Random rnd = new System.Random();
        List<Tile> posicionesDisponibles = sala.PuntosInicioEnemigo();
        if (posicionesDisponibles.Count > 0)
        {
            int numeroEnemigos = 0;
            while (numeroEnemigos == 0)
            {
                numeroEnemigos = rnd.Next(posicionesDisponibles.Count);
            }
            Debug.Log("creando " + numeroEnemigos + " enemigos ");

            foreach (GameObject enemigo in sala.dameEnemigos(numeroEnemigos))
            {
               
                Tile disponible = posicionesDisponibles[0];
                posicionesDisponibles.Remove(disponible);
                GameObject nuevoEnemigo = crearUnidad(enemigo, disponible);
                //CHAPUZAAA
                //FALSEANDO DATOS ENEMIGOS
                nuevoEnemigo.GetComponent<NPCMove>().setDatos(new DatosUnidad(0, new TipoUnidad(1, "rasek", 50, 3, 6, 23, 46, 0, 12), "enemigo1", 5,20));
                Debug.Log(enemigo.GetComponent<NPCMove>());

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

    private void seleccionarUnidadActiva()
	{
        unidadActiva = gameManager.DatosPlayer.EquipoUnidades.Where(u => u.estoyVivo).First();
        if (unidadActiva != null)
        {
            GameObject modelo = (GameObject)Resources.Load("Unidades/" + unidadActiva.tipo.nombre);
            if (modelo == null)
            {
                Debug.Log("no hay modelo para " + unidadActiva.tipo.nombre + " ... cargando modelo por defecto");
                modelo = (GameObject)Resources.Load("Unidades/" + modeloUnidadDefault);
            }
            
            unidadSeleccionada = modelo;
            //CHAPUZAA -> quizá sea el manager el q deba fijar la unidad activa
            gameManager.interfaz.UnidadActiva = unidadActiva;

            if (unidadSeleccionada != null)
			{
                //Debug.Log("CM: cambiando a fase: " + FaseCombate.COLOCANDO.ToString());
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

        //LayerMask layerMaskUI = LayerMask.GetMask("UI");
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
                            GameObject nuevaUnidad = crearUnidad(unidadSeleccionada, c);
                            //CHAPUZAAA testeo
                            nuevaUnidad.GetComponent<PlayerMove>().setDatos(gameManager.DatosPlayer.EquipoUnidades[0]); //TEMPORAL
                            Debug.Log("Colocando el modelo " + unidadSeleccionada + " en " + c);
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
       return  Instantiate(modeloUnidad, casilla.transform.position + new Vector3(0,0.9f,0), Quaternion.identity);
    }
    
}
