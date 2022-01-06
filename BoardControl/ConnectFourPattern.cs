using System;
using System.Xml;

namespace BoardControl
{
	/// These classes inherit from the Basic game pattern classes and are here
	/// to hold any specific game functionality
	/// Although I'm not sure any will be needed at present
	/// 
	/// 

	public enum CONNECTFOURPATTERNDIRECTION{ ERROR, VERTICAL, RIGHTHORIZONTAL, LEFTHORIZONTAL, LEFTDIAGONAL /* / */, RIGHTDIAGONAL /* \ */ }; 
	
	/// <summary>
	/// Summary description for ConnectFourPattern.
	/// </summary>
	public class ConnectFourPiece : BasicGamePiece
	{
		private bool bIsPieceRed;

		public bool IsPieceRed
		{
			get
			{
				return bIsPieceRed;
			}
			set
			{
				bIsPieceRed = value;
			}
		}

		public ConnectFourPiece() : base()
		{
			IsPieceRed = false;
		}

		public ConnectFourPiece( bool isStartForPattern, string squareIdentifier ) : base( isStartForPattern, squareIdentifier )
		{
			IsPieceRed = false;
		}

		public ConnectFourPiece( string squareIdentifier ) : base( squareIdentifier )
		{
			IsPieceRed = false;
		}

		public ConnectFourPiece( bool isStartForPattern, string squareIdentifier, bool isEnemy ) : base( isStartForPattern, squareIdentifier, isEnemy )
		{
			IsPieceRed = false;
		}

		public ConnectFourPiece( string squareIdentifier, bool isEnemy ) : base( squareIdentifier, isEnemy )
		{
			IsPieceRed = false;
		}

		public ConnectFourPiece( ConnectFourPiece piece ) : base( piece )
		{
			IsPieceRed = false;
		}

		public override void Save(System.Xml.XmlWriter xmlWriter)
		{
			xmlWriter.WriteStartElement( "ConnectFourPiece" );
			xmlWriter.WriteElementString( "IsRed", IsPieceRed.ToString() );
			base.Save (xmlWriter);
			xmlWriter.WriteEndElement();
		}


		public override void Load(System.Xml.XmlReader xmlReader)
		{
			while( xmlReader.Name != "IsRed" )
			{
				xmlReader.Read();
				if( xmlReader.EOF == true )
					return;
			}

			xmlReader.Read();
			if( xmlReader.Value == "True" )
				IsPieceRed = true;
			else
				IsPieceRed = false;

			base.Load (xmlReader);
		}

		public static bool operator ==( ConnectFourPiece pieceOne, ConnectFourPiece pieceTwo )
		{
			bool bOneIsNull = false;
			bool bBothAreNull = false;

			try
			{
				bool bTest = pieceOne.IsPieceRed;
			}
			catch( NullReferenceException nullRefExp )
			{
				string strTemp = nullRefExp.Message;

				bOneIsNull = true;
			}

			try
			{
				bool bTest = pieceTwo.IsPieceRed;
			}
			catch( NullReferenceException nullRefExp )
			{
				string strTemp = nullRefExp.Message;

				if( bOneIsNull == true )
					bBothAreNull = true;
				else
					bOneIsNull = true;
			}

			if( bOneIsNull == true && bBothAreNull == false )
				return false;

			if( bBothAreNull == true )
				return true;

			if( pieceOne.IsPieceRed == pieceTwo.IsPieceRed )
			{
				return ( BasicGamePiece )pieceOne == ( BasicGamePiece )pieceTwo;
			}

			return false;
		}

