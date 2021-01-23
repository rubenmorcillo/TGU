using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System.Linq;

public class ServerManager : MonoBehaviour
{
    
    string serverUri = "https://resealable-compress.000webhostapp.com/";

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ObtenerHabilidades()
	{
        StartCoroutine(peticionHabilidades());
	}

    public void ObtenerUnidades()
	{
        StartCoroutine(peticionUnidades());
	}

    IEnumerator peticionUnidades()
	{
        Debug.Log("SM: llamando a las unidades");
        string finalUri = "unidades.php";
        UnityWebRequest request = UnityWebRequest.Get(serverUri + finalUri);
        yield return request.SendWebRequest();

        if (request.isNetworkError)
        {
            Debug.Log("SM Error: " + request.error);
            //peticionUnidades();
            try
            {
                Debug.Log("SM:(Server no disponible) FALSEANDO unidades... " + request.downloadHandler.text);
                //si el servidor está caido tengo q rellenar la lista a mano
                string jsonColeccion = "[{\"id\":1,\"nombre\":\"rasek\",\"hp_base\":50,\"atq_fisico\":6,\"atq_especial\":23,\"def_fisico\":46,\"def_especial\":0,\"salto_base\":0,\"movimiento_base\":3,\"agilidad\":12},{\"id\":2,\"nombre\":\"omega\",\"hp_base\":50,\"atq_fisico\":2,\"atq_especial\":25,\"def_fisico\":50,\"def_especial\":0,\"salto_base\":0,\"movimiento_base\":3,\"agilidad\":4}]";
                TipoUnidad[] lista = JsonHelper.FromJson<TipoUnidad>(JsonHelper.fixJson(jsonColeccion));
                GameManager.instance.BDlocal.TiposUnidad = lista.ToList();
                Debug.Log("SM Unidades FALSEADAS: " + JsonHelper.ToJson(GameManager.instance.BDlocal.TiposUnidad.ToArray()));
            }
            catch (System.Exception e)
            {
                Debug.Log("SM: ERROR FALSEADAS unidades -> " + e);
            }
        }
        else
        {
            try
            {
                Debug.Log("SM Éxito recuperando unidades: " + request.downloadHandler.text);
             
                
                TipoUnidad[] lista = JsonHelper.FromJson<TipoUnidad>(JsonHelper.fixJson(request.downloadHandler.text));
                GameManager.instance.BDlocal.TiposUnidad = lista.ToList();
                Debug.Log("SM Unidades recuperadas: " + JsonHelper.ToJson(GameManager.instance.BDlocal.TiposUnidad.ToArray()));
            }
            catch(System.Exception e)
			{
                Debug.Log("SM: ERROR recuperando unidades -> " + e);
			}
          
        }
    }
    IEnumerator peticionHabilidades()
    {
        Debug.Log("SM: llamando a las habilidades");
        string finalUri = "habilidades.php";
        UnityWebRequest request = UnityWebRequest.Get(serverUri + finalUri);
        yield return request.SendWebRequest();

        if (request.isNetworkError)
        {
            Debug.Log("SM Error: " + request.error);
            //peticionHabilidades();
            
            //si no responde se falsean
            string jsonColeccion = "[{\"id\":\"1\",\"nombre\":\"habTest1\",\"poder\":null,\"tipo\":\"FISICO\",\"esfuerzo\":\"1\",\"tipoRango\":\"2\",\"rango\":\"5\",\"potencia\":\"1\"},{\"id\":\"2\",\"nombre\":\"habTest2\",\"poder\":null,\"tipo\":\"FISICO\",\"esfuerzo\":\"2\",\"tipoRango\":\"2\",\"rango\":\"5\",\"potencia\":\"2\"},{\"id\":\"3\",\"nombre\":\"habEsp1\",\"poder\":null,\"tipo\":\"ESPECIAL\",\"esfuerzo\":\"3\",\"tipoRango\":\"3\",\"rango\":\"2\",\"potencia\":\"10\"},{\"id\":\"4\",\"nombre\":\"habEsp2\",\"poder\":null,\"tipo\":\"ESPECIAL\",\"esfuerzo\":\"4\",\"tipoRango\":\"1\",\"rango\":\"7\",\"potencia\":\"20\"}]";
            try
            {
                Debug.Log("SM:(Server no disponible) FALSEANDO habilidades: " + jsonColeccion);
                Habilidad[] lista = JsonHelper.FromJson<Habilidad>(JsonHelper.fixJson(jsonColeccion));
                GameManager.instance.BDlocal.Habilidades = lista.ToList();
                Debug.Log("SM: Habilidades FALSEADAS: " + JsonHelper.ToJson(GameManager.instance.BDlocal.Habilidades.ToArray()));

            }
            catch (System.Exception e)
            {
                Debug.Log("SM: ERROR FALSEANDO habilidades -> " + e);
            }
        }
        else
        {
            try
            {
                Debug.Log("SM Éxito recuperando habilidades: " + request.downloadHandler.text);
                Habilidad[] lista = JsonHelper.FromJson<Habilidad>(JsonHelper.fixJson(request.downloadHandler.text));
                GameManager.instance.BDlocal.Habilidades = lista.ToList();
                Debug.Log("SM Habilidades recuperadas: " + JsonHelper.ToJson(GameManager.instance.BDlocal.Habilidades.ToArray()));

            }
            catch (System.Exception e)
            {
                Debug.Log("SM: ERROR recuperando habilidades -> " + e);
            }

        }
    }

    IEnumerator GetRequest(string uri)
    {
        Debug.Log("SM: llamando a " + uri);
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
			
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            if (webRequest.isNetworkError)
            {
                
                Debug.Log("SM Error: " + webRequest.error);
                GetRequest(uri);
            }
            else
            {
                Debug.Log("SM Éxito: " + webRequest.downloadHandler.text);
            }
        }
    }

  
}
