Shader "Custom/ScrollingEmissive"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        // _Glossiness ("Smoothness", Range(0,1)) = 0.5
        // _Metallic ("Metallic", Range(0,1)) = 0.0
        _Emissive ("Emission", Range(0.0, 5.0)) = 0.0
        _ScrollX ("Scroll X", Range(-5, 5)) = 1
        _ScrollY ("Scroll Y", Range(-5, 5)) = 1
    }
    SubShader
    {
        Tags { "Queue" = "Geometry" }
        

        CGPROGRAM
        #pragma surface surf Standard
        // #pragma target 3.0

        sampler2D _MainTex;
        float _ScrollX;
        float _ScrollY;
        half _Emissive;

        struct Input
        {
            float2 uv_MainTex;
        };

        // half _Glossiness;
        // half _Metallic;
        fixed4 _Color;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            _ScrollX *= _Time;
            _ScrollY *= _Time;
            
            // Albedo comes from a texture tinted by color
            float2 newUV = IN.uv_MainTex + float2(_ScrollX, _ScrollY);
            fixed4 c = tex2D(_MainTex, newUV) * _Color;
            o.Albedo = c.rgb;
            // Metallic and smoothness come from slider variables
            // o.Metallic = _Metallic;
            // o.Smoothness = _Glossiness;
            o.Alpha = c.a;
            o.Emission = tex2D(_MainTex, newUV).r * _Emissive * _Color;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
