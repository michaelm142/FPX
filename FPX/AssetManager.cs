using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace FPX
{
    public static class AssetManager
    {
        public static Dictionary<string, ContentReference> Assets { get; private set; } = new Dictionary<string, ContentReference>();

        private const string ContentRootDirectory = "\\Content\\";

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
                string contentRelitiveFilePath = file.FullName.Split(new string[] { ContentRootDirectory }, StringSplitOptions.RemoveEmptyEntries)[1];
                contentRelitiveFilePath = contentRelitiveFilePath.Split(new char[] { '.' })[0];
                using (var reader = file.OpenText())
                {
                    string line = reader.ReadLine();
                    while (line != null && !reader.EndOfStream)
                    {   
                        if (line.IndexOf("SpriteFontReade") != -1)
                        {
                            if (Assets.Keys.Contains(file.Name))
                                break;
                            Debug.Log("Adding asset {0} as type {1}", file.FullName, ContentType.SpriteFont);
                            Assets.Add(file.Name, new ContentReference(ContentType.SpriteFont, file.FullName, GameCore.content.Load<SpriteFont>(contentRelitiveFilePath)));
                            break;
                        }
                        else if (line.IndexOf("Texture2DReader") != -1)
                        {
                            if (Assets.Keys.Contains(file.Name))
                                break;
                            Debug.Log("Adding asset {0} as type {1}", file.FullName, ContentType.Texture);
                            Assets.Add(file.Name, new ContentReference(ContentType.Texture, file.FullName, GameCore.content.Load<Texture2D>(contentRelitiveFilePath)));
                            break;
                        }
                        else if (line.IndexOf("VertexBufferReader") != -1)
                        {
                            if (Assets.Keys.Contains(file.Name))
                                break;
                            Debug.Log("Adding asset {0} as type {1}", file.FullName, ContentType.Model);
                            Assets.Add(file.Name, new ContentReference(ContentType.Model, file.FullName, GameCore.content.Load<Model>(contentRelitiveFilePath)));
                            break;
                        }
                        else if (line.IndexOf("SoundEffectReader") != -1)
                        {
                            if (Assets.Keys.Contains(file.Name))
                                break;
                            Debug.Log("Adding asset {0} as type {1}", file.FullName, ContentType.Sound);
                            Assets.Add(file.Name, new ContentReference(ContentType.Sound, file.FullName, GameCore.content.Load<SoundEffect>(contentRelitiveFilePath)));
                            break;
                        }
                        else
                        {
                            if (Assets.Keys.Contains(file.Name))
                                break;
                            Debug.Log("Adding asset {0} as type {1}", file.FullName, ContentType.Default);
                            Assets.Add(file.Name, new ContentReference(ContentType.Texture, file.FullName, null));
                        }

                        line = reader.ReadLine();
                    }
                }
            }

            foreach (var dir in directory.GetDirectories())
                AnalyzeDirectory(dir);
        }

        public class ContentReference
        {
            public ContentType contentType;

            public Type type;

            public string filename;

            public object Data { get; private set; }

            public T GetData<T>()
                where T : class
            {
                return Data as T;
            }

            public ContentReference(ContentType contentType, string filename, object Data)
            {
                this.contentType = contentType;
                this.filename = filename;
                this.Data = Data;
                type = typeof(object);

                switch (contentType)
                {
                    case ContentType.Model:
                        type = typeof(Model);
                        break;
                    case ContentType.Sound:
                        type = typeof(SoundEffect);
                        break;
                    case ContentType.Texture:
                        type = typeof(Texture2D);
                        break;
                }
            }
        }

        public enum ContentType
        {
            Default,
            Model,
            Sound,
            Texture,
            SpriteFont,
            Folder,
        }
    }
}