using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class WaterWave : MonoBehaviour
{
    [System.Serializable]
    public class SinWave
    {
        public float waveSpeed;
        public float waveHeight;
    }

    [SerializeField] List<SinWave> waves;
    SpriteShapeController spriteShapeController;
    SpriteMask spriteMask;

    // Start is called before the first frame update
    void Start()
    {
        spriteShapeController = GetComponent<SpriteShapeController>();
        spriteMask = GetComponent<SpriteMask>();
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 1; i < spriteShapeController.spline.GetPointCount() - 1; i++)
        {
            Vector3 point = spriteShapeController.spline.GetPosition(i);
            point.y = GetWaveHeight(point.x);
            spriteShapeController.spline.SetPosition(i, point);
            spriteShapeController.spline.SetTangentMode(i, ShapeTangentMode.Continuous);

            // Set right tangent to the direction of the next spline
            if (i < spriteShapeController.spline.GetPointCount() - 1)
            {
                Vector3 nextPoint = spriteShapeController.spline.GetPosition(i + 1);
                Vector3 tangent = nextPoint - point;
                spriteShapeController.spline.SetRightTangent(i, tangent.normalized / 4);

                //set left tangent to the opposite direction of the right tangent
                spriteShapeController.spline.SetLeftTangent(i, -tangent.normalized / 4);
            }
        }


        // // Create a new RenderTexture
        // RenderTexture renderTexture = new RenderTexture(256, 256, 16, RenderTextureFormat.ARGB32);
        // RenderTexture.active = renderTexture;

        // // Render the SpriteShape to the RenderTexture
        // spriteShapeController.GetComponent<Renderer>().material.mainTexture = spriteShapeController.spriteShape.fillTexture;
        // spriteShapeController.GetComponent<Renderer>().material.SetPass(0);
        // Graphics.DrawMeshNow(spriteShapeController.GetComponent<MeshFilter>().mesh, Matrix4x4.identity);

        // // Create a new Texture2D
        // Texture2D texture2D = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.ARGB32, false);
        // texture2D.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        // texture2D.Apply();

        // // Create a new Sprite from the Texture2D
        // Sprite sprite = Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height), new Vector2(0.5f, 0.5f));

        // // Update sprite mask
        // spriteMask.sprite = sprite;

        // // Clean up
        // RenderTexture.active = null;
        // Destroy(renderTexture);

        // Update sprite mask
    }

    public float GetWaveHeight(float x)
    {
        float height = 0f;
        foreach (SinWave wave in waves)
        {
            height += Mathf.Sin(Time.time + wave.waveSpeed * x) * wave.waveHeight;
        }

        //return height round to 2 decimal places
        return Mathf.Round(height * 100f) / 100f;
    }
}
