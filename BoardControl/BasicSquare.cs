using System;
using System.Drawing;

namespace BoardControl
{
	/// <summary>
	/// Summary description for BasicSquare.
	/// </summary>
	public abstract class BasicSquare
	{
		/// <summary>
		/// Horizontal starting location for drawing a square
		/// </summary>
		private int nSquareHorizontalLocation;
		/// <summary>
		/// vertical starting location for drawing a square
		/// </summary>
		private int nSquareVerticalLocation;
		/// <summary>
		/// width of the square
		/// </summary>
		private static int nSquareWidth;
		/// <summary>
		/// height of the square
		/// </summary>
		private static int nSquareHeight;
		/// <summary>
		/// color of the square
		/// </summary>
		private Color backGroundColor;
		/// <summary>
		/// should a border be drawn
		/// </summary>
		private static bool bDrawBorder;
		/// <summary>
		/// color of the borders
		/// </summary>
		private static Color borderColor;
		/// <summary>
		/// pen for drawing the borders
		/// </summary>
		private static Pen borderPen = null;
		/// <summary>
		/// brush for painting the background
		/// </summary>
		private SolidBrush backGroundBrush = null;
		/// <summary>
		/// does the current square require drawing
		/// </summary>
		private bool bIsValid;
		/// <summary>
		/// draw the legend
		/// </summary>
		private static bool bDrawLegend = false;
		/// <summary>
		/// the width of the legend
		/// </summary>
		private static int nLegendWidth = 10;
		/// <summary>
		/// color of the legend
		/// </summary>
		private static Color cLegendColor;
		/// <summary>
		/// brush for drawing the legend
		/// </summary>
		private static SolidBrush legendBrush = null;
		/// <summary>
		/// Is the square on the right edge
		/// Needs to be set to draw the legend
		/// </summary>
		private bool bIsRightEdgeSquare;
		/// <summary>
		/// is the square a bottom square 
		/// needs to be set to draw the legend
		/// </summary>
		private bool bIsBottomSquare;
		/// <summary>
		/// the identifier used to store the square in the hash table
		/// </summary>
		private string strIdentifier;
		/// <summary>
		/// Is the square Occupied
		/// </summary>
		private bool bIsOccupied;
		/// <summary>
		/// the name of whatever is occupying the square
		/// for connect four the colour of the piece
		/// </summary>
		private string strOccupyingName;
		/// <summary>
		/// bitmap for drawing a drag drop image over the square
		/// </summary>
		private Bitmap bitDragDropImage;
		/// <summary>
		/// Set by the drag n drop code to allow the 
		/// square to draw an image on the square without setting it as occupied
		/// </summary>
		private bool bDrawDragDropImage;
		/// <summary>
		/// piece associated with this square
		/// </summary>
		private BasicPiece bpPiece;
		/// <summary>
		/// should the square be highlighted
		/// </summary>
		private bool bDrawHighlight;
		/// <summary>
		/// colour of the highlight
		/// </summary>
		private static Color cHighlightColor;
		/// <summary>
		/// Pen for drawing the highlight
		/// </summary>
		private static Pen highlightPen;

		


		public int SquareHorizontalLocation
		{
			get
			{
				return nSquareHorizontalLocation;
			}
			set
			{
				nSquareHorizontalLocation = value;
			}
		}

