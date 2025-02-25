using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyAnimationEventHandler : MonoBehaviour
{
    private Shooter shooter;

    private void Start()
    {
        shooter = GetComponentInParent<Shooter>();
    }

    public void ResetAttackState()
    {
        if (shooter != null)
        {
            shooter.ResetAttackState();
        }
    }
}