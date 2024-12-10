
using System;
using UnityEngine;

using UnityEngine.Events;

public class RaycastHandler
{
    Ray ray;
    RaycastHit hitInfo;
    Camera camera;
    LayerMask layersToHit;
    public Action<RaycastHit> OnHit;
    float maxDistance = 0;

    public RaycastHandler(Camera camera, LayerMask layersToHit, float maxDistance = float.PositiveInfinity)
    {
        this.camera = camera;
        this.layersToHit = layersToHit;
        this.maxDistance = maxDistance;
    }
    
    public void RaycastFromScreen(Vector2 touchPosition)
    {
        if(camera==null)
        {
            Debug.LogError("CANNOT RAYCAST WITHOUT CAMERA");
            return;
        }
        else
        {
            ray = camera.ScreenPointToRay(touchPosition);
            if(Physics.Raycast(ray.origin,ray.direction,out hitInfo,maxDistance, layersToHit,QueryTriggerInteraction.UseGlobal))
            {
                OnHit?.Invoke(hitInfo);
            }

        }
    }
}
