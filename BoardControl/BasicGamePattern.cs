using System;
using System.Collections;
using System.Xml;

namespace BoardControl
{
	public enum PIECEPOSITION{ START, ABOVE, ABOVERIGHT, RIGHT, BELOWRIGHT, BELOW, BELOWLEFT, LEFT, ABOVELEFT };

	/// <summary>
	/// A Holder for a single Pattern of pieces on the board
	/// </summary>
	public class BasicGamePiece
	{
		/// <summary>
		/// Identifer for the square in the pattern
		/// </summary>
		protected string strSquareIdentifier;
		/// <summary>
		/// is the square occupied by an enemy
		/// </summary>
		protected bool bIsEnemy;
		/// <summary>
		/// Is this the starting point of the pattern
		/// </summary>
		protected bool bIsStartForPattern;
		/// <summary>
		/// position relative to the start square
		/// </summary>
		protected PIECEPOSITION pPosition;
		/// <summary>
		/// what level is this piece at ie how many squares away from start square
		/// level 1 is adjacent
		/// </summary>
		protected int nLevel;
		/// <summary>
		/// identifier for this pattern
		/// </summary>
		private static int nBasicPieceID = 0;
		private int nPieceID;

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

		public bool IsEnemy
		{
			get
			{
				return bIsEnemy;
			}
			set
			{
				bIsEnemy = value;
			}
		}

		public bool IsStartForPattern
		{
			get
			{
				return bIsStartForPattern;
			}
			set
			{
				bIsStartForPattern = value;
			}
		}


		public PIECEPOSITION Position
		{
			get
			{
				return pPosition;
			}
			set
			{
				pPosition = value;
			}
		}

		public int PieceID
		{
			get
			{
				return nPieceID;
			}
			set
			{
				nPieceID = value;
			}
		}

		public int Level
		{
			get
			{
				return nLevel;
			}
			set
			{
				nLevel = value;
			}
		}

		public BasicGamePiece()
		{

			strSquareIdentifier = null;
			bIsEnemy = false;
			nPieceID = nBasicPieceID;
			nBasicPieceID++;
			bIsStartForPattern = false;
			nLevel = 0;
		}

		public BasicGamePiece( bool isStartForPattern, string squareIdentifier ) : this( squareIdentifier )
		{
			IsStartForPattern = isStartForPattern;
		}

		public BasicGamePiece( string squareIdentifier ) : this()
		{
			SquareIdentifier = squareIdentifier;
		}

		public BasicGamePiece( bool isStartForPattern, string squareIdentifier, bool isEnemy ) : this( squareIdentifier, isEnemy )
		{
			IsStartForPattern = isStartForPattern;
		}

		public BasicGamePiece( string squareIdentifier, bool isEnemy ) : this( squareIdentifier )
		{
			IsEnemy = isEnemy;
		}

		public BasicGamePiece( string squareIdentifier, bool isEnemy, PIECEPOSITION position ) : this( squareIdentifier, isEnemy )
		{
			Position = position;
		}

		public BasicGamePiece( BasicGamePiece piece )
		{
			IsStartForPattern = piece.IsStartForPattern;
			SquareIdentifier = piece.SquareIdentifier;
			IsEnemy = piece.IsEnemy;
			Position = piece.Position;
			Level = piece.Level;
		}

		/// Saving and loading
		
		public virtual void Save( XmlWriter xmlWriter )
		{
			xmlWriter.WriteStartElement( "BasicGamePiece" );
			xmlWriter.WriteElementString( "PieceID", nPieceID.ToString() );
			xmlWriter.WriteElementString( "IsStartForPattern", IsStartForPattern.ToString() );
			xmlWriter.WriteElementString( "Square", strSquareIdentifier );
			xmlWriter.WriteElementString( "IsEnemy", bIsEnemy.ToString() );
			xmlWriter.WriteElementString( "PiecePosition", pPosition.ToString() );
			xmlWriter.WriteElementString( "Level", Level.ToString() );
			xmlWriter.WriteEndElement();
		}

