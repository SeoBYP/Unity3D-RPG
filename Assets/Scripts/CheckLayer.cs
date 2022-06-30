using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckLayer : MonoBehaviour
{
    public LayerMask layers;
    Vector3 cameraPos;
    Vector3 direction;
    Vector3 Halfsize;
    //public Collider target;
    float radius = 20f;
    float maxdistance = 40f;

    BaseEnemy[] enemys;
    public static bool CheckEnemycast;

    public void Init()
    {
        enemys = FindObjectsOfType<BaseEnemy>();
    }

    void Update()
    {
        if (enemys == null)
            return;
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(Camera.main);
        foreach(BaseEnemy enemy in enemys)
        {
            if (GeometryUtility.TestPlanesAABB(planes, enemy.enemyCollider.bounds))
            {
                cameraPos = transform.position;
                direction = (enemy.transform.position - cameraPos).normalized;
                Halfsize = Vector3.one;
                RaycastHit[] hits = Physics.BoxCastAll(cameraPos, Halfsize, direction, Quaternion.identity, maxdistance, layers);
                if (hits.Length != 0)
                {
                    foreach (RaycastHit hit in hits)
                    {
                        if (hit.collider == enemy.enemyCollider)
                        {
                            CheckEnemycast = true;
                        }

                    }
                }

            }
            else
                CheckEnemycast = false;
        }
    }

}
