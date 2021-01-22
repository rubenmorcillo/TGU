﻿using System.Collections.Generic;
using UnityEngine;

public class TestTacticsMove : MonoBehaviour
{
    public bool turn = false;

    List<Tile> selectableTiles = new List<Tile>();
    GameObject[] tiles;

    protected Stack<Tile> path = new Stack<Tile>();
    protected Tile currentTile;

    public DatosUnidad datosUnidad;

    public bool moving = false;
    public float jumpHeight = 1;
    public float moveSpeed = 2;
    public float jumpVelocity = 5.5f;


    Vector3 velocity = new Vector3();
    Vector3 heading = new Vector3();

    float halfHeight = 0;
    float minDistance = 0.35f;


    bool fallingDown = false;
    bool jumpingUp = false;
    bool movingEdge = false;
    Vector3 jumpTarget;

    public Tile actualTargetTile;

    protected Animator animator;

    public Habilidad habilidadSeleccionada = null;

    public void setDatos(DatosUnidad du)
    {
        datosUnidad = du;
    }

    protected void Init()
    {
        tiles = GameObject.FindGameObjectsWithTag("Tile");

        halfHeight = GetComponent<Collider>().bounds.extents.y;

        animator = GetComponentInChildren<Animator>();

        //MiTurnManager.AddUnit(this);

    }

    public void GetCurrentTile()
    {
        currentTile = GetTargetTile(gameObject);
        currentTile.current = true;
    }

    public Tile GetTargetTile(GameObject target)
    {
        RaycastHit hit;
        Tile tile = null;

        if (Physics.Raycast(target.transform.position, -Vector3.up, out hit, 2))
        {
            tile = hit.collider.GetComponent<Tile>();
        }

        return tile;
    }

    public void ComputeAdjacencyLists(float jumpHeight, Tile target)
    {

        foreach (GameObject tile in tiles)
        {
            Tile t = tile.GetComponent<Tile>();
            if (habilidadSeleccionada?.id != 0)
			{
                t.FindNeighbors(jumpHeight, target, habilidadSeleccionada);
            }
            else
			{
                t.FindNeighbors(jumpHeight, target,null);
            }
        }
    }

   

    public void LimpiarTiles()
	{
        foreach(GameObject tile in tiles)
		{
            Tile t = tile.GetComponent<Tile>();
            t.Reset();
		}
	}

    public void FindSelectableTiles()
    {
        ComputeAdjacencyLists(jumpHeight, null);
        GetCurrentTile();

        Queue<Tile> process = new Queue<Tile>();

        process.Enqueue(currentTile);
        currentTile.visited = true;
        //currentTile.parent = ?? la casilla de origen tiene el padre NULL

        while (process.Count > 0)
        {
            Tile t = process.Dequeue();

            selectableTiles.Add(t);
            t.selectable = true;

            if ((habilidadSeleccionada.id != 0 && t.distance < habilidadSeleccionada.rango) || (habilidadSeleccionada.id == 0 && t.distance < datosUnidad.puntosMovimientoActual))
            {
                foreach (Tile tile in t.adjacencyList)
                {
                    if (!tile.visited)
                    {
                        tile.parent = t;
                        tile.visited = true;
                        tile.distance = 1 + t.distance;
                        process.Enqueue(tile);
                    }
                }
            }
           
        }

        //procesar otra vez para quitar las casillas de las habilidades que tengan un rango mínimo???
    }

    public void MoveToTile(Tile tile)
    {
        path.Clear();
        tile.target = true;

        moving = true;

        Tile next = tile;

        while (next != null)
        {
            path.Push(next);
            next = next.parent;
        }
    }

    public void Move()
    {
        if (path.Count > 0)
        {

            Tile t = path.Peek();

            Vector3 target = t.transform.position;
            //Calcular la posición de la unidad encima de la casilla (Tile) objetivo
            target.y += halfHeight + t.GetComponent<Collider>().bounds.extents.y;

            if (Vector3.Distance(transform.position, target) >= minDistance / 2)
            {
                bool jump = transform.position.y != target.y;

                if (jump)
                {
                    Jump(target);
                }
                else
                {
                    CalculateHeading(target);
                    SetHorizotalVelocity();
                }

                //Movimiento
                transform.forward = heading;
                transform.position += velocity * Time.deltaTime;
            }
            else
            {
                //Hemos llegado al centro
                transform.position = target;
                path.Pop();
            }
        }
        else
        {
            RemoveSelectableTiles();
            moving = false;
            //if (datosUnidad.puntosMovimientoActual <= 0 && datosUnidad.puntosEsfuerzoActual <= 0) si no ponemos esto, los enemigos cuando no atacan intentan moverse dos veces.
            // {
            //   MiTurnManager.EndTurn();
            //}
            if (datosUnidad.puntosMovimientoActual <= 0)
            {
                MiTurnManager.EndTurn();
            }

        }
    }

