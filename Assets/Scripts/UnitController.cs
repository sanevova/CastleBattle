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
    private static readonly List<string> kAttackAnimationNames = new() {
        "Martelo 2",
        "Mma Kick"
    };

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
        if (IsDead()) {
            return;
        }

        AdjustYSpeed();
        Aggro();
        Move();
        Turn();
    }

    void Move() {
        movementDirection.Normalize();
        var velocity = CanMove() ? movementDirection : Vector3.zero;
        velocity *= speed;
        velocity.y += ySpeed;
        characterController.Move(velocity * Time.deltaTime);

    }
    void Turn() {
        var dstRotation = Quaternion.LookRotation(movementDirection, Vector3.up);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, dstRotation, rotationSpeed * Time.deltaTime);
    }

    void AdjustYSpeed() {
        ySpeed += Physics.gravity.y * Time.deltaTime;
        if (characterController.isGrounded) {
            ySpeed = -2000f;
        }
    }

    public Killable ClosestEnemy() {
        return FindObjectsOfType<Killable>()
            .Where(guy => guy != null && IsEnemy(guy) && guy.IsAlive())
            .OrderBy(guy => DistanceTo(guy))
            .FirstOrDefault(x => x);
    }

    private bool IsInAttackAnimation() {
        var clip = animator.GetCurrentAnimatorClipInfo(0)[0].clip;
        return kAttackAnimationNames.Contains(clip.name);
    }

    private bool CanAttack() {
        return DistanceTo(target) <= basicAttackRadius // within attack range
            && target.IsAlive();
    }

    private bool CanMove() {
        return !CanAttack() && !IsInAttackAnimation();
    }

    private void Aggro() {
        // default is not attacking
        animator.SetBool("isAttacking", false);
        animator.SetBool("isDead", false);
        target = ClosestEnemy();
        if (target == null) {
            return;
        }

        if (CanAttack()) {
            BasicAttack(target);
        }

        if (CanMove()) {
            movementDirection = target.transform.position - transform.position;
            animator.SetBool("isMoving", true);
        } else {
            animator.SetBool("isMoving", false);
        }
    }

    private void BasicAttack(Killable attackTarget) {
        if (Time.time - basicAttackLastTime < basicAttackCooldown) {
            return;
        }
        animator.SetBool("isAttacking", true);
        attackTarget.hp.TakeDamageFrom(this, basicAttackDamage);
        basicAttackLastTime = Time.time;
    }

    private float DistanceTo(Killable other) {
        return other.DistanceFrom(this);
    }

    public override float DistanceFrom(Killable other) {
        return Vector3.Distance(transform.position, other.transform.position);
    }

    public override void Die() {
        animator.Play("Death");
        characterController.enabled = false;
        base.Die();
        Destroy(gameObject, 3f);
    }
}
