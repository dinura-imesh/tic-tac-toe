using UnityEngine;

namespace Scripts
{
    public class SaveSystemConverter : MonoBehaviour
    {
        /// <summary>
        /// Converts a color to a float4 array
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static float[] ConvertColortoFloat4(Color color)
        {
            float[] col = new float[4];
            col[0] = color.r;
            col[1] = color.g;
            col[2] = color.b;
            col[3] = color.a;
            
            return col;
        }

        /// <summary>
        /// Converts a float4 array to a color
        /// </summary>
        /// <param name="float4"></param>
        /// <returns></returns>
        public static Color ConvertFloat4ToColor(float[] float4)
        {
            Color col = new Color();
            col.r = float4[0];
            col.g = float4[1];
            col.b = float4[2];
            col.a = float4[3];
        
            return col;
        }

        /// <summary>
        /// Converts a vector2 to a float2 array.
        /// </summary>
        /// <param name="vector2"></param>
        /// <returns></returns>
        public static float[] ConvertVector2ToFloat2(Vector2 vector2)
        {
            float[] vect = new float[2];
            vect[0] = vector2.x;
            vect[1] = vector2.y;
        
            return vect;
        }

        /// <summary>
        /// Converts a float2 array into a vector2
        /// </summary>
        /// <param name="float2Array"></param>
        /// <returns></returns>
        public static Vector2 ConvertFloat2ToVector2(float[] float2Array)
        {
            Vector2 vect = new Vector2();
            vect.x = float2Array[0];
            vect.y = float2Array[1];

            return vect;
        }

        /// <summary>
        /// Converts a vector3 into a float3 array
        /// </summary>
        /// <param name="vector3"></param>
        /// <returns></returns>
        public static float[] ConvertVector3ToFloat3(Vector3 vector3)
        {
            float[] vect = new float[3];
            vect[0] = vector3.x;
            vect[1] = vector3.y;
            vect[2] = vector3.z;
        
            return vect;
        }
    
        /// <summary>
        /// Converts a float3 array into a vector3
        /// </summary>
        /// <param name="float3Array"></param>
        /// <returns></returns>
        public static Vector3 ConvertFloat3ToVector3(float[] float3Array)
        {
            Vector3 vect = new Vector3();
            vect.x = float3Array[0];
            vect.y = float3Array[1];
            vect.z = float3Array[2];

            return vect;
        }
    }
}