		public virtual void Load( XmlReader xmlReader )
		{
			while( xmlReader.Name != "PieceID" )
			{
				xmlReader.Read();
				if( xmlReader.EOF == true )
					return;
			}

			xmlReader.Read();
			nPieceID = Int32.Parse( xmlReader.Value );

			while( xmlReader.Name != "IsStartForPattern" )
			{
				xmlReader.Read();
				if( xmlReader.EOF == true )
					return;
			}

			xmlReader.Read();
			if( xmlReader.Value == "True" )
				bIsStartForPattern = true;
			else
				bIsStartForPattern = false;

			while( xmlReader.Name != "Square" )
			{
				xmlReader.Read();
				if( xmlReader.EOF == true )
					return;
			}

			xmlReader.Read();
			strSquareIdentifier = xmlReader.Value;

			while( xmlReader.Name != "IsEnemy" )
			{
				xmlReader.Read();
				if( xmlReader.EOF == true )
					return;
			}

			xmlReader.Read();
			if( xmlReader.Value == "True" )
				bIsEnemy = true;
			else
				bIsEnemy = false;

			while( xmlReader.Name != "PiecePosition" )
			{
				xmlReader.Read();
				if( xmlReader.EOF == true )
					return;
			}

			xmlReader.Read();
			string strTest = xmlReader.Value;


			switch( strTest )
			{
				case "START": Position = PIECEPOSITION.START; break;
				case "ABOVE": Position = PIECEPOSITION.ABOVE; break;
				case "ABOVERIGHT": Position = PIECEPOSITION.ABOVERIGHT; break;
				case "RIGHT": Position = PIECEPOSITION.RIGHT; break;
				case "BELOWRIGHT": Position = PIECEPOSITION.BELOWRIGHT; break;
				case "BELOW": Position = PIECEPOSITION.BELOW; break;
				case "BELOWLEFT": Position = PIECEPOSITION.BELOWLEFT; break;
				case "LEFT": Position = PIECEPOSITION.ABOVELEFT; break;
			}

			while( xmlReader.Name != "Level" )
			{
				xmlReader.Read();
				if( xmlReader.EOF == true )
					return;
			}

			xmlReader.Read();
			nLevel = int.Parse( xmlReader.Value );
		}

		/// 
		/// Compare basic patterns
		///
 
