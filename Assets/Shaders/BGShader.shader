Shader "Unlit/BGShader"
{
	SubShader
	{
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			uniform float _State;

			float random (float2 st) 
			{
    			return frac(sin(dot(st.xy, float2(12.9898,78.233)))*43758.5453123 * _Time.x);
			}

			float4 RenderPattern(float2 uv) 
			{	
 				half r = (random(floor(uv * 200)));
				half g = (random(floor(uv * 100)));
 				return float4(r, r, r, 1) - float4(g,g,g,1);
 			}
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}
			
			fixed4 frag (v2f i) : COLOR
			{
				if(_State == 0)
				{
					return RenderPattern(i.uv);
				}
				else if(_State == 1)
				{
					return float4(0,0,0,0);
				}
				else if(_State == 2)
				{
					return float4(0,1,0,0);
				}
				return float4(0,0,0,0);
			}
			ENDCG
		}
	}
}
