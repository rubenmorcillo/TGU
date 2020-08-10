using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class InterfazController : MonoBehaviour
{

    public Image[] imgUnidades = new Image[5];

    public Text textRep, textDinero, textNickname;
    public Text txtHab1, txtHab2, txtHab3, txtHab4, txtUnidadHp, txtUnidadNombre, txtPA, txtPM;

    public GameObject panelTurnos;

    public Image imgUnidadDetalle;

    public GameObject panelInfo;

    public Animator detalleAnimator;
    public Animator datosPlayerAnimator;

    DatosPlayer datosPlayer;
    DatosUnidad datosUnidadActiva;
    public DatosUnidad DatosUnidadActiva
    {
        get
        {
            return datosUnidadActiva;
        }
        set
        {
            datosUnidadActiva = value;
        }
    }

	private void Awake()
	{
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
        txtPA = GameObject.Find("txtPuntosEsfuerzo").GetComponent<Text>();
        txtPM = GameObject.Find("txtPuntosMovimiento").GetComponent<Text>();

        //info
        panelInfo = GameObject.Find("panelInfo");

        //anims
        detalleAnimator = GameObject.Find("MenuDetalle").GetComponent<Animator>();
        datosPlayerAnimator = GameObject.Find("datosPlayer").GetComponent<Animator>();
        GameManager.instance.interfaz = this;

        //panel turnos
        //panelTurnos = GameObject.Find("panelTurnos");
        //panelTurnos.SetActive(false);
        
    }

   

    void Update()
    {
        datosPlayer = GameManager.instance.DatosPlayer;
        if (datosPlayer != null)
        {
            if (GameManager.instance.estadoJuego == EstadosJuego.Estado.COMBATE)
            {
                if (datosUnidadActiva != null)
                {
                    MostrarDetallesUnidad();
     //               if (GameManager.instance.combateManager.fase == CombateManager.FaseCombate.COMBATE)
					//{
     //                   MostrarColaTurnos(MiTurnManager.unidadesCombate);
     //               }
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


    private void MostrarDetallesUnidad()
    {
        //Debug.Log("IC: mostrar detalles de "+unidadActiva.ToString());
        imgUnidadDetalle.sprite = Resources.Load<Sprite>("Kaos/" + datosUnidadActiva.tipo.nombre);
        txtUnidadHp.text = "HP: " + datosUnidadActiva.hpActual + " / " + datosUnidadActiva.hpMax;
        txtPM.text = "PM: " + datosUnidadActiva.puntosMovimientoActual + " / " + datosUnidadActiva.puntosMovimientoTotal;
        txtPA.text = "PA: " + datosUnidadActiva.puntosEsfuerzoActual + " / " + datosUnidadActiva.puntosEsfuerzoTotal;

        if (string.Compare(datosUnidadActiva.alias, "") == 1)
        {
            txtUnidadNombre.text = "\'" + datosUnidadActiva.alias + "\' ";
        }
        txtUnidadNombre.text += "[" + datosUnidadActiva.tipo.nombre + "]";
       

        if (datosUnidadActiva.Hab1 != null)
		{
            txtHab1.text = datosUnidadActiva.Hab1.nombre;
        }
		else
		{
            txtHab1.text = "-";
        }

        if (datosUnidadActiva.Hab2 != null)
        {
            txtHab2.text = datosUnidadActiva.Hab2.nombre;
        }
        else
        {
            txtHab2.text = "-";
        }

        if (datosUnidadActiva.Hab3 != null)
        {
            txtHab3.text = datosUnidadActiva.Hab3.nombre;
        }
        else
        {
            txtHab3.text = "-";
        }

        if (datosUnidadActiva.Hab4 != null)
        {
            txtHab4.text = datosUnidadActiva.Hab4.nombre;
        }
        else
        {
            txtHab4.text = "-";
        }
        
    }

 //   public void MostrarColaTurnos(List<TacticsMove> colaTurnos)
	//{
        
 //       panelTurnos.SetActive(true);
 //       List<Image> imagenesTurnos = panelTurnos.GetComponentsInChildren<Image>().ToList();
 //       imagenesTurnos.RemoveAt(0);
 //       Queue<DatosUnidad> unidadesTurnos = new Queue<DatosUnidad>();

 //       foreach (Image img in imagenesTurnos)
	//	{
 //           if (unidadesTurnos.Count() <= 0)
	//		{
 //               foreach (TacticsMove tm in colaTurnos.OrderByDescending(u => u.datosUnidad.iniciativa))
	//			{
 //                   unidadesTurnos.Enqueue(tm.datosUnidad);
	//			}
	//		}
 //           DatosUnidad unidadActual = unidadesTurnos.Dequeue();
 //           img.sprite = Resources.Load<Sprite>("Kaos/" + unidadActual.tipo.nombre);
 //           ImagenUnidad iu = img.gameObject.AddComponent<ImagenUnidad>();
 //           iu.Unidad = unidadActual;

 //       }
	//}
}