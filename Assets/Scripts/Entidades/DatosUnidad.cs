using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DatosUnidad
{

    
    public DatosUnidad()
    {
        
    }
    public DatosUnidad(int id, TipoUnidad tipo, string name,int rangoMovimiento, int hp)
    {
        this.id = id;
        tipoUnidad = tipo;
        unitName = name;
        estoyVivo = true;
        hpMax = hp;
        hpActual = hpMax;
        this.rangoMovimiento = rangoMovimiento;
        modelPrefabName = unitName;
    }

    public int id { get; set; }
    
    public string unitName { set; get; }

    public TipoUnidad tipoUnidad { get; set; }

    public int hpMax { get; set; }

    public int hpActual { get; set; }

   

    public string modelPrefabName { get; set; }

    public int iniciativa { get; set; }

    public int defensaCerca { get; set; }

    public int defensaLejos { get; set; }

    public int rangoMovimiento { get; set; }
    public bool isPlaced { get; set; }

    public bool estoyVivo { get; set; }



    //creo que estas cosas las debo controlar en el combateManager

    //void Update()
    //{
    //    if (this.hpActual <= 0)
    //    {
    //        //he morido
    //        this.estoyVivo = false;
    //    }
    //}
}
