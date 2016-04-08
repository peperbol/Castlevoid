Shader "Custom/InvertPos" {
	Properties{
		_MainTex("", 2D) = "white" {}
		_Maxdist("max distnace", Range(0.0, 1.0)) = 1
		_Mindist("min distnace", Range(0.0, 1.0)) = 0 
	}

	SubShader{

		ZTest Always
		Cull Off 
		ZWrite Off 
		Fog{ Mode Off } 
		//Rendering settings

	Pass{
		CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc" 
			//we include "UnityCG.cginc" to use the appdata_img struct
		
			struct v2f {
				float4 pos : POSITION;
				half2 uv : TEXCOORD0;
			};
		
			//Our Vertex Shader 
			v2f vert(appdata_img v) {
				v2f o;
				o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = MultiplyUV(UNITY_MATRIX_TEXTURE0, v.texcoord.xy);
				return o;
			}
		
			sampler2D _MainTex; //Reference in Pass is necessary to let us use this variable in shaders
			half _Mindist;
			half _Maxdist;
								//Our Fragment Shader
			fixed4 frag(v2f i) : COLOR{
				half xc = i.uv.x -0.5;
				half yc = i.uv.y - 0.5;
				half d = sqrt(xc* xc + yc *yc) ;

				d = _Maxdist / 2 - (d * (_Maxdist - _Mindist));

				half a = atan(yc / xc);
				half xr = cos(a)* d ;
				half yr = sin(a) * d ;
				if (xc < 0) {
					xr = -xr;
				}
				else {

					yr = -yr;
				}
		
		
				fixed4 orgCol = tex2D(_MainTex, half2(yr+0.5,-xr+0.5)); //Get the orginal rendered color 
		
				return orgCol;
			}
		ENDCG
	}
	}
		FallBack "Diffuse"
}