		public int SquareVerticalLocation
		{
			get
			{
				return nSquareVerticalLocation;
			}
			set
			{
				nSquareVerticalLocation = value;
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

		public Color BackGroundColor
		{
			get
			{
				return backGroundColor;
			}
			set
			{
				backGroundColor = value;
			}
		}

		public bool DrawBorder
		{
			get
			{
				return bDrawBorder;
			}
			set
			{
				bDrawBorder = value;
			}
		}

		public Color BorderColor
		{
			get
			{
				return borderColor;
			}
			set
			{
				borderColor = value;
			}
		}

		public Pen BorderPen
		{
			get
			{
				return borderPen;
			}
			set
			{
				borderPen = value;
			}
		}

		public SolidBrush BackGroundBrush
		{
			get
			{
				return backGroundBrush;
			}
			set
			{
				backGroundBrush = value;
			}
		}

		public bool IsValid
		{
			get
			{
				return bIsValid;
			}
			set
			{
				bIsValid = value;
			}
		}

		public bool DrawLegend
		{
			get
			{
				return bDrawLegend;
			}
			set
			{
				bDrawLegend = value;
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

		public Color LegendColor
		{
			get
			{
				return cLegendColor;
			}
			set
			{
				cLegendColor = value;
			}
		}

		public SolidBrush LegendBrush
		{
			get
			{
				return legendBrush;
			}
			set
			{
				legendBrush = value;
			}
		}

		public bool IsRightEdgeSquare
		{
			get
			{
				return bIsRightEdgeSquare;
			}
			set
			{
				bIsRightEdgeSquare = value;
			}
		}

		public bool IsBottomSquare
		{
			get
			{
				return bIsBottomSquare;
			}
			set
			{
				bIsBottomSquare = value;
			}
		}

		public string Identifier
		{
			get
			{
				return strIdentifier;
			}
			set
			{
				strIdentifier = value;
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

		public string OccupyingName
		{
			get
			{
				return strOccupyingName;
			}
			set
			{
				strOccupyingName = value;
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

		public bool DrawDragDropImage
		{
			get
			{
				return bDrawDragDropImage;
			}
			set
			{
				bDrawDragDropImage = value;
			}
		}

		public BasicPiece Piece 
		{
			get
			{
				return bpPiece;
			}
			set
			{
				bpPiece = value;
			}
		}

		public bool DrawHighlight
		{
			get
			{
				return bDrawHighlight;
			}
			set
			{
				bDrawHighlight = value;
			}
		}

		public Color HighlightColor
		{
			get
			{
				return cHighlightColor;
			}
			set
			{
				cHighlightColor = value;
			}
		}

		public Pen HighlightPen
		{
			get
			{
				return highlightPen;
			}
			set
			{
				highlightPen = value;
			}
		}

		public BasicSquare()
		{
			IsOccupied = false;
			OccupyingName = "EMPTY";

			SquareWidth = 0;
			SquareHeight = 0;
			SquareHorizontalLocation = 0;
			SquareVerticalLocation = 0;

			DrawBorder = true;
			BackGroundColor = Color.Sienna;
			BorderColor = Color.Black;
			LegendColor = Color.LightBlue;
			HighlightColor = Color.Green;


			if( borderPen == null )
				borderPen = new Pen( BorderColor );
			if( backGroundBrush == null )
				backGroundBrush = new SolidBrush( BackGroundColor );
			if( legendBrush == null )
				legendBrush = new SolidBrush( LegendColor );
			if( highlightPen == null )
			{
				highlightPen = new Pen( HighlightColor );
				highlightPen.Width = 3;
			}

			bIsValid = false;

			IsRightEdgeSquare = false;
			IsBottomSquare = false;

			Identifier = strIdentifier;
			DrawDragDropImage = false;
			Piece = null;
			DrawHighlight = false;

		}

		public BasicSquare( int squareWidth, int squareHeight, string identifier )
		{
			IsOccupied = false;
			OccupyingName = "EMPTY";

			SquareWidth = squareWidth;
			SquareHeight = squareHeight;
			SquareHorizontalLocation = 0;
			SquareVerticalLocation = 0;

			DrawBorder = true;
			BackGroundColor = Color.Sienna;
			BorderColor = Color.Black;
			LegendColor = Color.LightBlue;
			HighlightColor = Color.Green;

			if( borderPen == null )
				borderPen = new Pen( BorderColor );
			if( backGroundBrush == null )
				backGroundBrush = new SolidBrush( BackGroundColor );
			if( legendBrush == null )
				legendBrush = new SolidBrush( LegendColor );
			if( highlightPen == null )
			{
				highlightPen = new Pen( HighlightColor );
				highlightPen.Width = 3;
			}

			bIsValid = false;

			IsRightEdgeSquare = false;
			IsBottomSquare = false;

			Identifier = identifier;
			DrawDragDropImage = false;
			Piece = null;
			DrawHighlight = false;
		}

		public BasicSquare( int squareWidth, int squareHeight, int squareHorizontalLocation, int squareVerticalLocation, string identifier )
		{
			IsOccupied = false;
			OccupyingName = "EMPTY";

			SquareWidth = squareWidth;
			SquareHeight = squareHeight;
			SquareHorizontalLocation = squareHorizontalLocation;
			SquareVerticalLocation = squareVerticalLocation;

			DrawBorder = true;
			BackGroundColor = Color.Sienna;
			BorderColor = Color.Black;
			LegendColor = Color.LightBlue;
			HighlightColor = Color.Green;

			if( borderPen == null )
				borderPen = new Pen( BorderColor );
			if( backGroundBrush == null )
				backGroundBrush = new SolidBrush( BackGroundColor );
			if( legendBrush == null )
				legendBrush = new SolidBrush( LegendColor );
			if( highlightPen == null )
			{
				highlightPen = new Pen( HighlightColor );
				highlightPen.Width = 3;
			}

			bIsValid = false;

			IsRightEdgeSquare = false;
			IsBottomSquare = false;

			Identifier = identifier;
			DrawDragDropImage = false;
			Piece = null;
			DrawHighlight = false;

		}

		public virtual void DrawSquare( Graphics grfx )
		{

			borderPen.Color = BorderColor;
			BackGroundBrush.Color = BackGroundColor;
			LegendBrush.Color = LegendColor;
			HighlightPen.Color = HighlightColor;
			
			/// only draw the square if it is declared invalid
			if( IsValid == true )
				return;

			if( DrawLegend == false )
			{
				grfx.FillRectangle( backGroundBrush, this.SquareHorizontalLocation, this.SquareVerticalLocation, this.SquareWidth, this.SquareHeight ); 
			
				if( bDrawBorder == true )
				{
					grfx.DrawRectangle( borderPen, SquareHorizontalLocation, SquareVerticalLocation, this.SquareWidth -1, this.SquareHeight -1 );
				}

				if( DrawHighlight == true )
				{
					grfx.DrawRectangle( highlightPen, SquareHorizontalLocation +5, SquareVerticalLocation +5, SquareWidth -10, SquareHeight -10 );
				}
			}
			else
			{
				if( SquareHorizontalLocation == 0 && SquareVerticalLocation == 0 )
				{
					grfx.FillRectangle( LegendBrush, SquareHorizontalLocation, SquareVerticalLocation, SquareWidth + LegendWidth, LegendWidth );
					grfx.FillRectangle( LegendBrush, SquareHorizontalLocation, SquareVerticalLocation, LegendWidth, SquareHeight + LegendWidth );
				}
				else if( SquareHorizontalLocation == 0 && SquareVerticalLocation != 0 )
				{
					grfx.FillRectangle( LegendBrush, SquareHorizontalLocation, SquareVerticalLocation + LegendWidth, LegendWidth, SquareHeight + 1 );
				}
				else if( SquareVerticalLocation == 0 && SquareHorizontalLocation != 0 )
				{
					grfx.FillRectangle( LegendBrush, SquareHorizontalLocation + LegendWidth, SquareVerticalLocation, SquareWidth + 1, LegendWidth );
				}
				else
				{
				}

				if( IsRightEdgeSquare == true )
				{
					grfx.FillRectangle( LegendBrush, SquareHorizontalLocation + SquareWidth + LegendWidth + 1, SquareVerticalLocation, LegendWidth, SquareHeight + LegendWidth + LegendWidth + 1 );
				}

				if( IsBottomSquare == true )
				{
					grfx.FillRectangle( LegendBrush, SquareHorizontalLocation, SquareVerticalLocation + SquareHeight + LegendWidth + 1, SquareWidth + LegendWidth + 1, LegendWidth );
				}

				grfx.FillRectangle( BackGroundBrush, SquareHorizontalLocation + LegendWidth, SquareVerticalLocation + LegendWidth, SquareWidth, SquareHeight );
 
				if( bDrawBorder == true )
				{
					grfx.DrawRectangle( BorderPen, SquareHorizontalLocation + LegendWidth, SquareVerticalLocation + LegendWidth, SquareWidth, SquareHeight );
				}

				
				if( DrawHighlight == true )
				{
					grfx.DrawRectangle( highlightPen, SquareHorizontalLocation + LegendWidth +5, SquareVerticalLocation + LegendWidth +5, SquareWidth -10, SquareHeight -10 );
				}
			}

			if( DrawDragDropImage == true )
			{
				if( DrawLegend == false )
					grfx.DrawImage( DragDropImage, SquareHorizontalLocation, SquareVerticalLocation, SquareWidth, SquareHeight );
				else
					grfx.DrawImage( DragDropImage, SquareHorizontalLocation + LegendWidth, SquareVerticalLocation + LegendWidth, SquareWidth, SquareHeight );
			}
		
		}

	}
}
