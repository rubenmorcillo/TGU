using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

public class TestCombateManager : MonoBehaviour
{
    string modeloUnidadDefault = "UnidadSRC";

    GameManager gameManager;
    [SerializeField]
    MiTurnManager turnManager;

    [SerializeField]
    List<TestTacticsMove> unidades;
    [SerializeField]
    List<TestTacticsMove> aliados;
    [SerializeField]
    List<TestTacticsMove> enemigos;
    //[SerializeField]
    //public PlayerMove unidadActiva;
    //[SerializeField]
    //DatosUnidad datosUnidadActiva;




    //GameObject unidadSeleccionada;


    public enum FaseCombate { PAUSA, INICIO, COLOCANDO, INICIO_COMBATE, COMBATE, FIN_COMBATE }
    public FaseCombate fase;

    private void Awake()
    {
        Debug.Log("CM: Awake...");
        if (turnManager == null)
        {
            turnManager = new MiTurnManager();
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
        

    }

    public void AddUnitToCombat(TestTacticsMove unidad)
	{
        MiTurnManager.AddUnit(unidad);
        unidades.Add(unidad);
        if (unidad.GetComponent<TestNPCMove>() == null)
        {
            aliados.Add(unidad);
        }
        else
        {
            enemigos.Add(unidad);
        }
    }

    public void RemoveUnitFromCombat(TestTacticsMove unidad)
	{
        unidades.Remove(unidad);
        if (unidad.GetComponent<TestNPCMove>() == null)
        {
            aliados.Remove(unidad);
        }
        else
        {
            enemigos.Remove(unidad);
        }
    }

    private void Update()
    {

        //if (fase != FaseCombate.PAUSA)
        //{
        //    gameManager.MostrarTexto("");
        //}
        

        if (fase == FaseCombate.INICIO)
        {

            fase = FaseCombate.COLOCANDO;
        }
        else if (fase == FaseCombate.COLOCANDO)
        {

            //TODO ->mejora: Seleccionar en qué casilla empezar?

            TestSala sala = gameObject.GetComponent<TestSala>();
            sala.mostrarIniciosPlayer(); //mostramos los inicios disponibles (los puntos de Spawn para el player q no están ocupados)



			//LevelManager.salaActiva.GetComponent<Sala>().encontrarCasillasDisponibles();
			//if (gameManager.DatosPlayer.EquipoUnidades.Where(unidad => unidad.isPlaced).Count() <= 0)
			//{
			//    mostrarIniciosDisponibles(LevelManager.salaActiva.GetComponent<Sala>());
			//    checkMouse();
			//}
			//else
			//{
			//    gameManager.MostrarTexto("PULSA ESPACIO PARA COMENZAR EL COMBATE");
			if (Input.GetKeyDown(KeyCode.Space))
			{
				fase = FaseCombate.INICIO_COMBATE;
			}
			//}

		}
        else if (fase == FaseCombate.INICIO_COMBATE)
        {
            //controlamos todas las unidades existentes - suponemos que vamos a tener a todas presentes desde el inicio del combate
            foreach (TestTacticsMove tm in FindObjectsOfType<TestTacticsMove>())
            {
                AddUnitToCombat(tm);
            }
            fase = FaseCombate.COMBATE;
        }
        else if (fase == FaseCombate.COMBATE)
        {
            turnManager.Update();
			//mostrar datos enemigos

			//ToDo: decidir las condiciones para que acabe el combate
            
            if (enemigos.Count <= 0 ){
                //victoria
				fase = FaseCombate.FIN_COMBATE;
			}else if(aliados.Count <= 0)
			{
                //derrota
                fase = FaseCombate.FIN_COMBATE;
			}

		}
        else if (fase == FaseCombate.FIN_COMBATE)
        {
            //TODO: resetear cosas (los tiles se quedan seleccionables)

            fase = FaseCombate.PAUSA;
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
                Tile disponible = posicionesDisponibles[Random.Range(0, posicionesDisponibles.Count() - 1)];
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

   

    GameObject recuperarModeloUnidad(DatosUnidad unidad)
    {
        GameObject modeloUnidad = (GameObject)Resources.Load("Unidades/" + unidad.tipo.nombre);
        if (modeloUnidad == null)
        {
            modeloUnidad = (GameObject)Resources.Load("Unidades/" + modeloUnidadDefault);

        }
        return modeloUnidad;
    }

    //private void seleccionarUnidadActiva()
    //{
    //    datosUnidadActiva = gameManager.DatosPlayer.EquipoUnidades.Where(u => u.estoyVivo).First();
    //    if (datosUnidadActiva != null)
    //    {
    //        unidadSeleccionada = recuperarModeloUnidad(datosUnidadActiva);
    //        gameManager.interfaz.DatosUnidadActiva = datosUnidadActiva;

    //    }
    //    else
    //    {
    //        Debug.Log("No hay unidades disponibles");
    //    }
    //}

    //ESTA FUNCION DEBERÍA ESTAR EN EL CHEQUEADOR DE MOUSE
    //private void checkMouse()
    //{
    //    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    //    RaycastHit hit;

    //    if (unidadSeleccionada != null)
    //    {
    //        gameManager.interfaz.MostrarTexto("Posiciona a tu unidad en una casilla libre");
    //        if (Physics.Raycast(ray, out hit))
    //        {
    //            if (hit.collider.tag == "Tile")
    //            {
    //                Tile c = hit.collider.GetComponent<Tile>();
    //                if (c.selectable)
    //                {
    //                    c.target = true;
    //                    if (Input.GetMouseButton(0))
    //                    {
    //                        Debug.Log("Colocando el modelo " + unidadSeleccionada + " en " + c);
    //                        GameObject nuevaUnidad = crearUnidad(unidadSeleccionada, c);
    //                        unidadActiva = nuevaUnidad.AddComponent<PlayerMove>();
    //                        unidadActiva.setDatos(datosUnidadActiva);
    //                        gameManager.DatosPlayer.EquipoUnidades[0].isPlaced = true;
    //                        unidadSeleccionada = null;
    //                    }
    //                }
    //            }
    //        }
    //    }

    //}
    public GameObject crearUnidad(GameObject modeloUnidad, Tile casilla)
    {
        //CHAPUZAAA -> el new Vector es para encajar el modelo NPC, pero me jode otros...
        return Instantiate(modeloUnidad, casilla.transform.position + new Vector3(0, 0.9f, 0), Quaternion.identity);
    }

}
