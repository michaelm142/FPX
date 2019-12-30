using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Microsoft.Xna.Framework;

namespace FPX
{
    public static class AssetManager
    {
        public static Dictionary<string, ContentType> Assets { get; private set; } = new Dictionary<string, ContentType>();

        public static void Inilitize()
        {
            Debug.Log("Asset manager initializing...");

            AnalyzeDirectory(new DirectoryInfo(GameCore.content.RootDirectory));
        }

        private static void AnalyzeDirectory(DirectoryInfo directory)
        {
            var files = directory.GetFiles().ToList().FindAll(file =>  file.Extension == ".xnb");
            foreach (var file in files)
            {
                using (var reader = file.OpenText())
                {
                    string line = reader.ReadLine();
                    while (line != null && !reader.EndOfStream)
                    {
                        if (line.IndexOf("Texture2DReader") != -1)
                        {
                            if (Assets.Keys.Contains(file.Name))
                                break;
                            Debug.Log("Adding asset {0} as type {1}", file.FullName, ContentType.Texture);
                            Assets.Add(file.Name, ContentType.Texture);
                            break;
                        }
                        else if (line.IndexOf("VertexBufferReader") != -1)
                        {
                            if (Assets.Keys.Contains(file.Name))
                                break;
                            Debug.Log("Adding asset {0} as type {1}", file.FullName, ContentType.Model);
                            Assets.Add(file.Name, ContentType.Model);
                            break;
                        }
                        else if (line.IndexOf("SoundEffectReader") != -1)
                        {
                            if (Assets.Keys.Contains(file.Name))
                                break;
                            Debug.Log("Adding asset {0} as type {1}", file.FullName, ContentType.Sound);
                            Assets.Add(file.Name, ContentType.Sound);
                            break;
                        }

                        line = reader.ReadLine();
                    }
                }
            }

            foreach (var dir in directory.GetDirectories())
                AnalyzeDirectory(dir);
        }

        public enum ContentType
        {
            Texture,
            Model,
            Sound,
        }
    }
}