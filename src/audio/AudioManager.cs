using System.Collections.Generic;
using System.Diagnostics;
using Godot;


public partial class AudioManager : Node2D
{
	public static  AudioManager Instance => _instance;
	private static AudioManager _instance;
	
	[Export] private AudioStreamPlayer                       _musicPlayer;
	[Export] private Node2D                                  _sfxPool;
	[Export] private Godot.Collections.Array<AudioStreamWav> _audioResources;
	
	private List<AudioStreamPlayer2D> _localPlayerList = new();
	private List<AudioStreamPlayer> _globalPlayerList = new();
	private const float MinVolumeDb = -80.0f; // Equivalent to 0% volume
	private const float MaxVolumeDb = 0.0f;   // 100% volume
	
	public override void _Ready()
	{
		_instance = this;
		var nodes = _sfxPool.GetChildren();
		foreach (Node node in nodes)
		{
			if (node is AudioStreamPlayer2D audioStream)
			{
				_localPlayerList.Add(audioStream);
			}
			else if (node is AudioStreamPlayer audioStreamPlayer)
			{
				_globalPlayerList.Add(audioStreamPlayer);
			}
		}
		
		//PlayMusic(AudioType.MusicMainTheme, 0.35f);
	}

	public AudioStreamPlayer PlayGlobalSFX(AudioType audioType, float volume = 0.5f)
	{
		AudioStreamPlayer player = new AudioStreamPlayer();
		_sfxPool.AddChild(player);
		_globalPlayerList.Add(player);

		player.Stream     = _audioResources[(int)audioType];
		player.VolumeDb   = GetVolumeDbFromPercievedLinear(volume);
		player.PitchScale = 1.0f + GD.Randf() * 0.1f; // Small pitch variation

		player.Play();

		return player;

	}

	public AudioStreamPlayer PlayGlobalSFXLoop(AudioType audioType, float linearVolume = 0.5f)
	{
		AudioStreamPlayer player = new AudioStreamPlayer();
		_sfxPool.AddChild(player);
		_globalPlayerList.Add(player);

		player.Stream     = _audioResources[(int)audioType];
		player.VolumeDb   = GetVolumeDbFromPercievedLinear(linearVolume);
		player.PitchScale = 1.0f + GD.Randf() * 0.1f; // Small pitch variation

		player.Play();
		if (player.Stream  is AudioStreamWav streamWav)
		{
			streamWav.LoopMode  = AudioStreamWav.LoopModeEnum.Forward;
			streamWav.LoopBegin = 0;
			streamWav.LoopEnd   = (int)(streamWav.GetLength() * streamWav.MixRate);

		}

		return player;
	}

	public AudioStreamPlayer2D PlaySFX(AudioType audioType, Vector2 globalPosition, float radius = 250.0f,
		float                                    linearVolume = 0.5f)
	{
		AudioStreamPlayer2D player = new AudioStreamPlayer2D();
		_sfxPool.AddChild(player);
		_localPlayerList.Add(player);

		player.Stream         = _audioResources[(int)audioType];
		player.GlobalPosition = globalPosition;
		player.MaxDistance    = radius;
		player.VolumeDb       = GetVolumeDbFromPercievedLinear(linearVolume);
		player.PitchScale     = 1.0f + GD.Randf() * 0.1f; // Small pitch variation

		player.Finished += player.QueueFree;
		player.Play();

		return player;
	}

	public AudioStreamPlayer2D PlaySFXLoop(AudioType audioType, Vector2 globalPosition, float radius = 250.0f,
		float                                        linearVolume = 0.5f)
	{
		AudioStreamPlayer2D player = new AudioStreamPlayer2D();
		_sfxPool.AddChild(player);
		_localPlayerList.Add(player);

		player.Stream         = _audioResources[(int)audioType];
		player.GlobalPosition = globalPosition;
		player.MaxDistance    = radius;
		player.VolumeDb       = GetVolumeDbFromPercievedLinear(linearVolume);
		player.PitchScale     = 1.0f + GD.Randf() * 0.1f; // Small pitch variation
		player.Play();

		if (player.Stream  is AudioStreamWav streamWav)
		{
			streamWav.LoopMode  = AudioStreamWav.LoopModeEnum.Forward;
			streamWav.LoopBegin = 0;
			streamWav.LoopEnd   = (int)(streamWav.GetLength() * streamWav.MixRate);

		}

		return player;
	}
	
	public void StopMusic()
	{
		_musicPlayer.Stop();
	}

	public void PlayMusic(AudioType audioType, float linearVolume = 0.4f)
	{
		Debug.Assert(linearVolume >= 0.0f && linearVolume <= 1.0f);
		
		_musicPlayer.Stop();
		_musicPlayer.Stream     = _audioResources[(int)audioType];
		_musicPlayer.VolumeDb   = GetVolumeDbFromPercievedLinear(linearVolume);
		_musicPlayer.Play();

		if (_musicPlayer.Stream is AudioStreamWav streamWav)
		{
			streamWav.LoopMode  = AudioStreamWav.LoopModeEnum.Forward;
			streamWav.LoopBegin = 0;
			streamWav.LoopEnd   = (int)(streamWav.GetLength() * streamWav.MixRate);
		}
	}
	
	private float GetVolumeDbFromPercievedLinear(float linearVolume)
	{
		float adjustedVolume = Mathf.Pow(Mathf.Clamp(linearVolume,0f, 1f), 0.33f); 
		return Mathf.Lerp(-80.0f, 0.0f, adjustedVolume);	
	}
}
