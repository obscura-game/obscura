Shader "Custom/Prueba_olas"

{
    Properties
    {
        _ColorA ("Start Color", Color) = (0, 110, 0, 230) // Verde
        _ColorB ("End Color", Color) = (110, 0, 0, 230)   // Rojo
        _FillAmount ("Fill Amount", Range(0, 1)) = 1
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Frecuencia("Frecuencia", Range(0, 5)) = 0.0
        _Amplitud("Amplitud", Range(1,5)) = 1.0
        _vel("Vel", float) = 1 //Este parametro sirve para darle velocidad de desplazamiento a la onda
        _olas ("Olas", int) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows vertex:vert addshadow

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;
        struct Input
        {
            float2 uv_MainTex;
        };

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;
        float _Frecuencia;
        float _Amplitud;
        float _vel;
        int _olas;
        
        float4 _ColorA;
        float4 _ColorB;
        float _FillAmount;
        
        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)
		
		//Función de teselación para añadir más vértices a la textura
       
        
		//Función en la que modificamos el vertice para darle la forma de la onda
        void vert(inout appdata_full v)
        {
            if(_olas == 1)
            {
                fixed angle = (v.vertex.x + _Time.y * _vel) * _Frecuencia;
                fixed s = sin(angle) * lerp(0,_Amplitud, v.texcoord.x);
                v.vertex.y += s;
            
                fixed c = -cos(angle) * _Amplitud;
                v.normal = normalize(fixed3(c, abs(s),0));
            }
            
        }
        
        void surf (Input IN, inout SurfaceOutputStandard o)
        {

            float4 color = lerp(_ColorB, _ColorA, _FillAmount);

            // Aplica la textura principal y maneja transparencias
            fixed4 texColor = tex2D(_MainTex, IN.uv_MainTex);
           


            // Albedo comes from a texture tinted by color
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = texColor * color;// Mezcla de la textura con los colores
            
            // Metallic and smoothness come from slider variables
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}