		public static bool operator ==( BasicGamePiece pieceOne, BasicGamePiece pieceTwo )
		{
			bool bOneIsNull = false;
			bool bBothAreNull = false;

			try
			{
				int nTest = pieceOne.PieceID;
			}
			catch( NullReferenceException nullRefExp )
			{
				string strTemp = nullRefExp.Message;

				bOneIsNull = true;
			}

			try
			{
				int nTest = pieceTwo.PieceID;
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

			if( pieceOne.Position == pieceTwo.Position
				&& pieceOne.IsEnemy == pieceTwo.IsEnemy
				&& pieceOne.Level == pieceTwo.Level )
				return true;
			else
				return false;
		}

		public static bool operator !=( BasicGamePiece pieceOne, BasicGamePiece pieceTwo )
		{

			bool bOneIsNull = false;
			bool bBothAreNull = false;

			try
			{
				int nTest = pieceOne.PieceID;
			}
			catch( NullReferenceException nullRefExp )
			{
				string strTemp = nullRefExp.Message;

				bOneIsNull = true;
			}

			try
			{
				int nTest = pieceTwo.PieceID;
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

			if( pieceOne.Position != pieceTwo.Position
				|| pieceOne.IsEnemy != pieceTwo.IsEnemy
				|| pieceOne.Level != pieceTwo.Level
				|| pieceOne.SquareIdentifier != pieceTwo.SquareIdentifier )
				return true;
			else
				return false;
		}

		/// required overrides 
		/// 

		public override bool Equals(object obj)
		{
			if( obj == null || GetType() != obj.GetType() )
				return false;

			BasicGamePiece temp = ( BasicGamePiece )obj;

			return this == temp;
		}

		public override int GetHashCode()
		{
			return Position.GetHashCode() ^ IsEnemy.GetHashCode() ^ Level.GetHashCode();
		}
	}

	/// <summary>
	/// A collection of basic patterns which includes a weighting for this set
	/// Weighting should be based on success ie was this set used in a winning game
	/// what happened when this set was used?
	/// It's up to the individual game to decide how many patterns make a set ( could actually be a difficulty setting )
	/// </summary>
	public class BasicGamePattern
	{
		/// <summary>
		/// array to store the pattern objects
		/// </summary>
		protected ArrayList arrayGamePieces;
		/// <summary>
		/// number of times this pattern has been seen
		/// </summary>
		protected int nNumberOfTimesSeen;
		protected int nNumberOfTimesSeenInWinningGame;
		protected int nNumberOfTimesSeenInLosingGame;
		/// <summary>
		/// weighting for this pattern
		/// </summary>
		protected int nWeighting;
		/// <summary>
		/// what was the previous response to this pattern
		/// </summary>
		protected BasicGamePiece bgpResponse = null;
		/// <summary>
		/// id for this pattern set
		/// </summary>
		protected static int nBasicGamePatternID = 0;
		protected int nPatternID;

		/// <summary>
		/// Is there a response recorded for this pattern
		/// </summary>
		protected bool bResponsePresent = false;

		/// <summary>
		/// Is this pattern a deciding pattern
		/// </summary>
		protected bool bIsEndingPattern = false;


		/// <summary>
		/// class variables that are helpful for deciding what data to change but
		/// are not themselves saved
		/// </summary>
		protected bool bIsWinningPattern = false;
		protected bool bIsLosingPattern = false;


		public ArrayList GamePieces
		{
			get
			{
				return arrayGamePieces;
			}
		}

		public int NumberOfTimesSeen
		{
			get
			{
				return nNumberOfTimesSeen;
			}
			set
			{
				nNumberOfTimesSeen = value;
			}
		}

		public int NumberOfTimesSeenInWinningGame
		{
			get
			{
				return nNumberOfTimesSeenInWinningGame;
			}
			set
			{
				nNumberOfTimesSeenInWinningGame = value;
			}
		}

		public int NumberOfTimesSeenInLosingGame
		{
			get
			{
				return nNumberOfTimesSeenInLosingGame;
			}
			set
			{
				nNumberOfTimesSeenInLosingGame = value;
			}
		}

		public int Weighting
		{
			get
			{
				return nWeighting;
			}
			set
			{
				nWeighting = value;
			}
		}

		public BasicGamePiece Response
		{
			get
			{
				return bgpResponse;
			}
			set
			{
				bgpResponse = value;
				if( value != null )
					bResponsePresent = true;
			}
		}

		public bool IsEndingPattern
		{
			get
			{
				return bIsEndingPattern;
			}
			set
			{
				bIsEndingPattern = value;
			}
		}

		public bool IsWinningPattern
		{
			get
			{
				return bIsWinningPattern;
			}
			set
			{
				bIsWinningPattern = value;
			}
		}

		public bool IsLosingPattern
		{
			get
			{
				return bIsLosingPattern;
			}
			set
			{
				bIsLosingPattern = value;
			}
		}

		public int PatternID
		{
			get
			{
				return nPatternID;
			}
			set
			{
				nPatternID = value;
			}
		}

		public bool ResponsePresent
		{
			get
			{
				return bResponsePresent;
			}
		}

		private bool SetResponsePresent
		{
			set
			{
				bResponsePresent = value;
			}
		}

	
		public int Count
		{
			get
			{
				return arrayGamePieces.Count;
			}
		}

		public BasicGamePattern()
		{
			arrayGamePieces = new ArrayList();
			nNumberOfTimesSeen = 0;
			nWeighting = 0;
			nPatternID = nBasicGamePatternID;
			nBasicGamePatternID++;
			bResponsePresent = false;
			Weighting = 0;
			bIsEndingPattern = false;
		}

		public BasicGamePattern( int numberOfTimesSeen ) : this()
		{
			NumberOfTimesSeen = numberOfTimesSeen;
		}

		public BasicGamePattern( int numberOfTimesSeen, int weighting ) : this( numberOfTimesSeen )
		{
			Weighting = weighting;
		}

		public BasicGamePattern( BasicGamePattern pattern ) : this()
		{
			NumberOfTimesSeen = pattern.NumberOfTimesSeen;
			NumberOfTimesSeenInWinningGame = pattern.NumberOfTimesSeenInWinningGame;
			NumberOfTimesSeenInLosingGame = pattern.NumberOfTimesSeenInLosingGame;
			IsWinningPattern = pattern.IsWinningPattern;
			IsLosingPattern = pattern.IsLosingPattern;
			Weighting = pattern.Weighting;
			SetResponsePresent = pattern.ResponsePresent;
			Response = pattern.Response;
			PatternID = pattern.PatternID;
			IsEndingPattern = pattern.IsEndingPattern;

			for( int i=0; i<pattern.GamePieces.Count; i++ )
			{
				arrayGamePieces.Add( new BasicGamePiece( ( BasicGamePiece )pattern.GamePieces[ i ] ) );
			}
		}

		/// <summary>
		/// Clear out the data held in this pattern
		/// </summary>
		public void Clear()
		{
			NumberOfTimesSeen = 0;
			NumberOfTimesSeenInWinningGame = 0;
			NumberOfTimesSeenInLosingGame = 0;
			IsWinningPattern = false;
			IsLosingPattern = false;
			Weighting = 0;
			SetResponsePresent = false;
			Response = null;
			PatternID = 0;
			IsEndingPattern = false;

			arrayGamePieces.Clear();
		}


		/// <summary>
		/// add a piece to the set
		/// </summary>
		/// <param name="pattern"></param>
		public void AddBasicGamePiece( BasicGamePiece piece )
		{
			arrayGamePieces.Add( piece );
		}

		/// <summary>
		/// add a piece to the set
		/// </summary>
		/// <param name="squareIdentifer"></param>
		/// <param name="isEnemy"></param>
		/// <param name="position"></param>
		public void AddBasicGamePiece( string squareIdentifier, bool isEnemy, PIECEPOSITION position )
		{
			arrayGamePieces.Add( new BasicGamePiece( squareIdentifier, isEnemy, position ) );
		}	

		/// <summary>
		/// find out if the pattern starts with a given identifier
		/// </summary>
		/// <param name="identifier"></param>
		/// <returns></returns>
		public bool StartsWith( string identifier )
		{
			for( int i=0; i<arrayGamePieces.Count; i++ )
			{
				if( ( ( BasicGamePiece )arrayGamePieces[ i ] ).IsStartForPattern == true )
				{
					if( ( ( BasicGamePiece )arrayGamePieces[ i ] ).SquareIdentifier == identifier )
						return true;
					else
						return false;
				}
			}

			return false;
		}

		/// <summary>
		/// is there a piece with the given identifier
		/// </summary>
		/// <param name="identifier"></param>
		/// <returns></returns>
		public bool Contains( string identifier )
		{
			foreach( BasicGamePiece piece in GamePieces )
			{
				if( piece.SquareIdentifier == identifier )
					return true;
			}

			return false;
		}

		/// <summary>
		/// does the pattern contain three of the identifiers
		/// </summary>
		/// <param name="identifierOne"></param>
		/// <param name="identifierTwo"></param>
		/// <param name="identifierThree"></param>
		/// <param name="identifierFour"></param>
		/// <returns></returns>
		public bool ContainsThreeOf( string identifierOne, string identifierTwo, string identifierThree, string identifierFour )
		{
			int nCount = 0;

			if( Contains( identifierOne ) == true )
				nCount++;

			if( Contains( identifierTwo ) == true )
				nCount++;

			if( Contains( identifierThree ) == true )
				nCount++;

			if( Contains( identifierFour ) == true )
				nCount++;

			if( nCount >= 3 )
				return true;

			return false;
		}

		/// <summary>
		/// Get the identifier that the pattern starts with
		/// </summary>
		/// <returns></returns>
		public string GetStartsWith()
		{
			for( int i=0; i<GamePieces.Count; i++ )
			{
				if( ( ( BasicGamePiece )GamePieces[ i ] ).IsStartForPattern == true )
					return ( ( BasicGamePiece )GamePieces[ i ] ).SquareIdentifier;
			}

			return null;
		}

		public BasicGamePiece GetStartsWithPiece()
		{
			for( int i=0; i<GamePieces.Count; i++ )
			{
				if( ( ( BasicGamePiece )GamePieces[ i ] ).IsStartForPattern == true )
					return ( BasicGamePiece )GamePieces[ i ];
			}

			return null;
		}

		
		#region Get the strings that represent positions within the pattern

		public string GetAbovePieceIdentifier()
		{
			if( GamePieces.Count < 2 )
				return null;

			char szTemp = ( ( BasicGamePiece )GamePieces[ 0 ] ).SquareIdentifier[ 1 ];
			int nTopPiece = 0;

			for( int i=1; i<GamePieces.Count; i++ )
			{
				if( szTemp > ( ( BasicGamePiece )GamePieces[ i ] ).SquareIdentifier[ 1 ] )
				{
					szTemp = ( ( BasicGamePiece )GamePieces[ i ] ).SquareIdentifier[ 1 ];
					nTopPiece = i;
				}
			}

			return ( ( BasicGamePiece )GamePieces[ nTopPiece ] ).SquareIdentifier;
		}

		public string GetAboveRightPieceIdentifier()
		{
			if( GamePieces.Count < 2 )
				return null;

			char szTemp = ( ( BasicGamePiece )GamePieces[ 0 ] ).SquareIdentifier[ 0 ];
			char szTemp2 = ( ( BasicGamePiece )GamePieces[ 0 ] ).SquareIdentifier[ 1 ];

			int nAboveRightPiece = 0;

			for( int i=1; i<GamePieces.Count; i++ )
			{

				if( szTemp < ( ( BasicGamePiece )GamePieces[ i ] ).SquareIdentifier[ 0 ]
					&& szTemp2 > ( ( BasicGamePiece )GamePieces[ i ] ).SquareIdentifier[ 1 ] )
				{
					szTemp = ( ( BasicGamePiece )GamePieces[ i ] ).SquareIdentifier[ 0 ];
					szTemp2 = ( ( BasicGamePiece )GamePieces[ i ] ).SquareIdentifier[ 1 ];

					nAboveRightPiece = i;
				}
			}

			return ( ( BasicGamePiece )GamePieces[ nAboveRightPiece ] ).SquareIdentifier;
		}

		public string GetRightPieceIdentifier()
		{
			if( GamePieces.Count < 2 )
				return null;

			char szTemp = ( ( BasicGamePiece )GamePieces[ 0 ] ).SquareIdentifier[ 0 ];
			int nRightPiece = 0;

			for( int i=1; i<GamePieces.Count; i++ )
			{
				if( szTemp < ( ( BasicGamePiece )GamePieces[ i ] ).SquareIdentifier[ 0 ] )
				{
					szTemp = ( ( BasicGamePiece )GamePieces[ i ] ).SquareIdentifier[ 0 ];
					nRightPiece = i;
				}
			}

			return ( ( BasicGamePiece )GamePieces[ nRightPiece ] ).SquareIdentifier;
		}


		public string GetBelowRightPieceIdentifier()
		{
			if( GamePieces.Count < 2 )
				return null;

			char szTemp = ( ( BasicGamePiece )GamePieces[ 0 ] ).SquareIdentifier[ 0 ];
			char szTemp2 = ( ( BasicGamePiece )GamePieces[ 0 ] ).SquareIdentifier[ 1 ];
			int nBelowRightPiece = 0;

			for( int i=1; i<GamePieces.Count; i++ )
			{
				if( szTemp < ( ( BasicGamePiece )GamePieces[ i ] ).SquareIdentifier[ 0 ] 
					&& szTemp2 < ( ( BasicGamePiece )GamePieces[ i ] ).SquareIdentifier[ 1 ] )
				{
					szTemp = ( ( BasicGamePiece )GamePieces[ i ] ).SquareIdentifier[ 0 ];
					szTemp2 = ( ( BasicGamePiece )GamePieces[ i ] ).SquareIdentifier[ 1 ];

					nBelowRightPiece = i;
				}
			}

			return ( ( BasicGamePiece )GamePieces[ nBelowRightPiece ] ).SquareIdentifier;
		}

		public string GetBelowPieceIdentifier()
		{
			if( GamePieces.Count < 2 )
				return null;

			char szTemp = ( ( BasicGamePiece )GamePieces[ 0 ] ).SquareIdentifier[ 1 ];
			int nBelowPiece = 0;

			for( int i=1; i<GamePieces.Count; i++ )
			{
				if( szTemp < ( ( BasicGamePiece )GamePieces[ i ] ).SquareIdentifier[ 1 ] )
				{
					szTemp = ( ( BasicGamePiece )GamePieces[ i ] ).SquareIdentifier[ 1 ];
					nBelowPiece = i;
				}
			}

			return ( ( BasicGamePiece )GamePieces[ nBelowPiece ] ).SquareIdentifier;
		}

		public string GetBelowLeftPieceIdentifier()
		{
			if( GamePieces.Count < 2 )
				return null;

			char szTemp = ( ( BasicGamePiece )GamePieces[ 0 ] ).SquareIdentifier[ 0 ];
			char szTemp2 = ( ( BasicGamePiece )GamePieces[ 0 ] ).SquareIdentifier[ 1 ];
			int nBelowLeftPiece = 0;

			for( int i=1; i<GamePieces.Count; i++ )
			{
				if( szTemp > ( ( BasicGamePiece )GamePieces[ i ] ).SquareIdentifier[ 0 ] 
					&& szTemp2 < ( ( BasicGamePiece )GamePieces[ i ] ).SquareIdentifier[ 1 ] )
				{
					szTemp = ( ( BasicGamePiece )GamePieces[ i ] ).SquareIdentifier[ 0 ];
					szTemp2 = ( ( BasicGamePiece )GamePieces[ i ] ).SquareIdentifier[ 1 ];

					nBelowLeftPiece = i;
				}
			}


			return ( ( BasicGamePiece )GamePieces[ nBelowLeftPiece ] ).SquareIdentifier;
		}


		public string GetLeftPieceIdentifier()
		{
			if( GamePieces.Count < 2 )
				return null;

			char szTemp = ( ( BasicGamePiece )GamePieces[ 0 ] ).SquareIdentifier[ 0 ];
			int nLeftPiece = 0;

			for( int i=1; i<GamePieces.Count; i++ )
			{
				if( szTemp > ( ( BasicGamePiece )GamePieces[ i ] ).SquareIdentifier[ 0 ] )
				{
					szTemp = ( ( BasicGamePiece )GamePieces[ i ] ).SquareIdentifier[ 0 ];
					nLeftPiece = i;
				}
			}

			return ( ( BasicGamePiece )GamePieces[ nLeftPiece ] ).SquareIdentifier;
		}

		public string GetAboveLeftPieceIdentifier()
		{
			if( GamePieces.Count < 2 )
				return null;

			char szTemp = ( ( BasicGamePiece )GamePieces[ 0 ] ).SquareIdentifier[ 0 ];
			char szTemp2 = ( ( BasicGamePiece )GamePieces[ 0 ] ).SquareIdentifier[ 1 ];

			int nAboveLeftPiece = 0;

			for( int i=1; i<GamePieces.Count; i++ )
			{
				if( szTemp > ( ( BasicGamePiece )GamePieces[ i ] ).SquareIdentifier[ 0 ] 
					&& szTemp2 > ( ( BasicGamePiece )GamePieces[ i ] ).SquareIdentifier[ 1 ] )
				{
					szTemp = ( ( BasicGamePiece )GamePieces[ i ] ).SquareIdentifier[ 0 ];
					szTemp2 = ( ( BasicGamePiece )GamePieces[ i ] ).SquareIdentifier[ 1 ];

					nAboveLeftPiece = i;
				}
			}

			return ( ( BasicGamePiece )GamePieces[ nAboveLeftPiece ] ).SquareIdentifier;
		}


		#endregion


		public void UpdatePattern( BasicGamePattern pattern )
		{
			NumberOfTimesSeen++;

			if( pattern.IsLosingPattern == true )
				NumberOfTimesSeenInLosingGame++;
			if( pattern.IsWinningPattern == true )
				NumberOfTimesSeenInWinningGame++;
		}

		

		/// <summary>
		/// save this pattern set
		/// </summary>
		/// <param name="xmlWriter"></param>
		public virtual void Save( XmlWriter xmlWriter )
		{
			xmlWriter.WriteStartElement( "BasicGamePatternSet" );
			xmlWriter.WriteElementString( "PatternID", nPatternID.ToString() );
			for( int i=0; i<arrayGamePieces.Count; i++ )
			{
				( ( BasicGamePiece )arrayGamePieces[ i ] ).Save( xmlWriter );
			}
			xmlWriter.WriteElementString( "NumberOfTimesSeen", nNumberOfTimesSeen.ToString() );
			xmlWriter.WriteElementString( "NumberOfTimesSeenInWinningGame", nNumberOfTimesSeenInWinningGame.ToString() );
			xmlWriter.WriteElementString( "NumberOfTimesSeenInLosingGame", nNumberOfTimesSeenInLosingGame.ToString() );
			xmlWriter.WriteElementString( "EndingPattern", bIsEndingPattern.ToString() );
			xmlWriter.WriteElementString( "Weighting", nWeighting.ToString() );
			xmlWriter.WriteStartElement( "Response" );
			if( bResponsePresent == true )
			{
				xmlWriter.WriteElementString( "ResponsePresent", "1" );
				bgpResponse.Save( xmlWriter );
			}
			else
				xmlWriter.WriteElementString( "ResponsePresent", "0" );

			xmlWriter.WriteEndElement();
			xmlWriter.WriteEndElement();

		}


		public virtual void Load( XmlReader xmlReader )
		{
			while( xmlReader.Name != "PatternID" )
			{
				xmlReader.Read();
				if( xmlReader.EOF == true )
					return;
			}

			xmlReader.Read();
			nPatternID = int.Parse( xmlReader.Value );

			bool bBreak = false;
			for( ;; )
			{
				xmlReader.Read();
				switch( xmlReader.NodeType )
				{
					case XmlNodeType.Element:
					{
						switch( xmlReader.Name )
						{
							case "BasicGamePiece":
							{
								BasicGamePiece temp = new BasicGamePiece();
								temp.Load( xmlReader );
								arrayGamePieces.Add( temp );
								break;
							}
							case "NumberOfTimesSeen": bBreak = true; break;
						}
					} break;
				}

				if( bBreak == true )
					break;
			}

			/// should be on Number of times seen but doesn't hurt to check
			if( xmlReader.Name != "NumberOfTimesSeen" )
				return;

			xmlReader.Read();
			nNumberOfTimesSeen = int.Parse( xmlReader.Value );

			while( xmlReader.Name != "NumberOfTimesSeenInWinningGame" )
			{
				xmlReader.Read();
				if( xmlReader.EOF == true )
					return;
			}

			xmlReader.Read();
			nNumberOfTimesSeenInWinningGame = int.Parse( xmlReader.Value );

			while( xmlReader.Name != "NumberOfTimesSeenInLosingGame" )
			{
				xmlReader.Read();
				if( xmlReader.EOF == true )
					return;
			}

			xmlReader.Read();
			nNumberOfTimesSeenInLosingGame = int.Parse( xmlReader.Value );

			while( xmlReader.Name != "EndingPattern" )
			{
				xmlReader.Read();
				if( xmlReader.EOF == true )
					return;
			}

			xmlReader.Read();
			if( xmlReader.Value == "True" )
				bIsEndingPattern = true;
			else
				bIsEndingPattern = false;

			while( xmlReader.Name != "Weighting" )
			{
				xmlReader.Read();
				if( xmlReader.EOF == true )
					return;
			}

			xmlReader.Read();
			nWeighting = int.Parse( xmlReader.Value );

			while( xmlReader.Name != "Response" )
			{
				xmlReader.Read();
				if( xmlReader.EOF == true )
					return;
			}

			while( xmlReader.Name != "ResponsePresent" )
			{
				xmlReader.Read();
				if( xmlReader.EOF == true )
					return;
			}

			xmlReader.Read();
			int nResponse = int.Parse( xmlReader.Value );

			if( nResponse != 0 )
			{
				bResponsePresent = true;
				bgpResponse = new BasicGamePiece();
				bgpResponse.Load( xmlReader );
			}

		}


		/// comparison stuff
		/// 

		/// <summary>
		///  == operator
		///  Note that class members that change during execution are not tested for equality
		///  this means that say for eample the winning number of games has been incremented then the pattern
		///  wont be considered to have changed if compared to a version of the pattern that hasn't been
		///  incremented.
		/// </summary>
		/// <param name="patternOne"></param>
		/// <param name="patternTwo"></param>
		/// <returns></returns>

		public static bool operator ==( BasicGamePattern patternOne, BasicGamePattern patternTwo )
		{
			bool bOneIsNull = false;
			bool bBothAreNull = false;

			try
			{
				int nTest = patternOne.NumberOfTimesSeen;
			}
			catch( NullReferenceException nullRefExp )
			{
				string strTemp = nullRefExp.Message;

				bOneIsNull = true;
			}

			try
			{
				int nTest = patternTwo.NumberOfTimesSeen;
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

			if( patternOne.GamePieces.Count != patternTwo.GamePieces.Count )
				return false;

			for( int i=0; i<patternOne.GamePieces.Count; i++ )
			{
				if( ( BasicGamePiece )patternOne.GamePieces[ i ] != ( BasicGamePiece )patternTwo.GamePieces[ i ] )
					return false;
			}

			return true;
		}

		public static bool operator !=( BasicGamePattern patternOne, BasicGamePattern patternTwo )
		{
			bool bOneIsNull = false;
			bool bBothAreNull = false;

			try
			{
				int nTest = patternOne.NumberOfTimesSeen;
			}
			catch( NullReferenceException nullRefExp )
			{
				string strTemp = nullRefExp.Message;

				bOneIsNull = true;
			}

			try
			{
				int nTest = patternTwo.NumberOfTimesSeen;
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

			if( patternOne.GamePieces.Count != patternTwo.GamePieces.Count )
				return true;

			for( int i=0; i<patternOne.GamePieces.Count; i++ )
			{
				if( ( BasicGamePiece )patternOne.GamePieces[ i ] == ( BasicGamePiece )patternTwo.GamePieces[ i ] )
					return false;
			}

			return true;
		}

		/// required overrides 
		/// 

		public override bool Equals(object obj)
		{
			if( obj == null || GetType() != obj.GetType() )
				return false;

			BasicGamePattern temp = ( BasicGamePattern )obj;

			return this == temp;
		}

		public override int GetHashCode()
		{
			return Weighting.GetHashCode() ^ Response.GetHashCode() ^ NumberOfTimesSeen.GetHashCode() ^ GamePieces.GetHashCode();
		}

	}

	/// <summary>
	/// a Collection of Basic game Pattern sets
	/// </summary>
	public class BasicGamePatternCollection
	{
		/// <summary>
		/// array to hold the sets of patterns
		/// </summary>
		protected ArrayList arrayPatterns;


		public ArrayList Patterns
		{
			get
			{
				return arrayPatterns;
			}
		}

		public int Count
		{
			get
			{
				return arrayPatterns.Count;
			}
		}

		public BasicGamePatternCollection()
		{
			arrayPatterns = new ArrayList();
		}


		public void Clear()
		{
			for( int i=0; i<arrayPatterns.Count; i++ )
			{
				( ( BasicGamePattern )arrayPatterns[ i ] ).Clear();
			}

			arrayPatterns.Clear();
		}

		/// Methods for searching the pattern collection
		/// 

		/// <summary>
		/// Get all the Patterns that start with a given identifier ie CD
		/// </summary>
		/// <param name="identifier"></param>
		/// <returns></returns>
		public virtual BasicGamePatternCollection GetAllPatternsWithIdentifer( string identifier )
		{
			BasicGamePatternCollection unit = new BasicGamePatternCollection();

			for( int i=0; i<arrayPatterns.Count; i++ )
			{
				if( ( ( BasicGamePattern )arrayPatterns[ i ] ).StartsWith( identifier ) == true )
				{
					unit.AddPattern( new BasicGamePattern( ( BasicGamePattern )arrayPatterns[ i ] ) );
				}
			}

			return unit;
		}

		///
		/// Manipulate the pattern sets
		/// 

		public void AddPattern( BasicGamePattern basicGamePattern )
		{
			/// add a copy of the pattern using the copy constructor
			arrayPatterns.Add( new BasicGamePattern( basicGamePattern ) );
		}


		public bool IsIn( BasicGamePattern basicGamePattern )
		{
			for( int i=0; i<arrayPatterns.Count; i++ )
			{
				if( ( ( BasicGamePattern )arrayPatterns[ i ] ) == basicGamePattern )
					return true;
			}

			return false;
		}

		public void UpdatePattern( BasicGamePattern basicGamePattern )
		{
			/// nothing to update here
			/// 
			BasicGamePatternCollection collection = GetAllPatternsWithIdentifer( basicGamePattern.GetStartsWith() );

			if( collection.Count == 0 )
			{
				System.Diagnostics.Debug.Assert( collection.Count != 0, "Error updating the pattern", "Error the pattern has been identified as " + basicGamePattern.GetStartsWith() + " but GetAllPatternsWithIdentifier returns a count of 0 " );
				return;
			}

			for( int i=0; i<collection.Count; i++ )
			{
				if( ( ( BasicGamePattern )collection.Patterns[ i ] ) == basicGamePattern )
				{
					( ( BasicGamePattern )collection.Patterns[ i ] ).UpdatePattern( basicGamePattern );
					return;
				}
			}
		}

		public BasicGamePattern GetPattern( BasicGamePattern basicGamePattern )
		{
			for( int i=0; i<Patterns.Count; i++ )
			{
				if( basicGamePattern == ( BasicGamePattern )Patterns[ i ] )
				{
					return ( BasicGamePattern )Patterns[ i ];
				}
			}

			return null;
		}

		public BasicGamePattern GetPattern( int patternID )
		{
			if( patternID <= Patterns.Count )
			{
				return ( BasicGamePattern )Patterns[ patternID ];
			}

			return null;	
		}

		public BasicGamePattern GetStartPatternAt( string squareIdentifier )
		{
			for( int i=0; i<Patterns.Count; i++ )
			{
				if( ( ( BasicGamePattern )Patterns[ i ] ).StartsWith( squareIdentifier ) == true )
				{
					return ( BasicGamePattern )Patterns[ i ];
				}
			}

			return null;
		}

		/// Save and Load
		/// 

		public virtual void Save( XmlWriter xmlWriter )
		{
			xmlWriter.WriteStartElement( "BasicGamePatternCollection" );
			for( int i=0; i<Patterns.Count; i++ )
			{
				if( ( ( BasicGamePattern )Patterns[ i ] ).GamePieces.Count != 0 )
					( ( BasicGamePattern )Patterns[ i ] ).Save( xmlWriter );
			}
			xmlWriter.WriteEndElement();
		}

		public virtual void Load( XmlReader xmlReader )
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
							case "BasicGamePatternSet":
							{
								BasicGamePattern temp = new BasicGamePattern();
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
							case "BasicGamePatternCollection": bBreak = true; break;
						}
					} break;
				}

				if( bBreak == true )
					break;
			}
		}

	}
}
