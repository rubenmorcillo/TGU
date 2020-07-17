using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MiTurnManager
{
    static List<TacticsMove> unidadesCombate = new List<TacticsMove>();
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
        //Debug.Log("quedan " + orderedEnumerables.Count() + "unidades vivas");
        for (int i=0; i<orderedEnumerables.Count(); i++)
		{
            unidadesTurno.Enqueue(orderedEnumerables.ElementAt(i));
		}

        StartTurn();
    }

    public static void StartTurn()
    {
        if (unidadesTurno.Count > 0)
        {
            unidadesTurno.Peek().BeginTurn();

        }
    }

    public static void EndTurn()
    {
        TacticsMove unit = unidadesTurno.Dequeue();
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
