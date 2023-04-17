using System;
using Characters;
using Characters.Enemies;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private GameObject tar;
    private float speed = 300f;

    private int dmg;
    
    private void Update()
    {
        if (tar == null)
        {
            Destroy(gameObject);
        }
        else
        {
            var towards = Vector3.MoveTowards(transform.position, tar.transform.position, speed * Time.deltaTime);
            transform.position = towards;
        }
        
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.gameObject.CompareTag("Enemy")) return;
        Debug.Log("Hit Enemy");
        col.GetComponent<Enemy>().TakeDamage(dmg);
    }

    public void Spawn(GameObject agent, int direction, int damage, GameObject target, Vector3 spawnPt)
    {
        dmg = damage;
        
        var Down = 0;
        var Right = 2;
        var Up = 3;
        
        transform.SetParent(agent.transform);
        transform.localPosition = spawnPt;
        transform.localScale = Vector3.one;
        transform.localRotation = new Quaternion(0,0,direction==Right ? 0 : direction==Up ? 90 : direction==Down ? 270 : 180, 1);

        tar = target;
    }
}
