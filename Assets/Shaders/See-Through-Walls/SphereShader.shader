Shader "Custom/SphereShader"
{
    SubShader
    {
        ZWrite off

        Stencil
        {
            Ref 255
            Comp Always
            Pass Replace
        }
        
        Pass
        {
            
        }
    }
}
