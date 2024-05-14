using System;
using Godot;

namespace CastleOfDemise.mobs.Player;

public partial class Player
{
    private String _pixelShaderPath;
    private MeshInstance3D _pixelShader;
    public bool _pixelShaderEnabled;

    public void _graphismsInit()
    {
        _pixelShaderEnabled = false;
        _pixelShaderPath = "Head/Camera3D/PixeliseShader";
        _pixelShader = GetNode<MeshInstance3D>(_pixelShaderPath);
    }

    public void SwitchPixelShader()
    {
        _pixelShaderEnabled = !_pixelShaderEnabled;
        _pixelShader.Visible = _pixelShaderEnabled;
    }
}