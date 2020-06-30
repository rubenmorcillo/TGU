using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

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


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GetRequest("https://resealable-compress.000webhostapp.com/test.php"));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator GetRequest(string uri)
    {
        Debug.Log("SM: llamando a " + uri);
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            string[] pages = uri.Split('/');
            int page = pages.Length - 1;

            if (webRequest.isNetworkError)
            {
                
                Debug.Log("SM: " + pages[page] + ": Error: " + webRequest.error);
                GetRequest(uri);
            }
            else
            {
                Debug.Log("SM: " + pages[page] + ": Exito: " + webRequest.downloadHandler.text);
            }
        }
    }
}
