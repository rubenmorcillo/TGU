using UnityEngine;
using UnityEngine.UI;

public class MenuDataLoader : MonoBehaviour
{

    public float dinero,rep;
    public string nickname;
    public Image[] imgUnidades = new Image[5];

    public Text textRep, textDinero, textNickname;
    DatosPlayer datosPlayer;

    public static MenuDataLoader instance;

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

    }

    void Start()
    {
        textDinero = GameObject.Find("jugadorDinero").GetComponent<Text>();
        textRep = GameObject.Find("jugadorRep").GetComponent<Text>();
        textNickname = GameObject.Find("jugadorName").GetComponent<Text>();
        imgUnidades = GameObject.Find("jugadorUnidades").GetComponentsInChildren<Image>();
    }

    void Update()
    {
        datosPlayer = GameManager.instance.DatosPlayer;
        if (datosPlayer != null)
		{
            if (EstadosJuego.EstadoActual() == EstadosJuego.Estado.COMBATE)
            {

            }
            else {
                
                RefrescarDatosPlayer();
            }

		}
		
    }

    void RefrescarDatosPlayer()
	{
        //reputacion
        textRep.text = "Reputación: ";
        textRep.text = string.Concat(textRep.text, datosPlayer.reputacion);

        //dinero
        textDinero.text = "Dinero: ";
        textDinero.text = string.Concat(textDinero.text, datosPlayer.dinero);
        //nombre
        textNickname.text = datosPlayer.nickname;
    
        //unidades
        for (int i=0; i< datosPlayer.equipoUnidades.Count; i++)
		{
            
            string nombre = datosPlayer.equipoUnidades[i].unitName;
            Sprite img = Resources.Load<Sprite>("Kaos/" + nombre);
            imgUnidades[i].sprite = img;
			if (datosPlayer.equipoUnidades[i].isPlaced)
			{

			}


        }
    }
}
