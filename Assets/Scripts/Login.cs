using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class Login : MonoBehaviour
{
    string serverUri = "https://resealable-compress.000webhostapp.com/test.php";

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
            Debug.Log(request.downloadHandler.text);

            //CHAPUZAAAA

            //cojo los datos y relleno datosPlayer
            //falseando los datos
            DatosPlayer datosPlayerTest = new DatosPlayer();
            datosPlayerTest.nickname = "nicknameLoginTest";
            datosPlayerTest.dinero = 1;
            datosPlayerTest.reputacion = 1;


            //falseando las unidades q tiene
            DatosUnidad du = new DatosUnidad(1, "rasek", 5, 100);
            DatosUnidad du2 = new DatosUnidad(2, "rusuk", 5, 100);
            datosPlayerTest.addUnidadEquipo(du);
            datosPlayerTest.addUnidadEquipo(du2);


            GameManager.instance.DatosPlayer = datosPlayerTest;

            //cuando esté todo cargado, lo llevo a la siguiente escena
            SceneManager.LoadScene("base");

        }
    }

}
