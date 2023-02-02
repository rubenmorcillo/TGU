using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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
            UpdateCoveragesDirections();
            CheckAroundCoverages();
            if (!turn)
            {
                return;
            }
            if (ImDone())  //Puedo hacer algo o tengo que terminar?
            {
                MiTurnManager.EndTurn();
                return;
            }
            
            if (!moving)
            {
                animator.SetBool("moving", false);
                
                GetCurrentTile();
    //            if (!datosUnidad.movimientoRealizado)
				//{
                    
				//}
    //            if (habilidadSeleccionada.id != 0)
				//{
    //                FindSelectableTiles(false);
				//}
				//else
				//{
    //                FindSelectableTiles(true);
    //            }
                FindSelectableTiles(true);
                CheckMousePosition();
                CheckClick(); //TODO: revisar esto porque depende de la acción seleccionada...
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
                    //if (habilidadSeleccionada?.id != 0)
                    //{
                    //    //TODO: es nuestro turno, tenemos una habilidad seleccionada
                    //    //Este sería el onMouseOver
                    //}
                    //toDo: comprobar qué hay en la casilla target
                    c.target = true;
                    //TODO: mostrar las coberturas
                   
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
                        //if (habilidadSeleccionada?.id != 0)
                        //{
                        //    //TODO: atacar al tile
                        //    //ShotSkill(habilidadSeleccionada, t);
                        //}
                        //else
                        //{
                            if (t != currentTile)
							{
                                //SubstractMovementPoints(t.distance);
                                MoveToTile(t);
                            }
                        //}
                    }
                }
            }
        }
    }
    protected void UpdateCoveragesDirections()
    {

        GameObject[] muros = GameObject.FindGameObjectsWithTag("Muro");
        List<GameObject> coverages = new List<GameObject>();

        foreach (GameObject wall in muros)
        {
            if (wall.GetComponent<Coverage>() != null)
            {
                coverages.Add(wall);
            }
        }
        //Debug.Log("tenemos " + coverages.Count() + " coberturas");
        foreach (GameObject cover in coverages)
        {

            //CHAPUZA, CAMBIAR ENEMIGOS POR ENEMIGOS VISIBLES(???)
            cover.GetComponent<Coverage>().UpdateOrientationCoverages(gameObject, GameManager.instance.combateManager.enemigos);

        }

    }
}
