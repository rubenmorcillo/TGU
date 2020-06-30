using TMPro;
using UnityEngine;


public class LogicaJugadorMouse : MonoBehaviour
{
    //public LayerMask mascMov;


    //TextMeshProUGUI tmp;
    Camera cam;
	//public GameObject camPos;
    MotorJugador motor;
    GameManager gameManager;
    void Start()
    {
        //GameObject objeto =  GameObject.Find("Texto");
        
        //tmp = objeto.GetComponent<TextMeshProUGUI>();

        gameManager = GameManager.instance;
        if (motor == null)
        {
            motor = gameObject.AddComponent<MotorJugador>();
        }
        else
        {
            motor = GetComponent<MotorJugador>();
        }

       
    }


    void Update()
    {
        CheckTeclado();

        if (cam != null)
		{
            CheckMousse();
		}
		else
		{
            cam = Camera.main;
		}
       
        if (EstadosJuego.EstadoActual() == EstadosJuego.Estado.EXPLORAR)
        {
          
            CheckPuerta();
            CheckSala();
        }
       
    }


    void CheckTeclado()
	{

        //CHAPUZAA
        if (Input.GetKeyDown(KeyCode.C))
		{
            Debug.Log("fijando objetivo de la cámara");
            Camera.main.GetComponentInParent<CameraController>().SetTarget(gameObject);
        }
    }

   

    void CheckMousse()
    {
        if(EstadosJuego.EstadoActual() == EstadosJuego.Estado.EXPLORAR)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    if (!hit.collider.CompareTag("Muro"))
                    {
                        motor.MoverAlPunto(hit.point);
                    }

                }

            }
        }
        else if (EstadosJuego.EstadoActual() == EstadosJuego.Estado.COMBATE)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;
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
                            //activar unidad para colocarla
                            GameObject unidadProvisional = (GameObject)Resources.Load("Unidades/UnidadSRC"); //ESTO HAY QUE CAMBIARLO!!!
                           
                            CombateManager.instance.crearUnidad(unidadProvisional, c);
                            Debug.Log("Colocando " + unidadProvisional + " en " + c);
                        }
                    }
                }
            }
        }



    }


    public void CheckPuerta()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 2.0f))
        {
            if (hit.collider.tag == "Puerta")
            {
                //tmp.SetText("pulsa espacio para abrir");
               // Debug.Log("pulsa espacio para continuar");
                if (Input.GetKey(KeyCode.Space))
                {
                    //Debug.Log("Estoy abriendo la puerta " + hit.collider.GetComponentInParent<Puerta>());
                    Puerta puerta = hit.collider.GetComponentInParent<Puerta>();
                    puerta.GetComponentInChildren<Animator>().SetBool("open", true);
                    gameManager.abrirPuerta(puerta);
                    
                }
            }
            //else
            //{
            //    tmp.SetText("");
            //}
        }
    }

    void CheckSala()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 2.0f))
        {
            if (hit.collider.tag == "Combate")
            {
                Debug.Log("combate");
                motor.MoverAlPunto(transform.position); //con esto lo dejamos quieto cuando se active el evento
                gameManager.activarCombate();
                hit.collider.enabled = false;
            }
        }
    }

}
