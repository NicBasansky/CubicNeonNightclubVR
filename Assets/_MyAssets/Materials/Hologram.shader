Shader "Custom/Hologram"
{
    Properties {
         _MainTex ("MainTex", 2D) = "white" {}
         _NormalMap ("Normal Tex", 2D) = "bump" {}
         _mySlider ("Bump Amount", Range(0, 10)) = 1
        _RimColor ("Color", Color) = (0.0, 0.5, 0.5, 0.0)
        _RimPower ("Rim Power", Range(0.5, 8.0)) = 3.0
        _Alpha ("Transparency Alpha", Range(0, 1)) = 0.5
    }

    SubShader {
        Tags {
            "Queue" = "Transparent"
        }

        Pass {
            ZWrite On
            ColorMask 0
        }

        CGPROGRAM
            #pragma surface surf Lambert alpha:fade
            
            fixed4 _RimColor;
            float _RimPower;
            fixed _Alpha;
            sampler2D _MainTex;
            sampler2D _NormalMap;
            half _mySlider;
            
            struct Input {
                float3 viewDir;
                float2 uv_MainTex;
                float2 uv_NormalMap;
            };

            void surf (Input IN, inout SurfaceOutput o) {
                o.Albedo = tex2D(_MainTex, IN.uv_MainTex);
                o.Normal = UnpackNormal(tex2D(_NormalMap, IN.uv_NormalMap));
                o.Normal *= float3(_mySlider, _mySlider, 1);

                half rim = 1 - saturate(dot(normalize(IN.viewDir), o.Normal));
                o.Emission = _RimColor.rgb * pow(rim, _RimPower) * 10;
                o.Alpha = pow(rim, _RimPower) * _Alpha;

            }
        ENDCG
    }
    Fallback "Diffuse"
}
