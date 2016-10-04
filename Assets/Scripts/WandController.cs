using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WandController : MonoBehaviour {




    //ref for buttons
    private Valve.VR.EVRButtonId gripButton = Valve.VR.EVRButtonId.k_EButton_Grip;
    private Valve.VR.EVRButtonId triggerButton = Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger;


    public  bool gripButtonDown = false;
    public  bool gripButtonUp = false;
    public  bool gripButtonPressed = false;
     
    public  bool triggerButtonDown = false;
    public  bool triggerButtonUp = false;
    public  bool triggerButtonPressed = false;

    

    private SteamVR_Controller.Device controller { get { return SteamVR_Controller.Input((int)trackedObj.index); } }
    private SteamVR_TrackedObject trackedObj;


    HashSet<InteractableItem> objectsHoveringOver = new HashSet<InteractableItem>();

    private InteractableItem closestItem;
    private InteractableItem interactingItem;

	void Start () {

        trackedObj = GetComponent<SteamVR_TrackedObject>();
	
	}
	
	// Update is called once per frame
	void Update () {
	
        if(controller == null)
        {
            Debug.Log("Controller not inizialised");
            return;
        }


        gripButtonDown = controller.GetPressDown(gripButton);
        gripButtonUp = controller.GetPressUp(gripButton);
        gripButtonPressed = controller.GetPress(gripButton);


        triggerButtonDown = controller.GetPressDown(triggerButton);
        triggerButtonUp = controller.GetPressUp(triggerButton);
        triggerButtonPressed = controller.GetPress(triggerButton);

        if (gripButtonDown) // wenn durch den trigger bereits collided
        {
            float minDistance = float.MaxValue;
            float distance;
            foreach(InteractableItem i in objectsHoveringOver)
            {
                distance = (i.transform.position - transform.position).sqrMagnitude;
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closestItem = i;
                }
            }

            interactingItem = closestItem;

            if (interactingItem)
            {
                if (interactingItem.IsInteraction())
                {
                    interactingItem.EndInteraction(this);
                }
                interactingItem.BeginInteraction(this);
            }
    

        }

        if (gripButtonUp && interactingItem != null)
        {
            interactingItem.EndInteraction(this);
        }

    }

    private void OnTriggerEnter(Collider collider)
    {
        InteractableItem collidedItem = collider.GetComponent<InteractableItem>();
        if (collidedItem)
        {
            objectsHoveringOver.Add(collidedItem);
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        InteractableItem collidedItem = collider.GetComponent<InteractableItem>();
        if (collidedItem)
        {
            objectsHoveringOver.Remove(collidedItem);
        }
    }
}
