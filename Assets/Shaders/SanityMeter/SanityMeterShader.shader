Shader "Custom/SanityMeterBackground"
{
    Properties
    {
        _ColorA ("Start Color", Color) = (0, 110, 0, 230) // Verde
        _ColorB ("End Color", Color) = (110, 0, 0, 230)   // Rojo
        _FillAmount ("Fill Amount", Range(0, 1)) = 1
        _MainTex ("Texture", 2D) = "white" {} // Textura principal
        _Amplitud ("Amplitud", Range(0,1)) = 0.5
        _olas ("Olas", int) = 0
    }

    SubShader
    {
        Tags { "Queue" = "Transparent" "RenderType"="Transparent" }
        LOD 100

        Blend SrcAlpha OneMinusSrcAlpha // Para manejar transparencias
        ZWrite Off // Desactiva la escritura en el buffer de profundidad

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _ColorA;
            float4 _ColorB;
            float _FillAmount;
            float _Amplitud;
            int _olas;

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

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                
                if(_olas == 1)
                {

                }

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Combina colores dinámicamente basado en _FillAmount
                float4 color = lerp(_ColorB, _ColorA, _FillAmount);

                // Aplica la textura principal y maneja transparencias
                fixed4 texColor = tex2D(_MainTex, i.uv);
                return texColor * color; // Mezcla de la textura con los colores
            }
            ENDCG
        }
    }
}