using System;
using System.Collections;

namespace BoardControl
{
	/// <summary>
	/// class to control the game of connect four
	/// </summary>
	public class ConnectFourGame : BasicGame
	{
		/// <summary>
		/// array to hold the information about the squares in the game
		/// </summary>
		private ArrayList arraySquares;
		/// <summary>
		/// is the player playing as red
		/// </summary>
		private bool bPlayerIsRed;
		/// <summary>
		/// message for output display
		/// </summary>
		private string strOutputText;
		/// <summary>
		/// is there a new message to output
		/// </summary>
		private bool bNewOutputText;
		/// <summary>
		/// is it the computers turn to move
		/// </summary>
		private bool bIsComputersMove;
		/// <summary>
		/// set to true if the game is won
		/// </summary>
		private bool bIsGameWon;
		/// <summary>
		/// did the player win
		/// </summary>
		private bool bHasPlayerWon;
		/// <summary>
		/// has the game started
		/// </summary>
		private bool bIsStarted;
		/// <summary>
		/// has the game game been paused
		/// </summary>
		private bool bIsPaused;

		public ArrayList ArraySquares
		{
			get
			{
				return arraySquares;
			}
		}

		public bool PlayerIsRed
		{
			get
			{
				return bPlayerIsRed;
			}
			set
			{
				bPlayerIsRed = value;
			}
		}

		public string OutputText
		{
			get
			{
				NewOutputText = false;
				return strOutputText;
			}
			set
			{
				NewOutputText = true;
				strOutputText = value;
			}
		}

		public bool NewOutputText
		{
			get
			{
				return bNewOutputText;
			}
			set
			{
				bNewOutputText = value;
			}
		}

		public bool IsComputersMove
		{
			get
			{
				return bIsComputersMove;
			}
			set
			{
				bIsComputersMove = false;
			}
		}

		public bool IsGameWon
		{
			get
			{
				return bIsGameWon;
			}
			set
			{
				bIsGameWon = value;
			}
		}

		public bool HasPlayerWon
		{
			get
			{
				return bHasPlayerWon;
			}
			set
			{
				bHasPlayerWon = value;
			}
		}

		public bool IsStarted
		{
			get
			{
				return bIsStarted;
			}
			set
			{
				bIsStarted = value;
			}
		}

		public bool IsPaused
		{
			get
			{
				return bIsPaused;
			}
			set
			{
				bIsPaused = value;
			}
		}

		public ConnectFourGame()
		{
			arraySquares = new ArrayList();

			arraySquares.Add( new ConnectFourSquareInfo( "AA", "EMPTY" ) );
			arraySquares.Add( new ConnectFourSquareInfo( "AB", "EMPTY" ) );
			arraySquares.Add( new ConnectFourSquareInfo( "AC", "EMPTY" ) );
			arraySquares.Add( new ConnectFourSquareInfo( "AD", "EMPTY" ) );
			arraySquares.Add( new ConnectFourSquareInfo( "AE", "EMPTY" ) );
			arraySquares.Add( new ConnectFourSquareInfo( "AF", "EMPTY" ) );
			arraySquares.Add( new ConnectFourSquareInfo( "BA", "EMPTY" ) );
			arraySquares.Add( new ConnectFourSquareInfo( "BB", "EMPTY" ) );
			arraySquares.Add( new ConnectFourSquareInfo( "BC", "EMPTY" ) );
			arraySquares.Add( new ConnectFourSquareInfo( "BD", "EMPTY" ) );
			arraySquares.Add( new ConnectFourSquareInfo( "BE", "EMPTY" ) );
			arraySquares.Add( new ConnectFourSquareInfo( "BF", "EMPTY" ) );
			arraySquares.Add( new ConnectFourSquareInfo( "CA", "EMPTY" ) );
			arraySquares.Add( new ConnectFourSquareInfo( "CB", "EMPTY" ) );
			arraySquares.Add( new ConnectFourSquareInfo( "CC", "EMPTY" ) );
			arraySquares.Add( new ConnectFourSquareInfo( "CD", "EMPTY" ) );
			arraySquares.Add( new ConnectFourSquareInfo( "CE", "EMPTY" ) );
			arraySquares.Add( new ConnectFourSquareInfo( "CF", "EMPTY" ) );
			arraySquares.Add( new ConnectFourSquareInfo( "DA", "EMPTY" ) );
			arraySquares.Add( new ConnectFourSquareInfo( "DB", "EMPTY" ) );
			arraySquares.Add( new ConnectFourSquareInfo( "DC", "EMPTY" ) );
			arraySquares.Add( new ConnectFourSquareInfo( "DD", "EMPTY" ) );
			arraySquares.Add( new ConnectFourSquareInfo( "DE", "EMPTY" ) );
			arraySquares.Add( new ConnectFourSquareInfo( "DF", "EMPTY" ) );
			arraySquares.Add( new ConnectFourSquareInfo( "EA", "EMPTY" ) );
			arraySquares.Add( new ConnectFourSquareInfo( "EB", "EMPTY" ) );
			arraySquares.Add( new ConnectFourSquareInfo( "EC", "EMPTY" ) );
			arraySquares.Add( new ConnectFourSquareInfo( "ED", "EMPTY" ) );
			arraySquares.Add( new ConnectFourSquareInfo( "EE", "EMPTY" ) );
			arraySquares.Add( new ConnectFourSquareInfo( "EF", "EMPTY" ) );
			arraySquares.Add( new ConnectFourSquareInfo( "FA", "EMPTY" ) );
			arraySquares.Add( new ConnectFourSquareInfo( "FB", "EMPTY" ) );
			arraySquares.Add( new ConnectFourSquareInfo( "FC", "EMPTY" ) );
			arraySquares.Add( new ConnectFourSquareInfo( "FD", "EMPTY" ) );
			arraySquares.Add( new ConnectFourSquareInfo( "FE", "EMPTY" ) );
			arraySquares.Add( new ConnectFourSquareInfo( "FF", "EMPTY" ) );
			arraySquares.Add( new ConnectFourSquareInfo( "GA", "EMPTY" ) );
			arraySquares.Add( new ConnectFourSquareInfo( "GB", "EMPTY" ) );
			arraySquares.Add( new ConnectFourSquareInfo( "GC", "EMPTY" ) );
			arraySquares.Add( new ConnectFourSquareInfo( "GD", "EMPTY" ) );
			arraySquares.Add( new ConnectFourSquareInfo( "GE", "EMPTY" ) );
			arraySquares.Add( new ConnectFourSquareInfo( "GF", "EMPTY" ) );

			PlayerIsRed = true;
			NewOutputText = false;
			IsComputersMove = false;
			IsGameWon = false;
			HasPlayerWon = false;
			IsStarted = false;
			IsPaused = false;
		}

