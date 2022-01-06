using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Text;
using System.IO;
using System.Xml;

namespace BoardControl
{
	public class ConnectFourBoard : BoardControl.Board
	{
		/// <summary>
		/// has the board been intialized
		/// </summary>
		private static bool bIsInitialized = false;

		/// debug helper
		private bool bDisplayBestMove = false;


		/// <summary>
		/// class object to play the connect four game
		/// </summary>
		private ConnectFourGame connectFourGame = null;
		/// <summary>
		/// pattern collection for this game
		/// </summary>
		private ConnectFourPatternCollection patternCollection;
		private ConnectFourPatternCollection historicalPatternCollection;
		private string strFileName = "ConnectFourPatterns.xml";
		private ArrayList arrayValidSquares;

		private System.ComponentModel.IContainer components = null;


		/// <summary>
		/// Weighting values for the different types of patterns
		/// </summary>
		/// 
		const int nAggressiveWeight = 10;
		const int nDefensiveWeight = 7;
		const int nTacticalWeight = 3;
		const int nWarningWeight = 5;
		const int nMemoryAggressiveWeight = 105;
		const int nMemoryDefensiveWeight = 104;
		const int nMemoryTacticalWeight = 103;
		const int nMemoryWarningWeight = 102;

		/// <summary>
		/// try to make the random moves more
		/// well random really
		/// </summary>
		private char chPreviousMove;


		public ConnectFourBoard()
		{
			// This call is required by the Windows Form Designer.
			InitializeComponent();

			///InitializeBoard();
			
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		public ConnectFourGame GetConnectFourGame
		{
			get
			{
				return connectFourGame;
			}
		}


		/// <summary>
		/// Initialize the board
		/// </summary>
		public void InitializeBoard()
		{
			HorizontalSquares = 7;
			VerticalSquares = 6;
			SquareWidth = 75;
			SquareHeight = 75;
			BoardWidth = HorizontalSquares * SquareWidth;
			BoardHeight = VerticalSquares * SquareHeight;

			/// create the board
			Clear();

			/// add the Squares
					
			GetHashTable.Add( "AA", new ConnectFourSquare( SquareWidth, SquareHeight, 0, 0, "AA" ) );
			GetHashTable.Add( "AB", new ConnectFourSquare( SquareWidth, SquareHeight, 0, SquareHeight, "AB" ) );
			GetHashTable.Add( "AC", new ConnectFourSquare( SquareWidth, SquareHeight, 0, SquareHeight * 2, "AC" ) );
			GetHashTable.Add( "AD", new ConnectFourSquare( SquareWidth, SquareHeight, 0, SquareHeight * 3, "AD" ) );
			GetHashTable.Add( "AE", new ConnectFourSquare( SquareWidth, SquareHeight, 0, SquareHeight * 4, "AE" ) );
			GetHashTable.Add( "AF", new ConnectFourSquare( SquareWidth, SquareHeight, 0, SquareHeight * 5, "AF" ) );
			GetHashTable.Add( "BA", new ConnectFourSquare( SquareWidth, SquareHeight, SquareWidth, 0, "BA" ) );
			GetHashTable.Add( "BB", new ConnectFourSquare( SquareWidth, SquareHeight, SquareWidth, SquareHeight, "BB" ) );
			GetHashTable.Add( "BC", new ConnectFourSquare( SquareWidth, SquareHeight, SquareWidth, SquareHeight * 2, "BC" ) );
			GetHashTable.Add( "BD", new ConnectFourSquare( SquareWidth, SquareHeight, SquareWidth, SquareHeight * 3, "BD" ) );
			GetHashTable.Add( "BE", new ConnectFourSquare( SquareWidth, SquareHeight, SquareWidth, SquareHeight * 4, "BE" ) );
			GetHashTable.Add( "BF", new ConnectFourSquare( SquareWidth, SquareHeight, SquareWidth, SquareHeight * 5, "BF" ) );
			GetHashTable.Add( "CA", new ConnectFourSquare( SquareWidth, SquareHeight, SquareWidth * 2, 0, "CA" ) );
			GetHashTable.Add( "CB", new ConnectFourSquare( SquareWidth, SquareHeight, SquareWidth * 2, SquareHeight, "CB" ) );
			GetHashTable.Add( "CC", new ConnectFourSquare( SquareWidth, SquareHeight, SquareWidth * 2, SquareHeight * 2, "CC" ) );
			GetHashTable.Add( "CD", new ConnectFourSquare( SquareWidth, SquareHeight, SquareWidth * 2, SquareHeight * 3, "CD" ) );
			GetHashTable.Add( "CE", new ConnectFourSquare( SquareWidth, SquareHeight, SquareWidth * 2, SquareHeight * 4, "CE" ) );
			GetHashTable.Add( "CF", new ConnectFourSquare( SquareWidth, SquareHeight, SquareWidth * 2, SquareHeight * 5, "CF" ) );
			GetHashTable.Add( "DA", new ConnectFourSquare( SquareWidth, SquareHeight, SquareWidth * 3, 0, "DA" ) );
			GetHashTable.Add( "DB", new ConnectFourSquare( SquareWidth, SquareHeight, SquareWidth * 3, SquareHeight, "DB" ) );
			GetHashTable.Add( "DC", new ConnectFourSquare( SquareWidth, SquareHeight, SquareWidth * 3, SquareHeight * 2, "DC" ) );
			GetHashTable.Add( "DD", new ConnectFourSquare( SquareWidth, SquareHeight, SquareWidth * 3, SquareHeight * 3, "DD" ) );
			GetHashTable.Add( "DE", new ConnectFourSquare( SquareWidth, SquareHeight, SquareWidth * 3, SquareHeight * 4, "DE" ) );
			GetHashTable.Add( "DF", new ConnectFourSquare( SquareWidth, SquareHeight, SquareWidth * 3, SquareHeight * 5, "DF" ) );
			GetHashTable.Add( "EA", new ConnectFourSquare( SquareWidth, SquareHeight, SquareWidth * 4, 0, "EA" ) );
			GetHashTable.Add( "EB", new ConnectFourSquare( SquareWidth, SquareHeight, SquareWidth * 4, SquareHeight, "EB" ) );
			GetHashTable.Add( "EC", new ConnectFourSquare( SquareWidth, SquareHeight, SquareWidth * 4, SquareHeight * 2, "EC" ) );
			GetHashTable.Add( "ED", new ConnectFourSquare( SquareWidth, SquareHeight, SquareWidth * 4, SquareHeight * 3, "ED" ) );
			GetHashTable.Add( "EE", new ConnectFourSquare( SquareWidth, SquareHeight, SquareWidth * 4, SquareHeight * 4, "EE" ) );
			GetHashTable.Add( "EF", new ConnectFourSquare( SquareWidth, SquareHeight, SquareWidth * 4, SquareHeight * 5, "EF" ) );
			GetHashTable.Add( "FA", new ConnectFourSquare( SquareWidth, SquareHeight, SquareWidth * 5, 0, "FA" ) );
			GetHashTable.Add( "FB", new ConnectFourSquare( SquareWidth, SquareHeight, SquareWidth * 5, SquareHeight, "FB" ) );
			GetHashTable.Add( "FC", new ConnectFourSquare( SquareWidth, SquareHeight, SquareWidth * 5, SquareHeight * 2, "FC" ) );
			GetHashTable.Add( "FD", new ConnectFourSquare( SquareWidth, SquareHeight, SquareWidth * 5, SquareHeight * 3, "FD" ) );
			GetHashTable.Add( "FE", new ConnectFourSquare( SquareWidth, SquareHeight, SquareWidth * 5, SquareHeight * 4, "FE" ) );
			GetHashTable.Add( "FF", new ConnectFourSquare( SquareWidth, SquareHeight, SquareWidth * 5, SquareHeight * 5, "FF" ) );
			GetHashTable.Add( "GA", new ConnectFourSquare( SquareWidth, SquareHeight, SquareWidth * 6, 0, "GA" ) );
			GetHashTable.Add( "GB", new ConnectFourSquare( SquareWidth, SquareHeight, SquareWidth * 6, SquareHeight, "GB" ) );
			GetHashTable.Add( "GC", new ConnectFourSquare( SquareWidth, SquareHeight, SquareWidth * 6, SquareHeight * 2, "GC" ) );
			GetHashTable.Add( "GD", new ConnectFourSquare( SquareWidth, SquareHeight, SquareWidth * 6, SquareHeight * 3, "GD" ) );
			GetHashTable.Add( "GE", new ConnectFourSquare( SquareWidth, SquareHeight, SquareWidth * 6, SquareHeight * 4, "GE" ) );
			GetHashTable.Add( "GF", new ConnectFourSquare( SquareWidth, SquareHeight, SquareWidth * 6, SquareHeight * 5, "GF" ) );

			( ( BasicSquare )GetHashTable[ "AA" ] ).DrawLegend = true;
			( ( BasicSquare )GetHashTable[ "AF" ] ).IsBottomSquare = true;
			( ( BasicSquare )GetHashTable[ "BF" ] ).IsBottomSquare = true;
			( ( BasicSquare )GetHashTable[ "CF" ] ).IsBottomSquare = true;
			( ( BasicSquare )GetHashTable[ "DF" ] ).IsBottomSquare = true;
			( ( BasicSquare )GetHashTable[ "EF" ] ).IsBottomSquare = true;
			( ( BasicSquare )GetHashTable[ "FF" ] ).IsBottomSquare = true;
			( ( BasicSquare )GetHashTable[ "GA" ] ).IsRightEdgeSquare = true;
			( ( BasicSquare )GetHashTable[ "GB" ] ).IsRightEdgeSquare = true;
			( ( BasicSquare )GetHashTable[ "GC" ] ).IsRightEdgeSquare = true;
			( ( BasicSquare )GetHashTable[ "GD" ] ).IsRightEdgeSquare = true;
			( ( BasicSquare )GetHashTable[ "GE" ] ).IsRightEdgeSquare = true;
			( ( BasicSquare )GetHashTable[ "GF" ] ).IsRightEdgeSquare = true;
			( ( BasicSquare )GetHashTable[ "GF" ] ).IsBottomSquare = true;

			( ( BasicSquare )GetHashTable[ "AA" ] ).BackGroundColor = Color.Beige;

			connectFourGame = new ConnectFourGame();
			patternCollection = new ConnectFourPatternCollection();
			historicalPatternCollection = new ConnectFourPatternCollection();
			arrayValidSquares = new ArrayList();

			SetDisplayMode( "CONNECTFOUR" );

			LoadHistoricalPatterns();

		}

		#region Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			// 
			// ConnectFourBoard
			// 
			this.Name = "ConnectFourBoard";
			this.Size = new System.Drawing.Size(472, 456);
			this.Load += new System.EventHandler(this.OnLoad);
			this.Paint += new System.Windows.Forms.PaintEventHandler(this.OnPaint);
			this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnMouseDown);

		}
		#endregion

		private void OnPaint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
			Graphics grfx = e.Graphics;

			if( bIsInitialized == false )
			{
				bIsInitialized = true;
				InitializeBoard();
			}

			/// for debugging paint messages when you do something that requires a 
			/// debug of paint code set breakpoint in this if statement and set testpaint to true
			if( TestPaint == true )
			{
				TestPaint = false;
			}

			if( GetConnectFourGame == null )
				return;

			for( int i=0; i<GetConnectFourGame.ArraySquares.Count; i++ )
			{
				ConnectFourSquare square = ( ConnectFourSquare )GetHashTable[ GetConnectFourGame.GetIdentifierAt( i ) ];
				ConnectFourSquareInfo squareInfo = ( ConnectFourSquareInfo )GetConnectFourGame.ArraySquares[ i ];

				if( square.PieceColor != squareInfo.SquareColor )
				{
					square.IsOccupied = true;
					square.OccupyingName = squareInfo.SquareColor;
					square.IsValid = false;
				}
			}

