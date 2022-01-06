using System;

namespace BoardControl
{
	/// <summary>
	/// ControlDisplaySquare Is a basic square drawn when the control is initilialised and
	/// is just for display purposes
	/// </summary>
	public class ControlDisplaySquare : BasicSquare
	{
		/// <summary>
		/// does the square have a piece on it
		/// </summary>
		private bool bOccupied;
		/// <summary>
		/// the name of whatever is occupying the square
		/// for connect four the colour of the piece
		/// </summary>
		private string strOccupyingName;
 
		public bool Occupied
		{
			get
			{
				return bOccupied;
			}
			set
			{
				bOccupied = value;
			}
		}

		public new string OccupyingName
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

		public ControlDisplaySquare() : base()
		{
			bOccupied = false;
			strOccupyingName = null;

			IsValid = false;
		}

		public ControlDisplaySquare( int squareWidth, int squareHeight, string identifier ) : base( squareWidth, squareHeight, identifier )
		{
			bOccupied = false;
			strOccupyingName = null;

			IsValid = false;
		}

		public ControlDisplaySquare( int squareWidth, int squareHeight, int squareHorizontalLocation, int squareVerticalLocation, string identifier ) : base( squareWidth, squareHeight, squareHorizontalLocation, squareVerticalLocation, identifier )
		{
			bOccupied = false;
			strOccupyingName = null;

			IsValid = false;
		}


		/// Implement the abstract functions

/* MOVED TO BASE CLASS
		public override bool IsOccupied()
		{
			return bOccupied;
		}

		public override string OccupiedName()
		{
			return strOccupyingName;
		}
*/
	}
}
