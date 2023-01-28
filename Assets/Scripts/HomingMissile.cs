using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingMissile : MonoBehaviour
{
    private Transform target;
    [SerializeField] private float angleSpeed = 200;
    [SerializeField] private float moveSpeed = 7;
    public Rigidbody2D rb;
    private void Start()
    {
        GameObject[] allEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        float minDistance = Mathf.Infinity;
        GameObject closeEnemy = null;
        foreach (var enemy in allEnemies)
        {
            Enemy enemyTarget = enemy.GetComponent<Enemy>();
            float distance = Vector2.Distance(transform.position, enemy.transform.position);
            if (distance < minDistance && enemyTarget.isTargeted == false)
            {
                minDistance = distance;
                closeEnemy = enemy;
            }
        }

        if (closeEnemy != null)
        {
            target = closeEnemy.transform;
            closeEnemy.GetComponent<Enemy>().isTargeted = true;
        }
    }

    private void FixedUpdate()
    {
        if (target != null)
        {
            Vector2 direction = (Vector2)target.position - rb.position;
            direction.Normalize();
            float rotateAmount = Vector3.Cross(direction, transform.up).z;
            rb.angularVelocity = -angleSpeed * rotateAmount;
            rb.velocity = transform.up * moveSpeed;

        }
        else
        {
            transform.Translate(Vector2.up * moveSpeed * Time.deltaTime);
        }
    }
    private void OnDestroy()
    {
       if (target != null)
        {
            Enemy targetEnemy = target.GetComponent<Enemy>();
            if (targetEnemy != null)
            {
                if (targetEnemy.isTargeted == true && targetEnemy.isDestroyed == false)
                {
                    targetEnemy.isTargeted = false;
                }
            }
        }
    }

}

