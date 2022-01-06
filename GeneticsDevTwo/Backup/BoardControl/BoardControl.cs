using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.Text;

namespace BoardControl
{
	public enum SQUAREMODES{ DISPLAYONLY, CONNECTFOUR, APPLICATION };

	/// <summary>
	/// BoardControl is a user control that draws squares for games and life simulators
	/// </summary>
	public class Board : System.Windows.Forms.UserControl
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		/// <summary>
		/// Initialise the board
		/// used by inherited classes so they know to draw their own boards
		/// </summary>
		private bool bInitializeBoard;
		/// <summary>
		/// board width
		/// </summary>
		private int nBoardWidth;
		/// <summary>
		/// board height
		/// </summary>
		private int nBoardHeight;
		/// <summary>
		/// the number of hrizontal squares
		/// </summary>
		private int nHorizontalSquares;
		/// <summary>
		/// the number of vertical squares
		/// </summary>
		private int nVerticalSquares;
		/// <summary>
		/// Square width
		/// </summary>
		private int nSquareWidth;
		/// <summary>
		/// square height
		/// </summary>
		private int nSquareHeight;
		/// <summary>
		/// game types
		/// </summary>
		private SQUAREMODES mode;
		/// <summary>
		/// show the board legend
		/// </summary>
		private bool bShowLegend;
		/// <summary>
		/// colour of the legend
		/// </summary>
		private Color cLegendColor;
		/// <summary>
		/// size of the legend
		/// </summary>
		private int nLegendWidth;
		/// <summary>
		/// variable set so that I can decide which paint calls I want to debug
		/// </summary>
		private bool bTestPaint;
		/// <summary>
		/// Does this app use drag n drop?
		/// </summary>
		private bool bImplementDragDrop;
		/// <summary>
		/// Bitmap image for drag n drop operations
		/// </summary>
		private Bitmap bitDragDropImage;
		/// <summary>
		/// image to be displayed when dragging an object over the board
		/// </summary>
		private Bitmap bitDragOverImage;
		/// <summary>
		/// image to be displayed for a square that cannot be dropped to
		/// </summary>
		private Bitmap bitDragNoDropImage;
		/// <summary>
		/// The square that the drag an drop operation is started from
		/// </summary>
		private BasicSquare bsDragDropFrom;


		/// <summary>
		/// private hash table for storing the square information
		/// </summary>
		private Hashtable hashSquares;

		public bool IsBoardInitialized()
		{
			return bInitializeBoard;
		}

		public void SetBoardInitialized()
		{
			bInitializeBoard = true;
		}

		public int BoardWidth
		{
			get
			{
				return nBoardWidth;
			}
			set
			{
				nBoardWidth = value;
			}
		}

		public int BoardHeight
		{
			get
			{
				return nBoardHeight;
			}
			set
			{
				nBoardHeight = value;
			}
		}


		public int HorizontalSquares
		{
			get
			{
				return nHorizontalSquares;
			}
			set
			{
				nHorizontalSquares = value;
			}
		}

		public int VerticalSquares
		{
			get
			{
				return nVerticalSquares;
			}
			set
			{
				nVerticalSquares = value;
			}
		}

		public int SquareWidth 
		{
			get
			{
				return nSquareWidth;
			}
			set
			{
				nSquareWidth = value;
			}
		}

		public int SquareHeight
		{
			get
			{
				return nSquareHeight;
			}
			set
			{
				nSquareHeight = value;
			}
		}

		public bool ShowLegend
		{
			get
			{
				return bShowLegend;
			}
			set
			{
				bShowLegend = value;
			}
		}

		public Color LegendColor
		{
			get
			{
				return cLegendColor;
			}
			set
			{
				cLegendColor = value;

				if( hashSquares != null )
				{
					foreach( DictionaryEntry dicEnt in hashSquares )
					{
						BasicSquare square = ( BasicSquare )dicEnt.Value;
						square.LegendColor = cLegendColor;
					}

					this.Invalidate();
					this.Update();
				}
			}
		}

