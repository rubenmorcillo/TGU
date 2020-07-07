using UnityEngine;
using UnityEngine.ProBuilder;
using UnityEngine.UI;


public class InterfazController : MonoBehaviour
{

    public Image[] imgUnidades = new Image[5];

    public Text textRep, textDinero, textNickname;
    public Text txtHab1, txtHab2, txtHab3, txtHab4, txtUnidadHp, txtUnidadNombre;

    public Image imgUnidadDetalle;

    public GameObject panelInfo;

    public Animator detalleAnimator;
   // public bool detalleUnidadActivo = false;
    DatosPlayer datosPlayer;
    DatosUnidad unidadActiva;
    public DatosUnidad UnidadActiva
    {
        get
        {
            return unidadActiva;
        }
        set
        {
            unidadActiva = value;
        }
    }

    //public static InterfazController instance;
	private void Awake()
	{
		//	if (instance == null)
		//	{
		//		instance = this;
		//	}
		//	else if (instance != this)
		//	{
		//		Destroy(gameObject);
		//	}
		DontDestroyOnLoad(this);

	}

	void Start()
    {
        //header
        textDinero = GameObject.Find("jugadorDinero").GetComponent<Text>();
        textRep = GameObject.Find("jugadorRep").GetComponent<Text>();
        textNickname = GameObject.Find("jugadorName").GetComponent<Text>();
        imgUnidades = GameObject.Find("jugadorUnidades").GetComponentsInChildren<Image>();
        
        //unidad
        imgUnidadDetalle = GameObject.Find("imgUnidadDetalle").GetComponent<Image>();
        txtUnidadNombre = GameObject.Find("txtUnidadNombre").GetComponent<Text>();
        txtUnidadHp = GameObject.Find("txtUnidadHp").GetComponent<Text>();
        txtHab1 = GameObject.Find("txtHab1").GetComponent<Text>();
        txtHab2 = GameObject.Find("txtHab2").GetComponent<Text>();
        txtHab3 = GameObject.Find("txtHab3").GetComponent<Text>();
        txtHab4 = GameObject.Find("txtHab4").GetComponent<Text>();

        //info
        panelInfo = GameObject.Find("panelInfo");

        //anims
        detalleAnimator = GameObject.Find("MenuDetalle").GetComponent<Animator>();
        GameManager.instance.interfaz = this;
    }

   

    void Update()
    {
        datosPlayer = GameManager.instance.DatosPlayer;
        if (datosPlayer != null)
        {
            if (GameManager.instance.estadoJuego == EstadosJuego.Estado.COMBATE)
            {
                if (unidadActiva != null)
                {
                    MostrarDetallesUnidad();
                }
                else
                {
                    Debug.Log("no hay unidad activa");
                }
            }
            else
            {
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
        for (int i = 0; i < datosPlayer.EquipoUnidades.Count; i++)
        {

            string nombre = datosPlayer.EquipoUnidades[i].tipo.nombre;
            Sprite img = Resources.Load<Sprite>("Kaos/" + nombre);

            imgUnidades[i].sprite = img;
            imgUnidades[i].gameObject.GetComponent<ImagenUnidad>().Unidad = datosPlayer.EquipoUnidades[i];
            //aqui debería incorporar el comportamiento onMouseOver -> de momento he tenido q crear la clase imagenUnidad

        }
    }
    public void MostrarTexto(string texto)
	{
        panelInfo.GetComponentInChildren<Text>().text = texto;
	}
    public void Mostrar()
	{
        detalleAnimator.SetBool("mostrar", true);
	}

    public void Ocultar()
	{
        detalleAnimator.SetBool("mostrar", false);
    }

    private void MostrarDetallesUnidad()
    {
        //Debug.Log("IC: mostrar detalles de "+unidadActiva.ToString());
        imgUnidadDetalle.sprite = Resources.Load<Sprite>("Kaos/" + unidadActiva.tipo.nombre);
        txtUnidadHp.text = "HP: " + unidadActiva.hpActual + " / " + unidadActiva.hpMax;

        if (string.Compare(unidadActiva.alias, "") == 1)
        {
            txtUnidadNombre.text = "\'" + unidadActiva.alias + "\' ";
        }
        txtUnidadNombre.text += "[" + unidadActiva.tipo.nombre + "]";
       

        if (unidadActiva.Hab1 != null)
		{
            txtHab1.text = unidadActiva.Hab1.nombre;
        }
		else
		{
            txtHab1.text = "-";
        }

        if (unidadActiva.Hab2 != null)
        {
            txtHab2.text = unidadActiva.Hab2.nombre;
        }
        else
        {
            txtHab2.text = "-";
        }

        if (unidadActiva.Hab3 != null)
        {
            txtHab3.text = unidadActiva.Hab3.nombre;
        }
        else
        {
            txtHab3.text = "-";
        }

        if (unidadActiva.Hab4 != null)
        {
            txtHab4.text = unidadActiva.Hab4.nombre;
        }
        else
        {
            txtHab4.text = "-";
        }
        
    }
}