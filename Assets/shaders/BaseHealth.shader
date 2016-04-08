Shader "Custom/BaseHealth" {
	Properties{
		_MainTex("", 2D) = "white" {}
		_Split("Split", Range(0.0, 1.0)) = 0.5
		_AA("AntiAlias", Range(0.0, 1.0)) = 0.5
		_C1("Color 1", Color) = (1,0,0,1)
		_C2("Color 2", Color) = (0,1,0,1)
	}

	SubShader{

		ZTest Always Cull Off ZWrite Off Fog{ Mode Off 
	} //Rendering settings

	Pass{
		CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc" 
			#define PI 3.1415926535897932384626433832795
			//we include "UnityCG.cginc" to use the appdata_img struct
		
			struct v2f {
				float4 pos : POSITION;
				half2 uv : TEXCOORD0;
			};

			// r = radius
			// d = distance
			// p = % thickness used for dropoff
			float antialias(float r, float d,  float p) {
				if (d > r)
					return 1.0 -pow(d - r, 2) / pow(p, 2) ;
				else
					return 1.0;
			}
		
			//Our Vertex Shader 
			v2f vert(appdata_img v) {
				v2f o;
				o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = MultiplyUV(UNITY_MATRIX_TEXTURE0, v.texcoord.xy);
				return o;
			}
		
			sampler2D _MainTex; //Reference in Pass is necessary to let us use this variable in shaders
			half _Split, _AA;
			fixed4 _C1,  _C2;
								//Our Fragment Shader
			fixed4 frag(v2f i) : COLOR{
				half xc = i.uv.x -0.5;
				half yc = i.uv.y - 0.5;
				half d = sqrt(xc* xc + yc *yc) ;
				half angle =  atan(yc/abs(xc)) / PI + 0.5;
				fixed4 orgCol;
				if (angle >_Split) {
					orgCol = _C1;
				}
				else { orgCol = _C2; }
				//fixed4 orgCol = tex2D(_MainTex, half2(yr+0.5,-xr+0.5)); //Get the orginal rendered color 
				orgCol = lerp(orgCol,float4(0,0,0,0), antialias(0.45, d,  _AA));
				return orgCol;
			}
		ENDCG
	}
	}
		FallBack "Diffuse"
}