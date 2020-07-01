using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;



public class Login : MonoBehaviour
{
    string serverUri = "https://resealable-compress.000webhostapp.com/personaje.php";

    [SerializeField]
    TMP_InputField inputLogin, inputPass;

    public void validarLogin()
    {
        Debug.Log("USER: " + inputLogin.text);
        Debug.Log("PASSWORD: " + inputPass.text);
        
        WWWForm formu = new WWWForm();
        formu.AddField("login", inputLogin.text);
        formu.AddField("password", inputPass.text);
        StartCoroutine(testeando(formu));
    }

    IEnumerator testeando(WWWForm form)
    {
        UnityWebRequest request = UnityWebRequest.Post(serverUri, form);
        yield return request.SendWebRequest();

        if (request.isNetworkError || request.isHttpError)
        {
            Debug.Log("malamente");
        }
        else
        {
            Debug.Log("SERVIDOR RESPONDE: \n"+request.downloadHandler.text +" \n FIN RESPUESTA SERVIDOR");

            DatosPlayer dpt = new DatosPlayer();
            dpt.nickname = "HARDCODEnickname";
            dpt.reputacion = 444;
            dpt.dinero = 444;

            string mijson = JsonUtility.ToJson(dpt);
          //  DatosPlayer datosPlayerJson = JsonUtility.FromJson<DatosPlayer>(request.downloadHandler.text);
            DatosPlayer datosPlayerJson = JsonUtility.FromJson<DatosPlayer>(mijson);
            Debug.Log("he conseguido un player que se llama -> " + datosPlayerJson.nickname);
            //CHAPUZAAAA

            //cojo los datos y relleno datosPlayer
            //falseando los datos
            DatosPlayer datosPlayerTest = new DatosPlayer();
            datosPlayerTest.Nickname = "nicknameLoginTest";
            datosPlayerTest.Dinero = 1;
            datosPlayerTest.Reputacion = 1;
        
            //falseando las unidades q tiene
            DatosUnidad du = new DatosUnidad(1, new TipoUnidad(1, "rasek", 50, 3, 6, 23, 46, 0, 12), "rasek", 5, 100);
            DatosUnidad du2 = new DatosUnidad(2, new TipoUnidad(1, "rasek", 50, 3, 6, 23, 46, 0, 12), "rusuk", 5, 100);
            datosPlayerTest.addUnidadEquipo(du);
            datosPlayerTest.addUnidadEquipo(du2);


            GameManager.instance.DatosPlayer = datosPlayerJson;
            //cuando esté todo cargado, lo llevo a la siguiente escena
           // SceneManager.LoadScene("base");

        }
    }

}
