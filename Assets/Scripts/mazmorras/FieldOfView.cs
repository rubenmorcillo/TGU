using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{

    public float viewRadius;
    [Range (0,360)]
    public float viewAngle;

    public LayerMask unitsMask;
    public LayerMask coveragesMask;
    public LayerMask tilesMask;

    public List<Transform> visibleUnits = new List<Transform>();
    public List<Transform> visibleTiles = new List<Transform>();

	private void Start()
	{
        StartCoroutine("FindUnitsWithDelay", .2f);
	}

    IEnumerator FindUnitsWithDelay(float delay)
	{
		while (true)
		{
            yield return new WaitForSeconds(delay);
            FindVisibleUnits();
		}
	}
	void FindVisibleUnits()
	{
        visibleUnits.Clear();
        visibleTiles.Clear();
        Collider[] unitsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, unitsMask);
        Collider[] tilesInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, tilesMask);

        for (int i = 0; i < unitsInViewRadius.Length; i++)
		{
            Transform unit = unitsInViewRadius[i].transform;
            Vector3 directionToTarget = (unit.position - transform.position).normalized;
            if (Vector3.Angle (transform.forward, directionToTarget) < viewAngle / 2)
			{
                float distanceToTarget = Vector3.Distance(transform.position, unit.position);

                if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, coveragesMask))
				{
                    visibleUnits.Add(unit);
				}
			}
           
        }
        for (int i = 0; i < tilesInViewRadius.Length; i++)
        {
            Transform tile = tilesInViewRadius[i].transform;
            Vector3 directionToTarget = (tile.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, directionToTarget) < viewAngle / 2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, tile.position);

                if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, coveragesMask))
                {
                    visibleTiles.Add(tile);
                }
            }

        }

    }
    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
	{
        if (!angleIsGlobal)
		{
            angleInDegrees += transform.eulerAngles.y;
		}
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
	}

}
