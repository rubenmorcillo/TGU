using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DatosPlayer
{
    public string nickname;
    public int reputacion;
    public int dinero;

    //[SerializeField]
    //private DatosUnidad[] _coleccionUnidades;
    [SerializeField]
    List<DatosUnidad> equipoUnidades = new List<DatosUnidad>();

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
