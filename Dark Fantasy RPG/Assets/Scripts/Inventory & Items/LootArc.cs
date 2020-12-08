using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootArc : MonoBehaviour
{
    public bool arc = false;
    public bool expires = true;
    public float expiryTime = 10;
    Vector2 target, target_pos;
    int current_target = 0;
    public float distance = 64;
    public float move_speed = 500;
    public float max_height = 3, min_height = 0.5f;
    Vector2 startPos, middlePos, endPos;
    private int num_points = 100;
    private Vector2[] positions;
    bool landed = false;
    bool start = false;

    private void Start()
    {
        StartCoroutine(SetupRoutine());
    }

    IEnumerator SetupRoutine()
    {
        bool clearTarget = false;
        while (!clearTarget)
        {
            target = new Vector2(transform.position.x + Random.Range(-distance, distance), transform.position.y + Random.Range(-distance / 3, distance / 3));
            Debug.Log(target.ToString());
            if (!RoomController.Instance.currentRoom.GetComponent<Room>().wallCollider.OverlapPoint(target))
            {
                clearTarget = true;
            }
            yield return null;
        }
        positions = new Vector2[num_points];
        startPos = transform.position;
        endPos = target;

        if (expires)
        {
            StartCoroutine(Expire());
        }
        start = true;
    }

    IEnumerator Expire()
    {
        yield return new WaitForSeconds(expiryTime);
        Destroy(gameObject);
    }

    private void Update()
    {
        if (!landed && arc && start)
        {
            float diff = endPos.x - startPos.x;
            Vector2 middlePos = new Vector2(startPos.x + (diff * 0.5f), transform.position.y + 3);

            DrawQuadraticCurve();
            target_pos = positions[current_target];
            transform.position = Vector3.MoveTowards(transform.position, target_pos, move_speed * Time.deltaTime);

            if ((Vector2)transform.position == target_pos && current_target < num_points)
            {
                current_target++;
            }
        }


        if ((Vector2)transform.position == target)
        {
            landed = true;
        }
    }

    private void DrawQuadraticCurve()
    {
        for (int i = 1; i < num_points + 1; i++)
        {
            float t = i / (float)num_points;
            positions[i - 1] = CalculateQuadraticBezierPoint(t, startPos, middlePos, endPos);
        }
    }

    private Vector2 CalculateQuadraticBezierPoint(float t, Vector2 p0, Vector2 p1, Vector2 p2)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        Vector2 p = uu * p0;
        p += 2 * u * t * p1;
        p += tt * p2;
        return p;
    }
}
