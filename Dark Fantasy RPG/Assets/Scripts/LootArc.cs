using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootArc : MonoBehaviour
{
    public bool expires = true;
    public float expiryTime = 10;
    Vector3 target, target_pos;
    int current_target = 0;
    public float distance = 3;
    public float move_speed = 1;
    public float max_height = 3, min_height = 0.5f;
    Vector3 point0, point1, point2;
    private int num_points = 100;
    private Vector3[] positions;
    bool landed = false;


    private void Start()
    {
        target = new Vector3(transform.position.x + Random.Range(-distance, distance), transform.position.y + Random.Range(-distance / 3, distance / 3));
        positions = new Vector3[num_points];
        point0 = transform.position;
        Vector3 difference = point2 - point0;
        Vector3 center = point0 + difference * 0.5f;
        point1 = new Vector2(center.x, target.y + Random.Range(min_height, max_height));
        point2 = target;
        if (expires)
        {
            StartCoroutine(Expire());
        }
    }

    IEnumerator Expire()
    {
        yield return new WaitForSeconds(expiryTime);
        Destroy(gameObject);
    }

    private void Update()
    {
        if (!landed)
        {


            DrawQuadraticCurve();
            target_pos = positions[current_target];
            transform.position = Vector3.MoveTowards(transform.position, target_pos, move_speed * Time.deltaTime);

            if (transform.position == target_pos && current_target < num_points)
            {
                current_target++;
            }
        }


        if (transform.position == target)
        {
            landed = true;
        }
    }

    private void DrawQuadraticCurve()
    {
        for (int i = 1; i < num_points + 1; i++)
        {
            float t = i / (float)num_points;
            positions[i - 1] = CalculateQuadraticBezierPoint(t, point0, point1, point2);
        }
    }

    private Vector3 CalculateQuadraticBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        Vector3 p = uu * p0;
        p += 2 * u * t * p1;
        p += tt * p2;
        return p;
    }
}
