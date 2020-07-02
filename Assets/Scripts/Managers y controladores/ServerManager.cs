using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System.Linq;

public class ServerManager : MonoBehaviour
{
    public static ServerManager instance;
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
    string serverUri = "https://resealable-compress.000webhostapp.com/";

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PeticionGet(string uri)
	{
        StartCoroutine(GetRequest(uri));
	}

    public void ObtenerUnidades()
	{
        StartCoroutine(peticionUnidades());
	}

    IEnumerator peticionUnidades()
	{
        Debug.Log("SM: llamando a las unidades");
        string finalUri = "unidades.php";
       // UnityWebRequest request = UnityWebRequest.Get("http://www.google.es");
        UnityWebRequest request = UnityWebRequest.Get(serverUri + finalUri);
        yield return request.SendWebRequest();

        if (request.isNetworkError)
        {
            Debug.Log("SM Error: " + request.error);
            peticionUnidades();
        }
        else
        {
            //List<TipoUnidad> unidades = JsonUtility.FromJson<List<TipoUnidad>>(request.downloadHandler.text);
            string jsonColeccion = "{\"Items\":[{\"id\":\"1\", \"nombre\":\"nombre\",\"movimiento_base\":\"3\", \"hp_base\":\"100\", \"atq_fisico\":\"4\",\"atq_especial\":\"10\", \"def_fisico\":\"10\", \"def_especial\":\"45\", \"agilidad\":\"5\"},{\"id\":\"1\", \"nombre\":\"nombre\",\"movimiento_base\":\"3\", \"hp_base\":\"100\", \"atq_fisico\":\"4\",\"atq_especial\":\"10\", \"def_fisico\":\"10\", \"def_especial\":\"45\", \"agilidad\":\"5\"}]}";

            try
            {
                //Debug.Log("SM Éxito recuperando unidades: " + request.downloadHandler.text);
                TipoUnidad[] unidades = JsonHelper.FromJson<TipoUnidad>(jsonColeccion);
                GameManager.instance.BDlocal.TiposUnidad = unidades.ToList();
                Debug.Log("SM Unidades recuperadas: " + JsonHelper.ToJson(unidades));
            }
            catch(System.Exception e)
			{
                Debug.Log("SM: ERROR -> " + e);
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
