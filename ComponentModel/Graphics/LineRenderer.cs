using System.Collections.Generic;
using System.Xml;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FPX;
using System;

public class LineRenderer : Component, IDrawable
{
    private const float UpdateInterval = 0.1f;
    private float updateTimer = UpdateInterval;

    public bool useWorldSpace = true;

    public List<Vector3> positions = new List<Vector3>();

    public List<VertexPositionColor> vertecies = new List<VertexPositionColor>();

    public BasicEffect effect;

    public Material material;

    public event EventHandler<EventArgs> DrawOrderChanged;
    public event EventHandler<EventArgs> VisibleChanged;

    public int DrawOrder { get; set; }

    public bool Visible { get; set; } = true;


    public void Start()
    {
        if (material == null)
            material = gameObject.AddComponent<Material>();

        for (int i = 0; i < positions.Count; i++)
            vertecies.Add(new VertexPositionColor(positions[i], Color.White));

        effect = new BasicEffect(GameCore.graphicsDevice);
    }

    public void Update(GameTime gameTime)
    {
        updateTimer -= Time.deltaTime;
        if (updateTimer < 0.0f)
        {
            for (int i = 0; i < positions.Count; i++)
            {
                if (i < vertecies.Count)
                {
                    var vertex = vertecies[i];
                    vertex.Position = positions[i];
                    vertex.Color = material.DiffuseColor;
                    vertecies[i] = vertex;
                }
                else
                    vertecies.Add(new VertexPositionColor(positions[i], material.DiffuseColor));
            }

            updateTimer = UpdateInterval;
        }
    }

    public void Draw(GameTime gameTime)
    {
        var bs = GameCore.graphicsDevice.BlendState;
        GameCore.graphicsDevice.BlendState = material.blendState;

        effect.View = Camera.Active.ViewMatrix;
        effect.Projection = Camera.Active.ProjectionMatrix;
        if (useWorldSpace)
            effect.World = transform.worldPose;

        effect.DiffuseColor = material.DiffuseColor.ToVector3();
        effect.SpecularColor = material.SpecularColor.ToVector3();
        effect.CurrentTechnique.Passes[0].Apply();

        GameCore.graphicsDevice.DrawUserPrimitives(PrimitiveType.LineStrip, vertecies.ToArray(), 0, vertecies.Count - 1);
        GameCore.graphicsDevice.BlendState = bs;
    }

    public override void LoadXml(XmlElement element)
    {
        var positionsNode = element.SelectSingleNode("Positions");
        var useWorldSpaceNode = element.SelectSingleNode("UseWorldSpace");

        if (positionsNode != null)
        {
            foreach (XmlElement node in positionsNode.ChildNodes)
            {
                Vector3 pos = LinearAlgebraUtil.Vector3FromXml(node);
                positions.Add(pos);
            }
        }

        if (useWorldSpaceNode != null)
            useWorldSpace = bool.Parse(useWorldSpaceNode.InnerText);
    }
}