		public void SetSquare( string squareIdentifier, string squareColor )
		{
			bool bFound = false;

			for( int i=0; i<arraySquares.Count && bFound == false; i++ )
			{
				ConnectFourSquareInfo squareInfo = ( ConnectFourSquareInfo )arraySquares[ i ];

				if( squareInfo.SquareIdentifier == squareIdentifier )
				{
					squareInfo.SquareColor = squareColor;
					bFound = true;

					if( squareColor != "EMPTY" )
					{
						squareInfo.IsOccupied = true;
					}
					else
					{
						squareInfo.IsOccupied = false;
					}
				}
			}
		}

		public ConnectFourSquareInfo GetSquareInfo( string squareIdentifier )
		{
			for( int i=0; i<arraySquares.Count; i++ )
			{
				ConnectFourSquareInfo squareInfo = ( ConnectFourSquareInfo )arraySquares[ i ];

				if( squareInfo.SquareIdentifier == squareIdentifier )
				{
					return squareInfo;
				}
			}

			return null;
		}

		/// <summary>
		/// get the identifier for a square from the square info location
		/// </summary>
		/// <param name="nValue"></param>
		/// <returns></returns>
		public string GetIdentifierAt( int identifierValue )
		{
			if( identifierValue > arraySquares.Count )
				return null;

			return ( ( ConnectFourSquareInfo )arraySquares[ identifierValue ] ).SquareIdentifier;
		}

		/// <summary>
		/// reset the game back to the start
		/// </summary>
		public void Reset()
		{
			for( int i=0; i<arraySquares.Count; i++ )
			{
				SetSquare( GetIdentifierAt( i ), "EMPTY" );
			}
		}

	}


	/// <summary>
	/// class to hold the info needed by each square to play the game
	/// </summary>
	public class ConnectFourSquareInfo
	{
		/// <summary>
		/// string to hold the square identifier
		/// </summary>
		private string strSquareIdentifier;
		/// <summary>
		/// square colour will be "EMPTY" ( Green ) "RED" or "BLUE"
		/// </summary>
		private string strSquareColor; 
		/// <summary>
		/// has the square been occupied
		/// </summary>
		private bool bIsOccupied;

		public string SquareIdentifier
		{
			get
			{
				return strSquareIdentifier;
			}
			set
			{
				strSquareIdentifier = value;
			}
		}

		public string SquareColor
		{
			get
			{
				return strSquareColor;
			}
			set
			{
				strSquareColor = value;
			}
		}

		public bool IsRed 
		{
			get
			{
				if( strSquareColor == "RED" )
					return true;
				else
					return false;
			}
		}

		public bool IsOccupied 
		{
			get
			{
				return bIsOccupied;
			}
			set
			{
				bIsOccupied = value;
			}
		}

		public ConnectFourSquareInfo( string squareIdentifier, string squareColor )
		{
			SquareIdentifier = squareIdentifier;
			SquareColor = squareColor;
			IsOccupied = false;
		}
	}
}
