using Godot;
using System;
using System.Net.NetworkInformation;

public partial class hot_key_rebind_button : Control
{
	private Label _label;
	private Button _button;
	[Export] public string action_name = "key_z";
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_label = GetNode<Label>("HBoxContainer/Label");
		_button = GetNode<Button>("HBoxContainer/Button");
		SetProcessUnhandledKeyInput(false);
		SetActionName();
		SetTextForKey();
	}
	public void SetActionName()
	{
		_label.Text = action_name;
		switch (action_name)
		{
			case "key_z" :
				_label.Text = "Avancer";
				break;
			case "key_s" :
				_label.Text = "Reculer";
				break;
			case "key_q" :
				_label.Text = "Déplacement vers la gauche";
				break;
			case "key_d" :
				_label.Text = "Déplacement vers la droite";
				break;
			case "key_space" :
				_label.Text = "Sauter";
				break;
            case "key_r" :
				_label.Text = "Recharger";
	            break;
            case "key_escape" :
				_label.Text = "Pause";
	            break;
		}
	}
	public void SetTextForKey()
	{
		var actionEvents = InputMap.ActionGetEvents(action_name);
		InputEvent actionEvent = actionEvents[0];
		_button.Text = $"{actionEvent.AsText().TrimSuffix(" (Physical)")}";
	}

	private void _on_button_toggled(bool buttonPressed)
	{
		if (buttonPressed)
		{
			_button.Text = "Appuyer sur une touche...";
			SetProcessUnhandledKeyInput(buttonPressed);
			foreach (var i in GetTree().GetNodesInGroup("hotkey_button"))
			{
				if (i is hot_key_rebind_button button && button.action_name != this.action_name)
				{
					button._button.ToggleMode = false;
					button.SetProcessUnhandledKeyInput(false);
				}
			}
		}
		else
		{
			foreach (var i in GetTree().GetNodesInGroup("hotkey_button"))
			{
				if (i is hot_key_rebind_button button && button.action_name != this.action_name)
				{
					button._button.ToggleMode = true;
					button.SetProcessUnhandledKeyInput(false);
				}
			}
			SetTextForKey();
		}
	}

	public override void _UnhandledKeyInput(InputEvent @event)
	{
		base._UnhandledKeyInput(@event);
		RebindActionKey(@event);
		_button.ButtonPressed = false;
	}

	public void RebindActionKey(InputEvent @event)
	{
		InputMap.ActionEraseEvents(action_name);
		InputMap.ActionAddEvent(action_name,@event);
		SetProcessUnhandledKeyInput(false);
		SetTextForKey();
		SetActionName();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
