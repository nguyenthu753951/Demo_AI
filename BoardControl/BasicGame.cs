using System;

namespace BoardControl
{
	/// <summary>
	/// Basic game class to hold game related stuff that will be 
	/// generic to all the games
	/// </summary>
	public abstract class BasicGame
	{
		/// <summary>
		/// has the game been restarted
		/// </summary>
		protected bool bGameRestarted;
		/// <summary>
		/// has the game started
		/// </summary>
		protected bool bGameStarted;
		/// <summary>
		/// haqs the game been paused
		/// </summary>
		protected bool bGamePaused;

		public bool GameRestarted
		{
			get
			{
				return bGameRestarted;
			}
			set
			{
				bGameRestarted = value;
			}
		}

		public bool GameStarted 
		{
			get
			{
				return bGameStarted;
			}
			set
			{
				bGameStarted = value;
			}
		}

		public bool GamePaused 
		{
			get
			{
				return bGamePaused;
			}
			set
			{
				bGamePaused = value;
			}
		}

		public BasicGame()
		{
			GameRestarted = false;
			GameStarted = false;
			GamePaused = false;
		}
	}
}
