using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DrawPaint : MonoBehaviour
{
    
    public enum PaintObjective { Colour, Leave, None };

    private RenderTexture myRenderTexture;
    private Texture2D myTexture2D;
    private Renderer myRenderer;
    public PaintObjective objective;

    [Header("Goal")]
    [SerializeField] private float fillAmount;
    [SerializeField] private int paintedPixels;
    [SerializeField] private int maxPixels;
    


    // Start is called before the first frame update
    void Start()
    {
        gameObject.tag = "Paintable";

        myRenderer = GetComponent<Renderer>();
        Debug.Log(gameObject.name + ":" + myRenderer.bounds.size);
        myRenderTexture = new RenderTexture((int)(Constants.PixelsPerUnit * myRenderer.bounds.size.x), (int)(Constants.PixelsPerUnit * myRenderer.bounds.size.z), 32, RenderTextureFormat.ARGB32);
        
        //myRenderer.material = new Material(Shader.Find("Standard"));
        myTexture2D = new Texture2D(myRenderTexture.width, myRenderTexture.height);
        myTexture2D.filterMode = FilterMode.Point;
        myRenderer.material.mainTexture = myTexture2D;
        SetupTexture();

        if (objective == PaintObjective.Colour)
            Constants.Objectives.Add(this);
        else if (objective == PaintObjective.Leave)
            Constants.AntiObjectives.Add(this);

        maxPixels = myRenderTexture.width * myRenderTexture.height;
        paintedPixels = 0;
        

    }
    private void updateFillAmount()
    {
        fillAmount = (float)paintedPixels / (float)maxPixels;
    }

    private void SetupTexture()
    {
        RenderTexture.active = myRenderTexture;

        for (int i = 0; i < myRenderTexture.width; i++)
            for (int j = 0; j < myRenderTexture.height; j++)
                myTexture2D.SetPixel(i, j, Color.clear);

        myTexture2D.Apply();
        RenderTexture.active = null;
    }

    private class PixelPoint
        {
            public int x;
            public int y;
            public PixelPoint (int myX, int myY)
            {
            x = myX;
            y = myY;
            }
            public PixelPoint (int[] position)
            {
            x = position[0];
            y = position[1];
            }
            
        }

    public float Paint(Vector3 drawCenter, float radius, Color color)
    {
        PixelPoint circleCenter = new PixelPoint(pointOnTexture(drawCenter));
        List<PixelPoint> pointsToColour = getCirclePixels(circleCenter, Mathf.CeilToInt(radius/Constants.PixelsPerUnit));
        int amountOfColouredPixels = 0;
        RenderTexture.active = myRenderTexture;

        foreach (PixelPoint currentPoint in pointsToColour)
        {
            if (isOnTexture(currentPoint)&&color != myTexture2D.GetPixel(myRenderTexture.width - currentPoint.x, myRenderTexture.height - currentPoint.y))
            { 
                    myTexture2D.SetPixel(myRenderTexture.width - currentPoint.x, myRenderTexture.height - currentPoint.y, color);
                    amountOfColouredPixels++;
            }                             
        }
        paintedPixels += amountOfColouredPixels;
        updateFillAmount();

        myTexture2D.Apply();
        RenderTexture.active = null;

        return (float)amountOfColouredPixels;

    }

    private bool isOnTexture(PixelPoint point)
    {
        return point.x < myRenderTexture.width && point.x >= 0 && point.y < myRenderTexture.height && point.y >= 0;
    }

    private int[] pointOnTexture(Vector3 position)
    {
        int myX = (int)((position.x - transform.position.x + transform.localScale.x / 2f) / transform.localScale.x * (myTexture2D.width - 1));
        int myZ = (int)((position.z - transform.position.z + transform.localScale.z / 2f) / transform.localScale.z * (myTexture2D.height - 1));


        return new int[2] { myX, myZ };
    }

    private List<PixelPoint> getCirclePixels (PixelPoint center, float radius)
    {
        int intRadius = Mathf.CeilToInt(radius);

        List<PixelPoint> returnPoints = new List<PixelPoint>();

        for (int yOffset = -intRadius; yOffset <= intRadius; yOffset++)
            for (int xOffset = -intRadius; xOffset <= intRadius; xOffset++)
                if ((xOffset * xOffset + yOffset * yOffset) <= (radius * radius))
                {
                    returnPoints.Add(new PixelPoint(center.x + xOffset, center.y + yOffset));
                }
                    

        return returnPoints;
    }

    public float getFillAmount()
    {
        Debug.Log(fillAmount);
        return fillAmount;
    }

}
