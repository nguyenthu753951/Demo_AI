using System;
using System.Xml;

namespace BoardControl
{
	/// <summary>
	/// The basic definition of a game piece
	/// </summary>
	public class BasicPiece
	{
		private BasicSquare pieceSquare;

		public BasicSquare Square
		{
			get
			{
				return pieceSquare;
			}
			set
			{
				pieceSquare = value;
			}
		}

		public BasicPiece()
		{
			pieceSquare = null;
		}

		public BasicPiece( BasicSquare square )
		{
			Square = square;
		}
	}
}
