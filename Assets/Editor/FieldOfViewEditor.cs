using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomEditor (typeof (FieldOfView))]
public class FieldOfViewEditor : Editor
{
	private void OnSceneGUI()
	{
		FieldOfView fow = (FieldOfView)target;
		Handles.color = Color.white;
		Vector3 offsetY = new Vector3(0, -2, 0);
		Handles.DrawWireArc(fow.transform.position + offsetY, Vector3.up, Vector3.forward, 360, fow.viewRadius);
		Vector3 viewAngleA = fow.DirFromAngle(-fow.viewAngle / 2, false);
		Vector3 viewAngleB = fow.DirFromAngle(fow.viewAngle / 2, false);

		Handles.DrawLine(fow.transform.position, fow.transform.position + viewAngleA * fow.viewRadius);
		Handles.DrawLine(fow.transform.position, fow.transform.position + viewAngleB * fow.viewRadius);

		foreach (Transform visibleUnit in fow.visibleUnits)
		{
			Handles.DrawLine(fow.transform.position, visibleUnit.transform.position);
		}
		foreach (Transform visibleTile in fow.visibleTiles)
		{
			Handles.DrawLine(fow.transform.position, visibleTile.transform.position);
		}
	}
}