    protected void RemoveSelectableTiles()
    {
        if (currentTile != null)
        {
            currentTile.current = false;
            currentTile = null;
        }

        foreach (Tile tile in selectableTiles)
        {
            tile.Reset();
        }

        selectableTiles.Clear();
    }

    void CalculateHeading(Vector3 target)
    {
        heading = target - transform.position;
        heading.Normalize();
    }

    void SetHorizotalVelocity()
    {
        velocity = heading * moveSpeed;
    }

    void Jump(Vector3 target)
    {
        if (fallingDown)
        {
            FallDownward(target);
        }
        else if (jumpingUp)
        {
            JumpUpward(target);
        }
        else if (movingEdge)
        {
            MoveToEdge();
        }
        else
        {
            PrepareJump(target);
        }
    }

    void PrepareJump(Vector3 target)
    {
        float targetY = target.y;
        target.y = transform.position.y;

        CalculateHeading(target);

        if (transform.position.y > targetY)
        {
            fallingDown = false;
            jumpingUp = false;
            movingEdge = true;

            jumpTarget = transform.position + (target - transform.position) / 2.0f;
        }
        else
        {
            fallingDown = false;
            jumpingUp = true;
            movingEdge = false;

            velocity = heading * moveSpeed / 3.0f;

            float difference = targetY - transform.position.y;

            velocity.y = jumpVelocity * (0.5f + difference / 2.0f);
        }
    }

    void FallDownward(Vector3 target)
    {
        velocity += Physics.gravity * Time.deltaTime;

        if (transform.position.y <= target.y)
        {
            fallingDown = false;
            jumpingUp = false;
            movingEdge = false;

            Vector3 p = transform.position;
            p.y = target.y;
            transform.position = p;

            velocity = new Vector3();
        }
    }

    void JumpUpward(Vector3 target)
    {
        velocity += Physics.gravity * Time.deltaTime;

        if (transform.position.y > target.y)
        {
            jumpingUp = false;
            fallingDown = true;
        }
    }

    void MoveToEdge()
    {
        if (Vector3.Distance(transform.position, jumpTarget) >= minDistance)
        {
            SetHorizotalVelocity();
        }
        else
        {
            movingEdge = false;
            fallingDown = true;

            velocity /= 5.0f;
            velocity.y = 1.5f;
        }
    }

    protected Tile FindLowestF(List<Tile> list)
    {
        Tile lowest = list[0];

        foreach (Tile t in list)
        {
            if (t.f < lowest.f)
            {
                lowest = t;
            }
        }

        list.Remove(lowest);

        return lowest;
    }

    protected Tile FindEndTile(Tile t)
    {
        Stack<Tile> tempPath = new Stack<Tile>();

        Tile next = t.parent;
        while (next != null)
        {
            tempPath.Push(next);
            next = next.parent;
        }

        if (tempPath.Count <= datosUnidad.puntosMovimientoActual)
        {
            return t.parent;
        }

        Tile endTile = null;
        for (int i = 0; i <= datosUnidad.puntosMovimientoActual; i++)
        {
            endTile = tempPath.Pop();
        }

        return endTile;
    }

    protected void FindPath(Tile target)
    {
        ComputeAdjacencyLists(jumpHeight, target);
        GetCurrentTile();

        List<Tile> openList = new List<Tile>();
        List<Tile> closedList = new List<Tile>();
        openList.Add(currentTile);
        //currentTile.parent = ??
        currentTile.h = Vector3.Distance(currentTile.transform.position, target.transform.position);
        currentTile.f = currentTile.h;

        while (openList.Count > 0)
        {
            Tile t = FindLowestF(openList);

            closedList.Add(t);

            if (t == target)
            {
                actualTargetTile = FindEndTile(t);
                MoveToTile(actualTargetTile);
                return;
            }

            foreach (Tile tile in t.adjacencyList)
            {
                if (closedList.Contains(tile))
                {
                    //Do nothing, already processed
                }
                else if (openList.Contains(tile))
                {
                    float tempG = t.g + Vector3.Distance(tile.transform.position, t.transform.position);

                    if (tempG < tile.g)
                    {
                        tile.parent = t;

                        tile.g = tempG;
                        tile.f = tile.g + tile.h;
                    }
                }
                else
                {
                    tile.parent = t;

                    tile.g = t.g + Vector3.Distance(tile.transform.position, t.transform.position);
                    tile.h = Vector3.Distance(tile.transform.position, target.transform.position);
                    tile.f = tile.g + tile.h;

                    openList.Add(tile);
                }
            }
        }

        //no hay camino disponible...
        Debug.Log("No hay camino posible");
    }

    public void BeginTurn()
    {
        Debug.Log("empieza mi turno:" + datosUnidad.ToString());
        datosUnidad.RestorePoints();
        turn = true;
    }

    public void EndTurn()
    {
        turn = false;
        //CHAPUZAAAAA
        animator.SetBool("moving", false);
    }

    public void AplicarDamage(int damage)
	{
        //CHAPUZAAA -> habrá que implementar un algoritmo para calcular el daño real
        datosUnidad.PerderVida(damage);
	}


}