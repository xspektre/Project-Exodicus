using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKHandling : MonoBehaviour {

    Animator animator;

    public float ikWeight = 1;

    public Transform leftIKTarget;
    public Transform rightIKTarget;

    public Transform hintLeft;
    public Transform hintRight;


	// Use this for initialization
	void Start () {

        animator = GetComponent<Animator>();
		
	}
	
	// Update is called once per frame
	void Update () {
        OnAnimatorIK();
	}

    private void OnAnimatorIK()
    {
        animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, ikWeight);
        animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, ikWeight);

        animator.SetIKPosition(AvatarIKGoal.LeftFoot, leftIKTarget.position);
        animator.SetIKPosition(AvatarIKGoal.RightFoot, rightIKTarget.position);

        animator.SetIKHintPositionWeight(AvatarIKHint.LeftKnee, ikWeight);
        animator.SetIKHintPositionWeight(AvatarIKHint.RightKnee, ikWeight);

        animator.SetIKHintPosition(AvatarIKHint.LeftKnee, hintLeft.position);
        animator.SetIKHintPosition(AvatarIKHint.RightKnee, hintRight.position);

        animator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, ikWeight);
        animator.SetIKRotationWeight(AvatarIKGoal.RightFoot, ikWeight);

        animator.SetIKRotation(AvatarIKGoal.LeftFoot, leftIKTarget.rotation);
        animator.SetIKRotation(AvatarIKGoal.RightFoot, rightIKTarget.rotation);
    }
}
