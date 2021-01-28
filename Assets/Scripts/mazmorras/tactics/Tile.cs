﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Tile : MonoBehaviour 
{
    public bool walkable = true;
    public bool current = false;
    public bool target = false;
    public bool selectable = false;
    public bool spawnEnemigo = false;
    public bool spawnUnidad = false;

    public List<Tile> adjacencyList = new List<Tile>();

    //Needed BFS (breadth first search)
    public bool visited = false;
    public Tile parent = null;
    public int distance = 0;

    //For A*
    public float f = 0;
    public float g = 0;
    public float h = 0;

	// Use this for initialization
	void Start () 
	{
        gameObject.AddComponent<NavMeshSurface>();
	}
	
	// Update is called once per frame
	void Update () 
	{
       
            if (current)
            {
                GetComponent<Renderer>().material.color = Color.cyan;
            }
            else if (target)
            {
                GetComponent<Renderer>().material.color = Color.green;
            }
            else if (selectable)
            {
                GetComponent<Renderer>().material.color = Color.blue;
            }
            else
            {
                GetComponent<Renderer>().material.color = Color.white;
            }
        
       
	}

    public void Reset()
    {
        adjacencyList.Clear();

        current = false;
        target = false;
        selectable = false;

        visited = false;
        parent = null;
        distance = 0;

        f = g = h = 0;
    }

    public void FindNeighbors(float jumpHeight, Tile target, Habilidad habilidad, bool move)
    {
        Reset();
       
        CheckTile(Vector3.forward, jumpHeight, target, habilidad, move);
        CheckTile(-Vector3.forward, jumpHeight, target, habilidad, move);
        CheckTile(Vector3.right, jumpHeight, target, habilidad, move);
        CheckTile(-Vector3.right, jumpHeight, target, habilidad, move);
    }


    public void CheckTile(Vector3 direction, float jumpHeight, Tile target, Habilidad habilidad, bool move)
    {
        Vector3 halfExtents = new Vector3(0.25f, (1 + jumpHeight) / 2.0f, 0.25f);
        Collider[] colliders = Physics.OverlapBox(transform.position + direction, halfExtents);
        foreach (Collider item in colliders)
        {
            Tile tile = item.GetComponent<Tile>();
            //CHAPUZAAA: con la última condicion...los ataques de los enemigos no pueden traspasar muros y eso está MAL
            if ((tile != null && tile.walkable) || (tile != null && !move))
            {
                if (move)
				{
                    RaycastHit hit;
                    if (!Physics.Raycast(tile.transform.position, Vector3.up, out hit, 1) || (tile == target))
                    {
                        adjacencyList.Add(tile);
                    }
					else
					{
                        //TODO: si quiero disparar a los obstaculos tengo que ver qué hay encima del Tile
                    }
                }
				else
				{
                    adjacencyList.Add(tile);
				}
               
            }
        }
    }
    public void DoDamage(Damage damage)
	{
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.up, out hit, 1))
        {
            TestTacticsMove posibleObjetivo = hit.collider.gameObject.GetComponent<TestTacticsMove>();
            if (posibleObjetivo)
			{
                //CHAPUZAAA según la habilidad habrá que aplicar damage o hacer otro efecto
                posibleObjetivo.AplicarDamage(damage);
                Debug.Log("Le han dado a " + posibleObjetivo.datosUnidad.alias + " con " + damage);
			}
			else
			{
                Debug.Log("No le has dado a nada");
			}
        }
    }
}
