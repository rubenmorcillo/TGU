using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : TacticsMove 
{

	void Start () 
	{
        Init();
	}
	
	void Update () 
	{
        Debug.DrawRay(transform.position, transform.forward);

        if (!turn)
        {
            return;
        }

        if (!moving)
        {
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
                    if (habilidadSeleccionada != null)
					{
                        //TODO: es nuestro turno, tenemos una habilidad seleccionada, debemos mostrar el rango efectivo de la habilidad
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
                        if (habilidadSeleccionada != null)
						{
                            //TODO: debemos aplicar el efecto de la habilidad en la(s) casilla(s) correspondiente(s)
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
