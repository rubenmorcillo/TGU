﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayerMove : TestTacticsMove
{

    void Start()
    {
        Init();
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

            if (!moving)
            {
                animator.SetBool("moving", false);
                if (datosUnidad.puntosMovimientoActual <= 0 && datosUnidad.puntosEsfuerzoActual <= 0)
                {
                    MiTurnManager.EndTurn();
                }
                GetCurrentTile();
                if (habilidadSeleccionada.id != 0)
				{
                    FindSelectableTiles(false);
				}
				else
				{
                    FindSelectableTiles(true);
                }
                CheckMousePosition();
                CheckClick();
            }
            else
            {
                animator.SetBool("moving", true);
                Move();
            }
		}
		else
		{
            RemoveMe();
            Destroy(gameObject);
		}
        
    }

   

    public void CheckMousePosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray,out hit, Mathf.Infinity, ~(1 << 11)))
        {
            if (hit.collider.tag == "Tile")
            {
                Tile c = hit.collider.GetComponent<Tile>();

                if (c.selectable)
                {
                    if (habilidadSeleccionada?.id != 0)
                    {
                        //TODO: es nuestro turno, tenemos una habilidad seleccionada
                        //Este sería el onMouseOver
                    }
                    //toDo: comprobar qué hay en la casilla target
                    c.target = true;
                }
            }
        }
    }


    void CheckClick()
    {
        if (Input.GetMouseButtonUp(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, ~(1 << 11)))
            {
                if (hit.collider.tag == "Tile")
                {
                    Tile t = hit.collider.GetComponent<Tile>();

                    if (t.selectable)
                    {
                        if (habilidadSeleccionada?.id != 0)
                        {
                            //TODO: atacar al tile
                            Debug.Log(datosUnidad.tipo.nombre + ": atacando a la casilla " + t.name);
                            ShotSkill(habilidadSeleccionada, t);
                            t.DoSkill(habilidadSeleccionada);
                        }
                        else
                        {
                            if (t != currentTile)
							{
                                SubstractMovementPoints(t.distance);
                                MoveToTile(t);
                            }
                        }
                    }
                }
            }
        }
    }
}
