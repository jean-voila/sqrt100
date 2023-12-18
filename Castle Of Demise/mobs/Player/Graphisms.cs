using Godot;
using System;

public partial class Player
    {
        private String _pixelShaderPath;
        private MeshInstance _pixelShader;
        public bool _pixelShaderEnabled;

        public void _graphismsInit()
        {
            _pixelShaderEnabled = true;
            _pixelShaderPath = "Head/Camera/PixeliseShader";
            _pixelShader = GetNode<MeshInstance>(_pixelShaderPath);
        }

        public void SwitchPixelShader()
        {
            _pixelShaderEnabled = !_pixelShaderEnabled;
            _pixelShader.Visible = _pixelShaderEnabled;
        }
    }
