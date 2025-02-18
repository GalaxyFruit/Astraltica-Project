using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerIKController : MonoBehaviour
{
    [SerializeField] private Rig rigRightHand; 
    [SerializeField] private TwoBoneIKConstraint rightHandIK; 

    private void Start()
    {
        ToggleIK(false); 
    }

    public void SetRightHandIK(Transform newTarget)
    {
        if (newTarget != null)
        {
            rightHandIK.data.target = newTarget;
            ToggleIK(true);
        }
        else
        {
            Debug.LogWarning("RightPistolGrip nebyl nalezen! IK se vypíná.");
            ToggleIK(false);
        }
    }

    public void ToggleIK(bool state)
    {
        if (rigRightHand != null)
            rigRightHand.weight = state ? 1f : 0f;
    }
}
