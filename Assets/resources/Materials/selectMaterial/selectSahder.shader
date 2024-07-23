Shader "Unlit/selectSahder"
{
    Properties
    {
        // _MainTex ("Texture", 2D) = "white" {}
        _Diffuse ("Color", Color) = (1, 1, 1, 1)
        _OutlineColor ("OutlineColor", Color) = (1,1,1,1)
        _OutlineWidth ("OutlineWidth", Range(0, 10)) = 5
    }
    SubShader
    {
        // Tags { 
        //     "Queue"="Transparent+100"
        //     "RendeType"="Transparent"
        // }
        Tags { "RenderType"="Opaque" }
        LOD 100

        // Pass
        // {
        //     Cull Off


        //     // ZWrite Off
        //     // ZTest Always
        //     // Blend SrcAlpha OneMinusSrcAlpha

        //     // CGPROGRAM
        //     // #pragma vertex vert
        //     // #pragma fragment frag
        //     // // make fog work
        //     // // #pragma multi_compile_fog

        //     // #include "UnityCG.cginc"

        //     // struct appdata
        //     // {
        //     //     float4 vertex : POSITION;
        //     //     float2 uv : TEXCOORD0;
        //     // };

        //     // struct v2f
        //     // {
        //     //     float2 uv : TEXCOORD0;
        //     //     UNITY_FOG_COORDS(1)
        //     //     float4 vertex : SV_POSITION;
        //     // };

        //     // sampler2D _MainTex;
        //     // float4 _MainTex_ST;

        //     // v2f vert (appdata v)
        //     // {
        //     //     v2f o;
        //     //     o.vertex = UnityObjectToClipPos(v.vertex);
        //     //     o.uv = TRANSFORM_TEX(v.uv, _MainTex);
        //     //     UNITY_TRANSFER_FOG(o,o.vertex);
        //     //     return o;
        //     // }

        //     // fixed4 frag (v2f i) : SV_Target
        //     // {
        //     //     // sample the texture
        //     //     fixed4 col = tex2D(_MainTex, i.uv);
        //     //     // apply fog
        //     //     UNITY_APPLY_FOG(i.fogCoord, col);
        //     //     return col;
        //     // }
        //     // ENDCG
        // }

        Pass{
            Name "OutLine"// 外部使用此 需要大写 此名字
            Cull Front // 绘制哪里 ； 剔除正面 ，如果 是 Cull back 那么是 剔除背面 只绘制正面
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            float _OutLine;
            fixed4 _OutLineColor;
            struct v2f
            {
                float4 vertex : SV_POSITION;
            };


            v2f vert (appdata_base v)
            {
                v2f o;
                // 法线外移
                v.vertex.xyz += v.normal * _OutLine;//物体坐标外拓 法线外扩；  为什么 是 乘 ？？？;
                o.vertex = UnityObjectToClipPos(v.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // 环境光源
                // fixed3 ambient =  UNITY_LIGHTMODEL_AMBIENT.xyz;
                return _OutLineColor;
            }
            ENDCG
        }


        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
            #include "Lighting.cginc"// 包含光照计算函数

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                fixed3 worldNormal : TEXCOORD1;
                float3 worldPos : TEXCOORD2;

            };

            float4 _Diffuse;

            v2f vert (appdata_base v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                float3 worldNormal = UnityObjectToWorldNormal(v.normal);
                o.worldNormal = worldNormal;
                o.worldPos = UnityObjectToWorldDir(v.vertex);
                // UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // 环境光源
                fixed3 ambient =  UNITY_LIGHTMODEL_AMBIENT.xyz;
                // 纹理 采样
                // fixed4 albedo = tex2D(_MainTex, i.uv);
                // 漫反射
                fixed3 worldLightDir = UnityObjectToWorldDir(i.worldPos); // 世界光源方向
                float difLight = dot(worldLightDir, i.worldNormal) * 0.5 + 0.5;// 光源方向与法线 点积

                fixed3 diffuse = _LightColor0.rgb * _Diffuse.rgb * difLight; 

                return float4 (ambient + diffuse, 1);
            }
            ENDCG
        }


    }
}
