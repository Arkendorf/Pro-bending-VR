using UnityEngine;
using System;
using System.Collections;

[RequireComponent(typeof(Animator))]

public class IKControl : MonoBehaviour
{

    protected Animator animator;

    public bool ikActive = false;
    [Space(10)]
    public Transform headTarget;
    public Vector3 hipOffset = new Vector3(0, -.65f, -.1f);
    [Space(10)]
    public Transform leftHandTarget;
    public Transform rightHandTarget;
    [Space(10)]
    public Transform leftFootTarget;
    public Transform rightFootTarget;


    void Start()
    {
        animator = GetComponent<Animator>();
    }

    //a callback for calculating IK
    void OnAnimatorIK()
    {
        if (animator)
        {

            //if the IK is active, set the position and rotation directly to the goal. 
            if (ikActive)
            {

                // Set the look target position, if one has been assigned
                if (headTarget)
                {
                    animator.SetBoneLocalRotation(HumanBodyBones.Neck, headTarget.rotation);
                    transform.position = headTarget.position + hipOffset;
                }

                if (rightHandTarget)
                {
                    SetIK(AvatarIKGoal.RightHand, rightHandTarget.position, rightHandTarget.rotation * Quaternion.Euler(0, 0, -90f));
                }
                if (leftHandTarget)
                {
                    SetIK(AvatarIKGoal.LeftHand, leftHandTarget.position, leftHandTarget.rotation * Quaternion.Euler(0, 0, 90f));
                }

                if (rightFootTarget)
                {
                    SetIK(AvatarIKGoal.RightFoot, rightFootTarget.position, rightFootTarget.rotation);
                }
                if (leftFootTarget)
                {
                    SetIK(AvatarIKGoal.LeftFoot, leftFootTarget.position, leftFootTarget.rotation);
                }
            }

            //if the IK is not active, set the position and rotation of the hand and head back to the original position
            else
            {
                ResetIK(AvatarIKGoal.LeftHand);
                ResetIK(AvatarIKGoal.RightHand);
                ResetIK(AvatarIKGoal.LeftFoot);
                ResetIK(AvatarIKGoal.RightFoot);
            }
        }
    }

    private void SetIK(AvatarIKGoal limb, Vector3 position, Quaternion rotation)
    {
        animator.SetIKPositionWeight(limb, 1);
        animator.SetIKRotationWeight(limb, 1);
        animator.SetIKPosition(limb, position);
        animator.SetIKRotation(limb, rotation);
    }

    private void ResetIK(AvatarIKGoal limb)
    {
        animator.SetIKPositionWeight(limb, 0);
        animator.SetIKRotationWeight(limb, 0);
    }
}