		public static bool operator !=( ConnectFourPiece pieceOne, ConnectFourPiece pieceTwo )
		{
			bool bOneIsNull = false;
			bool bBothAreNull = false;

			try
			{
				bool bTest = pieceOne.IsPieceRed;
			}
			catch( NullReferenceException nullRefExp )
			{
				string strTemp = nullRefExp.Message;

				bOneIsNull = true;
			}

			try
			{
				bool bTest = pieceTwo.IsPieceRed;	
			}
			catch( NullReferenceException nullRefExp )
			{
				string strTemp = nullRefExp.Message;

				if( bOneIsNull == true )
					bBothAreNull = true;
				else
					bOneIsNull = true;
			}

			if( bOneIsNull == true && bBothAreNull == false )
				return true;

			if( bBothAreNull == true )
				return false;

			if( pieceOne.IsPieceRed != pieceTwo.IsPieceRed )
			{
				return ( BasicGamePiece )pieceOne != ( BasicGamePiece )pieceTwo;
			}

			return false;
		}

		public override bool Equals(object obj)
		{
			if( obj == null && GetType() != obj.GetType() )
				return false;

			ConnectFourPiece piece = ( ConnectFourPiece )obj;

			return this == piece;
		}

		public override int GetHashCode()
		{
			return IsPieceRed.GetHashCode() ^ base.GetHashCode ();
		}
	}

	public class ConnectFourPattern : BasicGamePattern
	{

		public ConnectFourPattern() : base()
		{
		}

		public ConnectFourPattern( int numberOfTimesSeen ) : base( numberOfTimesSeen )
		{
		}

		public ConnectFourPattern( int numberOfTimesSeen, int weighting ) : base( numberOfTimesSeen, weighting )
		{
		}

		public ConnectFourPattern( ConnectFourPattern patternSet ) : base( patternSet )
		{
		}

		public void UpdatePattern( ConnectFourPattern pattern )
		{
			base.UpdatePattern( ( BasicGamePattern )pattern );
		}

		public void AddGamePiece( ConnectFourPiece piece )
		{
			GamePieces.Add( piece );
		}

		public override void Save(System.Xml.XmlWriter xmlWriter)
		{
			if( GamePieces.Count == 0 )
				return;

			xmlWriter.WriteStartElement( "ConnectFourPattern" );

			base.Save( xmlWriter );

			xmlWriter.WriteEndElement();

		}

		public override void Load(System.Xml.XmlReader xmlReader)
		{
			base.Load( xmlReader );
		}

		public CONNECTFOURPATTERNDIRECTION GetPatternDirection()
		{
			if( GamePieces.Count == 1 )
				return CONNECTFOURPATTERNDIRECTION.ERROR;

			System.Diagnostics.Debug.Assert( GamePieces[ 1 ] != null, "Error first piece in the pattern is invalid" );

			BasicGamePiece piece = ( BasicGamePiece )GamePieces[ 1 ];

			if( piece.Position == PIECEPOSITION.ABOVE )
				return CONNECTFOURPATTERNDIRECTION.VERTICAL;
			else if( piece.Position == PIECEPOSITION.ABOVERIGHT )
				return CONNECTFOURPATTERNDIRECTION.LEFTDIAGONAL;
			else if( piece.Position == PIECEPOSITION.RIGHT )
				return CONNECTFOURPATTERNDIRECTION.RIGHTHORIZONTAL;
			else if( piece.Position == PIECEPOSITION.BELOWRIGHT )
				return CONNECTFOURPATTERNDIRECTION.RIGHTDIAGONAL;
			else if( piece.Position == PIECEPOSITION.BELOW )
				return CONNECTFOURPATTERNDIRECTION.VERTICAL;
			else if( piece.Position == PIECEPOSITION.BELOWLEFT )
				return CONNECTFOURPATTERNDIRECTION.LEFTDIAGONAL;
			else if( piece.Position == PIECEPOSITION.LEFT )
				return CONNECTFOURPATTERNDIRECTION.LEFTHORIZONTAL;
			else if( piece.Position == PIECEPOSITION.ABOVELEFT )
				return CONNECTFOURPATTERNDIRECTION.RIGHTDIAGONAL;
			else
				return CONNECTFOURPATTERNDIRECTION.ERROR;
		}

