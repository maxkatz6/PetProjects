using System;
using TDF.Core;
using TDF.Graphics.Data;
using SharpDX;
using SharpDX.Direct3D11;
using TDF.Graphics.Models;

namespace TDF.Graphics.Effects
{
    public static class EffectHelper
    {
        private static byte[] _array;
        public static void WriteValue<T>(this EffectVariable val, T obj)
        {
            _array =  BinaryOperationExtension.GetArray(obj);
            val.SetRawValue(DataStream.Create(_array, false, false), _array.Length);
        }

        public static EffectMaterialVariable AsMaterial(this EffectVariable effect)
        {
            return new EffectMaterialVariable(effect.NativePointer)
            {
                Ambient = effect.GetMemberByName("Ambient").AsVector(),
                Diffuse = effect.GetMemberByName("Diffuse").AsVector(),
                Specular = effect.GetMemberByName("Specular").AsVector(),
                Reflect = effect.GetMemberByName("Reflect").AsVector()
            };
        }
        public static EffectPointLightVariable AsPointLight(this EffectVariable effect)
        {
            return new EffectPointLightVariable(effect.NativePointer)
            {
                Ambient = effect.GetMemberByName("Ambient").AsVector(),
                Diffuse = effect.GetMemberByName("Diffuse").AsVector(),
                Specular = effect.GetMemberByName("Specular").AsVector(),
                Pad = effect.GetMemberByName("pad").AsScalar(),
                Range = effect.GetMemberByName("Range").AsScalar(),
                Position = effect.GetMemberByName("Position").AsVector(),
                Attenuation = effect.GetMemberByName("Att").AsVector(),
            };
        }
    }

    public class EffectMaterialVariable : EffectVariable
    {
        public EffectVectorVariable Ambient;
        public EffectVectorVariable Diffuse;
        public EffectVectorVariable Specular;
        public EffectVectorVariable Reflect;


        public EffectMaterialVariable(IntPtr nativePtr): base(nativePtr){}

        public void Set(Material material)
        {
            Ambient.Set(material.Ambient);
            Diffuse.Set(material.Diffuse);
            Specular.Set(material.Specular);
            Reflect.Set(material.Reflect);
        }
    }

    public class EffectPointLightVariable : EffectVariable
    {
        public EffectVectorVariable Ambient;
        public EffectVectorVariable Diffuse;
        public EffectVectorVariable Specular;
        public EffectVectorVariable Attenuation;
        public EffectVectorVariable Position;
        public EffectScalarVariable Pad;
        public EffectScalarVariable Range;

        public EffectPointLightVariable(IntPtr nativePtr) : base(nativePtr) { }

        public void Set(PointLight light)
        {
            Ambient.Set(light.Ambient);
            Diffuse.Set(light.Diffuse);
            Specular.Set(light.Specular);
            Attenuation.Set(light.Attenuation);
            Pad.Set(light.Pad);
            Range.Set(light.Range);
            Position.Set(light.Position);
        }
    }
}
