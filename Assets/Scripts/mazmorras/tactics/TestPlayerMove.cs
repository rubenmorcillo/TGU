using System.Collections;
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
        Debug.DrawRay(transform.position, transform.forward);

        if (!turn)
        {
            return;
        }

        if (!moving)
        {
            animator.SetBool("moving", false);
           
            FindSelectableTiles();

            CheckMousePosition();
            CheckMouse();
        }
        else
        {
            animator.SetBool("moving", true);
            Move();
        }
    }

   

    public void CheckMousePosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
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


    void CheckMouse()
    {
        if (Input.GetMouseButtonUp(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
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
                            t.DoSkill(habilidadSeleccionada);
                        }
                        else
                        {
                            datosUnidad.SubstractMovementPoints(t.distance);
                            MoveToTile(t);
                        }


                    }
                }

            }
        }
    }
}
