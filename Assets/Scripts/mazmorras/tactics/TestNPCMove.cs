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

    //public List<TestTacticsMove> visiblePlayers;
	public DatosUnidad playerObjetivo;
    FieldOfView fieldOfView;

	void Start()
    {
        //visiblePlayers = new List<TestTacticsMove>();
        comportamiento = new ComportamientoNPC(datosUnidad);
        fieldOfView = GetComponent<FieldOfView>();
        Init();
        animator.Play("Idle");
    }

    void Update()
    {
        if (datosUnidad.estoyVivo)
        {
            
            CheckAroundCoverages();
            Debug.DrawRay(transform.position, transform.forward);
            if (!turn)
            {
                return;
            }
            
            if (ImDone())  //Puedo hacer algo o tengo que terminar?
            {
                ForceEndTurn();
                playerObjetivo = null;
                return;
            }
          /*  if (visibleTargets.Count == 0)
			{
                LookForTargets(); //Crea la lista de enemigos visibles
            }
          */
            if (!moving)
            {
                readyToAttack = false;
                animator.SetBool("moving", false);

                //primero pensamos el comportamiento. Por defecto busca la cobertura más cercana.
                //comportamiento.DecidirSiguienteAccion(datosUnidad);

                
                //compruebo que tengo alguna cobertura. Debería comprobar si estoy flanqueado
                if (!Covered())
				{
                    Debug.Log("no estoy a cubierto");
                    if (!datosUnidad.movimientoRealizado)
                    {
						FindNearestValidCoverage();
						//FindNearestTarget();
						CalculatePath();
					}
					else
					{
                        if (CanAttack(fieldOfView.visibleEnemies))
						{
                            //TODOO: de momento cojo el primer enemigo de los posibles y ya
                            playerObjetivo = fieldOfView.visibleEnemies.First().GetComponentInChildren<TestTacticsMove>().datosUnidad;
                            Debug.Log("voy a atacar");
                            Shot();
						}
						else
						{
                            //TODOOO: CHAPUZAAAA De momento voy a terminar turno
                            Debug.Log("no he podido atacar");
                            ForceEndTurn();


                        }
					}
				}
				else
				{
                    Debug.Log("Estoy cubierto. Quiero atacar");
                    if (CanAttack(fieldOfView.visibleEnemies))
                    {
                        //buscarPlayerObjetivo
                        playerObjetivo = fieldOfView.visibleEnemies.First().GetComponentInChildren<TestTacticsMove>().datosUnidad;
                        Debug.Log("tengo a la vista a "+ playerObjetivo +" .Voy a atacarle");
                        Shot();
					}
					else
					{
                        Debug.Log("no he podido atacar");
                        ForceEndTurn();
					}
                }

                //ahora tiene que comprobar si puede disparar
                //comprobamos rango de disparo
                
    //            if(ImDone())
				//{
    //                ForceEndTurn();
				//}
                //            habilidadSeleccionada = comportamiento.SeleccionarHabilidad(); //decido qué habilidad debería usar
                //            FindNearestTarget(); //busca al objetivo(Player) más cercano y lo guarda en target
                //            CalculatePath(); //calcula el camino hacia una posición donde poder golpear al target con la habilidad seleccionada
                //            if (readyToAttack)
                //{
                //                ShotSkill(habilidadSeleccionada, GetTargetTile(target));
                //}
                //else
                //{
                //                if (datosUnidad.rangoMovimiento <= 0)
                //	{
                //                   //ha llegado a su objetivo pero no tiene el objetivo en rango
                //                    ForceEndTurn();
                //                }
                //            }
            }
            else
            {
                //nos aseguramos de que pagamos los puntos de movimiento
                //TODO: ESTO ES NECESARIO???
     //           if (!movementPayed && datosUnidad.rangoMovimiento > 0)
     //           {
     //               if (path.Count > 1) //el path siempre incluye la casilla actual del muñeco
					//{
     //                   //SubstractMovementPoints(path.Count - 1);
     //               }
     //               movementPayed = true;
     //           }
                Move();
            }
        }
        else
        {
            RemoveMe();
            Destroy(gameObject);
        }
    }
    
    void Shot()
	{
        //le disparamos al objetivo seleccionado
        GameManager.instance.combateManager.Shot(datosUnidad, playerObjetivo);
        datosUnidad.accionRealizada = true;
         
	}
    bool Covered()
	{
        bool covered = false;
        foreach(DatosUnidad.CoverageUnidad cu in datosUnidad.coberturas)
		{
            if (cu.type != DatosUnidad.CoverageUnidad.CoverageType.NADA)
			{
                covered = true;
			}
		}

        return covered;
	}
    

    void CalculatePath()
    {
        movementPayed = false;
        Tile targetTile = null;

        //      if (habilidadSeleccionada?.id != 0)
        //      {
        //          currentTile = GetTargetTile(target);
        //          FindSelectableTiles(false);
        //          targetTile = FindNearestTile();
        //}
        //else
        //{
        //         targetTile = GetTargetTile(target);
        //      }

        Debug.Log("calculate path para el target: " + target);
        targetTile = GetTargetTile(target);
 

        if (targetTile != null && targetTile != GetTargetTile(gameObject))
		{
            FindPath(targetTile);
		}
		else
		{
            //TODO: esto hace falta???
            readyToAttack = true;
            if (GameManager.instance.mostrarDebug) Debug.Log(datosUnidad.alias+" en posición para "+accionSeleccionada.type);
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
 //   bool IsValidCoverage(GameObject coverage)
	//{
 //       bool valid = false;
 //       foreach(GameObject visibleTarget in visibleTargets)
	//	{
	//	List<GameObject> lists = visibleTargets.ToList();
 //           float xCover = coverage.transform.position.x;
 //           if (transform.localScale.z >= 1)
 //           {
 //               if (xObjetivo > xWall)
 //               {
 //                   //muro izquierda, objetivo derecha
 //                   if (xWall > xUnidad)
 //                   {
 //                       //unidad izq, muro decha => cobertura oeste
 //                       currentOrientationCoverage.Add(directionName.OESTE);
 //                   }
 //               }
 //               else
 //               {
 //                   if (xWall < xUnidad)
 //                   {
 //                       currentOrientationCoverage.Add(directionName.ESTE);
 //                   }
 //               }
 //               if (xWall > xObjetivo)
 //               {
 //                   //muro a la derecha, objetivo izq
 //                   if (xUnidad > xWall)
 //                   {
 //                       currentOrientationCoverage.Add(directionName.ESTE);
 //                   }
 //               }
 //               else
 //               {
 //                   if (xUnidad < xWall)
 //                   {
 //                       currentOrientationCoverage.Add(directionName.OESTE);
 //                   }
 //               }
 //           }

 //           if (transform.localScale.x >= 1)
 //           {
 //               if (zObjetivo > zWall)
 //               {
 //                   //muro abajo, objetivo arriba
 //                   if (zWall > zUnidad)
 //                   {
 //                       //unidad abajo, muro arriba => cobertura oeste
 //                       currentOrientationCoverage.Add(directionName.SUR);
 //                   }
 //               }
 //               else
 //               {
 //                   if (zWall < zUnidad)
 //                   {
 //                       currentOrientationCoverage.Add(directionName.NORTE);
 //                   }
 //               }
 //               if (zWall > zObjetivo)
 //               {
 //                   //muro a la arriba, objetivo abajo
 //                   if (zUnidad > zWall)
 //                   {
 //                       currentOrientationCoverage.Add(directionName.NORTE);
 //                   }
 //               }
 //               else
 //               {
 //                   if (zUnidad < zWall)
 //                   {

 //                       currentOrientationCoverage.Add(directionName.SUR);
 //                   }
 //               }
 //           }
 //       }
        
 //       return valid;
	//}
    void FindNearestValidCoverage()
	{
        Debug.Log("buscando covertura válida");
        GameObject[] muros = GameObject.FindGameObjectsWithTag("Muro");
        List<GameObject> coverages = new List<GameObject>();
        List<GameObject> validCoverages = new List<GameObject>();

        foreach (GameObject wall in muros)
        {
            if (wall.GetComponent<Coverage>() != null)
            {
                coverages.Add(wall);
            }
        }
        Debug.Log("tenemos " + coverages.Count() + " coberturas");
		foreach (GameObject cover in coverages)
		{
			cover.GetComponent<Coverage>().UpdateOrientationCoverages(gameObject, GameManager.instance.combateManager.aliados);

		}
        //TODO: añadir la lista de validCoverages
        foreach (GameObject cover in coverages)
		{
			//if (IsValidCoverage(cover))
			//{
				validCoverages.Add(cover);
			//}
		}
        //ir a una casilla donde tenga cobertura

        Debug.Log("buscando la cobertura más cercana.\nTenemos "+validCoverages.Count()+" coberturas válidas");
		GameObject nearest = null;
        float distance = Mathf.Infinity;

        if (validCoverages.Count() > 0)
		{
            //en realidad no me sirve cualquier cobertura, depende de donde estén situados los players :D NORTE, SUR ,ESTE, OESTE
            foreach (GameObject obj in validCoverages)
            {
                float d = Vector3.Distance(transform.position, obj.transform.position);

                if (d < distance)
                {
                    distance = d;
                    nearest = obj;
                }
            }
		}
		else
		{
            //no hay coberturas válidas
		}
       
        //esto está mal porque estoy yendo a cualquier casilla entre yo y la cobertura
        //TODO: cada cobertura debe actualizarse y calcular qué cobertura ofrece (NORTE, SUR, ESTE, OESTE) en función  de la posición de los enemigos
        target = nearest;
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

    //private void OnGUI()
    //{
    //    //MOSTRAR VIDA DEL ENEMIGO
    //    //guardamos la posición del enemigo con respecto a la cámara.
    //    Vector2 pos = Camera.main.WorldToScreenPoint(transform.position);
    //    int offset = 40;
    //    GUI.Box(new Rect(pos.x - offset, Screen.height - (pos.y + offset), 80, 24), datosUnidad.hpActual + "/" + datosUnidad.hpMax);
    //}
}
