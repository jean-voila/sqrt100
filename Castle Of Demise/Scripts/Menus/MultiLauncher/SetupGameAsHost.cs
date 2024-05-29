using System;
using Godot;

namespace CastleOfDemise.Scripts.Menus.MultiLauncher;

public partial class SetupGameAsHost : Control
{
	// Called when the node enters the scene tree for the first time.

	[Export] private ItemList _chooseGameMode;
	[Export] private ItemList _scoreToReach;
	[Export] private Button _startGameButton;
	[Export] private string _address = "127.0.0.1";
	private int? _scoreToReachValue = null;
	private int? _gameModeValue = null;
	
	public override void _Ready()
	{
		_chooseGameMode = GetNode<ItemList>("ChooseGameMode");
		_scoreToReach = GetNode<ItemList>("ScoreToReach");
		_startGameButton = GetNode<Button>("StartGame");
		
		
		
		
	}
	

	// Ces 2 fonctions sont des fonctions de select pour le multi
	private void _on_choose_game_mode_item_selected(int index)
	{
		GD.Print("Game mode: " + _chooseGameMode.GetItemText(index));
		if (index != 0)
		{
			_gameModeValue = index;
		}
		else
		{
			_gameModeValue = null;
		}
		
		if (_gameModeValue != null && _scoreToReachValue != null)
		{
			_startGameButton.Disabled = false;
		}
		else
		{
			_startGameButton.Disabled = true;
		}
	}


	private void _on_score_to_reach_item_selected(int index)
	{
		GD.Print("Score to reach: " + _scoreToReach.GetItemText(index));
		if (index != 0)
		{
			_scoreToReachValue = int.Parse(_scoreToReach.GetItemText(index));
		}
		else
		{
			_scoreToReachValue = null;
		}
		
		if (_gameModeValue != null && _scoreToReachValue != null)
		{
			_startGameButton.Disabled = false;
		}
		else
		{
			_startGameButton.Disabled = true;
		}
	}
	
	private void _on_start_game_pressed()
	{
		_startGameButton.Disabled = true;
		var scene = GD.Load<PackedScene>("res://maps/mpMap01.tscn").Instantiate();
		GetTree().Root.AddChild(scene);
		this.Hide();
		Rpc("StartGame");
	}
	
	[Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = true, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]

	private void StartGame()
	{
		var scene = ResourceLoader.Load<PackedScene>("res://Scenes/Map/world1.tscn").Instantiate();
		GetTree().Root.AddChild(scene);
		this.Hide();
	}
	
	private void SendPlayerInformation(string name, int id)
	{
		throw new NotImplementedException();
	}
}






