Shader "Custom/BaseHealth" {
	Properties{
			_MainTex("Texture", 2D) = "white" {}
			_ImageTex("Texture", 2D) = "white" {}
			_Split("Split", Range(0.0, 1.0)) = 0.5
			_AAE("Anti Alias Edge", Range(0.0, 1.0)) = 0.5
			_AAPmin("Min Anti Alias Pie", Range(0.0, 1.0)) = 0.5
			_AAPmax("Max Anti Alias Pie", Range(0.0, 1.0)) = 0.5
			_minR("min Radius", Range(0.0, 0.5)) = 0.5
			_maxR("max Radius", Range(0.0, 0.5)) = 0.5
			_blurR("Blur Radius", Range(0.0, 0.5)) = 0.5
			_C1("Color 1", Color) = (1,0,0,1)
			_C2("Color 2", Color) = (0,1,0,1)
			_Detail1("Detail 1", Float) = 0.5
			_Detail2("Detail 2", Float) = 0.5
			_Detail3("Detail 3", Float) = 0.5
	}

	SubShader{
			Tags
		{
			"Queue" = "Transparent"
			"IgnoreProjector" = "True"
			"RenderType" = "Transparent"
			"PreviewType" = "Plane"
			"CanUseSpriteAtlas" = "True"
			"LightMode" = "Always"
		}

			Cull Off
			ZWrite Off
			Fog{ Mode Off }
			Blend SrcAlpha OneMinusSrcAlpha
			ZTest Always

	Pass{
		CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile DUMMY PIXELSNAP_ON
			#include "UnityCG.cginc" 
			#define PI 3.1415926535897932384626433832795
			#define AAoffset 0.79370052598409973737585281963615
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
					return saturate(-pow((d - r)/p - AAoffset, 3) ) ;
				else
					return saturate(1.0 - pow((d - r)/ p + AAoffset, 3) );
					
				//return  -pow(d-r, 3) / pow(p, 2);
			}
		
			//Our Vertex Shader 
			v2f vert(appdata_img v) {
				v2f o;
				o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = MultiplyUV(UNITY_MATRIX_TEXTURE0, v.texcoord.xy);
				return o;
			}
		
			sampler2D _MainTex; //Reference in Pass is necessary to let us use this variable in shaders
			half _Split, _AAE, _AAPmin, _AAPmax, _minR, _maxR, _blurR, _Detail1, _Detail2, _Detail3;
			fixed4 _C1,  _C2;
								//Our Fragment Shader
			fixed4 frag(v2f i) : COLOR{
				half xc = i.uv.x -0.5;
				half yc = i.uv.y - 0.5;
				half d = sqrt(xc* xc + yc *yc) ;
				half angle =  atan(yc/abs(xc)) / PI + 0.5;

				fixed4 orgCol;
				
				orgCol = lerp(_C1, _C2, antialias(angle, _Split, _AAPmin + (_AAPmax- _AAPmin)* saturate((_blurR - d)/ _blurR)));
					/*
				if (angle >_Split) {
					orgCol = _C1;
				}
				else { orgCol = _C2; }
				*/
				//fixed4 orgCol = tex2D(_MainTex, half2(yr+0.5,-xr+0.5)); //Get the orginal rendered color 
				orgCol.a *=  antialias(_minR + (_maxR - _minR)  * ((sin((angle + _Time)*_Detail1)+ sin((angle - _Time)*_Detail2 ) + sin((angle)*_Detail3)) / 3), d,  _AAE)  ;
				return orgCol;
			}
		ENDCG
	}
	}
}