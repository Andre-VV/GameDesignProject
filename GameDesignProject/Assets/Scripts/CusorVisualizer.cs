using UnityEngine;
using UnityEngine.InputSystem;

public class CusorVisualizer : MonoBehaviour
{
    public Transform player;

    public LineRenderer circleRenderer;
    public LineRenderer lineRenderer;

    public float circleRadius = 1f;
    public int circleSegments = 50;

  
    public float lineZ = 0f;


    void Update()
    {
        Vector3 mouseWorld = GetMouseWorldPosition();

        DrawCircle(mouseWorld);
        DrawLine(player.position, mouseWorld);
    }

    Vector3 GetMouseWorldPosition()
    {
        if (Mouse.current == null) return Vector3.zero;

        Vector2 mouseScreen = Mouse.current.position.ReadValue();

        Vector3 mouse = new Vector3(mouseScreen.x, mouseScreen.y, 10f);
        return Camera.main.ScreenToWorldPoint(mouse);
    }

    void DrawCircle(Vector3 center)
    {
        circleRenderer.positionCount = circleSegments + 1;

        float angleStep = 360f / circleSegments;

        for (int i = 0; i <= circleSegments; i++)
        {
            float angle = Mathf.Deg2Rad * angleStep * i;

            float x = Mathf.Cos(angle) * circleRadius;
            float y = Mathf.Sin(angle) * circleRadius;

            circleRenderer.SetPosition(i, center + new Vector3(x, y, 0));
        }
    }

    void DrawLine(Vector3 start, Vector3 end)
    {
        start.z = lineZ;
        end.z = lineZ;

        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);
    }



}
