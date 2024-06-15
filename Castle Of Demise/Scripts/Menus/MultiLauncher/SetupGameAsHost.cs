using System;
using System.Net;
using System.Net.Sockets;
using CastleOfDemise.mobs.Player;
using Godot;

namespace CastleOfDemise.Scripts.Menus.MultiLauncher;

public partial class SetupGameAsHost : Control
{
	// Called when the node enters the scene tree for the first time.

	[Export] private ItemList _chooseGameMode;
	[Export] private ItemList _scoreToReach;
	[Export] private Button _startGameButton;
	[Export] private string _address = GetLocalIPAdress();

	public static int _scoreToReachValue;
	public static int _gameModeValue;
	
	public override void _Ready()
	{
		_chooseGameMode = GetNode<ItemList>("ChooseGameMode");
		_scoreToReach = GetNode<ItemList>("ScoreToReach");
		_startGameButton = GetNode<Button>("StartGame");
		GetNode<Label>("%HostCode").Text = CodeParser.IpToCode(_address);
	}

	private static string GetLocalIPAdress()
	{
		var host = Dns.GetHostEntry(Dns.GetHostName());
		foreach (var ip in host.AddressList)
		{
			if (ip.AddressFamily == AddressFamily.InterNetwork) return ip.ToString();
		}

		return "127.0.0.1";
	}

	private void _on_back_button_fromhost_button_up()
	{
		GetNode<Control>("%SetupGameAsHost").Hide();
		GetNode<Control>("%MultiplayerMenu").Show();
		GD.Print("Going from HostMenu to MultiplayerMenu");
		if (MultiplayerMenu.Peer != null) MultiplayerMenu.Peer.Close();
	}
	
	// Ces 2 fonctions sont des fonctions de select pour le multi
	private void _on_choose_game_mode_item_selected(int index)
	{
		GD.Print("Game mode: " + _chooseGameMode.GetItemText(index));
		_gameModeValue = index;
	}


	private void _on_score_to_reach_item_selected(int index)
	{
		GD.Print("Score to reach: " + _scoreToReach.GetItemText(index));
		if (index == 0)
		{
			_scoreToReachValue = 0;
		}
		else
		{
			_scoreToReachValue = _scoreToReach.GetItemText(index).ToInt();
		}
	}
	
	private void _on_start_game_pressed()
	{
		_startGameButton.Disabled = true;

		var multiplayerMenu = (MultiplayerMenu)GetNode("%MultiplayerMenu");
		multiplayerMenu.Rpc(nameof(multiplayerMenu.StartGame));
		MultiplayerHUD.ScoretoReachValue = _scoreToReachValue;
	}
	
	
	// this function will 
	
	
	public override void _Process(double d)
	{
		if (_gameModeValue != 0 && _scoreToReachValue != 0 && GameManager.Players.Count == 2)
		{
			_startGameButton.Disabled = false;
		}
		else
		{
			_startGameButton.Disabled = true;
		}
	}

	
	
}