			foreach( DictionaryEntry dicEnt in GetHashTable )
			{
				BasicSquare square = ( BasicSquare )dicEnt.Value;

				square.DrawSquare( grfx );
			}
		}

		private void OnLoad(object sender, System.EventArgs e)
		{
///			InitializeBoard();

		}

		
		public ConnectFourSquareInfo GetConnectFourSquareInfo( string squareIdentifier )
		{
			if( connectFourGame != null )
			{
				return connectFourGame.GetSquareInfo( squareIdentifier );
			}

			return null;
		}

		/// <summary>
		/// set the player colour
		/// </summary>
		/// <param name="bRed">true == player is red false == blue</param>
		public void SetPlayerColour( bool red )
		{
			connectFourGame.PlayerIsRed = red;
		}

		public void SetStarted( bool started )
		{
			connectFourGame.IsStarted = started;

			Random rand = new Random( DateTime.Now.Second );

			int nTest = rand.Next( 2 );
			if( nTest >= 1 )
				ComputersMove();
		}

		public void SetPaused( bool paused )
		{
			connectFourGame.IsPaused = paused;
		}

		private void OnMouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if( connectFourGame.IsStarted == false || connectFourGame.IsPaused == true )
				return;

			if( connectFourGame.IsComputersMove == true )
				return;

			ConnectFourSquare square = ( ConnectFourSquare )GetSquareAt( e.X, e.Y );
			if( square != null )
			{
				Cursor.Current = Cursors.WaitCursor;
				ConnectFourSquareInfo squareInfo = GetConnectFourSquareInfo( square.Identifier );
				

				/// Make sure that gravity has an effect in this game
				StringBuilder strTest = new StringBuilder( GetIdentifierBelow( square.Identifier ) );
				ConnectFourSquareInfo squareTest;

				squareTest = connectFourGame.GetSquareInfo( strTest.ToString() );

				while( squareTest != null )
				{
					if( squareTest.IsOccupied == false )
					{
						square = ( ConnectFourSquare )GetSquare( squareTest.SquareIdentifier );
						squareInfo = squareTest;

						strTest.Remove( 0, strTest.Length );
						strTest.Append( squareInfo.SquareIdentifier );
						squareTest = connectFourGame.GetSquareInfo( GetIdentifierBelow( strTest.ToString() ) );
					}
					else
						squareTest = null;

				}


				if( squareInfo.IsOccupied == false )
				{
					if( connectFourGame.PlayerIsRed == true )
					{
						connectFourGame.SetSquare( square.Identifier, "RED" );
						connectFourGame.OutputText = "Square " + square.Identifier + " set to Red for the Player"; 
					}
					else
					{
						connectFourGame.SetSquare( square.Identifier, "BLUE" );
						connectFourGame.OutputText = "Square " + square.Identifier + " set to Blue for the Player";
					}

					InvalidateSquare( square.Identifier );
					connectFourGame.IsComputersMove = true;
					if( CheckForWin() == false )
					{
						ComputersMove();
						if( CheckForWin() == false )
							connectFourGame.IsComputersMove = false;
						else
							OnWin( squareInfo );
					}
					else
						OnWin( squareInfo );
				}

				Cursor.Current = Cursors.Default;
			}
		}

		public void Reset()
		{
			GetConnectFourGame.Reset();

			foreach( DictionaryEntry dicEnt in GetHashTable )
			{
				ConnectFourSquare square = ( ConnectFourSquare )dicEnt.Value;
				
				square.IsOccupied = false;
				square.OccupyingName = "EMTPY";
				square.IsValid = false;
				square.IsWinningSquare = false;
			}

			Invalidate();

			patternCollection.Clear();

			connectFourGame.OutputText = "Game Reset";
		}

		public bool CheckForWin()
		{
			/// check to see if we have four of the same colour anywhere on the board
			
			/// start at bottom left hand corner and move right checking 
			/// right, up, left diagonal, right diagonal
			

			bool bWinning = false;
			bool bRedWon = false;

			/// check for red
			foreach( DictionaryEntry dicEnt in GetHashTable )
			{
				ConnectFourSquare square = ( ConnectFourSquare )dicEnt.Value;
				
				if( CheckSquaresToRight( square.Identifier, true ) == true )
				{
					bWinning = true;
					bRedWon = true;

					square.IsWinningSquare = true;
					InvalidateSquare( square.Identifier );
					square.IsValid = false;
					square = ( ConnectFourSquare )GetSquareToRight( square.Identifier );
					square.IsWinningSquare = true;
					InvalidateSquare( square.Identifier );
					square.IsValid = false;
					square = ( ConnectFourSquare )GetSquareToRight( square.Identifier );
					square.IsWinningSquare = true;
					InvalidateSquare( square.Identifier );
					square.IsValid = false;
					square = ( ConnectFourSquare )GetSquareToRight( square.Identifier );
					square.IsWinningSquare = true;
					InvalidateSquare( square.Identifier );
					square.IsValid = false;

					break;
				}

				if( CheckSquaresAbove( square.Identifier, true ) == true )
				{
					bWinning = true;
					bRedWon = true;

					square.IsWinningSquare = true;
					InvalidateSquare( square.Identifier );
					square.IsValid = false;
					square = ( ConnectFourSquare )GetSquareAbove( square.Identifier );
					square.IsWinningSquare = true;
					InvalidateSquare( square.Identifier );
					square.IsValid = false;
					square = ( ConnectFourSquare )GetSquareAbove( square.Identifier );
					square.IsWinningSquare = true;
					InvalidateSquare( square.Identifier );
					square.IsValid = false;
					square = ( ConnectFourSquare )GetSquareAbove( square.Identifier );
					square.IsWinningSquare = true;
					InvalidateSquare( square.Identifier );
					square.IsValid = false;

					break;
				}


				if( CheckSquaresAboveRight( square.Identifier, true ) == true )
				{
					bWinning = true;
					bRedWon = true;

					square.IsWinningSquare = true;
					InvalidateSquare( square.Identifier );
					square.IsValid = false;
					square = ( ConnectFourSquare )GetSquareAboveRight( square.Identifier );
					square.IsWinningSquare = true;
					InvalidateSquare( square.Identifier );
					square.IsValid = false;
					square = ( ConnectFourSquare )GetSquareAboveRight( square.Identifier );
					square.IsWinningSquare = true;
					InvalidateSquare( square.Identifier );
					square.IsValid = false;
					square = ( ConnectFourSquare )GetSquareAboveRight( square.Identifier );
					square.IsWinningSquare = true;
					InvalidateSquare( square.Identifier );
					square.IsValid = false;

					break;
				}

				if( CheckSquaresAboveLeft( square.Identifier, true ) == true )
				{
					bWinning = true;
					bRedWon = true;

					square.IsWinningSquare = true;
					InvalidateSquare( square.Identifier );
					square.IsValid = false;
					square = ( ConnectFourSquare )GetSquareAboveLeft( square.Identifier );
					square.IsWinningSquare = true;
					InvalidateSquare( square.Identifier );
					square.IsValid = false;
					square = ( ConnectFourSquare )GetSquareAboveLeft( square.Identifier );
					square.IsWinningSquare = true;
					InvalidateSquare( square.Identifier );
					square.IsValid = false;
					square = ( ConnectFourSquare )GetSquareAboveLeft( square.Identifier );
					square.IsWinningSquare = true;
					InvalidateSquare( square.Identifier );
					square.IsValid = false;

					break;
				}
			}

			if( bWinning == false )
			{
				foreach( DictionaryEntry dicEnt in GetHashTable )
				{
					ConnectFourSquare square = ( ConnectFourSquare )dicEnt.Value;

					if( CheckSquaresToRight( square.Identifier, false ) == true )
					{
						bWinning = true;

						square.IsWinningSquare = true;
						InvalidateSquare( square.Identifier );
						square.IsValid = false;
						square = ( ConnectFourSquare )GetSquareToRight( square.Identifier );
						square.IsWinningSquare = true;
						InvalidateSquare( square.Identifier );
						square.IsValid = false;
						square = ( ConnectFourSquare )GetSquareToRight( square.Identifier );
						square.IsWinningSquare = true;
						InvalidateSquare( square.Identifier );
						square.IsValid = false;
						square = ( ConnectFourSquare )GetSquareToRight( square.Identifier );
						square.IsWinningSquare = true;
						InvalidateSquare( square.Identifier );
						square.IsValid = false;

						break;
					}

					if( CheckSquaresAbove( square.Identifier, false ) == true )
					{
						bWinning = true;

						square.IsWinningSquare = true;
						InvalidateSquare( square.Identifier );
						square.IsValid = false;
						square = ( ConnectFourSquare )GetSquareAbove( square.Identifier );
						square.IsWinningSquare = true;
						InvalidateSquare( square.Identifier );
						square.IsValid = false;
						square = ( ConnectFourSquare )GetSquareAbove( square.Identifier );
						square.IsWinningSquare = true;
						InvalidateSquare( square.Identifier );
						square.IsValid = false;
						square = ( ConnectFourSquare )GetSquareAbove( square.Identifier );
						square.IsWinningSquare = true;
						InvalidateSquare( square.Identifier );
						square.IsValid = false;

						break;
					}

					if( CheckSquaresAboveRight( square.Identifier, false ) == true )
					{
						bWinning = true;

						square.IsWinningSquare = true;
						InvalidateSquare( square.Identifier );
						square.IsValid = false;
						square = ( ConnectFourSquare )GetSquareAboveRight( square.Identifier );
						square.IsWinningSquare = true;
						InvalidateSquare( square.Identifier );
						square.IsValid = false;
						square = ( ConnectFourSquare )GetSquareAboveRight( square.Identifier );
						square.IsWinningSquare = true;
						InvalidateSquare( square.Identifier );
						square.IsValid = false;
						square = ( ConnectFourSquare )GetSquareAboveRight( square.Identifier );
						square.IsWinningSquare = true;
						InvalidateSquare( square.Identifier );
						square.IsValid = false;

						break;
					}

					if( CheckSquaresAboveLeft( square.Identifier, false ) == true )
					{
						bWinning = true;

						square.IsWinningSquare = true;
						InvalidateSquare( square.Identifier );
						square.IsValid = false;
						square = ( ConnectFourSquare )GetSquareAboveLeft( square.Identifier );
						square.IsWinningSquare = true;
						InvalidateSquare( square.Identifier );
						square.IsValid = false;
						square = ( ConnectFourSquare )GetSquareAboveLeft( square.Identifier );
						square.IsWinningSquare = true;
						InvalidateSquare( square.Identifier );
						square.IsValid = false;
						square = ( ConnectFourSquare )GetSquareAboveLeft( square.Identifier );
						square.IsWinningSquare = true;
						InvalidateSquare( square.Identifier );
						square.IsValid = false;

						break;
					}
				}
			}

			if( bWinning == true )
			{
				SetPaused( true );

				if( bRedWon == true )
				{
					connectFourGame.OutputText = "Red has Won the Game";
					if( connectFourGame.PlayerIsRed == true )
						connectFourGame.HasPlayerWon = true;
					else
						connectFourGame.HasPlayerWon = false;
				}
				else
				{
					connectFourGame.OutputText = "Blue has Won the Game";
					if( connectFourGame.PlayerIsRed == false )
						connectFourGame.HasPlayerWon = true;
					else
						connectFourGame.HasPlayerWon = false;
				}

				return true;
			}

			return false;

		}

		/// <summary>
		/// check the squares to the right to see if there is a win
		/// </summary>
		/// <param name="identifier">square identifier ie "AF"</param>
		/// <param name="bRed">are we looking for red or blue</param>
		/// <returns>true or false</returns>
		public bool CheckSquaresToRight( string identifier, bool red )
		{
			ConnectFourSquareInfo startSquare = connectFourGame.GetSquareInfo( identifier );
			
			if( startSquare.IsOccupied == false )
				return false;

			if( red == true )
			{
				if( startSquare.SquareColor != "RED" )
					return false;
			}
			else
			{
				if( startSquare.SquareColor != "BLUE" )
					return false;
			}

			ConnectFourSquareInfo squareOne = connectFourGame.GetSquareInfo( GetIdentifierToRight( startSquare.SquareIdentifier ) );
			if( squareOne == null )
				return false;

			if( squareOne.IsOccupied == false )
				return false;

			if( red == true )
			{
				if( squareOne.SquareColor != "RED" )
					return false;
			}
			else
			{
				if( squareOne.SquareColor != "BLUE" )
					return false;
			}

			ConnectFourSquareInfo squareTwo = connectFourGame.GetSquareInfo( GetIdentifierToRight( squareOne.SquareIdentifier ) );
			if( squareTwo == null )
				return false;

			if( squareTwo.IsOccupied == false )
				return false;

			if( red == true )
			{
				if( squareTwo.SquareColor != "RED" )
					return false;
			}
			else
			{
				if( squareTwo.SquareColor != "BLUE" )
					return false;
			}

			ConnectFourSquareInfo squareThree = connectFourGame.GetSquareInfo( GetIdentifierToRight( squareTwo.SquareIdentifier ) );
			if( squareThree == null )
				return false;

			if( squareThree.IsOccupied == false )
				return false;
		
			if( red == true )
			{
				if( squareThree.SquareColor != "RED" )
					return false;
			}
			else
			{
				if( squareThree.SquareColor != "BLUE" )
					return false;
			}

			return true;
		}

		public bool CheckSquaresAbove( string identifier, bool red )
		{
			ConnectFourSquareInfo startSquare = connectFourGame.GetSquareInfo( identifier );

			if( startSquare.IsOccupied == false )
				return false;

			if( red == true )
			{
				if( startSquare.SquareColor != "RED" )
					return false;
			}
			else
			{
				if( startSquare.SquareColor != "BLUE" )
					return false;
			}

			ConnectFourSquareInfo squareOne = connectFourGame.GetSquareInfo( GetIdentifierAbove( startSquare.SquareIdentifier ) );
			if( squareOne == null )
				return false;

			if( squareOne.IsOccupied == false )
				return false;

			if( red == true )
			{
				if( squareOne.SquareColor != "RED" )
					return false;
			}
			else
			{
				if( squareOne.SquareColor != "BLUE" )
					return false;
			}

			ConnectFourSquareInfo squareTwo = connectFourGame.GetSquareInfo( GetIdentifierAbove( squareOne.SquareIdentifier ) );
			if( squareTwo == null )
				return false;

			if( squareTwo.IsOccupied == false )
				return false;

			if( red == true )
			{
				if( squareTwo.SquareColor != "RED" )
					return false;
			}
			else
			{
				if( squareTwo.SquareColor != "BLUE" )
					return false;
			}

			ConnectFourSquareInfo squareThree = connectFourGame.GetSquareInfo( GetIdentifierAbove( squareTwo.SquareIdentifier ) );
			if( squareThree == null )
				return false;

			if( squareTwo.IsOccupied == false )
				return false;

			if( red == true )
			{
				if( squareThree.SquareColor != "RED" )
					return false;
			}
			else
			{
				if( squareThree.SquareColor != "BLUE" )
					return false;
			}

			return true;
		}


		public bool CheckSquaresAboveRight( string identifier, bool red )
		{
			ConnectFourSquareInfo startSquare = connectFourGame.GetSquareInfo( identifier );

			if( startSquare.IsOccupied == false )
				return false;

			if( red == true )
			{
				if( startSquare.SquareColor != "RED" )
					return false;
			}
			else
			{
				if( startSquare.SquareColor != "BLUE" )
					return false;
			}

			ConnectFourSquareInfo squareOne = connectFourGame.GetSquareInfo( GetIdentifierAboveRight( startSquare.SquareIdentifier ) );
			if( squareOne == null )
				return false;

			if( squareOne.IsOccupied == false )
				return false;

			if( red == true )
			{
				if( squareOne.SquareColor != "RED" )
					return false;
			}
			else
			{
				if( squareOne.SquareColor != "BLUE" )
					return false;
			}

			ConnectFourSquareInfo squareTwo = connectFourGame.GetSquareInfo( GetIdentifierAboveRight( squareOne.SquareIdentifier ) );
			if( squareTwo == null )
				return false;

			if( squareTwo.IsOccupied == false )
				return false;

			if( red == true )
			{
				if( squareTwo.SquareColor != "RED" )
					return false;
			}
			else
			{
				if( squareTwo.SquareColor != "BLUE" )
					return false;
			}

			ConnectFourSquareInfo squareThree = connectFourGame.GetSquareInfo( GetIdentifierAboveRight( squareTwo.SquareIdentifier ) );
			if( squareThree == null )
				return false;

			if( squareThree.IsOccupied == false )
				return false;

			if( red == true )
			{
				if( squareThree.SquareColor != "RED" )
					return false;
			}
			else
			{
				if( squareThree.SquareColor != "BLUE" )
					return false;
			}

			return true;
		}

		public bool CheckSquaresAboveLeft( string identifier, bool red )
		{
			ConnectFourSquareInfo startSquare = connectFourGame.GetSquareInfo( identifier );

			if( startSquare.IsOccupied == false )
				return false;

			if( red == true )
			{
				if( startSquare.SquareColor != "RED" )
					return false;
			}
			else
			{
				if( startSquare.SquareColor != "BLUE" )
					return false;
			}

			ConnectFourSquareInfo squareOne = connectFourGame.GetSquareInfo( GetIdentifierAboveLeft( startSquare.SquareIdentifier ) );
			if( squareOne == null )
				return false;

			if( squareOne.IsOccupied == false )
				return false;

			if( red == true )
			{
				if( squareOne.SquareColor != "RED" )
					return false;
			}
			else
			{
				if( squareOne.SquareColor != "BLUE" )
					return false;
			}

			ConnectFourSquareInfo squareTwo = connectFourGame.GetSquareInfo( GetIdentifierAboveLeft( squareOne.SquareIdentifier ) );
			if( squareTwo == null )
				return false;

			if( squareTwo.IsOccupied == false )
				return false;

			if( red == true )
			{
				if( squareTwo.SquareColor != "RED" )
					return false;
			}
			else
			{
				if( squareTwo.SquareColor != "BLUE" )
					return false;
			}

			ConnectFourSquareInfo squareThree = connectFourGame.GetSquareInfo( GetIdentifierAboveLeft( squareTwo.SquareIdentifier ) );
			if( squareThree == null )
				return false;

			if( squareThree.IsOccupied == false )
				return false;

			if( red == true )
			{
				if( squareThree.SquareColor != "RED" )
					return false;
			}
			else
			{
				if( squareThree.SquareColor != "BLUE" )
					return false;
			}

			return true;

		}

		/// <summary>
		/// If the computer doesn't move the suggested square can the player win the game by moving there
		/// next turn
		/// </summary>
		/// <param name="squareIdentifier"></param>
		/// <returns></returns>
		private bool IsWinningMoveForPlayer( string squareIdentifier )
		{
			string strTestSquare = squareIdentifier;

			ConnectFourSquareInfo squareOne = connectFourGame.GetSquareInfo( GetIdentifierToRight( strTestSquare ) );
			ConnectFourSquareInfo squareTwo = null;
			ConnectFourSquareInfo squareThree = null;
			if( squareOne != null && squareOne.IsOccupied == true 
				&& squareOne.IsRed == connectFourGame.PlayerIsRed )
			{

				squareTwo = connectFourGame.GetSquareInfo( GetIdentifierToRight( squareOne.SquareIdentifier ) );
				if( squareTwo != null && squareTwo.IsOccupied == true 
					&& squareTwo.IsRed == connectFourGame.PlayerIsRed )
				{

					squareThree = connectFourGame.GetSquareInfo( GetIdentifierToRight( squareTwo.SquareIdentifier ) );
					if( squareThree != null && squareThree.IsOccupied == true
						&& squareThree.IsRed == connectFourGame.PlayerIsRed )
						return true;
				}
			}

			squareOne = connectFourGame.GetSquareInfo( GetIdentifierToLeft( strTestSquare ) );
			if( squareOne != null && squareOne.IsOccupied == true 
				&& squareOne.IsRed == connectFourGame.PlayerIsRed )
			{
				squareTwo = connectFourGame.GetSquareInfo( GetIdentifierToLeft( squareOne.SquareIdentifier ) );
				if( squareTwo != null && squareTwo.IsOccupied == true 
					&& squareTwo.IsRed == connectFourGame.PlayerIsRed )
				{
					squareThree = connectFourGame.GetSquareInfo( GetIdentifierToLeft( squareTwo.SquareIdentifier ) );
					if( squareThree != null && squareThree.IsOccupied == true
						&& squareThree.IsRed == connectFourGame.PlayerIsRed )
						return true;
				}
			}

			squareOne = connectFourGame.GetSquareInfo( GetIdentifierToRight( strTestSquare ) );
			squareTwo = connectFourGame.GetSquareInfo( GetIdentifierToLeft( strTestSquare ) );

			if( squareOne != null && squareOne.IsOccupied == true 
				&& squareOne.IsRed == connectFourGame.PlayerIsRed 
				&& squareTwo != null && squareTwo.IsOccupied == true 
				&& squareOne.IsRed == connectFourGame.PlayerIsRed )
				return true;

			squareOne = connectFourGame.GetSquareInfo( GetIdentifierBelowRight( strTestSquare ) );
			if( squareOne != null && squareOne.IsOccupied == true 
				&& squareOne.IsRed == connectFourGame.PlayerIsRed )
			{
				squareTwo = connectFourGame.GetSquareInfo( GetIdentifierBelowRight( squareOne.SquareIdentifier ) );
				if( squareTwo != null && squareTwo.IsOccupied == true 
					&& squareTwo.IsRed == connectFourGame.PlayerIsRed )
				{
					squareThree = connectFourGame.GetSquareInfo( GetIdentifierBelowRight( squareTwo.SquareIdentifier ) );
					if( squareThree != null && squareThree.IsOccupied == true 
						&& squareThree.IsRed == connectFourGame.PlayerIsRed )
						return true;
				}
			}

			squareOne = connectFourGame.GetSquareInfo( GetIdentifierAboveLeft( strTestSquare ) );
			if( squareOne != null && squareOne.IsOccupied == true 
				&& squareOne.IsRed == connectFourGame.PlayerIsRed )
			{
				squareTwo = connectFourGame.GetSquareInfo( GetIdentifierAboveLeft( squareOne.SquareIdentifier ) );
				if( squareTwo != null && squareTwo.IsOccupied == true 
					&& squareTwo.IsRed == connectFourGame.PlayerIsRed )
				{
					squareThree = connectFourGame.GetSquareInfo( GetIdentifierAboveLeft( squareTwo.SquareIdentifier ) );
					if( squareThree != null && squareThree.IsOccupied == true 
						&& squareThree.IsRed == connectFourGame.PlayerIsRed )
						return true;
				}
			}

			squareOne = connectFourGame.GetSquareInfo( GetIdentifierBelowRight( strTestSquare ) );
			squareTwo = connectFourGame.GetSquareInfo( GetIdentifierAboveLeft( strTestSquare ) );

			if( squareOne != null && squareOne.IsOccupied == true 
				&& squareOne.IsRed == connectFourGame.PlayerIsRed 
				&& squareTwo != null && squareTwo.IsOccupied == true 
				&& squareTwo.IsRed == connectFourGame.PlayerIsRed )
				return true;


			squareOne = connectFourGame.GetSquareInfo( GetIdentifierAboveRight( strTestSquare ) );
			if( squareOne != null && squareOne.IsOccupied == true
				&& squareOne.IsRed == connectFourGame.PlayerIsRed )
			{
				squareTwo = connectFourGame.GetSquareInfo( GetIdentifierAboveRight( squareOne.SquareIdentifier ) );
				if( squareTwo != null && squareTwo.IsOccupied == true
					&& squareTwo.IsRed == connectFourGame.PlayerIsRed )
				{
					squareThree = connectFourGame.GetSquareInfo( GetIdentifierAboveRight( squareTwo.SquareIdentifier ) );
					if( squareThree != null && squareThree.IsOccupied == true 
						&& squareThree.IsRed == connectFourGame.PlayerIsRed )
						return true;
				}
			}

			squareOne = connectFourGame.GetSquareInfo( GetIdentifierBelowLeft( strTestSquare ) );
			if( squareOne != null && squareOne.IsOccupied == true 
				&& squareOne.IsRed == connectFourGame.PlayerIsRed )
			{
				squareTwo = connectFourGame.GetSquareInfo( GetIdentifierBelowLeft( squareOne.SquareIdentifier ) );
				if( squareTwo != null && squareTwo.IsOccupied == true 
					&& squareTwo.IsRed == connectFourGame.PlayerIsRed )
				{
					squareThree = connectFourGame.GetSquareInfo( GetIdentifierBelowLeft( squareTwo.SquareIdentifier ) );
					if( squareThree != null && squareThree.IsOccupied == true 
						&& squareThree.IsRed == connectFourGame.PlayerIsRed )
						return true;
				}
			}

			squareOne = connectFourGame.GetSquareInfo( GetIdentifierAboveRight( strTestSquare ) );
			squareTwo = connectFourGame.GetSquareInfo( GetIdentifierBelowLeft( strTestSquare ) );

			if( squareOne != null && squareOne.IsOccupied == true 
				&& squareOne.IsRed == connectFourGame.PlayerIsRed 
				&& squareTwo != null && squareTwo.IsOccupied == true
				&& squareTwo.IsRed == connectFourGame.PlayerIsRed )
				return true;


			return false;

		}

		/// <summary>
		/// If the computer moves to the suggested square will the player win with the next move.
		/// </summary>
		/// <param name="squareIdentifier"></param>
		/// <returns></returns>
		private bool IsGiveAwayMove( string squareIdentifier )
		{

			string strTestSquare = GetIdentifierAbove( squareIdentifier );

			ConnectFourSquareInfo squareOne = connectFourGame.GetSquareInfo( GetIdentifierToRight( strTestSquare ) );
			ConnectFourSquareInfo squareTwo = null;
			ConnectFourSquareInfo squareThree = null;
			if( squareOne != null && squareOne.IsOccupied == true 
				&& squareOne.IsRed == connectFourGame.PlayerIsRed )
			{

				squareTwo = connectFourGame.GetSquareInfo( GetIdentifierToRight( squareOne.SquareIdentifier ) );
				if( squareTwo != null && squareTwo.IsOccupied == true 
					&& squareTwo.IsRed == connectFourGame.PlayerIsRed )
				{

					squareThree = connectFourGame.GetSquareInfo( GetIdentifierToRight( squareTwo.SquareIdentifier ) );
					if( squareThree != null && squareThree.IsOccupied == true
						&& squareThree.IsRed == connectFourGame.PlayerIsRed )
						return true;
				}
			}

			squareOne = connectFourGame.GetSquareInfo( GetIdentifierToLeft( strTestSquare ) );
			if( squareOne != null && squareOne.IsOccupied == true 
				&& squareOne.IsRed == connectFourGame.PlayerIsRed )
			{
				squareTwo = connectFourGame.GetSquareInfo( GetIdentifierToLeft( squareOne.SquareIdentifier ) );
				if( squareTwo != null && squareTwo.IsOccupied == true 
					&& squareTwo.IsRed == connectFourGame.PlayerIsRed )
				{
					squareThree = connectFourGame.GetSquareInfo( GetIdentifierToLeft( squareTwo.SquareIdentifier ) );
					if( squareThree != null && squareThree.IsOccupied == true
						&& squareThree.IsRed == connectFourGame.PlayerIsRed )
						return true;
				}
			}

			squareOne = connectFourGame.GetSquareInfo( GetIdentifierToRight( strTestSquare ) );
			squareTwo = connectFourGame.GetSquareInfo( GetIdentifierToLeft( strTestSquare ) );

			if( squareOne != null && squareOne.IsOccupied == true 
				&& squareOne.IsRed == connectFourGame.PlayerIsRed 
				&& squareTwo != null && squareTwo.IsOccupied == true 
				&& squareOne.IsRed == connectFourGame.PlayerIsRed )
				return true;

			squareOne = connectFourGame.GetSquareInfo( GetIdentifierBelowRight( strTestSquare ) );
			if( squareOne != null && squareOne.IsOccupied == true 
				&& squareOne.IsRed == connectFourGame.PlayerIsRed )
			{
				squareTwo = connectFourGame.GetSquareInfo( GetIdentifierBelowRight( squareOne.SquareIdentifier ) );
				if( squareTwo != null && squareTwo.IsOccupied == true 
					&& squareTwo.IsRed == connectFourGame.PlayerIsRed )
				{
					squareThree = connectFourGame.GetSquareInfo( GetIdentifierBelowRight( squareTwo.SquareIdentifier ) );
					if( squareThree != null && squareThree.IsOccupied == true 
						&& squareThree.IsRed == connectFourGame.PlayerIsRed )
						return true;
				}
			}

			squareOne = connectFourGame.GetSquareInfo( GetIdentifierAboveLeft( strTestSquare ) );
			if( squareOne != null && squareOne.IsOccupied == true 
				&& squareOne.IsRed == connectFourGame.PlayerIsRed )
			{
				squareTwo = connectFourGame.GetSquareInfo( GetIdentifierAboveLeft( squareOne.SquareIdentifier ) );
				if( squareTwo != null && squareTwo.IsOccupied == true 
					&& squareTwo.IsRed == connectFourGame.PlayerIsRed )
				{
					squareThree = connectFourGame.GetSquareInfo( GetIdentifierAboveLeft( squareTwo.SquareIdentifier ) );
					if( squareThree != null && squareThree.IsOccupied == true 
						&& squareThree.IsRed == connectFourGame.PlayerIsRed )
						return true;
				}
			}

			squareOne = connectFourGame.GetSquareInfo( GetIdentifierBelowRight( strTestSquare ) );
			squareTwo = connectFourGame.GetSquareInfo( GetIdentifierAboveLeft( strTestSquare ) );

			if( squareOne != null && squareOne.IsOccupied == true 
				&& squareOne.IsRed == connectFourGame.PlayerIsRed 
				&& squareTwo != null && squareTwo.IsOccupied == true 
				&& squareTwo.IsRed == connectFourGame.PlayerIsRed )
				return true;


			squareOne = connectFourGame.GetSquareInfo( GetIdentifierAboveRight( strTestSquare ) );
			if( squareOne != null && squareOne.IsOccupied == true
				&& squareOne.IsRed == connectFourGame.PlayerIsRed )
			{
				squareTwo = connectFourGame.GetSquareInfo( GetIdentifierAboveRight( squareOne.SquareIdentifier ) );
				if( squareTwo != null && squareTwo.IsOccupied == true
					&& squareTwo.IsRed == connectFourGame.PlayerIsRed )
				{
					squareThree = connectFourGame.GetSquareInfo( GetIdentifierAboveRight( squareTwo.SquareIdentifier ) );
					if( squareThree != null && squareThree.IsOccupied == true 
						&& squareThree.IsRed == connectFourGame.PlayerIsRed )
						return true;
				}
			}

			squareOne = connectFourGame.GetSquareInfo( GetIdentifierBelowLeft( strTestSquare ) );
			if( squareOne != null && squareOne.IsOccupied == true 
				&& squareOne.IsRed == connectFourGame.PlayerIsRed )
			{
				squareTwo = connectFourGame.GetSquareInfo( GetIdentifierBelowLeft( squareOne.SquareIdentifier ) );
				if( squareTwo != null && squareTwo.IsOccupied == true 
					&& squareTwo.IsRed == connectFourGame.PlayerIsRed )
				{
					squareThree = connectFourGame.GetSquareInfo( GetIdentifierBelowLeft( squareTwo.SquareIdentifier ) );
					if( squareThree != null && squareThree.IsOccupied == true 
						&& squareThree.IsRed == connectFourGame.PlayerIsRed )
						return true;
				}
			}

			squareOne = connectFourGame.GetSquareInfo( GetIdentifierAboveRight( strTestSquare ) );
			squareTwo = connectFourGame.GetSquareInfo( GetIdentifierBelowLeft( strTestSquare ) );

			if( squareOne != null && squareOne.IsOccupied == true 
				&& squareOne.IsRed == connectFourGame.PlayerIsRed 
				&& squareTwo != null && squareTwo.IsOccupied == true
				&& squareTwo.IsRed == connectFourGame.PlayerIsRed )
				return true;


			return false;
		}

		/// <summary>
		/// save the patterns to a file Called whenever the game is won.
		/// </summary>
		private void SavePatterns()
		{
			try
			{
				if( File.Exists( strFileName ) == true )
				{
					File.Delete( strFileName );
				}
			}
			catch( ArgumentNullException argNullExp )
			{
				MessageBox.Show( this, "Error removing the file " + strFileName + " Due to " + argNullExp.Message + " If error persists delete the file and start again" );
				return;
			}
			catch( ArgumentException argExp )
			{
				MessageBox.Show( this, "Error removing the file " + strFileName + " Due to " + argExp.Message + " If error persists delete the file and start again" );
				return;
			}
			catch( UnauthorizedAccessException unAccessExp )
			{
				MessageBox.Show( this, "Error removing the file " + strFileName + " Due to " + unAccessExp.Message + " If error persists delete the file and start again" );
				return;
			}
			catch( PathTooLongException pathExp )
			{
				MessageBox.Show( this, "Error removing the file " + strFileName + " Due to " + pathExp.Message + " If error persists delete the file and start again" );
				return;
			}
			catch( DirectoryNotFoundException dirNFExp )
			{
				MessageBox.Show( this, "Error removing the file " + strFileName + " Due to " + dirNFExp.Message + " If error persists delete the file and start again" );
				return;
			}
			catch( IOException ioExp )
			{
				MessageBox.Show( this, "Error removing the file " + strFileName + " Due to " + ioExp.Message + " If error persists delete the file and start again" );
				return;
			}


			StreamWriter writer = File.CreateText( strFileName );
			XmlTextWriter xmlWriter = new XmlTextWriter( writer );

			historicalPatternCollection.Save( xmlWriter );

			xmlWriter.Close();
		}

		/// <summary>
		/// Get All valid or occupied squares
		/// Store all available patterns for this move
		/// Determine pattern types
		/// </summary>
		private void ComputersMove()
		{
			/// start at the top and work your way down trying to find the first square that contains a piece
			/// 

			StringBuilder strIdentifier = new StringBuilder( "AA" );
			ConnectFourSquareInfo squareInfo = null;

			arrayValidSquares.Clear();

			/// start at AA and move across
			for( int i=0; i<GetHashTable.Count; i++ )
			{
				squareInfo = connectFourGame.GetSquareInfo( strIdentifier.ToString() );
				if( squareInfo == null )
				{
					/// this means player always goes first ?????
					connectFourGame.OutputText = "Error square Info returned null for " + strIdentifier.ToString();
					return;
				}

				if( squareInfo.IsOccupied == true )
				{
					connectFourGame.OutputText = "Square to check is " + squareInfo.SquareIdentifier;
					
					arrayValidSquares.Add( strIdentifier.ToString() );
				}

				if( strIdentifier[ 0 ] == 'G' )
				{
					strIdentifier[ 1 ]++;
					strIdentifier[ 0 ] = 'A';
				}
				else
				{
					StringBuilder strTemp = new StringBuilder( strIdentifier.ToString() );
					strIdentifier.Remove( 0, strIdentifier.Length );
					strIdentifier.Append( GetIdentifierToRight( strTemp.ToString() ) );
				}
			}


			StringBuilder strCheckOutput = new StringBuilder( "Squares found are :- " );
			for( int i=0; i<arrayValidSquares.Count; i++ )
			{
				strCheckOutput.Append( ( string )arrayValidSquares[ i ] );
			}
			connectFourGame.OutputText = strCheckOutput.ToString();


			/// At this point there is a valid list of pieces that need checking.
			/// So build a pattern from each piece


			ConnectFourSquare square = null;
			squareInfo = null;

			///ConnectFourSquare patternSquare = null;
			ConnectFourSquareInfo patternSquareInfo = null;
			ConnectFourSquareInfo nextInfo = null;

			for( int i=0; i<arrayValidSquares.Count; i++ )
			{
				square = ( ConnectFourSquare )GetSquare( ( string )arrayValidSquares[ i ] );
				squareInfo = connectFourGame.GetSquareInfo( ( string )arrayValidSquares[ i ] );
		
				/// build the patterns from this square
				/// 

				ConnectFourPattern pattern = new ConnectFourPattern();
				ConnectFourPiece piece = new ConnectFourPiece( true, square.Identifier );
				piece.IsPieceRed = squareInfo.IsRed;
				if( connectFourGame.PlayerIsRed == squareInfo.IsRed )
					piece.IsEnemy = false;
				else
					piece.IsEnemy = true;
				piece.IsStartForPattern = true;
				piece.Position = PIECEPOSITION.START;

				pattern.AddGamePiece( piece ); 

				/// store all patterns of two squares of the same colour
				/// as long as there is another square in the same direction to move to
				/// 

				/// check top
				/// 
				patternSquareInfo = connectFourGame.GetSquareInfo( GetIdentifierAbove( square.Identifier ) );
				if( patternSquareInfo != null )
				{
					bool bTwoPieces = false;
					bool bThreePieces = false;

					if( patternSquareInfo.IsOccupied == true )
					{
						nextInfo = connectFourGame.GetSquareInfo( GetIdentifierAbove( patternSquareInfo.SquareIdentifier ) );
						if( nextInfo != null && nextInfo.IsOccupied == false )
						{
							/// allow this square
							/// 
							bTwoPieces = true;
						}
						else 
						{
							if( nextInfo != null )
							{
								bTwoPieces = true;
								bThreePieces = true;
							}
						}
					}

					if( bTwoPieces == true )
					{
						ConnectFourPiece abovePiece = new ConnectFourPiece( false, patternSquareInfo.SquareIdentifier );

						if( piece.IsPieceRed == patternSquareInfo.IsRed )
						{
							abovePiece.IsPieceRed = patternSquareInfo.IsRed;
							abovePiece.Position = PIECEPOSITION.ABOVE;
							abovePiece.Level = 1;
							
							if( connectFourGame.PlayerIsRed == patternSquareInfo.IsRed )
								abovePiece .IsEnemy = false;
							else
								abovePiece.IsEnemy = true;

							pattern.AddGamePiece( abovePiece );

							if( bThreePieces == false )
							{
								if( patternCollection.IsIn( pattern ) == false )
									patternCollection.AddPattern( pattern );
							}
						}
					}

					if( bThreePieces == true )
					{
						patternSquareInfo = nextInfo;
						nextInfo = connectFourGame.GetSquareInfo( GetIdentifierAbove( patternSquareInfo.SquareIdentifier ) );
						if( nextInfo != null && nextInfo.IsOccupied == false )
						{
							ConnectFourPiece abovePiece = new ConnectFourPiece( false, patternSquareInfo.SquareIdentifier );

							if( piece.IsPieceRed == patternSquareInfo.IsRed )
							{
								abovePiece.IsPieceRed = patternSquareInfo.IsRed;
								abovePiece.Position = PIECEPOSITION.ABOVE;
								abovePiece.Level = 2;

								if( connectFourGame.PlayerIsRed == patternSquareInfo.IsRed )
									abovePiece.IsEnemy = false;
								else
									abovePiece.IsEnemy = true;

								pattern.AddGamePiece( abovePiece );

								if( patternCollection.IsIn( pattern ) == false )
									patternCollection.AddPattern( pattern );
							}
						}
					}

					pattern.Clear();
					pattern.AddGamePiece( piece );
				}


				/// check top right
				/// 
				patternSquareInfo = connectFourGame.GetSquareInfo( GetIdentifierAboveRight( square.Identifier ) );
				if( patternSquareInfo != null )
				{
					bool bTwoPieces = false;
					bool bThreePieces = false;

					if( patternSquareInfo.IsOccupied == true )
					{
						nextInfo = connectFourGame.GetSquareInfo( GetIdentifierAboveRight( patternSquareInfo.SquareIdentifier ) );
						if( nextInfo != null && nextInfo.IsOccupied == false )
						{
							/// allow this square
							/// 
							bTwoPieces = true;
						}
						else 
						{
							if( nextInfo != null )
							{
								bTwoPieces = true;
								bThreePieces = true;
							}
						}
					}

					if( bTwoPieces == true )
					{
						ConnectFourPiece aboveRightPiece = new ConnectFourPiece( false, patternSquareInfo.SquareIdentifier );
						
						/// make sure we are collecting patterns of the correct colour pieces
						/// note values are true are false so checking for equality means they are the
						/// same colour
						/// 
						if( piece.IsPieceRed == patternSquareInfo.IsRed )
						{
							aboveRightPiece.IsPieceRed = patternSquareInfo.IsRed;
							aboveRightPiece.Position = PIECEPOSITION.ABOVERIGHT;
							aboveRightPiece.Level = 1;

							if( connectFourGame.PlayerIsRed == patternSquareInfo.IsRed )
								aboveRightPiece.IsEnemy = false;
							else
								aboveRightPiece.IsEnemy = true;

							pattern.AddGamePiece( aboveRightPiece );

							if( bThreePieces == false )
							{
								if( patternCollection.IsIn( pattern ) == false )
									patternCollection.AddPattern( pattern );
							}
						}
					}

					if( bThreePieces == true )
					{
						patternSquareInfo = nextInfo;
						nextInfo = connectFourGame.GetSquareInfo( GetIdentifierAboveRight( patternSquareInfo.SquareIdentifier ) );
						if( nextInfo != null && nextInfo.IsOccupied == false )
						{
							ConnectFourPiece aboveRightPiece = new ConnectFourPiece( false, patternSquareInfo.SquareIdentifier );

							if( piece.IsPieceRed == patternSquareInfo.IsRed )
							{
								aboveRightPiece.IsPieceRed = patternSquareInfo.IsRed;
								aboveRightPiece.Position = PIECEPOSITION.ABOVERIGHT;
								aboveRightPiece.Level = 2;

								if( connectFourGame.PlayerIsRed == patternSquareInfo.IsRed )
									aboveRightPiece.IsEnemy = false;
								else
									aboveRightPiece.IsEnemy = true;

								pattern.AddGamePiece( aboveRightPiece );

								if( patternCollection.IsIn( pattern ) == false )
									patternCollection.AddPattern( pattern );
							}
						}
					}

					pattern.Clear();
					pattern.AddGamePiece( piece ); 
				}

				/// check right
				/// 
				patternSquareInfo = connectFourGame.GetSquareInfo( GetIdentifierToRight( square.Identifier ) );
				if( patternSquareInfo != null )
				{
					bool bTwoPieces = false;
					bool bThreePieces = false;

					if( patternSquareInfo.IsOccupied == true )
					{
						nextInfo = connectFourGame.GetSquareInfo( GetIdentifierToRight( patternSquareInfo.SquareIdentifier ) );
						if( nextInfo != null && nextInfo.IsOccupied == false )
						{
							bTwoPieces = true;
						}
						else 
						{
							if( nextInfo != null )
							{
								bTwoPieces = true;
								bThreePieces = true;
							}
						}
					}

					if( bTwoPieces == true )
					{
						ConnectFourPiece rightPiece = new ConnectFourPiece( false, patternSquareInfo.SquareIdentifier );
						
						if( piece.IsPieceRed == patternSquareInfo.IsRed )
						{
							rightPiece.IsPieceRed = patternSquareInfo.IsRed;
							rightPiece.Position = PIECEPOSITION.RIGHT;
							rightPiece.Level = 1;

							if( connectFourGame.PlayerIsRed == patternSquareInfo.IsRed )
								rightPiece.IsEnemy = false;
							else
								rightPiece.IsEnemy = true;

							pattern.AddGamePiece( rightPiece );

							if( bThreePieces == false )
							{
								if( patternCollection.IsIn( pattern ) == false )
									patternCollection.AddPattern( pattern );
							}
						}
					}

					if( bThreePieces == true )
					{
						patternSquareInfo = nextInfo;
						nextInfo = connectFourGame.GetSquareInfo( GetIdentifierToRight( patternSquareInfo.SquareIdentifier ) );
						if( nextInfo != null && nextInfo.IsOccupied == false )
						{
							ConnectFourPiece rightPiece = new ConnectFourPiece( false, patternSquareInfo.SquareIdentifier );

							if( piece.IsPieceRed == patternSquareInfo.IsRed )
							{
								rightPiece.IsPieceRed = patternSquareInfo.IsRed;
								rightPiece.Position = PIECEPOSITION.RIGHT;
								rightPiece.Level = 2;

								if( connectFourGame.PlayerIsRed == patternSquareInfo.IsRed )
									rightPiece.IsEnemy = false;
								else
									rightPiece.IsEnemy = true;

								pattern.AddGamePiece( rightPiece );

								if( patternCollection.IsIn( pattern ) == false )
									patternCollection.AddPattern( pattern );
							}
						}
					}

					pattern.Clear();
					pattern.AddGamePiece( piece ); 
				}

				/// check below right
				/// 
				patternSquareInfo = connectFourGame.GetSquareInfo( GetIdentifierBelowRight( square.Identifier ) );
				if( patternSquareInfo != null )
				{
					bool bTwoPieces = false;
					bool bThreePieces = false;

					if( patternSquareInfo.IsOccupied == true )
					{
						nextInfo = connectFourGame.GetSquareInfo( GetIdentifierBelowRight( patternSquareInfo.SquareIdentifier ) );
						if( nextInfo != null && nextInfo.IsOccupied == false )
						{
							bTwoPieces = true;
						}
						else 
						{
							if( nextInfo != null )
							{
								bTwoPieces = true;
								bThreePieces = true;
							}
						}
					}

					if( bTwoPieces == true )
					{
						ConnectFourPiece belowRightPiece = new ConnectFourPiece( false, patternSquareInfo.SquareIdentifier );
						
						if( piece.IsPieceRed == patternSquareInfo.IsRed )
						{
							belowRightPiece.IsPieceRed = patternSquareInfo.IsRed;
							belowRightPiece.Position = PIECEPOSITION.BELOWRIGHT;
							belowRightPiece.Level = 1;

							if( connectFourGame.PlayerIsRed == patternSquareInfo.IsRed )
								belowRightPiece.IsEnemy = false;
							else
								belowRightPiece.IsEnemy = true;


							pattern.AddGamePiece( belowRightPiece );

							if( bThreePieces == false )
							{
								if( patternCollection.IsIn( pattern ) == false )
									patternCollection.AddPattern( pattern );
							}
						}
					}

					if( bThreePieces == true )
					{
						patternSquareInfo = nextInfo;
						nextInfo = connectFourGame.GetSquareInfo( GetIdentifierBelowRight( patternSquareInfo.SquareIdentifier ) );
						if( nextInfo != null && nextInfo.IsOccupied == false )
						{
							ConnectFourPiece belowRightPiece = new ConnectFourPiece( false, patternSquareInfo.SquareIdentifier );

							if( piece.IsPieceRed == patternSquareInfo.IsRed )
							{
								belowRightPiece.IsPieceRed = patternSquareInfo.IsRed;
								belowRightPiece.Position = PIECEPOSITION.BELOWRIGHT;
								belowRightPiece.Level = 2;

								if( connectFourGame.PlayerIsRed == patternSquareInfo.IsRed )
									belowRightPiece.IsEnemy = false;
								else
									belowRightPiece.IsEnemy = true;

								pattern.AddGamePiece( belowRightPiece );

								if( patternCollection.IsIn( pattern ) == false )
									patternCollection.AddPattern( pattern );
							}
						}
					}

					pattern.Clear();
					pattern.AddGamePiece( piece ); 
				}

				/// checking below is really kinda pointless
				/// 

				/// check below left
				/// 
				patternSquareInfo = connectFourGame.GetSquareInfo( GetIdentifierBelowLeft( square.Identifier ) );
				if( patternSquareInfo != null )
				{
					bool bTwoPieces = false;
					bool bThreePieces = false;

					if( patternSquareInfo.IsOccupied == true )
					{
						nextInfo = connectFourGame.GetSquareInfo( GetIdentifierBelowLeft( patternSquareInfo.SquareIdentifier ) );
						if( nextInfo != null && nextInfo.IsOccupied == false )
						{
							bTwoPieces = true;
						}
						else 
						{
							if( nextInfo != null )
							{
								bTwoPieces = true;
								bThreePieces = true;
							}
						}
					}

					if( bTwoPieces == true )
					{
						ConnectFourPiece belowLeftPiece = new ConnectFourPiece( false, patternSquareInfo.SquareIdentifier );
						
						if( piece.IsPieceRed == patternSquareInfo.IsRed )
						{
							belowLeftPiece.IsPieceRed = patternSquareInfo.IsRed;
							belowLeftPiece.Position = PIECEPOSITION.BELOWLEFT;
							belowLeftPiece.Level = 1;
						
							if( connectFourGame.PlayerIsRed == patternSquareInfo.IsRed )
								belowLeftPiece.IsEnemy = false;
							else
								belowLeftPiece.IsEnemy = true;

							pattern.AddGamePiece( belowLeftPiece );

							if( patternCollection.IsIn( pattern ) == false )
								patternCollection.AddPattern( pattern );
						}
					}

					if( bThreePieces == true )
					{
						patternSquareInfo = nextInfo;
						nextInfo = connectFourGame.GetSquareInfo( GetIdentifierBelowLeft( patternSquareInfo.SquareIdentifier ) );
						if( nextInfo != null && nextInfo.IsOccupied == false )
						{
							ConnectFourPiece belowLeftPiece = new ConnectFourPiece( false, patternSquareInfo.SquareIdentifier );

							if( piece.IsPieceRed == patternSquareInfo.IsRed )
							{
								belowLeftPiece.IsPieceRed = patternSquareInfo.IsRed;
								belowLeftPiece.Position = PIECEPOSITION.BELOWLEFT;
								belowLeftPiece.Level = 2;

								if( connectFourGame.PlayerIsRed == patternSquareInfo.IsRed )
									belowLeftPiece.IsEnemy = false;
								else
									belowLeftPiece.IsEnemy = true;

								pattern.AddGamePiece( belowLeftPiece );

								if( patternCollection.IsIn( pattern ) == false )
									patternCollection.AddPattern( pattern );
							}
						}
					}

					pattern.Clear();
					pattern.AddGamePiece( piece ); 
				}

				/// check left
				/// 
				patternSquareInfo = connectFourGame.GetSquareInfo( GetIdentifierToLeft( square.Identifier ) );
				if( patternSquareInfo != null )
				{
					bool bTwoPieces = false;
					bool bThreePieces = false;

					if( patternSquareInfo.IsOccupied == true )
					{
						nextInfo = connectFourGame.GetSquareInfo( GetIdentifierToLeft( patternSquareInfo.SquareIdentifier ) );
						if( nextInfo != null && nextInfo.IsOccupied == false )
						{
							bTwoPieces = true;
						}
						else 
						{
							if( nextInfo != null )
							{
								bTwoPieces = true;
								bThreePieces = true;
							}
						}
					}

					if( bTwoPieces == true )
					{
						ConnectFourPiece leftPiece = new ConnectFourPiece( false, patternSquareInfo.SquareIdentifier );
						
						if( piece.IsPieceRed == patternSquareInfo.IsRed )
						{
							leftPiece.IsPieceRed = patternSquareInfo.IsRed;
							leftPiece.Position = PIECEPOSITION.LEFT;
							leftPiece.Level = 1;

							if( connectFourGame.PlayerIsRed == patternSquareInfo.IsRed )
								leftPiece.IsEnemy = false;
							else
								leftPiece.IsEnemy = true;

							pattern.AddGamePiece( leftPiece );

							if( bThreePieces == false )
							{
								if( patternCollection.IsIn( pattern ) == false )
									patternCollection.AddPattern( pattern );
							}
						}
					}

					if( bThreePieces == true )
					{
						patternSquareInfo = nextInfo;
						nextInfo = connectFourGame.GetSquareInfo( GetIdentifierToLeft( patternSquareInfo.SquareIdentifier ) );
						if( nextInfo != null && nextInfo.IsOccupied == false )
						{
							ConnectFourPiece leftPiece = new ConnectFourPiece( false, patternSquareInfo.SquareIdentifier );

							if( piece.IsPieceRed == patternSquareInfo.IsRed )
							{
								leftPiece.IsPieceRed = patternSquareInfo.IsRed;
								leftPiece.Position = PIECEPOSITION.LEFT;
								leftPiece.Level = 2;

								if( connectFourGame.PlayerIsRed == patternSquareInfo.IsRed )
									leftPiece.IsEnemy = false;
								else
									leftPiece.IsEnemy = true;

								pattern.AddGamePiece( leftPiece );

								if( patternCollection.IsIn( pattern ) == false )
									patternCollection.AddPattern( pattern );
							}
						}
					}

					pattern.Clear();
					pattern.AddGamePiece( piece ); 
				}

				/// check above left
				/// 
				patternSquareInfo = connectFourGame.GetSquareInfo( GetIdentifierAboveLeft( square.Identifier ) );
				if( patternSquareInfo != null )
				{
					bool bTwoPieces = false;
					bool bThreePieces = false;

					if( patternSquareInfo.IsOccupied == true )
					{
						nextInfo = connectFourGame.GetSquareInfo( GetIdentifierAboveLeft( patternSquareInfo.SquareIdentifier ) );
						if( nextInfo != null && nextInfo.IsOccupied == false )
						{
							bTwoPieces = true;
						}
						else 
						{
							if( nextInfo != null )
							{
								bTwoPieces = true;
								bThreePieces = true;
							}
						}
					}

					if( bTwoPieces == true )
					{
						ConnectFourPiece aboveLeftPiece = new ConnectFourPiece( false, patternSquareInfo.SquareIdentifier );
						
						if( piece.IsPieceRed == patternSquareInfo.IsRed )
						{
							aboveLeftPiece.IsPieceRed = patternSquareInfo.IsRed;
							aboveLeftPiece.Position = PIECEPOSITION.ABOVELEFT;
							aboveLeftPiece.Level = 1;

							if( connectFourGame.PlayerIsRed == patternSquareInfo.IsRed )
								aboveLeftPiece.IsEnemy = false;
							else
								aboveLeftPiece.IsEnemy = true;

							pattern.AddGamePiece( aboveLeftPiece );

							if( bThreePieces == false )
							{
								if( patternCollection.IsIn( pattern ) == false )
									patternCollection.AddPattern( pattern );
							}
						}
					}

					if( bThreePieces == true )
					{
						patternSquareInfo = nextInfo;
						nextInfo = connectFourGame.GetSquareInfo( GetIdentifierAboveLeft( patternSquareInfo.SquareIdentifier ) );
						if( nextInfo != null && nextInfo.IsOccupied == false )
						{
							ConnectFourPiece aboveLeftPiece = new ConnectFourPiece( false, patternSquareInfo.SquareIdentifier );

							if( piece.IsPieceRed == patternSquareInfo.IsRed )
							{
								aboveLeftPiece.IsPieceRed = patternSquareInfo.IsRed;
								aboveLeftPiece.Position = PIECEPOSITION.ABOVELEFT;
								aboveLeftPiece.Level = 2;

								if( connectFourGame.PlayerIsRed == patternSquareInfo.IsRed )
									aboveLeftPiece.IsEnemy = false;
								else
									aboveLeftPiece.IsEnemy = true;

								pattern.AddGamePiece( aboveLeftPiece );

								if( patternCollection.IsIn( pattern ) == false )
									patternCollection.AddPattern( pattern );
							}
						}
					}

					pattern.Clear();
					pattern.AddGamePiece( piece );
				}
			}
			
			/// now that there is a list of patterns start adding game specific rules
			/// 
			ConnectFourPatternCollection aggressivePatterns = new ConnectFourPatternCollection();
			ConnectFourPatternCollection defensivePatterns = new ConnectFourPatternCollection();
			ConnectFourPatternCollection warningPatterns = new ConnectFourPatternCollection();
			ConnectFourPatternCollection tacticalPatterns = new ConnectFourPatternCollection();


			/// Get all patterns that have been seen before and the computer has lost the game
			///
 
			ConnectFourPattern holdingPattern = null;

			ConnectFourSquareInfo leftSquareInfo = null;
			ConnectFourSquareInfo leftSquareNextInfo = null;
			ConnectFourSquareInfo rightSquareInfo = null;
			ConnectFourSquareInfo rightSquareNextInfo = null;
			ConnectFourSquareInfo aboveSquareInfo = null;
			ConnectFourSquareInfo belowSquareInfo = null;
///			ConnectFourSquareInfo belowSquareNextInfo = null;
			ConnectFourSquareInfo belowLeftSquareInfo = null;
			ConnectFourSquareInfo belowLeftSquareNextInfo = null;
			ConnectFourSquareInfo aboveLeftSquareInfo = null;
			ConnectFourSquareInfo aboveLeftSquareNextInfo = null;
			ConnectFourSquareInfo belowRightSquareInfo = null;
			ConnectFourSquareInfo belowRightSquareNextInfo = null;
			ConnectFourSquareInfo aboveRightSquareInfo = null;
			ConnectFourSquareInfo aboveRightSquareNextInfo = null;
			ConnectFourSquareInfo nextSquareInfo = null;

			for( int i=0; i<patternCollection.Count; i++ )
			{
				holdingPattern = patternCollection.GetPattern( i );

				if( historicalPatternCollection.IsIn( holdingPattern ) == true )
				{
					holdingPattern = ( ConnectFourPattern )historicalPatternCollection.GetPattern( holdingPattern );

					if( holdingPattern.NumberOfTimesSeenInLosingGame > 0 && IsValidPattern( holdingPattern ) == true )
					{
						holdingPattern.Weighting = nMemoryDefensiveWeight;
						defensivePatterns.AddPattern( holdingPattern );
					}
					else
					{
						if( connectFourGame.GetSquareInfo( holdingPattern.GetStartsWith() ).IsRed != connectFourGame.PlayerIsRed )
							defensivePatterns.AddPattern( holdingPattern );
					}
				}
				else /// determine if this is a defensive pattern
				{
					patternSquareInfo = connectFourGame.GetSquareInfo( holdingPattern.GetStartsWith() );

					/// if the pattern square colour is NOT the same as the player colour
					/// it's a pattern that needs defending against
					/// 
					if( patternSquareInfo.IsRed != connectFourGame.PlayerIsRed )
					{
						leftSquareInfo = connectFourGame.GetSquareInfo( GetIdentifierToLeft( patternSquareInfo.SquareIdentifier ) );
						leftSquareNextInfo = connectFourGame.GetSquareInfo( GetIdentifierToLeft( GetIdentifierToLeft( patternSquareInfo.SquareIdentifier ) ) );
						rightSquareInfo = connectFourGame.GetSquareInfo( GetIdentifierToRight( patternSquareInfo.SquareIdentifier ) );
						rightSquareNextInfo = connectFourGame.GetSquareInfo( GetIdentifierToRight( GetIdentifierToRight( patternSquareInfo.SquareIdentifier ) ) );

						if( leftSquareNextInfo != null )
							nextSquareInfo = connectFourGame.GetSquareInfo( GetIdentifierToLeft( leftSquareNextInfo.SquareIdentifier ) );
						else
							nextSquareInfo = null;

						if( leftSquareInfo != null && leftSquareNextInfo != null )
						{
							if( leftSquareInfo.IsOccupied == true && leftSquareInfo.IsRed != connectFourGame.PlayerIsRed ) 
							{
								if( leftSquareNextInfo.IsOccupied == false )
									defensivePatterns.AddPattern( holdingPattern );
								else
								{
									if( nextSquareInfo != null && nextSquareInfo.IsOccupied == false
										&& leftSquareNextInfo.IsRed != connectFourGame.PlayerIsRed )
									{
										defensivePatterns.AddPattern( holdingPattern );
									}
								}
							}
						}

						if( rightSquareNextInfo != null )
							nextSquareInfo = connectFourGame.GetSquareInfo( GetIdentifierToRight( rightSquareNextInfo.SquareIdentifier ) );
						else
							nextSquareInfo = null;

						if( rightSquareInfo != null && rightSquareNextInfo != null )
						{
							if( rightSquareInfo.IsOccupied == true && rightSquareInfo.IsRed != connectFourGame.PlayerIsRed ) 
							{
								if( rightSquareNextInfo.IsOccupied == false )
									defensivePatterns.AddPattern( holdingPattern );
								else
								{
									if( nextSquareInfo != null && nextSquareInfo.IsOccupied == false 
										&& rightSquareNextInfo.IsRed != connectFourGame.PlayerIsRed )
									{
										defensivePatterns.AddPattern( holdingPattern );
									}
								}
							}
						}

						if( leftSquareInfo != null && rightSquareInfo != null )
						{
							if( leftSquareInfo.IsOccupied == true && leftSquareInfo.IsRed != connectFourGame.PlayerIsRed 
								&& rightSquareInfo.IsOccupied == true && rightSquareInfo.IsRed != connectFourGame.PlayerIsRed
								&& leftSquareNextInfo != null && leftSquareNextInfo.IsOccupied == false ) 
							{
								
								defensivePatterns.AddPattern( holdingPattern );
							}

							if( leftSquareInfo.IsOccupied == true && leftSquareInfo.IsRed != connectFourGame.PlayerIsRed
								&& rightSquareInfo.IsOccupied == true && rightSquareInfo.IsRed != connectFourGame.PlayerIsRed
								&& rightSquareNextInfo != null && rightSquareNextInfo.IsOccupied == false )
							{
								defensivePatterns.AddPattern( holdingPattern );
							}
						}


						belowLeftSquareInfo = connectFourGame.GetSquareInfo( GetIdentifierBelowLeft( patternSquareInfo.SquareIdentifier ) );
						belowLeftSquareNextInfo = connectFourGame.GetSquareInfo( GetIdentifierBelowLeft( GetIdentifierBelowLeft( patternSquareInfo.SquareIdentifier ) ) );
						aboveRightSquareInfo = connectFourGame.GetSquareInfo( GetIdentifierAboveRight( patternSquareInfo.SquareIdentifier ) );
						aboveRightSquareNextInfo = connectFourGame.GetSquareInfo( GetIdentifierAboveRight( GetIdentifierAboveRight( patternSquareInfo.SquareIdentifier ) ) );

						if( belowLeftSquareNextInfo != null )
							nextSquareInfo = connectFourGame.GetSquareInfo( GetIdentifierBelowLeft( belowLeftSquareNextInfo.SquareIdentifier ) );
						else
							nextSquareInfo = null;

						if( belowLeftSquareInfo != null && belowLeftSquareNextInfo != null )
						{
							if( belowLeftSquareInfo.IsOccupied == true && belowLeftSquareInfo.IsRed != connectFourGame.PlayerIsRed ) 
							{
								if( belowLeftSquareNextInfo.IsOccupied == false )
									defensivePatterns.AddPattern( holdingPattern );
								else
								{
									if( nextSquareInfo != null && nextSquareInfo.IsOccupied == false 
										&& belowLeftSquareNextInfo.IsRed != connectFourGame.PlayerIsRed )
										defensivePatterns.AddPattern( holdingPattern );
								}
							}
						}

						if( aboveRightSquareNextInfo != null )
							nextSquareInfo = connectFourGame.GetSquareInfo( GetIdentifierAboveRight( aboveRightSquareNextInfo.SquareIdentifier ) );
						else
							nextSquareInfo = null;

						if( aboveRightSquareInfo != null && aboveRightSquareNextInfo != null )
						{
							if( aboveRightSquareInfo.IsOccupied == true && aboveRightSquareInfo.IsRed != connectFourGame.PlayerIsRed ) 
							{
								if( aboveRightSquareNextInfo.IsOccupied == false )
									defensivePatterns.AddPattern( holdingPattern );
								else
								{
									if( nextSquareInfo != null && nextSquareInfo.IsOccupied == false 
										&& aboveRightSquareNextInfo.IsRed != connectFourGame.PlayerIsRed )
										defensivePatterns.AddPattern( holdingPattern );
								}
							}
						}

						if( belowLeftSquareInfo != null && aboveRightSquareInfo != null )
						{
							if( belowLeftSquareInfo.IsOccupied == true && belowLeftSquareInfo.IsRed != connectFourGame.PlayerIsRed
								&& aboveRightSquareInfo.IsOccupied == true && aboveRightSquareInfo.IsRed != connectFourGame.PlayerIsRed 
								&& belowLeftSquareNextInfo != null && belowLeftSquareNextInfo.IsOccupied == false )
							{
								defensivePatterns.AddPattern( holdingPattern );
							}

							if( belowLeftSquareInfo.IsOccupied == true && belowLeftSquareInfo.IsRed != connectFourGame.PlayerIsRed
								&& aboveRightSquareInfo.IsOccupied == true && aboveRightSquareInfo.IsRed != connectFourGame.PlayerIsRed
								&& aboveRightSquareNextInfo != null && aboveRightSquareNextInfo.IsOccupied == false )
							{
								defensivePatterns.AddPattern( holdingPattern );
							}
						}

						aboveSquareInfo = connectFourGame.GetSquareInfo( GetIdentifierAbove( patternSquareInfo.SquareIdentifier ) );

						if( aboveSquareInfo != null )
						{
							if( aboveSquareInfo.IsOccupied == true
								&& connectFourGame.PlayerIsRed != aboveSquareInfo.IsRed )
							{
								defensivePatterns.AddPattern( holdingPattern );
							}

							nextSquareInfo = connectFourGame.GetSquareInfo ( GetIdentifierAbove( patternSquareInfo.SquareIdentifier ) );

							if( nextSquareInfo != null )
							{
								if( nextSquareInfo.IsOccupied == true 
									&& connectFourGame.PlayerIsRed != aboveSquareInfo.IsRed 
									&& connectFourGame.PlayerIsRed != nextSquareInfo.IsRed )
								{
									defensivePatterns.AddPattern( holdingPattern );
								}
							}
						}
								


						belowRightSquareInfo = connectFourGame.GetSquareInfo( GetIdentifierBelowRight( patternSquareInfo.SquareIdentifier ) );
						belowRightSquareNextInfo = connectFourGame.GetSquareInfo( GetIdentifierBelowRight( GetIdentifierBelowRight( patternSquareInfo.SquareIdentifier ) ) );
						aboveLeftSquareInfo = connectFourGame.GetSquareInfo( GetIdentifierAboveLeft( patternSquareInfo.SquareIdentifier ) );
						aboveLeftSquareNextInfo = connectFourGame.GetSquareInfo( GetIdentifierAboveLeft( GetIdentifierAboveLeft( patternSquareInfo.SquareIdentifier ) ) );

						if( belowRightSquareNextInfo != null )
							nextSquareInfo = connectFourGame.GetSquareInfo( GetIdentifierBelowRight( belowRightSquareNextInfo.SquareIdentifier ) );
						else
							nextSquareInfo = null;

						if( belowRightSquareInfo != null && belowRightSquareNextInfo != null )
						{
							if( belowRightSquareInfo.IsOccupied == true && belowRightSquareInfo.IsRed != connectFourGame.PlayerIsRed ) 
							{
								if( belowRightSquareNextInfo.IsOccupied == false )
									defensivePatterns.AddPattern( holdingPattern );
								else
								{
									if( nextSquareInfo != null && nextSquareInfo.IsOccupied == false 
										&& belowRightSquareNextInfo.IsRed != connectFourGame.PlayerIsRed )
										defensivePatterns.AddPattern( holdingPattern );
								}
							}
						}

						if( aboveLeftSquareNextInfo != null )
							nextSquareInfo = connectFourGame.GetSquareInfo( GetIdentifierAboveLeft( aboveLeftSquareNextInfo.SquareIdentifier ) );
						else
							nextSquareInfo = null;

						if( aboveLeftSquareInfo != null && aboveLeftSquareNextInfo != null )
						{
							if( aboveLeftSquareInfo.IsOccupied == true && aboveLeftSquareInfo.IsRed != connectFourGame.PlayerIsRed )
							{
								if( aboveLeftSquareNextInfo.IsOccupied == false ) 
									defensivePatterns.AddPattern( holdingPattern );
								else
								{
									if( nextSquareInfo != null && nextSquareInfo.IsOccupied == false 
										&& aboveLeftSquareNextInfo.IsRed != connectFourGame.PlayerIsRed )
										defensivePatterns.AddPattern( holdingPattern );
								}
							}
						}

						if( belowRightSquareInfo != null && aboveLeftSquareInfo != null )
						{
							if( belowRightSquareInfo.IsOccupied == true && belowLeftSquareInfo.IsRed != connectFourGame.PlayerIsRed
								&& aboveLeftSquareInfo.IsOccupied == true && aboveLeftSquareInfo.IsRed != connectFourGame.PlayerIsRed 
								&& aboveLeftSquareNextInfo != null && aboveLeftSquareNextInfo.IsOccupied == false )
							{
								defensivePatterns.AddPattern( holdingPattern );
							}

							if( belowRightSquareInfo.IsOccupied == true && belowRightSquareInfo.IsRed != connectFourGame.PlayerIsRed
								&& aboveLeftSquareInfo.IsOccupied == true && aboveLeftSquareInfo.IsRed != connectFourGame.PlayerIsRed
								&& belowRightSquareNextInfo != null && belowRightSquareNextInfo.IsOccupied == false )
							{
								defensivePatterns.AddPattern( holdingPattern );
							}
						}
/*

						belowSquareInfo = connectFourGame.GetSquareInfo( GetIdentifierBelow( patternSquareInfo.SquareIdentifier ) );
						belowSquareNextInfo = connectFourGame.GetSquareInfo( GetIdentifierBelow( GetIdentifierBelow( patternSquareInfo.SquareIdentifier ) ) );

						if( belowSquareInfo != null && belowSquareNextInfo != null )
						{
							if( belowSquareInfo.IsOccupied == true && belowSquareInfo.IsRed != connectFourGame.PlayerIsRed
								&& belowSquareNextInfo.IsOccupied == false ) 
							{
								defensivePatterns.AddPattern( holdingPattern );
							}
						}
						
*/
					}
				}
			}

			/// Get all the patterns that have been seen before and the computer has won the game
			/// 

			for( int i=0; i<patternCollection.Count; i++ )
			{
				holdingPattern = ( ConnectFourPattern )patternCollection.Patterns[ i ];

				if( historicalPatternCollection.IsIn( holdingPattern ) == true )
				{
					holdingPattern = historicalPatternCollection.GetPattern( holdingPattern );

					if( holdingPattern.NumberOfTimesSeenInWinningGame > 0 && IsValidPattern( holdingPattern ) == true )
					{
						holdingPattern.Weighting = nMemoryAggressiveWeight;
						aggressivePatterns.AddPattern( holdingPattern );
					}
					else
					{
						if( connectFourGame.GetSquareInfo( holdingPattern.GetStartsWith() ).IsRed == connectFourGame.PlayerIsRed )
							aggressivePatterns.AddPattern( holdingPattern );
					}
				}
				else /// determine if an this is an aggressive pattern
				{

					patternSquareInfo = connectFourGame.GetSquareInfo( holdingPattern.GetStartsWith() );

					/// if the pattern square info is the same colour as the player
					/// it's an aggressive pattern
					/// 
					if( patternSquareInfo.IsRed == connectFourGame.PlayerIsRed )
					{
						leftSquareInfo = connectFourGame.GetSquareInfo( GetIdentifierToLeft( patternSquareInfo.SquareIdentifier ) );
						leftSquareNextInfo = connectFourGame.GetSquareInfo( GetIdentifierToLeft( GetIdentifierToLeft( patternSquareInfo.SquareIdentifier ) ) );
						rightSquareInfo = connectFourGame.GetSquareInfo( GetIdentifierToRight( patternSquareInfo.SquareIdentifier ) );
						rightSquareNextInfo = connectFourGame.GetSquareInfo( GetIdentifierToRight( GetIdentifierToRight( patternSquareInfo.SquareIdentifier ) ) );

						if( leftSquareNextInfo != null )
							nextSquareInfo = connectFourGame.GetSquareInfo( GetIdentifierToLeft( leftSquareNextInfo.SquareIdentifier ) );
						else
							nextSquareInfo = null;

						if( leftSquareInfo != null && leftSquareNextInfo != null )
						{
							if( leftSquareInfo.IsOccupied == true && leftSquareInfo.IsRed == connectFourGame.PlayerIsRed )
							{
								
								if( leftSquareNextInfo.IsOccupied == false ) 
									aggressivePatterns.AddPattern( holdingPattern );
								else
								{
									if( nextSquareInfo != null && nextSquareInfo.IsOccupied == false 
										&& leftSquareNextInfo.IsRed == connectFourGame.PlayerIsRed )
										aggressivePatterns.AddPattern( holdingPattern );
								}
							}
						}

						if( rightSquareNextInfo != null )
							nextSquareInfo = connectFourGame.GetSquareInfo( GetIdentifierToRight( rightSquareNextInfo.SquareIdentifier ) );
						else
							nextSquareInfo = null;

						if( rightSquareInfo != null && rightSquareNextInfo != null )
						{
							if( rightSquareInfo.IsOccupied == true && rightSquareInfo.IsRed == connectFourGame.PlayerIsRed )
							{
								if( rightSquareNextInfo.IsOccupied == false ) 
									aggressivePatterns.AddPattern( holdingPattern );
								else
								{
									if( nextSquareInfo != null && nextSquareInfo.IsOccupied == false 
										&& rightSquareNextInfo.IsRed == connectFourGame.PlayerIsRed )
										aggressivePatterns.AddPattern( holdingPattern );
								}
							}
						}

						if( leftSquareInfo != null && rightSquareInfo != null )
						{
							if( leftSquareInfo.IsOccupied == true && leftSquareInfo.IsRed == connectFourGame.PlayerIsRed
								&& rightSquareInfo.IsOccupied == true && rightSquareInfo.IsRed == connectFourGame.PlayerIsRed 
								&& leftSquareNextInfo != null && leftSquareNextInfo.IsOccupied == false )
							{
								aggressivePatterns.AddPattern( holdingPattern );
							}

							if( leftSquareInfo.IsOccupied == true && leftSquareInfo.IsRed == connectFourGame.PlayerIsRed
								&& rightSquareInfo.IsOccupied == true && rightSquareInfo.IsRed == connectFourGame.PlayerIsRed
								&& rightSquareNextInfo != null && rightSquareNextInfo.IsOccupied == false )
							{
								aggressivePatterns.AddPattern( holdingPattern );
							}
						}

						aboveSquareInfo = connectFourGame.GetSquareInfo( GetIdentifierAbove( patternSquareInfo.SquareIdentifier ) );
						
						if( aboveSquareInfo != null )
						{
							if( aboveSquareInfo.IsOccupied == true
								&& connectFourGame.PlayerIsRed == aboveSquareInfo.IsRed )
							{
								aggressivePatterns.AddPattern( holdingPattern );
							}

							nextSquareInfo = connectFourGame.GetSquareInfo ( GetIdentifierAbove( patternSquareInfo.SquareIdentifier ) );

							if( nextSquareInfo != null )
							{
								if( nextSquareInfo.IsOccupied == true 
									&& connectFourGame.PlayerIsRed == aboveSquareInfo.IsRed 
									&& connectFourGame.PlayerIsRed == nextSquareInfo.IsRed )
								{
									aggressivePatterns.AddPattern( holdingPattern );
								}
							}
						}



						belowLeftSquareInfo = connectFourGame.GetSquareInfo( GetIdentifierBelowLeft( patternSquareInfo.SquareIdentifier ) );
						belowLeftSquareNextInfo = connectFourGame.GetSquareInfo( GetIdentifierBelowLeft( GetIdentifierBelowLeft( patternSquareInfo.SquareIdentifier ) ) );
						aboveRightSquareInfo = connectFourGame.GetSquareInfo( GetIdentifierAboveRight( patternSquareInfo.SquareIdentifier ) );
						aboveRightSquareNextInfo = connectFourGame.GetSquareInfo( GetIdentifierAboveRight( GetIdentifierAboveRight( patternSquareInfo.SquareIdentifier ) ) );

						if( belowLeftSquareNextInfo != null )
							nextSquareInfo = connectFourGame.GetSquareInfo( GetIdentifierBelowLeft( belowLeftSquareNextInfo.SquareIdentifier ) );
						else
							nextSquareInfo = null;

						if( belowLeftSquareInfo != null && belowLeftSquareNextInfo != null )
						{
							if( belowLeftSquareInfo.IsOccupied == true && belowLeftSquareInfo.IsRed == connectFourGame.PlayerIsRed )
							{
								if( belowLeftSquareNextInfo.IsOccupied == false ) 
									aggressivePatterns.AddPattern( holdingPattern );
								else
								{
									if( nextSquareInfo != null && nextSquareInfo.IsOccupied == false
										&& belowLeftSquareNextInfo.IsRed == connectFourGame.PlayerIsRed )
										aggressivePatterns.AddPattern( holdingPattern );
								}
							}
						}

						if( aboveRightSquareNextInfo != null )
							nextSquareInfo = connectFourGame.GetSquareInfo( GetIdentifierAboveRight( aboveRightSquareNextInfo.SquareIdentifier ) );
						else
							nextSquareInfo = null;

						if( aboveRightSquareInfo != null && aboveRightSquareNextInfo != null )
						{
							if( aboveRightSquareInfo.IsOccupied == true && aboveRightSquareInfo.IsRed == connectFourGame.PlayerIsRed )
							{
								if( aboveRightSquareNextInfo.IsOccupied == false ) 
									aggressivePatterns.AddPattern( holdingPattern );
								else
								{
									if( nextSquareInfo != null && nextSquareInfo.IsOccupied == false 
										&& aboveRightSquareNextInfo.IsRed == connectFourGame.PlayerIsRed )
										aggressivePatterns.AddPattern( holdingPattern );
								}
							}
						}

						if( belowLeftSquareInfo != null && aboveRightSquareInfo != null )
						{
							if( belowLeftSquareInfo.IsOccupied == true && belowLeftSquareInfo.IsRed == connectFourGame.PlayerIsRed
								&& aboveRightSquareInfo.IsOccupied == true && aboveRightSquareInfo.IsRed == connectFourGame.PlayerIsRed 
								&& belowLeftSquareNextInfo != null && belowLeftSquareNextInfo.IsOccupied == false )
							{
								aggressivePatterns.AddPattern( holdingPattern );
							}

							if( belowLeftSquareInfo.IsOccupied == true && belowLeftSquareInfo.IsRed == connectFourGame.PlayerIsRed
								&& aboveRightSquareInfo.IsOccupied == true && aboveRightSquareInfo.IsRed == connectFourGame.PlayerIsRed
								&& aboveRightSquareNextInfo != null && aboveRightSquareNextInfo.IsOccupied == false )
							{
								aggressivePatterns.AddPattern( holdingPattern );
							}
						}


						belowRightSquareInfo = connectFourGame.GetSquareInfo( GetIdentifierBelowRight( patternSquareInfo.SquareIdentifier ) );
						belowRightSquareNextInfo = connectFourGame.GetSquareInfo( GetIdentifierBelowRight( GetIdentifierBelowRight( patternSquareInfo.SquareIdentifier ) ) );
						aboveLeftSquareInfo = connectFourGame.GetSquareInfo( GetIdentifierAboveLeft( patternSquareInfo.SquareIdentifier ) );
						aboveLeftSquareNextInfo = connectFourGame.GetSquareInfo( GetIdentifierAboveLeft( GetIdentifierAboveLeft( patternSquareInfo.SquareIdentifier ) ) );

						if( belowRightSquareNextInfo != null )
							nextSquareInfo = connectFourGame.GetSquareInfo( GetIdentifierBelowRight( belowRightSquareNextInfo.SquareIdentifier ) );
						else
							nextSquareInfo = null;

						if( belowRightSquareInfo != null && belowRightSquareNextInfo != null )
						{
							if( belowRightSquareInfo.IsOccupied == true && belowRightSquareInfo.IsRed == connectFourGame.PlayerIsRed  )
							{
								if( belowRightSquareNextInfo.IsOccupied == false )
									aggressivePatterns.AddPattern( holdingPattern );
								else
								{
									if( nextSquareInfo != null && nextSquareInfo.IsOccupied == false 
										&& belowRightSquareNextInfo.IsRed == connectFourGame.PlayerIsRed )
										aggressivePatterns.AddPattern( holdingPattern );
								}
							}
						}

						if( aboveLeftSquareNextInfo != null )
							nextSquareInfo = connectFourGame.GetSquareInfo( GetIdentifierAboveLeft( aboveLeftSquareNextInfo.SquareIdentifier ) );
						else
							nextSquareInfo = null;

						if( aboveLeftSquareInfo != null && aboveLeftSquareNextInfo != null )
						{
							if( aboveLeftSquareInfo.IsOccupied == true && aboveLeftSquareInfo.IsRed == connectFourGame.PlayerIsRed )
							{
								if( aboveLeftSquareNextInfo.IsOccupied == false ) 
									aggressivePatterns.AddPattern( holdingPattern );
								else
								{
									if( nextSquareInfo != null && nextSquareInfo.IsOccupied == false 
										&& aboveLeftSquareNextInfo.IsRed == connectFourGame.PlayerIsRed )
										aggressivePatterns.AddPattern( holdingPattern );
								}
							}
						}

						if( belowRightSquareInfo != null && aboveLeftSquareInfo != null )
						{
							if( belowRightSquareInfo.IsOccupied == true && belowRightSquareInfo.IsRed == connectFourGame.PlayerIsRed
								&& aboveLeftSquareInfo.IsOccupied == true && aboveLeftSquareInfo.IsRed == connectFourGame.PlayerIsRed 
								&& belowRightSquareNextInfo != null && belowRightSquareNextInfo.IsOccupied == false )
							{
								aggressivePatterns.AddPattern( holdingPattern );
							}

							if( belowRightSquareInfo.IsOccupied == true && belowRightSquareInfo.IsRed == connectFourGame.PlayerIsRed
								&& aboveLeftSquareInfo.IsOccupied == true && aboveLeftSquareInfo.IsRed == connectFourGame.PlayerIsRed
								&& aboveLeftSquareNextInfo != null && aboveLeftSquareNextInfo.IsOccupied == false )
							{
								aggressivePatterns.AddPattern( holdingPattern );
							}
						}

/*
						belowSquareInfo = connectFourGame.GetSquareInfo( GetIdentifierBelow( patternSquareInfo.SquareIdentifier ) );
						belowSquareNextInfo = connectFourGame.GetSquareInfo( GetIdentifierBelow( GetIdentifierBelow( patternSquareInfo.SquareIdentifier ) ) );

						if( belowSquareInfo != null && belowSquareNextInfo != null )
						{
							if( belowSquareInfo.IsOccupied == true && belowSquareInfo.IsRed == connectFourGame.PlayerIsRed 
								&& belowSquareNextInfo.IsOccupied == false )
							{
								aggressivePatterns.AddPattern( holdingPattern );
							}
						}
			*/
					}
				}
			}


			/// Now get the warning patterns ( Basically two opponent pieces next to each other 
			/// with spaces at either end )
			/// 

			for( int i=0; i<patternCollection.Count; i++ )
			{
				holdingPattern = ( ConnectFourPattern )patternCollection.Patterns[ i ];

				if( historicalPatternCollection.IsIn( holdingPattern ) == true )
				{
					holdingPattern = historicalPatternCollection.GetPattern( holdingPattern );

					if( holdingPattern.NumberOfTimesSeenInLosingGame > 0 && IsValidPattern( holdingPattern ) == true )
					{
						holdingPattern.Weighting = nMemoryWarningWeight;
						warningPatterns.AddPattern( holdingPattern );
					}
				}
				else 
				{
					patternSquareInfo = connectFourGame.GetSquareInfo( holdingPattern.GetStartsWith() );

					/// if the pattern square info is NOT the same as the player colour
					/// then it's a warning pattern
					/// 
					if( patternSquareInfo.IsRed == true && connectFourGame.PlayerIsRed == false ||
						patternSquareInfo.IsRed == false && connectFourGame.PlayerIsRed == true )
					{
						leftSquareInfo = connectFourGame.GetSquareInfo( GetIdentifierToLeft( patternSquareInfo.SquareIdentifier ) );
						rightSquareInfo = connectFourGame.GetSquareInfo( GetIdentifierToRight( patternSquareInfo.SquareIdentifier ) );

						if( leftSquareInfo != null )
						{
							if( leftSquareInfo.IsOccupied == true && leftSquareInfo.IsRed != connectFourGame.PlayerIsRed )
							{
								warningPatterns.AddPattern( holdingPattern );
							}
						}

						if( rightSquareInfo != null )
						{
							if( rightSquareInfo.IsOccupied == true && rightSquareInfo.IsRed != connectFourGame.PlayerIsRed )
							{
								warningPatterns.AddPattern( holdingPattern );
							}
						}

						aboveSquareInfo = connectFourGame.GetSquareInfo( GetIdentifierAbove( patternSquareInfo.SquareIdentifier ) );
						if( aboveSquareInfo != null && aboveSquareInfo.IsOccupied == true
							&& connectFourGame.PlayerIsRed != aboveSquareInfo.IsRed )
						{
							warningPatterns.AddPattern( holdingPattern );
						}
							

						belowLeftSquareInfo = connectFourGame.GetSquareInfo( GetIdentifierBelowLeft( patternSquareInfo.SquareIdentifier ) );
						aboveRightSquareInfo = connectFourGame.GetSquareInfo( GetIdentifierAboveRight( patternSquareInfo.SquareIdentifier ) );

						if( belowLeftSquareInfo != null )
						{
							if( belowLeftSquareInfo.IsOccupied == true && belowLeftSquareInfo.IsRed != connectFourGame.PlayerIsRed )
							{
								warningPatterns.AddPattern( holdingPattern );
							}
						}

						if( aboveRightSquareInfo != null )
						{
							if( aboveRightSquareInfo.IsOccupied == true && aboveRightSquareInfo.IsRed != connectFourGame.PlayerIsRed )
							{
								warningPatterns.AddPattern( holdingPattern );
							}
						}

						aboveLeftSquareInfo = connectFourGame.GetSquareInfo( GetIdentifierAboveLeft( patternSquareInfo.SquareIdentifier ) );
						belowRightSquareInfo = connectFourGame.GetSquareInfo( GetIdentifierBelowRight( patternSquareInfo.SquareIdentifier ) );

						if( aboveLeftSquareInfo != null )
						{
							if( aboveLeftSquareInfo.IsOccupied == true && aboveLeftSquareInfo.IsRed != connectFourGame.PlayerIsRed )
							{
								warningPatterns.AddPattern( holdingPattern );
							}
						}

						if( belowRightSquareInfo != null )
						{
							if( belowRightSquareInfo.IsOccupied == true && belowRightSquareInfo.IsRed != connectFourGame.PlayerIsRed )
							{
								warningPatterns.AddPattern( holdingPattern );
							}
						}

						belowSquareInfo = connectFourGame.GetSquareInfo( GetIdentifierBelow( patternSquareInfo.SquareIdentifier ) );

						if( belowSquareInfo != null )
						{
							if( belowSquareInfo.IsOccupied == true && belowSquareInfo.IsRed != connectFourGame.PlayerIsRed )
							{
								warningPatterns.AddPattern( holdingPattern );
							}
						}
					}
				}
			}


			for( int i=0; i<patternCollection.Count; i++ )
			{
				holdingPattern = ( ConnectFourPattern )patternCollection.Patterns[ i ];

				if( historicalPatternCollection.IsIn( holdingPattern ) == true )
				{
					holdingPattern = historicalPatternCollection.GetPattern( holdingPattern );

					if( IsValidPattern( holdingPattern ) == true )
					{
						holdingPattern.Weighting = nMemoryTacticalWeight;
						tacticalPatterns.AddPattern( holdingPattern );
					}
				}
				else 
				{
					patternSquareInfo = connectFourGame.GetSquareInfo( holdingPattern.GetStartsWith() );

					/// if the pattern square info colour is the same as the player colour
					/// then it's a potential tactical pattern
					/// 

					if( patternSquareInfo.IsRed == true && connectFourGame.PlayerIsRed == true ||
						patternSquareInfo.IsRed == false && connectFourGame.PlayerIsRed == false )
					{
						leftSquareInfo = connectFourGame.GetSquareInfo( GetIdentifierToLeft( patternSquareInfo.SquareIdentifier ) );
						rightSquareInfo = connectFourGame.GetSquareInfo( GetIdentifierToRight( patternSquareInfo.SquareIdentifier ) );

						if( leftSquareInfo != null )
						{
							if( leftSquareInfo.IsOccupied == true && leftSquareInfo.IsRed == connectFourGame.PlayerIsRed )
							{
								tacticalPatterns.AddPattern( holdingPattern );
							}
						}

						if( rightSquareInfo != null )
						{
							if( rightSquareInfo.IsOccupied == true && rightSquareInfo.IsRed == connectFourGame.PlayerIsRed )
							{
								tacticalPatterns.AddPattern( holdingPattern );
							}
						}

						aboveSquareInfo = connectFourGame.GetSquareInfo( GetIdentifierAbove( patternSquareInfo.SquareIdentifier ) );
						if( aboveSquareInfo != null && aboveSquareInfo.IsOccupied == true 
							&& connectFourGame.PlayerIsRed == patternSquareInfo.IsRed )
						{
							tacticalPatterns.AddPattern( holdingPattern );
						}


						belowLeftSquareInfo = connectFourGame.GetSquareInfo( GetIdentifierBelowLeft( patternSquareInfo.SquareIdentifier ) );
						aboveRightSquareInfo = connectFourGame.GetSquareInfo( GetIdentifierAboveRight( patternSquareInfo.SquareIdentifier ) );


						if( belowLeftSquareInfo != null )
						{
							if( belowLeftSquareInfo.IsOccupied == true && belowLeftSquareInfo.IsRed == connectFourGame.PlayerIsRed )
							{
								tacticalPatterns.AddPattern( holdingPattern );
							}
						}

						if( aboveRightSquareInfo != null )
						{
							if( aboveRightSquareInfo.IsOccupied == true && aboveRightSquareInfo.IsRed == connectFourGame.PlayerIsRed )
							{
								tacticalPatterns.AddPattern( holdingPattern );
							}
						}

						aboveLeftSquareInfo = connectFourGame.GetSquareInfo( GetIdentifierAboveLeft( patternSquareInfo.SquareIdentifier ) );
						belowRightSquareInfo = connectFourGame.GetSquareInfo( GetIdentifierBelowRight( patternSquareInfo.SquareIdentifier ) );

						if( aboveLeftSquareInfo != null )
						{
							if( aboveLeftSquareInfo.IsOccupied == true && aboveLeftSquareInfo.IsRed == connectFourGame.PlayerIsRed )
							{
								tacticalPatterns.AddPattern( holdingPattern );
							}
						}

						if( belowRightSquareInfo != null )
						{
							if( belowRightSquareInfo.IsOccupied == true && belowRightSquareInfo.IsRed == connectFourGame.PlayerIsRed )
							{
								tacticalPatterns.AddPattern( holdingPattern );
							}
						}

						belowSquareInfo = connectFourGame.GetSquareInfo( GetIdentifierBelow( patternSquareInfo.SquareIdentifier ) );

						if( belowSquareInfo != null )
						{
							if( belowSquareInfo.IsOccupied == true && belowSquareInfo.IsRed == connectFourGame.PlayerIsRed )
							{
								tacticalPatterns.AddPattern( holdingPattern );
							}
						}
					}
				}
			}


			if( aggressivePatterns.Count == 0 && defensivePatterns.Count == 0 
				&& warningPatterns.Count == 0 && tacticalPatterns.Count == 0 )
			{
				MakeRandomMove();
				return;
			}

			/// All the information for a move has been collected no decide which
			/// move to make
			/// 

			bool bMoved = false;
			int nAttempts = 5;
			int nCount = 0;
			ConnectFourPattern bestAggressivePattern = null;
			ConnectFourPattern bestDefensivePattern = null;
			ConnectFourPattern bestWarningPattern = null;
			ConnectFourPattern bestTacticalPattern = null;
			ConnectFourPattern bestPattern = null;



			while( bMoved == false )
			{
				if( aggressivePatterns.Count > 0 )
				{
					ProcessPatterns( ref aggressivePatterns, nAggressiveWeight );

					if( aggressivePatterns.Count >= 1 )
					{
						bestAggressivePattern = aggressivePatterns.HighestPatternWeighting();
					}
					else
						bestAggressivePattern = ( ConnectFourPattern )aggressivePatterns.Patterns[ 0 ];
				}

				if( defensivePatterns.Count > 0 )
				{
					ProcessPatterns( ref defensivePatterns, nDefensiveWeight );

					if( defensivePatterns.Count >= 1 )
					{
						bestDefensivePattern = defensivePatterns.HighestPatternWeighting();
					}
					else
						bestDefensivePattern = ( ConnectFourPattern )defensivePatterns.Patterns[ 0 ];
				}

				if( warningPatterns.Count > 0 )
				{
					ProcessPatterns( ref warningPatterns, nWarningWeight );

					if( warningPatterns.Count >= 1 )
					{
						bestWarningPattern = warningPatterns.HighestPatternWeighting();
					}
					else
						bestWarningPattern = ( ConnectFourPattern )warningPatterns.Patterns[ 0 ];
				}

				if( tacticalPatterns.Count > 0 )
				{
					ProcessPatterns( ref tacticalPatterns, nTacticalWeight );

					if( tacticalPatterns.Count >= 1 )
					{
						bestTacticalPattern = tacticalPatterns.HighestPatternWeighting();
					}
					else
						bestTacticalPattern = ( ConnectFourPattern )tacticalPatterns.Patterns[ 0 ];
				}

				if( bestAggressivePattern != null )
					bestPattern = bestAggressivePattern;

				if( bestDefensivePattern != null )
				{
					if( bestAggressivePattern != null )
					{
						if( bestAggressivePattern.Weighting < bestDefensivePattern.Weighting )
							bestPattern = bestDefensivePattern;
					}
					else
						bestPattern = bestDefensivePattern;
				}

				if( bestWarningPattern != null )
				{
					if( bestDefensivePattern != null )
					{
						if( bestDefensivePattern.Weighting < bestWarningPattern.Weighting
							&& bestWarningPattern.Weighting > bestPattern.Weighting )
							bestPattern = bestWarningPattern;
					}
					else
						bestPattern = bestWarningPattern;
				}

				if( bestTacticalPattern != null )
				{
					if( bestWarningPattern != null )
					{
						if( bestWarningPattern.Weighting < bestTacticalPattern.Weighting
							&& bestTacticalPattern.Weighting > bestPattern.Weighting )
							bestPattern = bestTacticalPattern;
					}
					else
						bestPattern = bestTacticalPattern;
				}

				if( bestPattern != null )
				{
					/// see just what I have worked out to be the best move
					/// 

					if( bDisplayBestMove == true )
					{
						StringBuilder outputString = new StringBuilder( "Starts with :- " + bestPattern.GetStartsWith() + "\n" 
							+ "Direction :- " + bestPattern.GetPatternDirection().ToString() + "\n"
							+ "Is Ending Pattern :- " + bestPattern.IsEndingPattern.ToString() + "\n"
							+ "Is Losing Pattern :- " + bestPattern.IsLosingPattern.ToString() + "\n"
							+ "Is Winning Pattern :- " + bestPattern.IsWinningPattern.ToString() );
 
						if( bestPattern.ResponsePresent == true )
						{
							outputString.Append( "\n Response :- " + bestPattern.Response.SquareIdentifier );
						}

						MessageBox.Show( outputString.ToString() );
					}

					if( connectFourGame.PlayerIsRed == true )
					{
						if( bestPattern.Response != null )
						{
							bool bMove = false;

							if( IsGiveAwayMove( bestPattern.Response.SquareIdentifier ) == false )
								bMove = true;

							if( IsWinningMoveForPlayer( bestPattern.Response.SquareIdentifier ) == true )
								bMove = true;

							if( bMove == true )
							{
								ConnectFourSquareInfo bestInfo = connectFourGame.GetSquareInfo( bestPattern.Response.SquareIdentifier );
								ConnectFourSquareInfo info = connectFourGame.GetSquareInfo( GetIdentifierBelow( bestPattern.Response.SquareIdentifier ) );

								if( bestInfo != null && bestInfo.IsOccupied == false )
								{
									if( info == null || info != null && info.IsOccupied == true )
									{
										bMoved = true;
										connectFourGame.SetSquare( bestPattern.Response.SquareIdentifier, "BLUE" );
										connectFourGame.OutputText = "Square " + square.Identifier + " set to blue for the Computer";
									}
								}
							}
						}
					}
					else
					{
						if( bestPattern.Response != null )
						{
							bool bMove = false;

							if( IsWinningMoveForPlayer( bestPattern.Response.SquareIdentifier ) == false && IsGiveAwayMove( bestPattern.Response.SquareIdentifier ) == true )
								bMove = false;

							if( IsWinningMoveForPlayer( bestPattern.Response.SquareIdentifier ) == true )
								bMove = true;

							if( bMove == true )
							{
								
								ConnectFourSquareInfo bestInfo = connectFourGame.GetSquareInfo( bestPattern.Response.SquareIdentifier );
								ConnectFourSquareInfo info = connectFourGame.GetSquareInfo( GetIdentifierBelow( bestPattern.Response.SquareIdentifier ) );

								if( bestInfo != null && bestInfo.IsOccupied == false )
								{
									if( info == null || info != null && info.IsOccupied == true )
									{
										bMoved = true;
										connectFourGame.SetSquare( bestPattern.Response.SquareIdentifier, "RED" );
										connectFourGame.OutputText = "Square " + square.Identifier + " set to red for the Computer";
									}
								}
							}
						}
					}
				

					if( bestPattern.Response != null )
						InvalidateSquare( bestPattern.Response.SquareIdentifier );
				}

				if( nCount < nAttempts )
				{
					if( aggressivePatterns != null && aggressivePatterns.Count > 0 )
						aggressivePatterns.Patterns.Remove( bestPattern );
					if( defensivePatterns != null && defensivePatterns.Count > 0 )
						defensivePatterns.Patterns.Remove( bestPattern );
					if( warningPatterns != null && warningPatterns.Count > 0 )
						warningPatterns.Patterns.Remove( bestPattern );
					if( tacticalPatterns != null && tacticalPatterns.Count > 0 )
						tacticalPatterns.Patterns.Remove( bestPattern );
					
					bestAggressivePattern = null;
					bestDefensivePattern = null;
					bestWarningPattern = null;
					bestTacticalPattern = null;
					bestPattern = null;
				}

				if( nCount >= nAttempts )
				{
					if( bMoved == false )
					{
						nCount = nAttempts;
						MakeRandomMove();
						bMoved = true;
					}
				}
		

				nCount++;
			}
		}

		#region Process Important Patterns for this move


		/// <summary>
		/// process the patterns and give each a weighting
		/// </summary>
		/// <param name="patterns">group of patterns to process can be aggressive, defensive etc</param>
		/// <param name="patternWeight">weight value attributed to this type of pattern</param>
		private void ProcessPatterns( ref ConnectFourPatternCollection patterns, int patternWeight )
		{
			ConnectFourPattern pattern = null;
			ConnectFourSquareInfo squareInfo = null;
			bool bFindMove = false;
			for( int i=0; i<patterns.Count; i++ )
			{
				bFindMove = true;
				pattern = ( ConnectFourPattern )patterns.Patterns[ i ];

				/// if you've seen this pattern once before respond to it
				/// 


				if( pattern.NumberOfTimesSeen > 1 || pattern.Weighting == nAggressiveWeight )
				{

					/// ensures that a pattern of three pieces will have a higher priority
					/// than a pattern with two pieces
					/// 

					if( pattern.Count == 3 )
					{
						pattern.Weighting = patternWeight * 2;

						/// A winning pattern can be ignored if a defensive pattern from memory is used 
						/// and the winning pattern has not been seen before. 
						/// 
 
						if( patternWeight == nAggressiveWeight )
							pattern.Weighting += nMemoryAggressiveWeight;
					}
					else 
						pattern.Weighting = 0;

					/// give added weight if pattern is part of an ending move
					/// 

					if( pattern.IsEndingPattern == true )
						pattern.Weighting += 2;

					if( pattern.ResponsePresent == true )
					{
						if( pattern.NumberOfTimesSeenInWinningGame > pattern.NumberOfTimesSeenInLosingGame )
						{
							pattern.Weighting += pattern.NumberOfTimesSeenInWinningGame * 3;
							bFindMove = false;
						}
						else
							bFindMove = true;
					}

					/// try to increase the warning level in an attempt to detect 
					/// two way tricks
					/// 

					if( patternWeight == nWarningWeight && pattern.NumberOfTimesSeenInLosingGame > pattern.NumberOfTimesSeenInWinningGame )
						pattern.Weighting += pattern.NumberOfTimesSeenInLosingGame;

					if( bFindMove == true )
					{
						/// need to work out the direction of the pattern
						/// 

						BasicGamePiece responsePiece = ( BasicGamePiece )pattern.Response;

						if( pattern.PatternDirectionAbove() == true )
						{
							squareInfo = connectFourGame.GetSquareInfo( GetIdentifierAbove( pattern.GetAbovePieceIdentifier() ) );

							if( squareInfo != null && squareInfo.IsOccupied == false )
							{
								ConnectFourPiece piece = new ConnectFourPiece( squareInfo.SquareIdentifier );
								
								if( piece != responsePiece )
								{
									pattern.Response = piece;

									pattern.Weighting += patternWeight;
								}
							}
						}

						if( pattern.PatternDirectionAboveRight() == true ) 
						{
							int nTempWeightAboveRight = 0;
							int nTempWeightBelowLeft = 0;
							ConnectFourPiece pieceAboveRight = null;
							ConnectFourPiece pieceBelowLeft = null;

							ConnectFourSquareInfo squareInfoAboveRight = connectFourGame.GetSquareInfo( GetIdentifierAboveRight( pattern.GetAboveRightPieceIdentifier() ) );

							if( squareInfoAboveRight != null && squareInfoAboveRight.IsOccupied == true )
							{
								pieceAboveRight = new ConnectFourPiece( squareInfoAboveRight.SquareIdentifier );

								if( pieceAboveRight == responsePiece )
								{
									nTempWeightAboveRight++;	
								}

								if( connectFourGame.GetSquareInfo( GetIdentifierBelow( squareInfoAboveRight.SquareIdentifier ) ) != null )
								{
									if( connectFourGame.GetSquareInfo( GetIdentifierBelow( squareInfoAboveRight.SquareIdentifier ) ).IsOccupied == true )
										nTempWeightAboveRight++;
									else
										nTempWeightAboveRight--;
								}
								else
									nTempWeightAboveRight++;
							}

							ConnectFourSquareInfo squareInfoBelowLeft = connectFourGame.GetSquareInfo( GetIdentifierBelowLeft( pattern.GetBelowLeftPieceIdentifier() ) );

							if( squareInfoBelowLeft != null && squareInfoBelowLeft.IsOccupied == false )
							{
								pieceBelowLeft = new ConnectFourPiece( squareInfoBelowLeft.SquareIdentifier );

								if( pieceBelowLeft == responsePiece )
								{
									nTempWeightBelowLeft++;
								}

								if( connectFourGame.GetSquareInfo( GetIdentifierBelow( squareInfoBelowLeft.SquareIdentifier ) ) != null )
								{
									if( connectFourGame.GetSquareInfo( GetIdentifierBelow( squareInfoBelowLeft.SquareIdentifier ) ).IsOccupied == true )
										nTempWeightBelowLeft++;
									else
										nTempWeightBelowLeft--;
								}
								else
									nTempWeightBelowLeft++;
							}

							if( nTempWeightAboveRight == nTempWeightBelowLeft )
							{
								Random rand = new Random();

								if( rand.Next( 0, 1 ) > 0.5 )
									nTempWeightAboveRight++;
								else
									nTempWeightBelowLeft++;
							}

							if( nTempWeightAboveRight > nTempWeightBelowLeft )
							{
								pattern.Response = pieceAboveRight;
								pattern.Weighting += patternWeight;
							}
							else
							{
								pattern.Response = pieceBelowLeft;
								pattern.Weighting += patternWeight;
							}
						}


						if( pattern.PatternDirectionRight() == true ) 
						{
							int nTempWeightRight = 0;
							int nTempWeightLeft = 0;
							ConnectFourPiece pieceRight = null;
							ConnectFourPiece pieceLeft = null;

							ConnectFourSquareInfo squareInfoRight = connectFourGame.GetSquareInfo( GetIdentifierToRight( pattern.GetRightPieceIdentifier() ) );

							if( squareInfoRight != null && squareInfoRight.IsOccupied == false )
							{
								pieceRight = new ConnectFourPiece( squareInfoRight.SquareIdentifier );

								/// encourage response piece
								/// but don't penalize if it isn't
								if( pieceRight == responsePiece )
								{
									nTempWeightRight++;
								}
								

								if( connectFourGame.GetSquareInfo( GetIdentifierBelow( squareInfoRight.SquareIdentifier ) ) != null )
								{
									if( connectFourGame.GetSquareInfo( GetIdentifierBelow( squareInfoRight.SquareIdentifier ) ).IsOccupied == true )
										nTempWeightRight++;
									else
										nTempWeightRight--;
								}
								else
									nTempWeightRight++;
							}

							ConnectFourSquareInfo squareInfoLeft = connectFourGame.GetSquareInfo( GetIdentifierToLeft( pattern.GetLeftPieceIdentifier() ) );

							if( squareInfoLeft != null && squareInfoLeft.IsOccupied == false )
							{
								pieceLeft = new ConnectFourPiece( squareInfoLeft.SquareIdentifier );

								if( pieceLeft == responsePiece )
								{
									nTempWeightLeft++;
								}

								if( connectFourGame.GetSquareInfo( GetIdentifierBelow( squareInfoLeft.SquareIdentifier ) ) != null )
								{
									if( connectFourGame.GetSquareInfo( GetIdentifierBelow( squareInfoLeft.SquareIdentifier ) ).IsOccupied == true )
										nTempWeightLeft++;
									else
										nTempWeightLeft--;
								}
								else
									nTempWeightLeft++;
							}

							if( nTempWeightRight == nTempWeightLeft )
							{
								Random rand = new Random();

								if( rand.Next( 0, 1 ) > 0.5 )
									nTempWeightRight++;
								else
									nTempWeightLeft++;
							}

							if( nTempWeightRight > nTempWeightLeft )
							{
								pattern.Response = pieceRight;
								pattern.Weighting += patternWeight;
							}
							else
							{
								pattern.Response = pieceLeft;
								pattern.Weighting += patternWeight;
							}


						}

						if( pattern.PatternDirectionBelowRight() == true ) 
						{
							int nTempWeightBelowRight = 0;
							int nTempWeightAboveLeft = 0;
							ConnectFourPiece pieceBelowRight = null;
							ConnectFourPiece pieceAboveLeft = null;

							ConnectFourSquareInfo squareInfoBelowRight = connectFourGame.GetSquareInfo( GetIdentifierBelowRight( pattern.GetBelowRightPieceIdentifier() ) );

							if( squareInfoBelowRight != null && squareInfoBelowRight.IsOccupied == true )
							{
								pieceBelowRight = new ConnectFourPiece( squareInfoBelowRight.SquareIdentifier );

								if( pieceBelowRight == responsePiece )
								{
									nTempWeightBelowRight++;	
								}

								if( connectFourGame.GetSquareInfo( GetIdentifierBelow( squareInfoBelowRight.SquareIdentifier ) ) != null )
								{
									if( connectFourGame.GetSquareInfo( GetIdentifierBelow( squareInfoBelowRight.SquareIdentifier ) ).IsOccupied == true )
										nTempWeightBelowRight++;
									else
										nTempWeightBelowRight--;
								}
								else
									nTempWeightBelowRight++;
							}

							ConnectFourSquareInfo squareInfoAboveLeft = connectFourGame.GetSquareInfo( GetIdentifierAboveLeft( pattern.GetAboveLeftPieceIdentifier() ) );

							if( squareInfoAboveLeft != null && squareInfoAboveLeft.IsOccupied == false )
							{
								pieceAboveLeft = new ConnectFourPiece( squareInfoAboveLeft.SquareIdentifier );

								if( pieceAboveLeft == responsePiece )
								{
									nTempWeightAboveLeft++;
								}

								if( connectFourGame.GetSquareInfo( GetIdentifierBelow( squareInfoAboveLeft.SquareIdentifier ) ) != null )
								{
									if( connectFourGame.GetSquareInfo( GetIdentifierBelow( squareInfoAboveLeft.SquareIdentifier ) ).IsOccupied == true )
										nTempWeightAboveLeft++;
									else
										nTempWeightAboveLeft--;
								}
								else
									nTempWeightAboveLeft++;
							}

							if( nTempWeightBelowRight == nTempWeightAboveLeft )
							{
								Random rand = new Random();

								if( rand.Next( 0, 1 ) > 0.5 )
									nTempWeightBelowRight++;
								else
									nTempWeightAboveLeft++;
							}

							if( nTempWeightBelowRight > nTempWeightAboveLeft )
							{
								pattern.Response = pieceBelowRight;
								pattern.Weighting += patternWeight;
							}
							else
							{
								pattern.Response = pieceAboveLeft;
								pattern.Weighting += patternWeight;
							}
						}
					}
				}
			}
		}


		#endregion


		private bool IsValidPattern( ConnectFourPattern pattern )
		{
			switch( pattern.GetPatternDirection() )
			{
				case CONNECTFOURPATTERNDIRECTION.LEFTDIAGONAL:
				{
					ConnectFourSquareInfo squareInfo = connectFourGame.GetSquareInfo( GetIdentifierAboveLeft( pattern.GetAboveLeftPieceIdentifier() ) );

					if( squareInfo == null || squareInfo.IsOccupied == true )
						return false;
					else
						return true;

				}
				case CONNECTFOURPATTERNDIRECTION.LEFTHORIZONTAL:
				{
					ConnectFourSquareInfo squareInfo = connectFourGame.GetSquareInfo( GetIdentifierToLeft( pattern.GetLeftPieceIdentifier() ) );

					if( squareInfo == null || squareInfo.IsOccupied == true )
						return false;
					else
						return true;
				}
				case CONNECTFOURPATTERNDIRECTION.RIGHTDIAGONAL:
				{
					ConnectFourSquareInfo squareInfo = connectFourGame.GetSquareInfo( GetIdentifierAboveRight( pattern.GetAboveRightPieceIdentifier() ) );

					if( squareInfo == null || squareInfo.IsOccupied == true )
						return false;
					else
						return true;
				}
				case CONNECTFOURPATTERNDIRECTION.RIGHTHORIZONTAL:
				{
					ConnectFourSquareInfo squareInfo = connectFourGame.GetSquareInfo( GetIdentifierToRight( pattern.GetRightPieceIdentifier() ) );

					if( squareInfo == null || squareInfo.IsOccupied == true )
						return false;
					else
						return true;
				}
				case CONNECTFOURPATTERNDIRECTION.VERTICAL:
				{
					ConnectFourSquareInfo squareInfo = connectFourGame.GetSquareInfo( GetIdentifierAbove( pattern.GetAbovePieceIdentifier() ) );

					if( squareInfo == null || squareInfo.IsOccupied == true )
						return false;
					else
						return true;
				}
			}

			return false;
		}

		private void LoadHistoricalPatterns()
		{
			/// will certainly fail the first time run and possibly on occaision thereafter 
			try
			{
				StreamReader reader = new StreamReader( strFileName );
				XmlTextReader xmlReader = new XmlTextReader( reader );

				historicalPatternCollection.Load( xmlReader );

				xmlReader.Close();
			}
			catch( ArgumentNullException argNullExp )
			{
				string strTemp = argNullExp.Message;
			}
			catch( ArgumentException argExp )
			{
				string strTemp = argExp.Message;
			}
			catch( FileNotFoundException fileNFExp )
			{
				string strTemp = fileNFExp.Message;
			}
			catch( DirectoryNotFoundException dirNFExp )
			{
				string strTemp = dirNFExp.Message;
			}
			catch( IOException ioExp )
			{
				string strTemp = ioExp.Message;
			}
			catch( XmlException xmlExp )
			{
				string strTemp = xmlExp.Message;
			}

		}

		/// <summary>
		/// make a random move for the computer when there are no patterns to follow
		/// </summary>
		private bool MakeRandomMove()
		{
			Random rand = new Random( DateTime.Now.Second );

			ArrayList lettersArray = new ArrayList();
			lettersArray.Add( 'A' );
			lettersArray.Add( 'B' );
			lettersArray.Add( 'C' );
			lettersArray.Add( 'D' );
			lettersArray.Add( 'E' );
			lettersArray.Add( 'F' );
			lettersArray.Add( 'G' );

			int nLetter = rand.Next( 0, 6 );

			char letter = ( char )lettersArray[ nLetter ];
			char letter2 = 'A';
			StringBuilder strLetters = new StringBuilder();
			strLetters.Append( letter );
			strLetters.Append( letter2 );

			ConnectFourSquare square = null;
			ConnectFourSquareInfo squareTest = null;
			ConnectFourSquareInfo squareInfo = null;

			squareTest = connectFourGame.GetSquareInfo( strLetters.ToString() );
			squareInfo = squareTest;
			square = ( ConnectFourSquare )GetSquare( squareTest.SquareIdentifier );
			StringBuilder strTest = new StringBuilder();

			bool bFound = false;
			while( bFound == false )
			{
				if( squareTest.IsOccupied == false )
				{
					square = ( ConnectFourSquare )GetSquare( squareTest.SquareIdentifier );
					squareInfo = squareTest;

					strLetters.Remove( 0, strLetters.Length );
					strLetters.Append( squareTest.SquareIdentifier );
					squareTest = connectFourGame.GetSquareInfo( GetIdentifierBelow( strLetters.ToString() ) );

					if( chPreviousMove == strLetters.ToString()[ 0 ] )
					{
						if( rand.Next( 2 ) > 1 )
							continue;
					}

					while( squareTest != null )
					{
						if( squareTest.IsOccupied == false )
						{
							square = ( ConnectFourSquare )GetSquare( squareTest.SquareIdentifier );
							squareInfo = squareTest;

							strTest.Remove( 0, strTest.Length );
							strTest.Append( squareInfo.SquareIdentifier );
							squareTest = connectFourGame.GetSquareInfo( GetIdentifierBelow( strTest.ToString() ) );
						}
						else
							squareTest = null;
										
					}

					if( connectFourGame.PlayerIsRed == true )
					{
						connectFourGame.SetSquare( squareInfo.SquareIdentifier, "BLUE" );
						connectFourGame.OutputText = "Square " + squareInfo.SquareIdentifier + " set to Blue for the Computer"; 
					}
					else
					{
						connectFourGame.SetSquare( squareInfo.SquareIdentifier, "RED" );
						connectFourGame.OutputText = "Square " + squareInfo.SquareIdentifier + " set to Red for the Computer";
					}

					InvalidateSquare( square.Identifier );

					chPreviousMove = square.Identifier[ 0 ];

					bFound = true;
				}
				else
				{
					lettersArray.RemoveAt( nLetter );

					if( lettersArray.Count > 1 )
					{
						nLetter = rand.Next( 0, lettersArray.Count );

						strLetters.Remove( 0, strLetters.Length );
						strLetters.Append( lettersArray[ nLetter ] );
						strLetters.Append( letter2 );

						squareTest = connectFourGame.GetSquareInfo( strLetters.ToString() );
						squareInfo = squareTest;
						square = ( ConnectFourSquare )GetSquare( squareTest.SquareIdentifier );
					}
					else
						return false;
				}
			}

			return true;
		}



		/// <summary>
		/// someone has won
		/// </summary>
		private void OnWin( ConnectFourSquareInfo squareInfo )
		{

			/// if the player won
			/// 

			if( historicalPatternCollection.Count == 0 )
				LoadHistoricalPatterns();

			ConnectFourPatternCollection tempCollection = new ConnectFourPatternCollection();

			/// Get the patterns from the surrounding squares to see
			/// which gets the blame/credit
			/// 

			/// square above is impossible
			/// 
/*
			if( historicalPatternCollection.Count > 0 )
			{
				ConnectFourPattern pattern = historicalPatternCollection.GetStartPatternAt( GetIdentifierAboveRight( squareInfo.SquareIdentifier ) );
				if( pattern != null )
					tempCollection.AddPattern( pattern );

				pattern = historicalPatternCollection.GetStartPatternAt( GetIdentifierAboveRight( GetIdentifierAboveRight( squareInfo.SquareIdentifier ) ) );
				if( pattern != null )
					tempCollection.AddPattern( pattern );

				pattern = historicalPatternCollection.GetStartPatternAt( GetIdentifierAboveRight( GetIdentifierAboveRight( GetIdentifierAboveRight( squareInfo.SquareIdentifier ) ) ) );
				if( pattern != null )
					tempCollection.AddPattern( pattern );

				pattern = historicalPatternCollection.GetStartPatternAt( GetIdentifierToRight( squareInfo.SquareIdentifier ) );
				if( pattern != null )
					tempCollection.AddPattern( pattern );

				pattern = historicalPatternCollection.GetStartPatternAt( GetIdentifierToRight( GetIdentifierToRight( squareInfo.SquareIdentifier ) ) );
				if( pattern != null )
					tempCollection.AddPattern( pattern );

				pattern = historicalPatternCollection.GetStartPatternAt( GetIdentifierToRight( GetIdentifierToRight( GetIdentifierToRight( squareInfo.SquareIdentifier ) ) ) );
				if( pattern != null )
					tempCollection.AddPattern( pattern );

				pattern = historicalPatternCollection.GetStartPatternAt( GetIdentifierBelowRight( squareInfo.SquareIdentifier ) );
				if( pattern != null )
					tempCollection.AddPattern( pattern );

				pattern = historicalPatternCollection.GetStartPatternAt( GetIdentifierBelowRight( GetIdentifierBelowRight( squareInfo.SquareIdentifier ) ) );
				if( pattern != null )
					tempCollection.AddPattern( pattern );

				pattern = historicalPatternCollection.GetStartPatternAt( GetIdentifierBelowRight( GetIdentifierBelowRight( GetIdentifierBelowRight( squareInfo.SquareIdentifier ) ) ) );
				if( pattern != null )
					tempCollection.AddPattern( pattern );

				pattern = historicalPatternCollection.GetStartPatternAt( GetIdentifierBelow( squareInfo.SquareIdentifier ) );
				if( pattern != null )
					tempCollection.AddPattern( pattern );

				pattern = historicalPatternCollection.GetStartPatternAt( GetIdentifierBelow( GetIdentifierBelow( squareInfo.SquareIdentifier ) ) );
				if( pattern != null )
					tempCollection.AddPattern( pattern );

				pattern = historicalPatternCollection.GetStartPatternAt( GetIdentifierBelow( GetIdentifierBelow( GetIdentifierBelow( squareInfo.SquareIdentifier ) ) ) );
				if( pattern != null )
					tempCollection.AddPattern( pattern );

				pattern = historicalPatternCollection.GetStartPatternAt( GetIdentifierBelowLeft( squareInfo.SquareIdentifier ) );
				if( pattern != null )
					tempCollection.AddPattern( pattern );

				pattern = historicalPatternCollection.GetStartPatternAt( GetIdentifierBelowLeft( GetIdentifierBelowLeft( squareInfo.SquareIdentifier ) ) );
				if( pattern != null )
					tempCollection.AddPattern( pattern );

				pattern = historicalPatternCollection.GetStartPatternAt( GetIdentifierBelowLeft( GetIdentifierBelowLeft( GetIdentifierBelowLeft( squareInfo.SquareIdentifier ) ) ) );
				if( pattern != null )
					tempCollection.AddPattern( pattern );

				pattern = historicalPatternCollection.GetStartPatternAt( GetIdentifierToLeft( squareInfo.SquareIdentifier ) );
				if( pattern != null )
					tempCollection.AddPattern( pattern );

				pattern = historicalPatternCollection.GetStartPatternAt( GetIdentifierToLeft( GetIdentifierToLeft( squareInfo.SquareIdentifier ) ) );
				if( pattern != null )
					tempCollection.AddPattern( pattern );

				pattern = historicalPatternCollection.GetStartPatternAt( GetIdentifierAboveLeft( squareInfo.SquareIdentifier ) );
				if( pattern != null )
					tempCollection.AddPattern( pattern );

				pattern = historicalPatternCollection.GetStartPatternAt( GetIdentifierAboveLeft( GetIdentifierAboveLeft( squareInfo.SquareIdentifier ) ) );
				if( pattern != null )
					tempCollection.AddPattern( pattern );

				pattern = historicalPatternCollection.GetStartPatternAt( GetIdentifierAboveLeft( GetIdentifierAboveLeft( GetIdentifierAboveLeft( squareInfo.SquareIdentifier ) ) ) );
				if( pattern != null )
					tempCollection.AddPattern( pattern );
			}

*/
			/// now that it's narrowed down find out which pattern is the one that
			/// won/lost the game
			/// 

			ConnectFourPattern endingPattern = null;
			ArrayList winningSquares = new ArrayList();

			/// get the winning squares
			/// 

			foreach( DictionaryEntry dicEnt in GetHashTable )
			{
				ConnectFourSquare square = ( ConnectFourSquare )dicEnt.Value;
				if( square.IsWinningSquare == true )
				{
					winningSquares.Add( square );
				}
			}


			/// now find the pattern that contains the squares
			/// 

			for( int i=0; i<tempCollection.Count; i++ )
			{
				ConnectFourPattern pattern = ( ConnectFourPattern )tempCollection.Patterns[ i ];
				if( pattern.ContainsThreeOf( ( ( BasicSquare )winningSquares[ 0 ] ).Identifier, ( ( BasicSquare )winningSquares[ 1 ] ).Identifier, ( ( BasicSquare )winningSquares[ 2 ] ).Identifier, ( ( BasicSquare )winningSquares[ 3 ] ).Identifier ) == true )
				{
					endingPattern = pattern;
					break;
				}
			}

			/// not a historical pattern
			/// 
/*
			if( endingPattern == null )
			{
				ConnectFourPattern pattern = patternCollection.GetStartPatternAt( GetIdentifierAboveRight( squareInfo.SquareIdentifier ) );
				if( pattern != null )
					tempCollection.AddPattern( pattern );

				pattern = patternCollection.GetStartPatternAt( GetIdentifierAboveRight( GetIdentifierAboveRight( squareInfo.SquareIdentifier ) ) );
				if( pattern != null )
					tempCollection.AddPattern( pattern );

				pattern = patternCollection.GetStartPatternAt( GetIdentifierAboveRight( GetIdentifierAboveRight( GetIdentifierAboveRight( squareInfo.SquareIdentifier ) ) ) );
				if( pattern != null )
					tempCollection.AddPattern( pattern );

				pattern = patternCollection.GetStartPatternAt( GetIdentifierToRight( squareInfo.SquareIdentifier ) );
				if( pattern != null )
					tempCollection.AddPattern( pattern );

				pattern = patternCollection.GetStartPatternAt( GetIdentifierToRight( GetIdentifierToRight( squareInfo.SquareIdentifier ) ) );
				if( pattern != null )
					tempCollection.AddPattern( pattern );

				pattern = patternCollection.GetStartPatternAt( GetIdentifierToRight( GetIdentifierToRight( GetIdentifierToRight( squareInfo.SquareIdentifier ) ) ) );
				if( pattern != null )
					tempCollection.AddPattern( pattern );

				pattern = patternCollection.GetStartPatternAt( GetIdentifierBelowRight( squareInfo.SquareIdentifier ) );
				if( pattern != null )
					tempCollection.AddPattern( pattern );

				pattern = patternCollection.GetStartPatternAt( GetIdentifierBelowRight( GetIdentifierBelowRight( squareInfo.SquareIdentifier ) ) );
				if( pattern != null )
					tempCollection.AddPattern( pattern );

				pattern = patternCollection.GetStartPatternAt( GetIdentifierBelowRight( GetIdentifierBelowRight( GetIdentifierBelowRight( squareInfo.SquareIdentifier ) ) ) );
				if( pattern != null )
					tempCollection.AddPattern( pattern );

				pattern = patternCollection.GetStartPatternAt( GetIdentifierBelow( squareInfo.SquareIdentifier ) );
				if( pattern != null )
					tempCollection.AddPattern( pattern );

				pattern = patternCollection.GetStartPatternAt( GetIdentifierBelow( GetIdentifierBelow( squareInfo.SquareIdentifier ) ) );
				if( pattern != null )
					tempCollection.AddPattern( pattern );

				pattern = patternCollection.GetStartPatternAt( GetIdentifierBelow( GetIdentifierBelow( GetIdentifierBelow( squareInfo.SquareIdentifier ) ) ) );
				if( pattern != null )
					tempCollection.AddPattern( pattern );

				pattern = patternCollection.GetStartPatternAt( GetIdentifierBelowLeft( squareInfo.SquareIdentifier ) );
				if( pattern != null )
					tempCollection.AddPattern( pattern );

				pattern = patternCollection.GetStartPatternAt( GetIdentifierBelowLeft( GetIdentifierBelowLeft( squareInfo.SquareIdentifier ) ) );
				if( pattern != null )
					tempCollection.AddPattern( pattern );

				pattern = patternCollection.GetStartPatternAt( GetIdentifierBelowLeft( GetIdentifierBelowLeft( GetIdentifierBelowLeft( squareInfo.SquareIdentifier ) ) ) );
				if( pattern != null )
					tempCollection.AddPattern( pattern );

				pattern = patternCollection.GetStartPatternAt( GetIdentifierToLeft( squareInfo.SquareIdentifier ) );
				if( pattern != null )
					tempCollection.AddPattern( pattern );

				pattern = patternCollection.GetStartPatternAt( GetIdentifierToLeft( GetIdentifierToLeft( squareInfo.SquareIdentifier ) ) );
				if( pattern != null )
					tempCollection.AddPattern( pattern );

				pattern = patternCollection.GetStartPatternAt( GetIdentifierToLeft( GetIdentifierToLeft( GetIdentifierToLeft( squareInfo.SquareIdentifier ) ) ) );
				if( pattern != null )
					tempCollection.AddPattern( pattern );

				pattern = patternCollection.GetStartPatternAt( GetIdentifierAboveLeft( squareInfo.SquareIdentifier ) );
				if( pattern != null )
					tempCollection.AddPattern( pattern );

				pattern = patternCollection.GetStartPatternAt( GetIdentifierAboveLeft( GetIdentifierAboveLeft( squareInfo.SquareIdentifier ) ) );
				if( pattern != null )
					tempCollection.AddPattern( pattern );

				pattern = patternCollection.GetStartPatternAt( GetIdentifierAboveLeft( GetIdentifierAboveLeft( GetIdentifierAboveLeft( squareInfo.SquareIdentifier ) ) ) );
				if( pattern != null )
					tempCollection.AddPattern( pattern );


				/// now find the pattern that contains the squares
				/// 

				for( int i=0; i<tempCollection.Count; i++ )
				{
					pattern = ( ConnectFourPattern )tempCollection.Patterns[ i ];
					if( pattern.ContainsThreeOf( ( ( BasicSquare )winningSquares[ 0 ] ).Identifier, ( ( BasicSquare )winningSquares[ 1 ] ).Identifier, ( ( BasicSquare )winningSquares[ 2 ] ).Identifier, ( ( BasicSquare )winningSquares[ 3 ] ).Identifier ) == true )
					{
						endingPattern = pattern;
						break;
					}
				}
				if( endingPattern == null )
				{
			///		MessageBox.Show( "Error ending pattern is screwed, get the debugger ready" );
			///		return;
				}
			}
*/
			for( int i=0; i<tempCollection.Count; i++ )
			{
				( ( ConnectFourPattern )tempCollection.Patterns[ i ] ).IsEndingPattern = true;
			}

			if( connectFourGame.HasPlayerWon == true )
			{

				for( int i=0; i<patternCollection.Count; i++ )
				{
					( ( ConnectFourPattern )patternCollection.Patterns[ i ] ).IsLosingPattern = true;
					if( historicalPatternCollection.IsIn( ( ConnectFourPattern )patternCollection.Patterns[ i ] ) == true )
					{
						/// update the pattern
						///
						ConnectFourPattern pattern = historicalPatternCollection.GetPattern( ( ConnectFourPattern )patternCollection.Patterns[ i ] );
						pattern.NumberOfTimesSeen++;
						pattern.NumberOfTimesSeenInLosingGame++;

						///historicalPatternCollection.UpdatePattern( ( ConnectFourPattern )patternCollection.Patterns[ i ] );
					}
					else
					{
						( ( ConnectFourPattern )patternCollection.Patterns[ i ] ).NumberOfTimesSeen++;
						( ( ConnectFourPattern )patternCollection.Patterns[ i ] ).NumberOfTimesSeenInLosingGame++;
						historicalPatternCollection.AddPattern( ( ConnectFourPattern )patternCollection.Patterns[ i ] );
					}
				}

				if( endingPattern != null )
					endingPattern.IsLosingPattern = true;
			}
			else /// if the computer won
			{
				for( int i=0; i<patternCollection.Count; i++ )
				{
					( ( ConnectFourPattern )patternCollection.Patterns[ i ] ).IsWinningPattern = true;
					if( historicalPatternCollection.IsIn( ( ConnectFourPattern )patternCollection.Patterns[ i ] ) == true )
					{
						/// update the pattern
						///

						ConnectFourPattern pattern = historicalPatternCollection.GetPattern( ( ConnectFourPattern )patternCollection.Patterns[ i ] );
						pattern.NumberOfTimesSeen++;
						pattern.NumberOfTimesSeenInWinningGame++;
					
						///	historicalPatternCollection.UpdatePattern( ( ConnectFourPattern )patternCollection.Patterns[ i ] );
					}
					else
					{
						( ( ConnectFourPattern )patternCollection.Patterns[ i ] ).NumberOfTimesSeen++;
						( ( ConnectFourPattern )patternCollection.Patterns[ i ] ).NumberOfTimesSeenInWinningGame++;
						historicalPatternCollection.AddPattern( ( ConnectFourPattern )patternCollection.Patterns[ i ] );
					}
				}
				
				if( endingPattern != null )
					endingPattern.IsWinningPattern = true;
			}

			SavePatterns();


			/// put it back
			/// 
			squareInfo.IsOccupied = true;
		}

	}
}

