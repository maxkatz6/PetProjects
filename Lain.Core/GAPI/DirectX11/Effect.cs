#if DX
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Lain.Core.Patterns;
using SharpDX;
using SharpDX.D3DCompiler;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using Lain.Core;

namespace Lain.GAPI
{
    public class Effect : Disposable
    {
        private static readonly Dictionary<string, Effect> effects = new Dictionary<string, Effect>();

        private static readonly Regex EffectRegex = new Regex(@"^technique(?:\s([^{]*))?(?:\s)?{([^{]*)}",
            RegexOptions.IgnoreCase | RegexOptions.Multiline);

        private readonly Dictionary<string, Technique> techniques = new Dictionary<string, Technique>();
        private Buffer ConstantMatrixBuffer;
        private static Texture LastTexture;
        private SamplerState SampleState;

        private Effect()
        {
        }

        public static Effect FromMemory(string file)
        {
            /*try */
            {
                var e = new Effect();
                var mc = EffectRegex.Matches(file);
                file = EffectRegex.Replace(file, "");
                foreach (Match mat in mc)
                {
                    var tech = new Technique();
                    var ts = mat.Groups[2].Value.Split(new[] { "=", ";" }, StringSplitOptions.RemoveEmptyEntries);
                    for (var i = 0; i < ts.Length; i += 2)
                    {
                        switch (ts[i].Trim())
                        {
                            case "VertexShader":
                                {
                                    var en = ts[i + 1].Trim();
                                    var sh = ShaderBytecode.Compile(file, en, GetShaderProfile("vs"));
                                    if (sh.HasErrors)
                                        throw new Exception(sh.Message);
                                    tech.VertexShader = new VertexShader(App.Render.Device, sh);
                                    string a;
                                    var m = new Regex(en + @"[\s]*\((.+:.+)\)").Match(file);
                                    if (m.Success && m.Groups.Count > 1)
                                        a = m.Groups[1].Value;
                                    else
                                        a =
                                            new Regex(@"struct " + new Regex(en + @"[\s]*\(([^\s]*)").Match(file).Groups[1].Value + @"[\s]*{([^}]*)}")
                                                .Match(file).Groups[1].Value;
                                    tech.AttribsContainer = new AttribsContainer(a, new ShaderSignature(sh));
                                    sh.Dispose();
                                    break;
                                }
                            case "PixelShader":
                                {
                                    var sh = ShaderBytecode.Compile(file, ts[i + 1].Trim(), GetShaderProfile("ps"));
                                    if (sh.HasErrors)
                                        throw new Exception(sh.Message);
                                    tech.PixelShader = new PixelShader(App.Render.Device, sh);
                                    sh.Dispose();
                                    break;
                                }
                            case "GeometryShader":
                                {
                                    var sh = ShaderBytecode.Compile(file, ts[i + 1].Trim(), GetShaderProfile("gs"));
                                    if (sh.HasErrors)
                                        throw new Exception(sh.Message);
                                    tech.GeometryShader = new GeometryShader(App.Render.Device, sh);
                                    sh.Dispose();
                                    break;
                                }
                        }
                    }
                    e.techniques.Add(mat.Groups[1].Value.Trim(), tech);
                }

                e.ConstantMatrixBuffer = Buffer.Create<Matrix>(BindFlag.ConstantBuffer);

                e.SampleState = new SamplerState(App.Render.Device, new SamplerStateDescription
                {
                    //Filter = Filter.Anisotropic,
                    AddressU = TextureAddressMode.Wrap,
                    AddressV = TextureAddressMode.Wrap,
                    AddressW = TextureAddressMode.Wrap,
                    MipLodBias = 0,
                    MaximumAnisotropy = 1,
                    ComparisonFunction = Comparison.Always,
                    BorderColor = new Color4(0, 0, 0, 0),
                    MinimumLod = 0,
                    MaximumLod = float.MaxValue
                });
                return e; }
            /*catch (Exception ex)
            {
                ErrorProvider.SendError(ex);
                return new Effect();
            }*/
        }

        public static Effect FromFile(string s)
        {
            if (effects.ContainsKey(s)) return effects[s];
            effects.Add(s, FromMemory(FileSystem.LoadContent(FileSystem.GetPath(s, "Effects"))));
            return effects[s];
        }
        public void Render(int count, int startIndex = 0, bool draw = true)
        {
            Render(techniques.First().Key, count, startIndex, draw);
        }
        public void Render(string techName, int count, int startIndex = 0, bool draw = true)
        {
            try
            {
                var tech = techniques[techName];
                tech.AttribsContainer.Accept();
                App.Render.DeviceContext.VertexShader.Set(tech.VertexShader);
                App.Render.DeviceContext.PixelShader.Set(tech.PixelShader);
                App.Render.DeviceContext.PixelShader.SetSampler(0, SampleState);
                if (tech.GeometryShader != null)
                {
                    App.Render.DeviceContext.InputAssembler.PrimitiveTopology = PrimitiveTopology.PointList;
                    App.Render.DeviceContext.GeometryShader.Set(tech.GeometryShader);
                    App.Render.DeviceContext.Draw(count, startIndex);
                    App.Render.DeviceContext.InputAssembler.PrimitiveTopology = PrimitiveTopology.TriangleList;
                    App.Render.DeviceContext.GeometryShader.Set(null);
                }
                else if (draw)
                {
                    App.Render.DeviceContext.DrawIndexed(count, startIndex, 0);
                }
            }
            catch (Exception ex)
            {
                ErrorProvider.SendError(ex);
            }
        }

        public unsafe void SetMatrix(Matrix mt)
        {
            *(Matrix*)ConstantMatrixBuffer.MapBuffer() = Matrix.Transpose(mt);
            ConstantMatrixBuffer.UnmapBuffer();
            App.Render.DeviceContext.VertexShader.SetConstantBuffer(0, ConstantMatrixBuffer);
        }

        public void SetTexture(Texture tex)
        {
            if (tex == LastTexture) return;
            App.Render.DeviceContext.PixelShader.SetShaderResource(0, tex);
            LastTexture = tex;
        }

        public void SetTextures(Texture[] tex)
        {
            var arr = new ShaderResourceView[tex.Length];
            for (var i = 0; i < tex.Length; i++)
                arr[i] = tex[i];
            App.Render.DeviceContext.PixelShader.SetShaderResources(0, arr);
        }

        private struct Technique
        {
            public AttribsContainer AttribsContainer;
            public GeometryShader GeometryShader;
            public PixelShader PixelShader;
            public VertexShader VertexShader;
        }

        private static string GetShaderProfile(string shader)
        {
            switch (App.Render.ShaderModel)
            {
                case 5:
                    return shader + "_5_0";
                case 4:
                    return shader + "_4_0";
                default:
                    return shader + "_4_0_level_9_3";
            }
        }
	}
}

#endif