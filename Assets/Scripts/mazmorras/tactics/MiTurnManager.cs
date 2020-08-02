using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class MiTurnManager
{
    public static List<TacticsMove> unidadesCombate = new List<TacticsMove>();
    public static Queue<TacticsMove> unidadesTurno = new Queue<TacticsMove>();


    public void Update()
    {
        if (unidadesTurno.Count == 0)
        {
            IniciarColaTurnos();
        }
    }

    public static void AddUnit(TacticsMove unidad)
	{
        unidadesCombate.Add(unidad);
	}


    public static void IniciarColaTurnos()
	{
		IOrderedEnumerable<TacticsMove> orderedEnumerables = unidadesCombate.Where(u => u.datosUnidad.estoyVivo).OrderByDescending(u => u.datosUnidad.iniciativa);
        for (int i=0; i<orderedEnumerables.Count(); i++)
		{
            Debug.Log("TM: metiendo en cola a " + orderedEnumerables.ElementAt(i).datosUnidad.tipo.nombre);
            unidadesTurno.Enqueue(orderedEnumerables.ElementAt(i));
		}

        StartTurn();
    }

    public static void StartTurn()
    {
        if (unidadesTurno.Count > 0)
        {
            Debug.Log("TM: pickeando de la cola a " + unidadesTurno.First().datosUnidad.tipo.nombre);
            unidadesTurno.Peek().BeginTurn();

        }
    }

    public static void EndTurn()
    {
        
        TacticsMove unit = unidadesTurno.Dequeue();
        Debug.Log("TM: sacando de la cola a " + unit + " porque ha terminado su turno");
        unit.EndTurn();

        if (unidadesTurno.Count > 0)
        {
            StartTurn();
        }
        else
        {
            IniciarColaTurnos();
        }
    }
}
