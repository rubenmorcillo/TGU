using System;
using System.Collections.Generic;
using UnityEngine;

public class DatosPlayer
{
    public string Nickname { get; set; }
    public int Dinero { get; set; }
    public int Reputacion { get; set; }


   
    

    public string nickname;
    public int reputacion;
    public int dinero;

    //[SerializeField]
    //private DatosUnidad[] _coleccionUnidades;
    [SerializeField]
    List<DatosUnidad> equipoUnidades = new List<DatosUnidad>();

    // public string nickname

    //tb debería tener un modelo
    //private GameObject _avatarModelPrefab;




    public List<DatosUnidad> EquipoUnidades
    {
        get
        {
            return equipoUnidades;
        }
        set{
            equipoUnidades = value;
        }
    }

    public void addUnidadEquipo(DatosUnidad unidad)
    {
        equipoUnidades.Add(unidad);
    }

    public void removeUnidadEquipo(DatosUnidad unidad)
    {
        equipoUnidades.Remove(unidad);
    }
}
