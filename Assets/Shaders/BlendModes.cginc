fixed4 Darken (fixed4 a, fixed4 b)
{ 
    return min(a, b);
}

fixed4 Multiply (fixed4 a, fixed4 b)
{ 
    return a * b;
}

fixed4 ColorBurn (fixed4 a, fixed4 b) 
{ 
    return saturate(1.0 - (1.0 - a) / b);
}

fixed4 LinearBurn (fixed4 a, fixed4 b)
{ 
    return a + b - 1.0;
}

/*fixed4 DarkerColor (fixed4 a, fixed4 b) 
{ 
    return G(a) < G(b) ? a : b; 
}*/

fixed4 Lighten (fixed4 a, fixed4 b)
{ 
    return max(a, b);
}

fixed4 Screen (fixed4 a, fixed4 b) 
{   
    return 1.0 - (1.0 - a) * (1.0 - b);
}

fixed4 ColorDodge (fixed4 a, fixed4 b) 
{ 
    return saturate(a / (1.0 - b));
}

fixed4 LinearDodge (fixed4 a, fixed4 b)
{ 
    return a + b;
} 

/*fixed4 LighterColor (fixed4 a, fixed4 b) 
{ 
    return G(a) > G(b) ? a : b; 
}*/

fixed4 Overlay (fixed4 a, fixed4 b) 
{
    return a > .5 ? 1.0 - 2.0 * (1.0 - a) * (1.0 - b) : 2.0 * a * b;
}

fixed4 SoftLight (fixed4 a, fixed4 b)
{
    return (1.0 - a) * a * b + a * (1.0 - (1.0 - a) * (1.0 - b));
}

fixed4 HardLight (fixed4 a, fixed4 b)
{
    return b > .5 ? 1.0 - (1.0 - a) * (1.0 - 2.0 * (b - .5)) : a * (2.0 * b);
}

fixed4 VividLight (fixed4 a, fixed4 b)
{
    return saturate(b > .5 ? a / (1.0 - (b - .5) * 2.0) : 1.0 - (1.0 - a) / (b * 2.0));
}

fixed4 LinearLight (fixed4 a, fixed4 b)
{
    return b > .5 ? a + 2.0 * (b - .5) : a + 2.0 * b - 1.0;
}

fixed4 PinLight (fixed4 a, fixed4 b)
{
    return b > .5 ? max(a, 2.0 * (b - .5)) : min(a, 2.0 * b);
}

fixed4 HardMix (fixed4 a, fixed4 b)
{
    return (b > 1.0 - a) ? 1.0 : .0;
}

fixed4 Difference (fixed4 a, fixed4 b) 
{ 
    return abs(a - b); 
}

fixed4 Exclusion (fixed4 a, fixed4 b)
{ 
    return a + b - 2.0 * a * b; 
}

fixed4 Subtract (fixed4 a, fixed4 b)
{ 
    return a - b; 
}

fixed4 Divide (fixed4 a, fixed4 b)
{ 
    return saturate(a / b); 
}