using System.Collections.Generic;
using CastleOfDemise.mobs.Player;
using Godot;

namespace CastleOfDemise.Scripts.Menus.MultiLauncher;

public partial class GameManager : Control
{
    public static readonly Dictionary<int,Player> Players = new Dictionary<int,Player>();
    
    public override void _Ready()
    {
        
    }
    
    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double d)
    {

    }
    
    
}