		public int LegendWidth
		{
			get
			{
				return nLegendWidth;
			}
			set
			{
				nLegendWidth = value;
			}
		}

		public bool TestPaint
		{
			get
			{
				return bTestPaint;
			}
			set
			{
				bTestPaint = value;
			}
		}

		public Hashtable GetHashTable
		{
			get
			{
				return hashSquares;
			}
		}

		public void SetDisplayMode( string strDisplayMode )
		{
			switch( strDisplayMode )
			{
				case "APPLICATION": mode = SQUAREMODES.APPLICATION; break;
				case "CONNECTFOUR": mode =	SQUAREMODES.CONNECTFOUR; break;
				default : mode = SQUAREMODES.DISPLAYONLY; break;
			}
		}

		public bool ImplementDragDrop
		{
			get
			{
				return bImplementDragDrop;
			}
			set
			{
				bImplementDragDrop = value;

				this.AllowDrop = bImplementDragDrop;
			}
		}

		public Bitmap DragDropImage
		{
			get
			{
				return bitDragDropImage;
			}
			set
			{
				bitDragDropImage = value;
			}
		}

		public Bitmap DragOverImage
		{
			get
			{
				return bitDragOverImage;
			}
			set
			{
				bitDragOverImage = value;
			}
		}

		public Bitmap DragNoDropImage
		{
			get
			{
				return bitDragNoDropImage;
			}
			set
			{
				bitDragNoDropImage = value;
			}
		}

		public BasicSquare DragDropFrom
		{
			get
			{
				if( ImplementDragDrop == true )
					return bsDragDropFrom;
				
				return null;
			}
			set
			{
				if( ImplementDragDrop == true )
					bsDragDropFrom = value;
				else
					bsDragDropFrom = null;
			}
		}


		public Board()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			// _TODO: Add any initialization after the InitComponent call
			bInitializeBoard = false;
			BoardWidth = 0;
			BoardHeight = 0;
			HorizontalSquares = 0;
			VerticalSquares = 0;
			SquareWidth = 0;
			SquareHeight = 0;
			ShowLegend = true;
			LegendColor = Color.LightBlue;
			LegendWidth = 10;

			mode = SQUAREMODES.DISPLAYONLY;

			hashSquares = new Hashtable();

			TestPaint = false;

			SetStyle( ControlStyles.UserPaint, true ); 
			SetStyle( ControlStyles.AllPaintingInWmPaint, true ); 
			SetStyle( ControlStyles.DoubleBuffer, true ); 

