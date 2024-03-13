// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "CookbookShaders/Chapter06/WaterShader" 
{
	Properties 
	{
		_NoiseTex("Noise text", 2D) = "white" {}
		_Colour ("Colour", Color) = (1,1,1,1)

		_Period ("Period", Range(0,50)) = 1
		_Magnitude ("Magnitude", Range(0,0.5)) = 0.05
		_Scale ("Scale", Range(0,10)) = 1
        _Magnification("Magnification", Float) = 1
    	_SinMagnifier("SinMagnifier", Float) = 1.5
    	_SinDistortion("SinDistortion", Float)  = 0.22
    	_HightPercentage("Hight percentage", Float) = 0.96
	}
	
	SubShader
	{
		Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
		ZWrite Off Lighting Off Cull Off Fog { Mode Off } Blend One Zero
		LOD 110

		GrabPass { "_GrabTexture" }
		
		Pass 
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			sampler2D _GrabTexture;

			sampler2D _NoiseTex;	
			fixed4 _Colour;

			float  _Period;
			float  _Magnitude;
			float  _Scale;
			float  _SinMagnifier;
			float  _SinDistortion;
			float  _HightPercentage;
			struct vin_vct
			{
				float4 vertex : POSITION;
				float4 color : COLOR;
				float2 texcoord : TEXCOORD0;
			};

			struct v2f_vct
			{
				float4 vertex : POSITION;
				fixed4 color : COLOR;
				float2 texcoord : TEXCOORD0;

				float4 worldPos : TEXCOORD1;
				float4 uvgrab : TEXCOORD2;
			};
			 half _Magnification;
			// Vertex function 
			v2f_vct vert (vin_vct v)
			{
				v2f_vct o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.color = v.color;
				o.texcoord = v.texcoord;

				o.worldPos = mul(unity_ObjectToWorld, v.vertex);
				o.uvgrab = ComputeGrabScreenPos(o.vertex);

				//magnify
			
			if(_Magnification != 1) {
            o.vertex = UnityObjectToClipPos(v.vertex);

            float4 uv_center = ComputeGrabScreenPos(	(float4(0, 0, 0, 0)));
            //the vector from uv_center to our UV coordinate on the GrabTexture
            float4 uv_diff = ComputeGrabScreenPos(o.vertex) - uv_center;
            //apply magnification
            uv_diff /= _Magnification;
            //save result
            o.uvgrab = uv_center + uv_diff;
            }

				//


				return o;
			}

			// Fragment function
			fixed4 frag (v2f_vct i) : COLOR
			{
		 
				float sinT = sin(_Time.w / _Period);
				float2 distortion = float2
				(	tex2D(_NoiseTex, i.worldPos.xy / _Scale + float2(sinT, 0) ).r - 0.5,
					tex2D(_NoiseTex, i.worldPos.xy / _Scale + float2(0, sinT) ).r - 0.5
					);

		 	distortion.xy -=   sin((_Time.w*_SinMagnifier) + i.worldPos.xy )*_SinDistortion	;


				fixed4 col = tex2Dproj( _GrabTexture, UNITY_PROJ_COORD(i.uvgrab));


				i.uvgrab.xy += distortion * _Magnitude;
				float modifier = (distortion * _Magnitude * 1.0) ;

				if(i.texcoord.y + modifier <_HightPercentage)	 
				{
					col = tex2Dproj( _GrabTexture, UNITY_PROJ_COORD(i.uvgrab));
						  col.rgb*=_Colour.rgb;
			 	
				}


	
			 
			
				return col ; 
			}
		
			ENDCG
		} 
	}
}