using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public struct Attack {
    public float radius;
    public float cooldown;
}

public class UnitController : Killable {
    [Header("Movement Params")]
    public float speed = 5;
    public float rotationSpeed = 540;

    [Header("Basic Attack")]
    public float basicAttackRadius = 1;
    public float basicAttackCooldown = 2;
    public float basicAttackDamage = 10;
    private float basicAttackLastTime;
    private Killable target;

    // private Attack basicAttack = new() {
    //     radius = 2f,
    //     cooldown = 2f
    //     damage
    // };

    private Vector3 movementDirection;
    private CharacterController characterController;
    private float ySpeed;
    private Animator animator;

    void Start() {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        movementDirection = Vector3.forward;
        if (!characterController.isGrounded) {
            ySpeed = -2000f;
        }
    }

    void Update() {
        if (Input.GetMouseButtonDown(1)) {
            if (CameraController.RaycastMouse(out var hit)) {
                hit.point -= hit.point.y * Vector3.up;
                movementDirection = hit.point - transform.position;
            }
        }

        AdjustYSpeed();
        Aggro();


        movementDirection.Normalize();
        var velocity = movementDirection * speed;
        velocity.y += ySpeed;
        characterController.Move(velocity * Time.deltaTime);

        if (movementDirection != Vector3.zero) {
            animator.SetBool("isMoving", true);
            var dstRotation = Quaternion.LookRotation(movementDirection, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, dstRotation, rotationSpeed * Time.deltaTime);
        } else {
            animator.SetBool("isMoving", false);
        }
    }


    void AdjustYSpeed() {
        ySpeed += Physics.gravity.y * Time.deltaTime;
        if (characterController.isGrounded) {
            ySpeed = -2000f;
        }
    }

    public Killable ClosestEnemy() {
        return FindObjectsOfType<Killable>()
            .Where(guy => IsEnemy(guy))
            .OrderBy(
                guy => DistanceTo(guy)
            )
            .FirstOrDefault(x => x);
    }

    private void Aggro() {
        // default is not attacking
        animator.SetBool("isAttacking", false);
        target = ClosestEnemy();
        if (target == null) {
            return;
        }
        // if in attack range
        if (DistanceTo(target) > basicAttackRadius) {
            // move
            movementDirection = target.transform.position - transform.position;
        } else {
            // stop
            movementDirection = Vector3.zero;
            // hit
            BasicAttack(target);
        }
    }

    private void BasicAttack(Killable attackTarget) {
        if (Time.time - basicAttackLastTime < basicAttackCooldown) {
            return;
        }
        animator.SetBool("isAttacking", true);
        attackTarget.hp.TakeDamage(basicAttackDamage);
        basicAttackLastTime = Time.time;
    }

    private bool IsEnemy(Killable other) {
        return owner.team != other.owner.team;
    }

    private float DistanceTo(Killable other) {
        return other.DistanceFrom(this);
    }

    public override float DistanceFrom(Killable other) {
        return Vector3.Distance(transform.position, other.transform.position);
    }

}
