using System;
using System.Drawing;
using System.Windows.Forms;

namespace BoardControl
{
	/// <summary>
	/// Summary description for ConnectFourSquare.
	/// </summary>
	public class ConnectFourSquare : BasicSquare
	{

		/// <summary>
		/// pen for drawing the circle
		/// </summary>
		private static Pen circlePen = null;
		/// <summary>
		/// Brush for drawing a circle on an unused square
		/// </summary>
		private static SolidBrush emptyCircleBrush = null;
		/// <summary>
		/// brush for drawing a circle occupied by red player
		/// </summary>
		private static SolidBrush redBrush = null;
		/// <summary>
		/// brush for drawing a circle occupied by blue player
		/// </summary>
		private static SolidBrush blueBrush = null;
		/// <summary>
		/// distance of the circle from the square
		/// </summary>
		private int nCircleDistance;
		/// <summary>
		/// width of the circle
		/// </summary>
		private int nCircleWidth;
		/// <summary>
		/// the color of the connect four piece ( just makes thinking about it easier )
		/// </summary>
		private string strColor;
		/// <summary>
		/// grey brush
		/// </summary>
		private static SolidBrush greyBrush = null;
		/// <summary>
		/// is this square a winning square
		/// </summary>
		private bool bIsWinningSquare;


		public new string OccupyingName
		{
			get
			{
				return base.OccupyingName;
			}
			set
			{
				base.OccupyingName = value;
				strColor = value;
			}
		}

		public Pen CirclePen
		{
			get
			{
				return circlePen;
			}
		}

		public SolidBrush EmptyCircleBrush
		{
			get
			{
				return emptyCircleBrush;
			}
		}

		public SolidBrush RedBrush
		{
			get
			{
				return redBrush;
			}
		}

		public SolidBrush BlueBrush
		{
			get
			{
				return blueBrush;
			}
		}

		public int CircleDistance
		{
			get
			{
				return nCircleDistance;
			}
			set
			{
				nCircleDistance = value;
			}
		}

		public int CircleWidth
		{
			get
			{
				return nCircleWidth;
			}
			set
			{
				nCircleWidth = value;
			}
		}

		public string PieceColor
		{
			get
			{
				return strColor;
			}
		}
		
		public SolidBrush GreyBrush 
		{
			get
			{
				return greyBrush;
			}
		}
		
		public bool IsWinningSquare
		{
			get
			{
				return bIsWinningSquare;
			}
			set
			{
				bIsWinningSquare = value;
			}
		}


		public ConnectFourSquare() : base()
		{
			IsOccupied = false;
			OccupyingName = "EMPTY";

			CircleDistance = 11;
			CircleWidth = SquareWidth -2;

			if( CirclePen == null )
				circlePen = new Pen( Color.Black );
			if( EmptyCircleBrush == null )
				emptyCircleBrush = new SolidBrush( Color.Green );
			if( RedBrush == null )
				redBrush = new SolidBrush( Color.Crimson );
			if( BlueBrush == null )
				blueBrush = new SolidBrush( Color.Blue );

			IsWinningSquare = false;
		}

		public ConnectFourSquare( int squareWidth, int squareHeight, string identifier ) : base( squareWidth, squareHeight, identifier )
		{
			IsOccupied = false;
			OccupyingName = "EMPTY";

			CircleDistance = 11;
			CircleWidth = SquareWidth -2;

			if( CirclePen == null )
				circlePen = new Pen( Color.Black );
			if( EmptyCircleBrush == null )
				emptyCircleBrush = new SolidBrush( Color.Green );
			if( RedBrush == null )
				redBrush = new SolidBrush( Color.Crimson );
			if( BlueBrush == null )
				blueBrush = new SolidBrush( Color.Blue );

			IsWinningSquare = false;

		}

		public ConnectFourSquare( int squareWidth, int squareHeight, int squareHorizontalLocation, int squareVerticalLocation, string identifier ) : base( squareWidth, squareHeight, squareHorizontalLocation, squareVerticalLocation, identifier )
		{
			IsOccupied = false;
			OccupyingName = "EMPTY";

			CircleDistance = 11;
			CircleWidth = SquareWidth -2;

			if( CirclePen == null )
				circlePen = new Pen( Color.Black );
			if( EmptyCircleBrush == null )
				emptyCircleBrush = new SolidBrush( Color.Green );
			if( RedBrush == null )
				redBrush = new SolidBrush( Color.Crimson );
			if( BlueBrush == null )
				blueBrush = new SolidBrush( Color.Blue );

			IsWinningSquare = false;
		}

		public override void DrawSquare( Graphics grfx )
		{
			if( IsValid == true )
				return;

			if( IsWinningSquare == false )
			{
				BackGroundColor = Color.Beige; 
			}
			else
			{
				BackGroundColor = Color.DarkMagenta;
			}

			base.DrawSquare( grfx );

			grfx.DrawEllipse( CirclePen, SquareHorizontalLocation + CircleDistance, SquareVerticalLocation + CircleDistance, CircleWidth, CircleWidth );

			switch( OccupyingName )
			{
				case "EMPTY": 
				{
					grfx.FillEllipse( EmptyCircleBrush, SquareHorizontalLocation + CircleDistance, SquareVerticalLocation + CircleDistance, CircleWidth -1, CircleWidth -1 );
				}break;
				case "RED":
				{
					grfx.FillEllipse( RedBrush, SquareHorizontalLocation + CircleDistance, SquareVerticalLocation + CircleDistance, CircleWidth -1, CircleWidth -1 );
				}break;
				case "BLUE":
				{
					grfx.FillEllipse( BlueBrush, SquareHorizontalLocation + CircleDistance, SquareVerticalLocation + CircleDistance, CircleWidth -1, CircleWidth -1 );
				}break;
				default : MessageBox.Show( "Huge Cock up Connect Four is trying to display stuff that shouldn't exist :- " + OccupyingName  ); break;
			}

			IsValid = true;

		}

	}
}
