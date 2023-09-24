using UnityEngine;

public struct LineDrawer
{
    private LineRenderer lineRenderer;
    private float lineSize;

    public LineDrawer(float lineSize = 0.2f)
    {
        GameObject lineObj = new GameObject("LineObj");
        lineRenderer = lineObj.AddComponent<LineRenderer>();
        //Particles/Additive
        lineRenderer.material = new Material(Shader.Find("Hidden/Internal-Colored"));

        this.lineSize = lineSize;
    }

    private void init(float lineSize = 0.2f)
    {
        if (lineRenderer == null)
        {
            GameObject lineObj = new GameObject("LineObj");
            lineRenderer = lineObj.AddComponent<LineRenderer>();
            //Particles/Additive
            lineRenderer.material = new Material(Shader.Find("Hidden/Internal-Colored"));

            this.lineSize = lineSize;
        }
    }

    //Draws lines through the provided vertices
    public void DrawLineInGameView(Vector3 start, Vector3 end, Color color)
    {
        if (lineRenderer == null)
        {
            init(0.2f);
        }

        //Set color
        lineRenderer.startColor = color;
        lineRenderer.endColor = color;

        //Set width
        lineRenderer.startWidth = lineSize;
        lineRenderer.endWidth = lineSize;

        //Set line count which is 2
        lineRenderer.positionCount = 2;

        //Set the postion of both two lines
        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);
    }

    public void DrawCircleInGameView(Vector3 center, float radius, Color color)
    {
        if (lineRenderer == null)
        {
            init(0.2f);
        }

        lineRenderer.startColor = color;
        lineRenderer.endColor = color;

        lineRenderer.startWidth = lineSize;
        lineRenderer.endWidth = lineSize;

        float theta_scale = 0.1f;             //Set lower to add more points
        int size = (int)((1f / theta_scale) + 1f); //Total number of points in circle.

        lineRenderer.positionCount = size; //Plus one makes up for the circle to be continuous

        //lineRenderer.SetColors(color, color);

        float theta = 0f;
        for (int i = 0; i < size + 1; i++)
        {
            theta += (2.0f * Mathf.PI * theta_scale);
            float x = radius * Mathf.Cos(theta);
            float y = radius * Mathf.Sin(theta);

            x += center.x;
            y += center.y;

            Vector3 pos = new Vector3(x, y, center.z);

            lineRenderer.SetPosition(i, pos);
        }
    }

    public void DrawPointInGameView(Vector3 center, Color color)
    {
        if (lineRenderer == null)
        {
            init(0.2f);
        }

        lineRenderer.startColor = color;
        lineRenderer.endColor = color;

        lineRenderer.startWidth = lineSize;
        lineRenderer.endWidth = lineSize;

        lineRenderer.positionCount = 1;

        lineRenderer.SetPosition(0, center);
    }

    public void Destroy()
    {
        if (lineRenderer != null)
        {
            UnityEngine.Object.Destroy(lineRenderer.gameObject);
        }
    }
}