		public bool PatternDirectionAbove()
		{
			if( GamePieces.Count == 1 )
				return false;

			BasicGamePiece piece = null;

			for( int i=1; i<GamePieces.Count; i++ )
			{
				piece = ( BasicGamePiece )GamePieces[ i ];

				if( piece.Position == PIECEPOSITION.ABOVE )
					return true;
				if( piece.Position == PIECEPOSITION.BELOW )
					return true;
			}

			return false;
		}

		public bool PatternDirectionAboveRight()
		{
			if( GamePieces.Count == 1 )
				return false;

			BasicGamePiece piece = null;

			for( int i=1; i<GamePieces.Count; i++ )
			{
				piece = ( BasicGamePiece )GamePieces[ i ];

				if( piece.Position == PIECEPOSITION.ABOVERIGHT )
					return true;
				if( piece.Position == PIECEPOSITION.BELOWLEFT )
					return true;
			}

			return false;
		}

		public bool PatternDirectionRight()
		{
			if( GamePieces.Count == 1 )
				return false;

			BasicGamePiece piece = null;

			for( int i=1; i<GamePieces.Count; i++ )
			{
				piece = ( BasicGamePiece )GamePieces[ i ];

				if( piece.Position == PIECEPOSITION.RIGHT )
					return true;
				if( piece.Position == PIECEPOSITION.LEFT )
					return true;
			}

			return false;
		}

		public bool PatternDirectionBelowRight()
		{
			if( GamePieces.Count == 1 )
				return false;

			BasicGamePiece piece = null;

			for( int i=1; i<GamePieces.Count; i++ )
			{
				piece = ( BasicGamePiece )GamePieces[ i ];

				if( piece.Position == PIECEPOSITION.BELOWRIGHT )
					return true;
				if( piece.Position == PIECEPOSITION.ABOVELEFT )
					return true;
			}

			return false;
		}

		public bool PatternDirectionBelow()
		{
			if( GamePieces.Count == 1 )
				return false;

			BasicGamePiece piece = null;

			for( int i=1; i<GamePieces.Count; i++ )
			{
				piece = ( BasicGamePiece )GamePieces[ i ];

				if( piece.Position == PIECEPOSITION.BELOW )
					return true;
				if( piece.Position == PIECEPOSITION.ABOVE )
					return true;
			}

			return false;
		}

		public bool PatternDirectionBelowLeft()
		{
			if( GamePieces.Count == 1 )
				return false;

			BasicGamePiece piece = null;

			for( int i=1; i<GamePieces.Count; i++ )
			{
				piece = ( BasicGamePiece )GamePieces[ i ];

				if( piece.Position == PIECEPOSITION.BELOWLEFT )
					return true;
				if( piece.Position == PIECEPOSITION.ABOVERIGHT )
					return true;
			}

			return false;
		}

		public bool PatternDirectionLeft()
		{
			if( GamePieces.Count == 1 )
				return false;

			BasicGamePiece piece = null;

			for( int i=1; i<GamePieces.Count; i++ )
			{
				piece = ( BasicGamePiece )GamePieces[ i ];

				if( piece.Position == PIECEPOSITION.LEFT )
					return true;
				if( piece.Position == PIECEPOSITION.RIGHT )
					return true;
			}

			return false;
		}

		public bool PatternDirectionAboveLeft()
		{
			if( GamePieces.Count == 1 )
				return false;

			BasicGamePiece piece = null;

			for( int i=1; i<GamePieces.Count; i++ )
			{
				piece = ( BasicGamePiece )GamePieces[ i ];

				if( piece.Position == PIECEPOSITION.ABOVELEFT )
					return true;
				if( piece.Position == PIECEPOSITION.BELOWRIGHT )
					return true;
			}

			return false;
		}

		public override bool Equals(object obj)
		{
			return base.Equals (obj);
		}

		public override int GetHashCode()
		{
			return base.GetHashCode ();
		}
	}


