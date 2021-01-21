using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class MiTurnManager
{
    public static List<TestTacticsMove> unidadesCombate = new List<TestTacticsMove>();
    public static Queue<TestTacticsMove> unidadesTurno = new Queue<TestTacticsMove>();


    public void Update()
    {
        if (unidadesTurno.Count == 0)
        {
            IniciarColaTurnos();
        }
    }

    public static void AddUnit(TestTacticsMove unidad)
	{
        unidadesCombate.Add(unidad);
	}


    public static void IniciarColaTurnos()
	{
        Debug.Log("TM: iniciando cola turnos...");
		IOrderedEnumerable<TestTacticsMove> orderedEnumerables = unidadesCombate.Where(u => u.datosUnidad.estoyVivo).OrderByDescending(u => u.datosUnidad.iniciativa);
        for (int i=0; i<orderedEnumerables.Count(); i++)
		{
            Debug.Log("TM: metiendo en cola a " + orderedEnumerables.ElementAt(i).datosUnidad.tipo.nombre +" cuya iniciativa es -> "+orderedEnumerables.ElementAt(i).datosUnidad.iniciativa);
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
        
        TestTacticsMove unit = unidadesTurno.Dequeue();
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
