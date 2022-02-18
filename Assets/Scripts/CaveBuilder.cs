using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
public class CaveBuilder : MonoBehaviour
{
    float Grid(float a0, float a1, float w)
    {
        if (0.0 > w) return a0;
        if (1.0 < w) return a1;

        return (a1 - a0) * ((w * (w * 6.0f - 15.0f) + 10.0f) * w * w * w) + a0;
    }

    Vector2 randomGradient(int ix, int iy)
    {
        const float w = 8;
        const float s = w / 2; // rotation width
        float a = ix, b = iy;
        a *= 3284157443; b = a / s / (w - s);
        b *= 1911520717; a = b * s / (w - s);
        a *= 2048419325;
        float random = a * (3.14159265f / ~(~0u >> 1)); // in [0, 2*Pi]
        Vector2 v;
        v.x = Mathf.Sin(random); v.y = Mathf.Cos(random);
        return v;
    }

    float dotGridGradient(int ix, int iy, float x, float y)
    {
        // Get gradient from integer coordinates
        Vector2 gradient = randomGradient(ix, iy);

        // Compute the distance vector
        float dx = x - ix;
        float dy = y - iy;

        // Compute the dot-product
        return dx * gradient.x + dy * gradient.y;
    }
    float perlin(float x, float y)
    {
        // Determine grid cell coordinates
        int x0 = (int)x;
        int x1 = x0 + 1;
        int y0 = (int)y;
        int y1 = y0 + 1;

        // Determine interpolation weights
        // Could also use higher order polynomial/s-curve here
        float sx = x - x0;
        float sy = y - y0;
    
        // Interpolate between grid point gradients
        float n0, n1, ix0, ix1, value;

        n0 = dotGridGradient(x0, y0, x, y);
        n1 = dotGridGradient(x1, y0, x, y);
        ix0 = Grid(n0, n1, sx);

        n0 = dotGridGradient(x0, y1, x, y);
        n1 = dotGridGradient(x1, y1, x, y);
        ix1 = Grid(n0, n1, sx);

        value = Grid(ix0, ix1, sy);
        return value;
    }

    // Start is called before the first frame update
    void Start()
    {
        Grid(0, 0, 1);
    }

    // Update is called once per frame
    void Update()
    {
        perlin(10, 10);
    }
}