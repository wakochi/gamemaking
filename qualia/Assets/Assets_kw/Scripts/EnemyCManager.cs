using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCManager : MonoBehaviour
{
    [SerializeField] GameObject enemyDeathEffect;
    public GameObject attackObj;
    public bool canAttack = true;

    GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (canAttack)
        {
            Attack();
        }
    }

    public void Attack()
    {
        GameObject g = Instantiate(attackObj);
        g.transform.SetParent(transform);
        g.transform.position = attackObj.transform.position;
        g.transform.rotation = attackObj.transform.rotation;
        g.SetActive(true);
        canAttack = false;
        StartCoroutine(AttackFlagOn());
    }

    private IEnumerator AttackFlagOn()
    {
        yield return new WaitForSeconds(2.0f);
        canAttack = true;
    }

    public void DestroyEnemy()
    {
        gameManager.Addscore(100);
        Instantiate(enemyDeathEffect, this.transform.position, this.transform.rotation);
        Destroy(this.gameObject);
    }
}
