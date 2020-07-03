using System.Linq;
using UnityEngine;

public class TecladoController : MonoBehaviour
{
    public GameObject menuEsc;
    
    // Update is called once per frame
    void Update()
    {
        CheckMenuEsc();
        TeclaComodin();
    }

    void CheckMenuEsc()
	{
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!EstadosJuego.MenuActivo())
            {
                menuEsc.SetActive(true);
                EstadosJuego.setMenuActivo(true);
            }
            else
            {
                menuEsc.SetActive(false);
                EstadosJuego.setMenuActivo(false);
            }
        }
    }

    void TeclaComodin()
	{
        if (Input.GetKeyDown(KeyCode.D))
		{
            Debug.Log("restando");
            GameManager.instance.DatosPlayer.EquipoUnidades.First().hpActual -= 10;
		}
	}
}
