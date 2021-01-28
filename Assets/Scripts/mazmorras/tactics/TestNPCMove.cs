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

    bool readyToAttack = false;


    void Start()
    {
        comportamiento = new ComportamientoNPC(datosUnidad);
        Init();
        animator.Play("Idle");
    }

    void Update()
    {
        if (datosUnidad.estoyVivo)
        {
            Debug.DrawRay(transform.position, transform.forward);
            if (!turn)
            {
                return;
            }

            //CHAPUZAAAA (???) La siguiente comprobación va aquí???
            if (ImDone())  //Puedo hacer algo o tengo que terminar?
            {
                //movementPayed = false; //no sé si es necesario reiniciarlo, solo lo tiene el NPC
                ForceEndTurn();
                return;
            }

            if (!moving)
            {
                readyToAttack = false;
                animator.SetBool("moving", false);

                //TODO: Comportamiento de selección de ataques, etc
                //comportamiento.DecidirSiguienteAccion(datosUnidad);
                //if (atacar)
                habilidadSeleccionada = comportamiento.SeleccionarHabilidad(); //decido qué habilidad debería usar
                FindNearestTarget(); //busca al objetivo(Player) más cercano y lo guarda en target
                CalculatePath(); //calcula el camino hacia una posición donde poder golpear al target con la habilidad seleccionada
                if (readyToAttack)
				{
                    ShotSkill(habilidadSeleccionada, GetTargetTile(target));
				}
            }
            else
            {
                //nos aseguramos de que pagamos los puntos de movimiento
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
 

        if (targetTile != null && targetTile != GetTargetTile(gameObject))
		{
            FindPath(targetTile);
		}
		else
		{
            readyToAttack = true;
            Debug.Log(datosUnidad.alias+" en posición para usar la "+habilidadSeleccionada.nombre);
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
        //si vamos a atacar buscamos a los players
        GameObject[] targets = GameObject.FindGameObjectsWithTag("Player");
        //si fuesemos a curar el target sería otro

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
