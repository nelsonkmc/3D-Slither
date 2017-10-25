//
// Modified from 
// https://github.com/Chaser324/unity-wireframe/blob/master/Shaders/WireframeTransparent.shader
//
// We changed the color, and implemented the dynamic wire color base on _PlayerPosition
//

Shader "SuperSystems/Wireframe"
{
    Properties
    {
        _WireThickness ("Wire Thickness", RANGE(0, 800)) = 300
        _WireColor ("Wire Color", Color) = (1.0, 1.0, 1.0, 1.0)
        _BaseColor ("Base Color", Color) = (1.0, 1.0, 1.0, 1.0)
        _TransparentColor ("Transparent Color", Color) = (1.0, 1.0, 1.0, 0.0)
        _PlayerPosition ("Player Position", Vector) = (0.0, 0.0, 0.0)
        _CameraDirection ("Camera Facing Direction", Vector) = (0.0, 0.0, 0.0)
        _WorldRadius ("World Radius", Float) = 100
    }

    SubShader
    {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            Cull Off

            // Wireframe shader based on the the following
            // http://developer.download.nvidia.com/SDK/10/direct3d/Source/SolidWireframe/Doc/SolidWireframe.pdf

            CGPROGRAM
            #pragma vertex vert
            #pragma geometry geom
            #pragma fragment frag

            #include "UnityCG.cginc"

            float _WireThickness;
            float _WorldRadius;
            uniform float3 _PlayerPosition;
            uniform float3 _CameraDirection;
            uniform float4 _WireColor; 
            uniform float4 _BaseColor;
            uniform float4 _TransparentColor;

            struct appdata
            {
                float4 vertex : POSITION;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2g
            {
                float4 projectionSpaceVertex : SV_POSITION;
                float4 worldSpacePosition : TEXCOORD1;
                UNITY_VERTEX_OUTPUT_STEREO
            };

            struct g2f
            {
                float4 projectionSpaceVertex : SV_POSITION;
                float4 worldSpacePosition : TEXCOORD0;
                float4 dist : TEXCOORD1;
                UNITY_VERTEX_OUTPUT_STEREO
            };
            
            v2g vert (appdata v)
            {
                v2g o;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
                o.projectionSpaceVertex = UnityObjectToClipPos(v.vertex);
                o.worldSpacePosition = mul(unity_ObjectToWorld, v.vertex);
                return o;
            }
            
            [maxvertexcount(3)]
            void geom(triangle v2g i[3], inout TriangleStream<g2f> triangleStream)
            {
                float2 p0 = i[0].projectionSpaceVertex.xy / i[0].projectionSpaceVertex.w;
                float2 p1 = i[1].projectionSpaceVertex.xy / i[1].projectionSpaceVertex.w;
                float2 p2 = i[2].projectionSpaceVertex.xy / i[2].projectionSpaceVertex.w;

                float2 edge0 = p2 - p1;
                float2 edge1 = p2 - p0;
                float2 edge2 = p1 - p0;

                // To find the distance to the opposite edge, we take the
                // formula for finding the area of a triangle Area = Base/2 * Height, 
                // and solve for the Height = (Area * 2)/Base.
                // We can get the area of a triangle by taking its cross product
                // divided by 2.  However we can avoid dividing our area/base by 2
                // since our cross product will already be double our area.
                float area = abs(edge1.x * edge2.y - edge1.y * edge2.x);
                float wireThickness = 800 - _WireThickness;

                g2f o;

                o.worldSpacePosition = i[0].worldSpacePosition;
                o.projectionSpaceVertex = i[0].projectionSpaceVertex;
                o.dist.xyz = float3( (area / length(edge0)), 0.0, 0.0) * o.projectionSpaceVertex.w * wireThickness;
                o.dist.w = 1.0 / o.projectionSpaceVertex.w;
                UNITY_TRANSFER_VERTEX_OUTPUT_STEREO(i[0], o);
                triangleStream.Append(o);

                o.worldSpacePosition = i[1].worldSpacePosition;
                o.projectionSpaceVertex = i[1].projectionSpaceVertex;
                o.dist.xyz = float3(0.0, (area / length(edge1)), 0.0) * o.projectionSpaceVertex.w * wireThickness;
                o.dist.w = 1.0 / o.projectionSpaceVertex.w;
                UNITY_TRANSFER_VERTEX_OUTPUT_STEREO(i[1], o);
                triangleStream.Append(o);

                o.worldSpacePosition = i[2].worldSpacePosition;
                o.projectionSpaceVertex = i[2].projectionSpaceVertex;
                o.dist.xyz = float3(0.0, 0.0, (area / length(edge2))) * o.projectionSpaceVertex.w * wireThickness;
                o.dist.w = 1.0 / o.projectionSpaceVertex.w;
                UNITY_TRANSFER_VERTEX_OUTPUT_STEREO(i[2], o);
                triangleStream.Append(o);
            }

            fixed4 frag (g2f i) : SV_Target
            {
            	float distance2this = length(_PlayerPosition - i.worldSpacePosition.xyz);
            	float colorWeight = distance2this / (_WorldRadius * 2.0f);

                float minDistanceToEdge = min(i.dist[0], min(i.dist[1], i.dist[2])) * i.dist[3];

                // Early out if we know we are not on a line segment.
                if(minDistanceToEdge > 0.9)
                {
                	float3 relativeDirection = normalize(_CameraDirection) - normalize(i.worldSpacePosition.xyz);
                	float lenDot = length(dot(relativeDirection, _CameraDirection));
                	float angle = lenDot / (length(relativeDirection) * length(_CameraDirection));
                	if (angle > 0.85f) {
                		return _TransparentColor;
            		}
                    return _BaseColor;
                }

                // Smooth our line out
                float t = exp2(-2 * minDistanceToEdge * minDistanceToEdge);
                fixed4 finalColor = lerp(_BaseColor, _WireColor, t);

                finalColor.rgb *= colorWeight;

                return finalColor;
            }
            ENDCG
        }
    }
}
