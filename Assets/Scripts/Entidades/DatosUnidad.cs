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
        this.tipo = tipo;
        alias = name;
        estoyVivo = true;
        hpMax = hp;
        hpActual = hpMax;
        this.rangoMovimiento = rangoMovimiento;
		modelPrefabName = tipo.nombre;
        exp = 0;
        nivel = 1;
	}
    public int id;

    public string alias;

    public TipoUnidad tipo;

    public int nivel;

    public int exp;

    public int hpMax;

    public int hpActual;

    public string modelPrefabName;

    public int iniciativa;

    public int defensaCerca;

    public int defensaLejos;

    public int rangoMovimiento;
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