			ImplementDragDrop = true;
			DragDropImage = null;
			DragOverImage = null;
			bsDragDropFrom = null;
		}

		/// <summary>
		/// remove all the squares from the hashtable
		/// </summary>
		public void Clear()
		{
			hashSquares.Clear();
		}
			

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if( components != null )
					components.Dispose();
			}
			base.Dispose( disposing );
		}

		/// <summary>
		/// Quick little function from Tomaz Stih
		/// use if( Designing() == true )
		///		return
		///	before you do anything tricky as it will prevent developer studio
		///	from deleting the user controls if something fails to load or 
		///	initialize correctly. 
		/// </summary>
		/// <returns></returns>
		protected bool Designing() 
		{
			string exePath = Application.ExecutablePath; 
			exePath = exePath.ToLower(); 
			if(Application.ExecutablePath.ToLower().IndexOf("devenv.exe") > -1) 
				return true; 
			else
				return false;
		}


		#region Component Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			// 
			// Board
			// 
			this.Name = "Board";
			this.GiveFeedback += new System.Windows.Forms.GiveFeedbackEventHandler(this.OnGiveFeedBack);
			this.DragEnter += new System.Windows.Forms.DragEventHandler(this.OnDragEnter);
			this.Paint += new System.Windows.Forms.PaintEventHandler(this.OnPaint);
			this.DragLeave += new System.EventHandler(this.OnDragLeave);
			this.DragDrop += new System.Windows.Forms.DragEventHandler(this.OnDragDrop);
			this.DragOver += new System.Windows.Forms.DragEventHandler(this.OnDragOver);
			this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnMouseDown);

		}
		#endregion

		private void OnPaint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
			Graphics grfx = e.Graphics;

			if( mode == SQUAREMODES.DISPLAYONLY )
			{
				/// this code just gives something to look at when the control is dropped onto a 
				/// form the control should be set up programmatically by the container program 
				/// which will then select modes etc to use.
				Clear();

				/// create some squares 
				
				hashSquares.Add( "AA", new ControlDisplaySquare( 100, 100, 0, 0, "AA" ) );
				hashSquares.Add( "AB", new ControlDisplaySquare( 100, 100, 0, 100, "AB" ) );
				hashSquares.Add( "AC", new ControlDisplaySquare( 100, 100, 0, 200, "AC" ) );
				hashSquares.Add( "BA", new ControlDisplaySquare( 100, 100, 100, 0, "BA" ) );
				hashSquares.Add( "BB", new ControlDisplaySquare( 100, 100, 100, 100, "BB" ) );
				hashSquares.Add( "BC", new ControlDisplaySquare( 100, 100, 100, 200, "BC" ) );
				hashSquares.Add( "CA", new ControlDisplaySquare( 100, 100, 200, 0, "CA" ) );
				hashSquares.Add( "CB", new ControlDisplaySquare( 100, 100, 200, 100, "CB" ) );
				hashSquares.Add( "CC", new ControlDisplaySquare( 100, 100, 200, 200, "CC" ) );

				( ( BasicSquare )hashSquares[ "AA" ] ).DrawLegend = true;
				( ( BasicSquare )hashSquares[ "AC" ] ).IsBottomSquare = true;
				( ( BasicSquare )hashSquares[ "BC" ] ).IsBottomSquare = true;
				( ( BasicSquare )hashSquares[ "CA" ] ).IsRightEdgeSquare = true;
				( ( BasicSquare )hashSquares[ "CB" ] ).IsRightEdgeSquare = true;
				( ( BasicSquare )hashSquares[ "CC" ] ).IsRightEdgeSquare = true;
				( ( BasicSquare )hashSquares[ "CC" ] ).IsBottomSquare = true;

				foreach( DictionaryEntry dicEnt in hashSquares )
				{
					BasicSquare square = ( BasicSquare )dicEnt.Value;

					square.DrawSquare( e.Graphics );
				}

			}
			else /// draw the board
			{
				/// assume that the calls to paint are only made when necassary so draw all squares
				/// this avoids having to call in from the container ( hopefully )
				
				if( e.ClipRectangle.Width >= BoardWidth )
				{
					foreach( DictionaryEntry dicEnt in hashSquares )
					{
						BasicSquare square = ( BasicSquare )dicEnt.Value;

						square.IsValid = false;
					}
				}
				else
				{
					/// if not drawing the complet board work out which squares need drawing
					/// NOTE maths is slightly out if using the legend ( but not enough to make much difference )
					foreach( DictionaryEntry dicEnt in hashSquares )
					{
						BasicSquare square = ( BasicSquare )dicEnt.Value;

						if( e.ClipRectangle.Width >= square.SquareHorizontalLocation || 
							e.ClipRectangle.Height >= square.SquareVerticalLocation )
						{
							square.IsValid = false;
						}
					}
				}
			}
		}

		/// <summary>
		/// get a square given the passed in identifier
		/// </summary>
		/// <param name="strSquareIdentifier"></param>
		/// <returns></returns>
		public BasicSquare GetSquare( string squareIdentifier )
		{
			if( squareIdentifier == null )
				return null;

			return ( BasicSquare )hashSquares[ squareIdentifier ];
		}

	
		/// <summary>
		/// Get square to the right of the square with the passed in identifier 
		/// </summary>
		/// <param name="squareIdentifier">current square ie "AF"</param>
		/// <returns>either the square to the right or null</returns>
		public BasicSquare GetSquareToRight( string squareIdentifier )
		{
			StringBuilder strString = new StringBuilder( squareIdentifier );
			
			char firstChar = strString[ 0 ];

			firstChar++;

			strString[ 0 ] = firstChar;

			return ( BasicSquare )this.hashSquares[ strString.ToString() ];

		}

		/// <summary>
		/// as above but returns the identifier
		/// </summary>
		/// <param name="squareIdentifier"></param>
		/// <returns></returns>
		public string GetIdentifierToRight( string squareIdentifier )
		{
			StringBuilder strString = new StringBuilder( squareIdentifier );

			char firstChar = strString[ 0 ];

			firstChar++;

			strString[ 0 ] = firstChar;

			return strString.ToString();
		}

		/// <summary>
		/// Get square to the left of the square with the passed in identifier
		/// </summary>
		/// <param name="squareIdentifier">current square ie "AF"</param>
		/// <returns>either the square to the left or null</returns>
		public BasicSquare GetSquareToLeft( string squareIdentifier )
		{
			StringBuilder strString = new StringBuilder( squareIdentifier );

			char firstChar = strString[ 0 ];

			firstChar--;

			strString[ 0 ] = firstChar;

			return ( BasicSquare )hashSquares[ strString.ToString() ];
		}

		public string GetIdentifierToLeft( string squareIdentifier )
		{
			StringBuilder strString = new StringBuilder( squareIdentifier );

			char firstChar = strString[ 0 ];

			firstChar--;

			strString[ 0 ] = firstChar;

			return strString.ToString();
		}

		/// <summary>
		/// Get square above the square with the passed identifier
		/// </summary>
		/// <param name="squareIdentifier">current square ie "AF"</param>
		/// <returns>either the square above or null</returns>
		public BasicSquare GetSquareAbove( string squareIdentifier )
		{
			StringBuilder strString = new StringBuilder( squareIdentifier );

			char firstChar = strString[ 1 ];

			firstChar++;

			strString[ 1 ] = firstChar;

			return ( BasicSquare )hashSquares[ strString.ToString() ];
		}

		public string GetIdentifierAbove( string squareIdentifier )
		{
			StringBuilder strString = new StringBuilder( squareIdentifier );

			char firstChar = strString[ 1 ];

			firstChar++;

			strString[ 1 ] = firstChar;

			return strString.ToString();
		}

		/// <summary>
		/// Get square to the above left of the square with the passed identifier
		/// </summary>
		/// <param name="squareIdentifier">current square ie "AF"</param>
		/// <returns>either the square above left of the current square or null</returns>
		public BasicSquare GetSquareAboveLeft( string squareIdentifier )
		{
			StringBuilder strString = new StringBuilder( squareIdentifier );

			char firstChar = strString[ 0 ];
			char secondChar = strString[ 1 ];

			firstChar--;
			secondChar++;

			strString[ 0 ] = firstChar;
			strString[ 1 ] = secondChar;

			return ( BasicSquare )hashSquares[ strString.ToString() ];
		}

		public string GetIdentifierAboveLeft( string squareIdentifier )
		{
			StringBuilder strString = new StringBuilder( squareIdentifier );

			char firstChar = strString[ 0 ];
			char secondChar = strString[ 1 ];

			firstChar--;
			secondChar++;

			strString[ 0 ] = firstChar;
			strString[ 1 ] = secondChar;

			return strString.ToString();
		}

		/// <summary>
		/// Get square to the above right of the square with the passed in identifier
		/// </summary>
		/// <param name="squareIdentifier">current square ie "AF"</param>
		/// <returns>either the square above right of the current square or null</returns>
		public BasicSquare GetSquareAboveRight( string squareIdentifier )
		{
			StringBuilder strString = new StringBuilder( squareIdentifier );

			char firstChar = strString[ 0 ];
			char secondChar = strString[ 1 ];

			firstChar++;
			secondChar++;

			strString[ 0 ] = firstChar;
			strString[ 1 ] = secondChar;

			return ( BasicSquare )hashSquares[ strString.ToString() ];
		}

		public string GetIdentifierAboveRight( string squareIdentifier )
		{
			StringBuilder strString = new StringBuilder( squareIdentifier );

			char firstChar = strString[ 0 ];
			char secondChar = strString[ 1 ];

			firstChar++;
			secondChar++;

			strString[ 0 ] = firstChar;
			strString[ 1 ] = secondChar;

			return strString.ToString();
		}

		/// <summary>
		/// Get the square below the square with the passed in identifier
		/// </summary>
		/// <param name="squareIdentifier">current square ie "AF"</param>
		/// <returns>either ther square below the current square or null</returns>
		public BasicSquare GetSquareBelow( string squareIdentifier )
		{
			StringBuilder strString = new StringBuilder( squareIdentifier );

			char firstChar = strString[ 1 ];

			firstChar--;

			strString[ 1 ] = firstChar;

			return ( BasicSquare )hashSquares[ strString.ToString() ];
		}

		public string GetIdentifierBelow( string squareIdentifier )
		{
			StringBuilder strString = new StringBuilder( squareIdentifier );
			
			char firstChar = strString[ 1 ];

			firstChar--;

			strString[ 1 ] = firstChar;

			return strString.ToString();
		}

		/// <summary>
		/// Get the square to the below left of the square with the passed in identifier
		/// </summary>
		/// <param name="squareIdentifier">current square ie "AF"</param>
		/// <returns>either the square below left of the square with the passed in identifier or null</returns>
		public BasicSquare GetSquareBelowLeft( string squareIdentifier )
		{
			StringBuilder strString = new StringBuilder( squareIdentifier );

			char firstChar = strString[ 0 ];
			char secondChar = strString[ 1 ];

			firstChar++;
			secondChar++;

			strString[ 0 ] = firstChar;
			strString[ 1 ] = secondChar;

			return ( BasicSquare )hashSquares[ strString.ToString() ];
		}

		public string GetIdentifierBelowLeft( string squareIdentifier )
		{
			StringBuilder strString = new StringBuilder( squareIdentifier );

			char firstChar = strString[ 0 ];
			char secondChar = strString[ 1 ];

			firstChar++;
			secondChar++;

			strString[ 0 ] = firstChar;
			strString[ 1 ] = secondChar;

			return strString.ToString();
		}

		/// <summary>
		/// GEt the square to the below right of the square with the passed identifier
		/// </summary>
		/// <param name="squareIdentifier">square identifier ie "AF"</param>
		/// <returns>either the square below left of the square with the passed in identifier or null</returns>
		public BasicSquare GetSquareBelowRight( string squareIdentifier )
		{
			StringBuilder strString = new StringBuilder( squareIdentifier );

			char firstChar = strString[ 0 ];
			char secondChar = strString[ 1 ];

			firstChar--;
			secondChar++;

			strString[ 0 ] = firstChar;
			strString[ 1 ] = secondChar;

			return ( BasicSquare )hashSquares[ strString.ToString() ];
		}

		public string GetIdentifierBelowRight( string squareIdentifier )
		{
			StringBuilder strString = new StringBuilder( squareIdentifier );

			char firstChar = strString[ 0 ];
			char secondChar = strString[ 1 ];

			firstChar--;
			secondChar++;

			strString[ 0 ] = firstChar;
			strString[ 1 ] = secondChar;

			return strString.ToString();
		}


		/// <summary>
		/// invalidate a given square
		/// </summary>
		/// <param name="strIdentifier">hash table identifier for the string</param>
		public void InvalidateSquare( string identifier )
		{
			BasicSquare square = ( BasicSquare )hashSquares[ identifier ];

			if( square == null )
				return;

			if( ShowLegend == false )
				Invalidate( new Rectangle( square.SquareHorizontalLocation, square.SquareVerticalLocation, square.SquareWidth, square.SquareHeight ) ); 
			else
				Invalidate( new Rectangle( square.SquareHorizontalLocation + LegendWidth, square.SquareVerticalLocation + LegendWidth, square.SquareWidth, square.SquareHeight ) ); 

			square.IsValid = false;
		}

		/// <summary>
		/// get a square on the board given the width and height
		/// </summary>
		/// <param name="width">horizontal position of the square on the board</param>
		/// <param name="height">vertical position of the square on the board</param>
		/// <returns></returns>
		public BasicSquare GetSquareAt( int width, int height )
		{
			foreach( DictionaryEntry dicEnt in hashSquares )
			{
				BasicSquare square = ( BasicSquare )dicEnt.Value;

				if( this.ShowLegend == false )
				{
					if( square.SquareHorizontalLocation <= width &&
						( square.SquareHorizontalLocation + square.SquareWidth ) >= width &&
						square.SquareVerticalLocation <= height &&
						( square.SquareVerticalLocation + square.SquareHeight ) >= height )
					{
						return square;
					}
				}
				else
				{
					if( ( square.SquareHorizontalLocation /*+ this.LegendWidth*/ ) <= width &&
						( square.SquareHorizontalLocation /*+ LegendWidth */ + square.SquareWidth ) >= width &&
						( square.SquareVerticalLocation /*+ LegendWidth*/ ) <= height &&
						( square.SquareVerticalLocation /*+ LegendWidth*/ + square.SquareHeight ) >= height )
					{
						return square;
					}
				}
			}

			return null;
		}

		public BasicSquare GetSquareAt( Point point )
		{
			foreach( DictionaryEntry dicEnt in hashSquares )
			{
				BasicSquare square = ( BasicSquare )dicEnt.Value;

				if( this.ShowLegend == false )
				{
					if( square.SquareHorizontalLocation <= point.Y && 
						( square.SquareHorizontalLocation + square.SquareWidth ) >= point.Y &&
						square.SquareVerticalLocation <= point.X &&
						( square.SquareVerticalLocation + square.SquareHeight ) >= point.X )
					{
						return square;
					}
				}
				else
				{
					if( ( square.SquareHorizontalLocation + LegendWidth ) <= point.Y &&
						( square.SquareHorizontalLocation + LegendWidth + square.SquareWidth ) >= point.Y && 
						( square.SquareVerticalLocation + LegendWidth ) <= point.X && 
						( square.SquareVerticalLocation + LegendWidth + square.SquareHeight ) >= point.X )
					{
						return square;
					}
				}
			}

			return null;
		}

		/// <summary>
		/// Set all the sqaures to the same background color
		/// </summary>
		/// <param name="color"></param>
		public void SetBackGroundColor( Color color )
		{
			SetBackGroundColor( color, color );
		}


		/// <summary>
		/// Set alternating background colors unless called from
		/// SetBackGroundColor( Color color )
		/// Which will mean all squares are the same color
		/// </summary>
		/// <param name="colorOne"></param>
		/// <param name="colorTwo"></param>
		public virtual void SetBackGroundColor( Color colorOne, Color colorTwo )
		{
			char firstChar = 'A';
			char secondChar = 'A';

			bool bFirstColor = true;

			StringBuilder strString = new StringBuilder();
			strString.Append( firstChar );
			strString.Append( secondChar );

			BasicSquare squareTest = ( BasicSquare )GetHashTable[ strString.ToString() ];

			while( squareTest != null )
			{
				if( bFirstColor == true )
				{
					squareTest.BackGroundColor = colorOne;
					bFirstColor = false;
				}
				else
				{
					squareTest.BackGroundColor = colorTwo;
					bFirstColor = true;
				}

				firstChar++;

				strString.Remove( 0, strString.Length );
				strString.Append( firstChar );
				strString.Append( secondChar );
				squareTest = ( BasicSquare )GetHashTable[ strString.ToString() ];

				if( squareTest == null )
				{
					firstChar = 'A';
					secondChar++;
					strString.Remove( 0, strString.Length );
					strString.Append( firstChar );
					strString.Append( secondChar );
					squareTest = ( BasicSquare )GetHashTable[ strString.ToString() ];

					if( bFirstColor == true )
						bFirstColor = false;
					else
						bFirstColor = true;
				}
			}
		}


		/// <summary>
		/// Draw the Legend
		/// </summary>
		/// <param name="drawLegend"></param>
		public virtual void DrawLegend( bool drawLegend )
		{
			if( GetHashTable[ "AA" ] != null )
				( ( BasicSquare )GetHashTable[ "AA" ] ).DrawLegend = drawLegend;

			SetEdges();
		}

		public virtual void SetHighlightColor( Color color )
		{
			if( GetHashTable[ "AA" ] != null )
				( ( BasicSquare )GetHashTable[ "AA" ] ).HighlightColor = color;
		}

		/// <summary>
		/// Set the edges for drawing the legends
		/// </summary>
		protected virtual void SetEdges()
		{
			char firstChar = 'A';
			char secondChar = 'A';

			StringBuilder strString = new StringBuilder();
			strString.Append( firstChar );
			strString.Append( secondChar );

			BasicSquare squareTest = ( BasicSquare )GetHashTable[ strString.ToString() ];

			while( squareTest != null )
			{
				firstChar++;

				strString.Remove( 0, strString.Length );
				strString.Append( firstChar );
				strString.Append( secondChar );
				squareTest = ( BasicSquare )GetHashTable[ strString.ToString() ];

				if( squareTest == null )
				{
					firstChar--;
					strString.Remove( 0, strString.Length );
					strString.Append( firstChar );
					strString.Append( secondChar );
					squareTest = ( BasicSquare )GetHashTable[ strString.ToString() ];

					squareTest.IsRightEdgeSquare = true;

					firstChar = 'A';
					secondChar++;
					strString.Remove( 0, strString.Length );
					strString.Append( firstChar );
					strString.Append( secondChar );
					squareTest = ( BasicSquare )GetHashTable[ strString.ToString() ];
				}
			}

			firstChar = 'A';
			secondChar--;
			strString.Remove( 0, strString.Length );
			strString.Append( firstChar );
			strString.Append( secondChar );

			squareTest = ( BasicSquare )GetHashTable[ strString.ToString() ];

			while( squareTest != null )
			{
				squareTest.IsBottomSquare = true;

				firstChar++;

				strString.Remove( 0, strString.Length );
				strString.Append( firstChar );
				strString.Append( secondChar );

				squareTest = ( BasicSquare )GetHashTable[ strString.ToString() ];
			}
		}

		private void OnDragDrop(object sender, System.Windows.Forms.DragEventArgs e)
		{
		}

		private void OnDragEnter(object sender, System.Windows.Forms.DragEventArgs e)
		{
		}

		private void OnDragLeave(object sender, System.EventArgs e)
		{
		}

		private void OnGiveFeedBack(object sender, System.Windows.Forms.GiveFeedbackEventArgs e)
		{
		}

		private void OnMouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
		}

		private void OnDragOver(object sender, System.Windows.Forms.DragEventArgs e)
		{
		}

		public void ClearBoard()
		{
			foreach( DictionaryEntry dicEnt in GetHashTable )
			{
				BasicSquare square = ( BasicSquare )dicEnt.Value;

				square.IsOccupied = false;
				square.IsValid = false;
				square.OccupyingName = "EMPTY";
			}

			Invalidate();
			Update();
		}
	}
}
