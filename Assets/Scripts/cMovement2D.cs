using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cMovement2D : MonoBehaviour
{
    [SerializeField]
    private float moveTime = 0.5f;
    [SerializeField]
    private float aftermoveDelayTime = 0.1f;

    public Vector3Int MoveDirection { set; get; } = Vector3Int.zero;
    public bool IsMove { set; get; } = false;

    private IEnumerator Start()
    {
        while (true)
        {
            if (MoveDirection != Vector3.zero && IsMove == false)
            {
                Vector3 end =cGameManager.instance.WorldTilePosition[MoveDirection.x, MoveDirection.y];
                yield return StartCoroutine(GridSmoothMovement(end));
            }
            yield return null;
        }
    }
    private IEnumerator GridSmoothMovement(Vector3 end)
    {
        Vector3 start = transform.position;
        float current = 0;
        float percent = 0;
        IsMove = true;
        while (percent < 1)
        {
            current += Time.deltaTime;
            percent = current / moveTime;
            transform.position = Vector3.Lerp(start, end, percent);
            yield return null;
        }
        yield return new WaitForSeconds(aftermoveDelayTime);
        IsMove = false;
        MoveDirection = Vector3Int.zero;
    }
}