	public class ConnectFourPatternCollection : BasicGamePatternCollection
	{
		public ConnectFourPatternCollection() : base()
		{
		}

/*
		public ConnectFourPattern GetStartPatternAt( string squareIdentifier )
		{
			for( int i=0; i<Patterns.Count; i++ )
			{
				if( ( ( ConnectFourPattern )Patterns[ i ] ).StartsWith( squareIdentifier ) == true )
					return ( ConnectFourPattern )Patterns[ i ];
			}

			return null;
		}
*/
		public void AddPattern( ConnectFourPattern connectFourPattern )
		{
			Patterns.Add( new ConnectFourPattern( connectFourPattern ) );
		}

		public bool IsIn( ConnectFourPattern connectFourPattern )
		{
			for( int i=0; i<Patterns.Count; i++ )
			{
				if( ( ( BasicGamePattern )Patterns[ i ] ) == connectFourPattern )
					return true;
			}

			return false;
		}

		public void UpdatePattern( ConnectFourPattern pattern )
		{
			ConnectFourPatternCollection collection = ( ConnectFourPatternCollection )GetAllPatternsWithIdentifer( pattern.GetStartsWith() );

			if( collection.Count == 0 )
			{
				return;
			}

			for( int i=0; i<collection.Count; i++ )
			{
				if( ( ( ConnectFourPattern )collection.Patterns[ i ] ) == pattern )
				{
					( ( ConnectFourPattern )collection.Patterns[ i ] ).UpdatePattern( pattern );
				}
			}
		}

		public ConnectFourPattern GetPattern( ConnectFourPattern pattern )
		{
			for( int i=0; i<Patterns.Count; i++ )
			{
				if( pattern == ( ConnectFourPattern )Patterns[ i ] )
				{
					return ( ConnectFourPattern )Patterns[ i ];
				}
			}

			return null;
		}

		public new ConnectFourPattern GetPattern( int patternID )
		{
			if( patternID <= Patterns.Count )
			{
				return ( ConnectFourPattern )Patterns[ patternID ];
			}

			return null;
		}

		public new ConnectFourPatternCollection GetAllPatternsWithIdentifer(string identifier)
		{
			ConnectFourPatternCollection unit = new ConnectFourPatternCollection();

			for( int i=0; i<Patterns.Count; i++ )
			{
				if( ( ( ConnectFourPattern )Patterns[ i ] ).StartsWith( identifier ) == true )
				{
					/// note do not use copy constructor here moron.
					/// 

					unit.AddPattern( ( ( ConnectFourPattern )Patterns[ i ] ) );
				}
			}

			return unit;
		}

		public ConnectFourPattern HighestPatternWeighting()
		{
			int nHighest = 0;
			int nWeight = ( ( ConnectFourPattern )Patterns[ 0 ] ).Weighting;

			ConnectFourPattern pattern = null;

			for( int i=1; i<Patterns.Count; i++ )
			{
				pattern = ( ConnectFourPattern )Patterns[ i ];

				if( pattern.Weighting >= nWeight )
				{
					nHighest = i;
					nWeight = pattern.Weighting;
				}
			}

			return ( ConnectFourPattern )Patterns[ nHighest ];
		}


		public override void Save(System.Xml.XmlWriter xmlWriter)
		{
			xmlWriter.WriteStartElement( "ConnectFourPatternCollection" );
			base.Save (xmlWriter);
			xmlWriter.WriteEndElement();
		}

		public override void Load(System.Xml.XmlReader xmlReader)
		{
			bool bBreak = false;
			for( ;; )
			{
				xmlReader.Read();
				if( xmlReader.EOF == true )
					return;
				switch( xmlReader.NodeType )
				{
					case XmlNodeType.Element:
					{
						switch( xmlReader.Name )
						{
							case "ConnectFourPattern":
							{
								ConnectFourPattern temp = new ConnectFourPattern();
								temp.Load( xmlReader );
								Patterns.Add( temp );
								break;
							}
						}
					} break;
					case XmlNodeType.EndElement:
					{
						switch( xmlReader.Name )
						{
							case "ConnectFourPatternCollection": bBreak = true; break;
						}
					} break;
				}

				if( bBreak == true )
					break;
			}

		}
	}
}
