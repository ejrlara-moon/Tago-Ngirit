using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRaycast2 : MonoBehaviour
{
    public static PlayerRaycast2 instance;

    float interactionDistance = 20f;
    bool isRaycast;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        // Define the Ray parameters

        Vector3 origin = transform.position;

        Vector3 direction = transform.TransformDirection(Vector3.forward);

        float distance = interactionDistance;

        RaycastHit hitInfo;


        if (Physics.Raycast(origin, direction, out hitInfo, distance))
        {

            Collider hitCollider = hitInfo.collider;

            Debug.DrawRay(origin, direction * hitInfo.distance, Color.red);



            if (isRaycast && hitCollider.CompareTag("TagoNgirit"))
            {
                AntagonistWatchingMechanic.instance.StopTagoNgiritWhenLook(true);
            }
            else
            {
                AntagonistWatchingMechanic.instance.StopTagoNgiritWhenLook(false);

            }

        }
        else
        {
            AntagonistWatchingMechanic.instance.StopTagoNgiritWhenLook(false);
        }









    }



    public void ActivateRaycast()

    {

        isRaycast = true;

    }



    public void DeactivateRaycast()

    {

        isRaycast = false;

    }

}