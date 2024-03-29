﻿using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using System.Collections.Generic;



public class Login : MonoBehaviour
{
    string serverUri = "https://resealable-compress.000webhostapp.com/personaje.php";

    [SerializeField]
    TMP_InputField inputLogin, inputPass;

    public void validarLogin()
    {
      //  Debug.Log("USER: " + inputLogin.text);
      //  Debug.Log("PASSWORD: " + inputPass.text);
        
        WWWForm formu = new WWWForm();
        formu.AddField("login", inputLogin.text);
        formu.AddField("password", inputPass.text);
        StartCoroutine(corrutinaValidarLogin(formu));
    }

    IEnumerator corrutinaValidarLogin(WWWForm form)
    {
        Debug.Log("LOGIN: Comenzando validación de datos...");
        using (UnityWebRequest request = UnityWebRequest.Post(serverUri, form)) {
            yield return request.SendWebRequest();

            if (request.isNetworkError || request.isHttpError)
            {
                Debug.Log("LOGIN: No he podido llamar al servidor");

                //System.Threading.Thread.Sleep(3000); //No se puede usar esto para esperar porque bloquea el programa
                validarLogin();
            }
            else
            {
                try
                {
                    Debug.Log("LOGIN: SERVIDOR RESPONDE: \n" + request.downloadHandler.text + " \n FIN RESPUESTA SERVIDOR");
                    DatosPlayer datosPlayerJson = JsonUtility.FromJson<DatosPlayer>(request.downloadHandler.text);
                    //Debug.Log("SERIALIZADO: un player -> " + JsonUtility.ToJson(datosPlayerJson));

                    //mockup de datosPlayer
                    DatosPlayer datosPlayerTest = new DatosPlayer();
                    datosPlayerTest.nickname = "nicknameLoginTest";
                    datosPlayerTest.dinero = 1;
                    datosPlayerTest.reputacion = 1;

                    //mockup de las unidades q tiene en su escuadron
                    //DatosUnidad du = new DatosUnidad(1, new TipoUnidad(1, "rasek", 50, 3, 6, 23, 46, 0, 12), "rasek", 5, 100);
                    //DatosUnidad du2 = new DatosUnidad(2, new TipoUnidad(1, "rasek", 50, 3, 6, 23, 46, 0, 12), "rusuk", 5, 100);
                    //datosPlayerTest.addUnidadEquipo(du);
                    //datosPlayerTest.addUnidadEquipo(du2);
                    //Debug.Log("el ejemplo sería: \n" + JsonUtility.ToJson(datosPlayerTest));


                    GameManager.instance.DatosPlayer = datosPlayerJson;
                    //cuando esté todo cargado, lo llevo a la siguiente escena
                    SceneManager.LoadScene("base");

                }
                catch (System.Exception e)
                {
                    Debug.Log("ERROR CARGANDO DATOS DE USUARIO: " + e);
                    //validarLogin();

                }
            }
        }
        
      
    }

}
