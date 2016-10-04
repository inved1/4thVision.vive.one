using UnityEngine;
using System.Collections;

public class InteractableItem : MonoBehaviour {


    public Rigidbody rigibody;


    private bool currentlyInteracting;
    private WandController attachedWand;
    private Transform interactionPoint;

    private float velocityFactor = 20000f;
    private float rotationFactor = 400f;
   

    private Vector3 posDelta;
    private Quaternion rotDelta;
    private float angle;
    private Vector3 axis;

	// Use this for initialization
	void Start () {
        rigibody = GetComponent<Rigidbody>();
        interactionPoint = new GameObject().transform;
        velocityFactor /= rigibody.mass;
        rotationFactor /= rigibody.mass;
	
	}
	
	// Update is called once per frame
	void Update () {
	
        if(attachedWand && currentlyInteracting)
        {
            posDelta = attachedWand.transform.position - interactionPoint.position;
            this.rigibody.velocity = posDelta * velocityFactor * Time.fixedDeltaTime;


            rotDelta = attachedWand.transform.rotation * Quaternion.Inverse(interactionPoint.rotation);
            rotDelta.ToAngleAxis(out angle, out axis);
            if (angle > 180)
            {
                angle -= 360;
            }

            this.rigibody.angularVelocity = (Time.fixedDeltaTime * angle * axis) * rotationFactor;
        }
	}

    public void BeginInteraction(WandController wand)
    {
        attachedWand = wand;
        interactionPoint.position = wand.transform.position;
        interactionPoint.rotation = wand.transform.rotation;
        interactionPoint.SetParent(transform, true);

        currentlyInteracting = true;
    }

    public void EndInteraction(WandController wand)
    {
        if(wand == attachedWand)
        {
            attachedWand = null;
            currentlyInteracting = false;
        }
    }
    public bool IsInteraction()
    {
        return currentlyInteracting;
    }
}
