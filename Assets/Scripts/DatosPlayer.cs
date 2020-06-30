using System.Collections.Generic;
using UnityEngine;

[SerializeField]
public class DatosPlayer
{

    public string nickname { get; set; }
    public int dinero { get; set; }
    public int reputacion { get; set; }


    //[SerializeField]
    //private DatosUnidad[] _coleccionUnidades;
    [SerializeField]
    List<DatosUnidad> _equipoUnidades = new List<DatosUnidad>();


    //tb debería tener un modelo
    private GameObject _avatarModelPrefab;


    //private void Start()
    //{
    //    int id;

    //}
  
   

    //public Unidad[] coleccionUnidades
    //{
    //    get
    //    {
    //        return _coleccionUnidades;
    //    }
    //    set
    //    {
    //        _coleccionUnidades = value;
    //    }
    //}

    public List<DatosUnidad> equipoUnidades
    {
        get
        {
            return _equipoUnidades;
        }
        set{
            _equipoUnidades = value;
        }
    }

    public void addUnidadEquipo(DatosUnidad unidad)
    {
        _equipoUnidades.Add(unidad);
    }

    public void removeUnidadEquipo(DatosUnidad unidad)
    {
        _equipoUnidades.Remove(unidad);
    }
}
