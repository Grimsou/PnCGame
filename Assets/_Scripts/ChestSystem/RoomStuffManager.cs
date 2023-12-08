using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class RoomStuffManager : MonoBehaviour
{
    [Header("--- Room Settings ---")] 
    public string roomName;
    public bool drawGizmos;
    public bool showWireframe;

    [Space(10)]
    [Header("--- Crate Positions ---")]
    public List<Transform> cratePositions;
    
    [Header("!!! EDIT ONLY IF OBJECT PREFAB HAS CHANGED !!!")]
    public List<GizmosProperties> gizmosPropertiesList;

    private int _positionsCount;
    private List<Transform> childsList;

    private void Start()
    {
        this.gameObject.name = $"Room Stuff Manager : {roomName}";
        int childIndex = 1;
        foreach (Transform child in transform)
        {
            child.name = $"{roomName} Chest Position {childIndex}";
            cratePositions.Add(child);
            childIndex++;
        }
    }

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying)
        {
            if (drawGizmos && cratePositions != null)
            {
                foreach (var cratePosition in cratePositions)
                {
                    foreach (var gizmoProperties in gizmosPropertiesList)
                    {
                        if (gizmoProperties.drawGizmo)
                        {
                            gizmoProperties.gizmoRenderer._gMesh = gizmoProperties.gizmoRenderer.chest.chestMesh;
                            DrawGizmo(gizmoProperties, cratePosition);
                        }
                    }
                }
            }
        }
    }

    private void DrawGizmo(GizmosProperties gizmoProperties, Transform position)
    {
        if (gizmoProperties.gizmoRenderer != null && gizmoProperties.gizmoTransform != null && gizmoProperties.gizmoRenderer._gMesh != null)
        {
            Vector3 scaleVector = new Vector3(gizmoProperties.gizmoTransform.customScale, gizmoProperties.gizmoTransform.customScale, gizmoProperties.gizmoTransform.customScale);

            Quaternion rotation = Quaternion.Euler(gizmoProperties.gizmoTransform.offsetRotation) * position.rotation;

            Vector3 finalOffset = position.rotation * gizmoProperties.gizmoTransform.offsetPosition + position.position;

            Gizmos.color = gizmoProperties.gizmoRenderer._gColor;

            foreach (var mesh in gizmoProperties.gizmoRenderer._gMesh)
            {
                if (mesh != null)
                {
                    if (!showWireframe)
                    {
                        Gizmos.DrawMesh(mesh, 0, finalOffset, rotation, scaleVector);
                    }
                    else
                    {
                        Gizmos.DrawWireMesh(mesh, 0, finalOffset, rotation, scaleVector);
                    }
                }
            }
        }
    }
}

[System.Serializable]
public class GizmosProperties
{
    public string name = "Gizmo name";
    
    [Space(20)]
    public GizmoTransform gizmoTransform;

    [Space(20)]
    public GizmoRenderer gizmoRenderer;
    
    [Space(20)]
    public bool drawGizmo;
}

[System.Serializable]
public class GizmoTransform
{
    public Vector3 offsetPosition;
    public Vector3 offsetRotation;
    public float customScale;
}

[System.Serializable]
public class GizmoRenderer
{
    public RandomizedChestManager chest;
    public Color _gColor;
    [HideInInspector]
    public List<Mesh> _gMesh = new List<Mesh>();
}
