// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "MIVR/Unlit/Controller Reticle" {
	Properties{
		_MainTex("Texture", 2D) = "white" {}
	    _Color("Color", Color) = (1,1,1,1)
			_ReticleStatus("Reticle Status", Vector) = (1,0,0,0)
	}

		SubShader{
		Tags{
		"Queue" = "Overlay+100"
		"IgnoreProjector" = "True"
		"RenderType" = "TransparentCutout"
	}

		LOD 100

		Cull Back
		Lighting Off
		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha
		//Offset - 150, -150

		Pass{
		CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#pragma target 2.0

#include "UnityCG.cginc"

		struct appdata_t {
		float4 vertex : POSITION;
		float2 texcoord : TEXCOORD0;
	};

	struct v2f {
		float4 vertex : SV_POSITION;
		half2 texcoord : TEXCOORD0;
	};

	sampler2D _MainTex;
	float4 _MainTex_ST;
	half4 _ReticleStatus;
	half4 _Color;

	v2f vert(appdata_t v) {
		v2f o;
		o.vertex = UnityObjectToClipPos(v.vertex);
		o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex) *_ReticleStatus.x + _ReticleStatus.y;
		return o;
	}

	fixed4 frag(v2f i) : SV_Target{
		fixed4 col = tex2D(_MainTex, i.texcoord) * _Color;
	    half theta = atan((i.texcoord.y - 0.5) / (i.texcoord.x - 0.5));
		col.a += (1.0 - length(i.texcoord - half2(0.5, 0.5)))*0.7 * _ReticleStatus.z;//(1.0 -(abs(i.texcoord.x -0.5) + abs(i.texcoord.y-0.5))) * _ReticleStatus.z;
	    clip(col.a-0.5);
	return col;
	}
		ENDCG
	}
	}
}
