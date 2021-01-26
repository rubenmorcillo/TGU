using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class TestNPCMove : TestTacticsMove
{
    GameObject target;
    bool movementPayed = false;

    ComportamientoNPC comportamiento;



    //enum TipoComportamientoEnum { DEFAULT = 0}
    //TipoComportamientoEnum comportamiento;


    // Use this for initialization
    void Start()
    {
        comportamiento = new ComportamientoNPC();
        //comportamiento = TipoComportamientoEnum.DEFAULT;
        Init();
        animator.Play("Idle");
    }

    // Update is called once per frame
    void Update()
    {
        if (datosUnidad.estoyVivo)
        {
            Debug.DrawRay(transform.position, transform.forward);
            if (!turn)
            {
                return;
            }

            if (!moving)
            {
                animator.SetBool("moving", false);

                //TODO: Comportamiento de selección de ataques, etc
                comportamiento.DecidirSiguienteAccion(datosUnidad);
                habilidadSeleccionada = comportamiento.SeleccionarHabilidad();
                //Debug.Log("Soy " + name + " y me apetece atacar a " + target + " con " + habilidadSeleccionada.nombre);


                //si no estoy suficientemente cerca
                FindNearestTarget(); //busca al objetivo(Player) más cercano y lo guarda en target
                CalculatePath(); //calcula el camino hacia una posición donde poder golpear al target con la habilidad seleccionada
                //GetCurrentTile();
                //FindSelectableTiles(true);
                //actualTargetTile.target = true; //esto no hace ni falta



                //CHAPUZAAAA ???
                if (datosUnidad.puntosMovimientoActual <= 0 && datosUnidad.puntosEsfuerzoActual <= 0)
                {
                    MiTurnManager.EndTurn();
                }
            }
            else
            {
                if (!movementPayed && datosUnidad.puntosMovimientoActual > 0)
                {
                    if (path.Count > 1) //el path siempre incluye la casilla actual del muñeco
					{
                        SubstractMovementPoints(path.Count - 1);
                    }
                    movementPayed = true;
                }
                Move();
            }
        }
        else
        {
            RemoveMe();
            Destroy(gameObject);
        }
    }

    

    void CalculatePath()
    {
        movementPayed = false;
        Tile targetTile = null;
        
        
        if (habilidadSeleccionada?.id != 0)
        {
            currentTile = GetTargetTile(target);
            FindSelectableTiles(false);
            targetTile = FindNearestTile();
		}
		else
		{
           targetTile = GetTargetTile(target);
        }
        Debug.Log("tengo que ir a " + targetTile.name + "que está a una distancia de " + targetTile.distance);

        if (GetTargetTile(gameObject) != targetTile)
		{
            Debug.Log("yendo desde" + GetTargetTile(gameObject));
            FindPath(targetTile);
		}
		else
		{
            Debug.Log("así que no me tengo que mover");
		}
        
    }
    Tile FindNearestTile()
	{
        Tile nearest = null;
        float distance = Mathf.Infinity;
        //Debug.Log("Las casillas desde donde puedo usar mi skill son: (" +selectableTiles.Count+ ")");
        
        foreach (Tile t in selectableTiles)
        {
            //Debug.Log(t.name);
            float d = Vector3.Distance(transform.position, t.transform.position);
            //Debug.Log(t.name + " a "+d +" porque origen -> "+transform.position + " // objetivo: "+t.transform.position);
            if (d < distance)
			{
                distance = d;
                nearest = t;
			}
        }

        return nearest;
	}
    void FindNearestTarget()
    {
        GameObject[] targets = GameObject.FindGameObjectsWithTag("Player");

        GameObject nearest = null;
        float distance = Mathf.Infinity;

        foreach (GameObject obj in targets)
        {
            float d = Vector3.Distance(transform.position, obj.transform.position);

            if (d < distance)
            {
                distance = d;
                nearest = obj;
            }
        }

        target = nearest;
    }

    private void OnGUI()
    {
        //MOSTRAR VIDA DEL ENEMIGO
        //guardamos la posición del enemigo con respecto a la cámara.
        Vector2 pos = Camera.main.WorldToScreenPoint(transform.position);
        int offset = 40;
        GUI.Box(new Rect(pos.x - offset, Screen.height - (pos.y + offset), 80, 24), datosUnidad.hpActual + "/" + datosUnidad.hpMax);
    }
}
