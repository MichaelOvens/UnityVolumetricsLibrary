Shader "UnityVolume/Scalar Volume Surface"
{

	Properties{
		_Color("Color", Color) = (1,1,1,1)
		_Glossiness("Smoothness", Range(0,1)) = 0.5
		_Metallic("Metallic", Range(0,1)) = 0.0
	}
		SubShader{
		Tags{ "RenderType" = "Opaque" }
		Cull Back
		LOD 200

		CGPROGRAM
#pragma surface surf Standard

		sampler2D _MainTex;
	half _Glossiness;
	half _Metallic;
	fixed4 _Color;

	struct Input {
		float2 uv_MainTex;
	};

	void surf(Input IN, inout SurfaceOutputStandard o) {
		half4 c = _Color;
		o.Albedo = c.rgb;
		o.Metallic = _Metallic;
		o.Smoothness = _Glossiness;
		o.Alpha = c.a;
	}
	ENDCG

		Cull Front
		LOD 200

		CGPROGRAM
#pragma surface surf Standard vertex:vert

		sampler2D _MainTex;
	half _Glossiness;
	half _Metallic;
	fixed4 _Color;

	struct Input {
		float2 uv_MainTex;
	};

	void vert(inout appdata_full v, out Input o) {
		UNITY_INITIALIZE_OUTPUT(Input, o);
		v.normal = -v.normal;
	}

	void surf(Input IN, inout SurfaceOutputStandard o) {
		half4 c = _Color;
		o.Albedo = c.rgb;
		o.Metallic = _Metallic;
		o.Smoothness = _Glossiness;
		o.Alpha = c.a;
	}
	ENDCG
	}
}
