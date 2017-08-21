﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    public GameObject highBatPrefab;
    public GameObject lowBatPrefab;
    public GameObject endPrefab;
    public PlayerControllerV2 player;

    public int n = 0; //position counter
    public int distanceToDanger;
    public int[] level;
    public GameObject[] enemies;
    int enemyIndex = 0;

    public bool enemyAttack = false;
    bool damageable = false;

    void Start () {

        int levelMax = 40;
        // build level
        level = new int[levelMax];
        enemies = new GameObject[levelMax];
        // 9 = finish/level complete, 1 = high bat, 2 = low bat
        level[level.Length - 1] = 9;

        level[2] = 2;
        level[5] = 1;
        level[8] = 2;
        level[9] = 1;
        level[18] = 1;
        level[21] = 2;

        // spawn everything
        for (int i = 0; i < level.Length; i++)
        {
            if (level[i] == 1)
                enemies[i] = Instantiate<GameObject>(highBatPrefab, new Vector3((i) * 0.5f * 4, 1f * 4, 0), Quaternion.identity, transform.GetChild(2));

            if (level[i] == 2)
                enemies[i] = Instantiate<GameObject>(lowBatPrefab, new Vector3((i) * 0.5f * 4, 0.3f * 4, 0), Quaternion.identity, transform.GetChild(2));

            if (level[i] == 9)
            {
                Instantiate<GameObject>(endPrefab, new Vector3(i * 2f, -1.5f, 0), Quaternion.identity, transform);
            }
        }
    }

    void Update()
    {
        if (IsNextNDanger() && !enemyAttack && enemies[n + 1] != null && player.moving)
        {
            enemyAttack = true;
        }

        if (IsNextNDanger() && enemies[n + 1] != null)
        {
            if (damageable)
            {
                enemies[n + 1].GetComponentInChildren<SpriteRenderer>().color = Color.red;
            }
            else if (!damageable)
            {
                enemies[n + 1].GetComponentInChildren<SpriteRenderer>().color = Color.white;
            }
        }
	}


    public void DistanceToDanger()
    {
        distanceToDanger = 0;

        for (int i = 1; i <= 3; i++) // check for every danger type in front
        {
            if (level[n + 3] == i)
                distanceToDanger = 3;

            if (level[n + 2] == i)
                distanceToDanger = 2;

            if (level[n + 1] == i)
                distanceToDanger = 1;
        }
    }


    /// <summary>
    /// Informs player about nearest enemy location.
    /// </summary>
    public void GetClue()
    {
        DistanceToDanger();

        if (distanceToDanger == 3)
        {

        }
        else if (distanceToDanger == 2)
        {

        }

    }

    public int CheckForAttackHit()
    {
        DistanceToDanger();

        if (distanceToDanger == 1)
        {
            Debug.Log("Damageable Check");
            if (damageable == true)
            {
                level[n + 1] = 0;
                return 0;
            }
            else
            {
                return 2;
            }
        }
        else
        {
            return 2;
        }
    }

    bool IsNextNDanger()
    {
        if (level[n + 1] == 1 || level[n + 1] == 2 || level[n + 1] == 3)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public IEnumerator EnemyAttackAnimation()
    {
        Animator anim = enemies[n + 1].GetComponentInChildren<Animator>();
        anim.SetTrigger("Attack");
        anim.Update(0f);

        float minDamageableSec = 0;
        float maxDamageableSec = 0;

        // The window at which an enemy is damageable
        // timing varies by enemy type
        if (level[n + 1] == 1)
        {
            minDamageableSec = 0.40f;
            maxDamageableSec = 0.7f;
        }
        else if (level[n + 1] == 2)
        {
            minDamageableSec = 0.2333f;
            maxDamageableSec = 0.6f;
        }

        float currAnimTime;
        while (level[n + 1] != 0 && anim.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
        {

            currAnimTime = anim.GetCurrentAnimatorStateInfo(0).normalizedTime;
            if (maxDamageableSec >= currAnimTime && currAnimTime >= minDamageableSec)
            {
                damageable = true;
                //Debug.Log(anim.GetCurrentAnimatorStateInfo(0).normalizedTime);
            }
            else
            {
                damageable = false;
            }
            yield return null;
        }
        damageable = false;
        enemyAttack = false;

        //Debug.Log("Ended");

        Destroy(enemies[n + 1]);
    }

    public void doGameOver()
    {

    }
}