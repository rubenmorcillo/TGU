using UnityEngine;

[RequireComponent(typeof(TecladoController))]
public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    GameObject mazmorra;
    GameObject playerModel;
    DatosPlayer datosPlayer;
    InterfazController interfaz;
    public ServerManager serverManager;
    public BDLocal BDlocal;

    CombateManager combateManager;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(this);

        combateManager = gameObject.GetComponent<CombateManager>();
        if (combateManager == null)
        {
            combateManager = gameObject.AddComponent<CombateManager>();
        }

        serverManager = gameObject.GetComponent<ServerManager>();
        if(serverManager == null)
		{
            serverManager = gameObject.AddComponent<ServerManager>();
		}
        
        if (BDlocal == null)
		{
            BDlocal = new BDLocal();
		}

        if (datosPlayer == null)
        {
            //FALSEANDO DATOS DEL JUGADOR
            DatosPlayer datosPlayerTest = new DatosPlayer();
            datosPlayerTest.dinero = 150;
            datosPlayerTest.reputacion = 99;
            datosPlayerTest.nickname = "nicknameTest";
            datosPlayerTest.addUnidadEquipo(new DatosUnidad(1, new TipoUnidad(1, "rasek", 50, 3, 6, 23, 46, 0, 12), "rasek", 5, 100));
            datosPlayer = datosPlayerTest;
            //datosPlayer = gameObject.AddComponent<DatosPlayer>();
        }
    }
    
    


    public DatosPlayer DatosPlayer
    {
        get
        {
            return datosPlayer;
        }
        set
        {
            datosPlayer = value;
        }
    }


    //toDo: void CargarDatosPlayer(){}

    public void Start()
    {
        Debug.Log("GameManager: start...");
        EstadosJuego.activarEstado(EstadosJuego.Estado.MENU);

        //inico los otros managers
        combateManager.enabled = false;
        BDlocal.Init();


        //EL LEVEL MANAGER SOLO LO NECESITO CUANDO VOY A LA MAZMORRA
        LevelManager.Init();

        //iniciarMazmorra();
    }

    //esta función está un poco mezclada...no puede hacerla directamente en LevelCreator y GameManager ocuparse de activar estado?
	public void iniciarMazmorra()
    {
        Debug.Log("GM: iniciando mazmorra");
        mazmorra = LevelManager.CrearMazmorra();
        LevelManager.CrearSalaInicial();
        playerModel = LevelManager.posicionarJugador();
        EstadosJuego.activarEstado(EstadosJuego.Estado.EXPLORAR);
    }
    //SERVIDOR


    //COMBATE

    public void abrirPuerta(Puerta puerta)
    {
        LevelManager.nuevaSala(puerta);
    }

    public void activarCombate()
    {
        EstadosJuego.activarEstado(EstadosJuego.Estado.COMBATE);
     
        //ToDo:hago la animación que tenga que hacer
        //pongo la musica
        playerModel.SetActive(false); //desactivo a mi avatar
        combateManager.enabled = true;
        combateManager.Combate(LevelManager.salaActiva.GetComponent<Sala>());
    }
}
