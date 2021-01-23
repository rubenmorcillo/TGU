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
                Debug.Log("Soy " + name + " y me apetece atacar a " + target + " con " + habilidadSeleccionada.nombre);
                

                //si no estoy suficientemente cerca
                FindNearestTarget(); //busca al objetivo(Player) más cercano y lo guarda en target
                CalculatePath();
                GetCurrentTile();
                FindSelectableTiles();
                actualTargetTile.target = true;



                //CHAPUZAAAA
                //Debug.Log("La distancia con el tile real es: " + actualTargetTile.distance);
                //if (actualTargetTile.distance <= 0)
                //{
                //    //TODO: aqui debo hacer las habilidades por lo visto(???)
                //    MiTurnManager.EndTurn();
                //}
                //este es el verdadero fin del turno
                if (datosUnidad.puntosMovimientoActual <= 0 && datosUnidad.puntosEsfuerzoActual <= 0)
                {
                    MiTurnManager.EndTurn();
                }
            }
            else
            {
               
                
                if (!movementPayed && datosUnidad.puntosMovimientoActual > 0)
                {
                    SubstractMovementPoints(actualTargetTile.distance);
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
        //TODO: el camino depende de la habilidad seleccionada
        if (habilidadSeleccionada?.id != 0)
        {
            //debo decidir el nuevo target
            Debug.Log("llevo una habilidad seleccionada");
            //lo primero es posicionar la habilidad en el target
            currentTile = GetTargetTile(target);
            FindSelectableTiles();
			targetTile = FindNearestTile();
		}
		else
		{
           targetTile = GetTargetTile(target);
        }
        //targetTile = GetTargetTile(target);
        FindPath(targetTile);
    }
    Tile FindNearestTile()
	{
        Tile nearest = null;
        float distance = Mathf.Infinity;

        foreach (Tile t in selectableTiles)
        {
            float d = Vector3.Distance(transform.position, t.transform.position);
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
