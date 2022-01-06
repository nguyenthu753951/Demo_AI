using System;
using System.Xml;

namespace BoardControl
{
	/// <summary>
	/// Basic class for describing a move
	/// </summary>
	public class BasicMove
	{
		/// <summary>
		/// square to move to
		/// </summary>
		protected string strIdentifier;

		/// <summary>
		/// Number of times used
		/// </summary>
		protected int nTimesUsed;

		protected int nTimesUsedInWinningGame;
		protected int nTimesUsedInLosingGame;

		protected bool bUsedThisTime;

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

		public int TimesUsed
		{
			get
			{
				return nTimesUsed;
			}
			set
			{
				nTimesUsed = value;
			}
		}

		public int TimesUsedInWinningGame
		{
			get
			{
				return nTimesUsedInWinningGame;
			}
			set
			{
				nTimesUsedInWinningGame = value;
			}
		}

		public int TimesUsedInLosingGame
		{
			get
			{
				return nTimesUsedInLosingGame;
			}
			set
			{
				nTimesUsedInLosingGame = value;
			}
		}

		public bool UsedThisTime
		{
			get
			{
				return bUsedThisTime;
			}
			set
			{
				bUsedThisTime = value;
			}
		}


		public BasicMove()
		{

			Identifier = null;
			TimesUsed = 0;
			UsedThisTime = false;
		}

		public BasicMove( string identifier )
		{
			Identifier = identifier;
			UsedThisTime = false;
		}

		public void Save( XmlWriter xmlWriter )
		{
			xmlWriter.WriteStartElement( "BasicMove" );
			xmlWriter.WriteElementString( "SquareToMoveTo", Identifier );
			xmlWriter.WriteElementString( "TimesUsed", TimesUsed.ToString() );
			xmlWriter.WriteElementString( "TimesUsedInWinningGame", TimesUsedInWinningGame.ToString() );
			xmlWriter.WriteElementString( "TimesUsedInLosingGame", TimesUsedInLosingGame.ToString() );
			xmlWriter.WriteEndElement();
		}

		public void Load( XmlReader xmlReader )
		{
			while( xmlReader.Name != "SquareToMoveTo" )
			{
				xmlReader.Read();
			}

			xmlReader.Read();

			Identifier = xmlReader.Value;

			while( xmlReader.Name != "TimesUsed" )
			{
				xmlReader.Read();
			}

			xmlReader.Read();

			TimesUsed = Int32.Parse( xmlReader.Value );

			while( xmlReader.Name != "TimesUsedInWinningGame" )
			{
				xmlReader.Read();
			}

			xmlReader.Read();

			TimesUsedInWinningGame = Int32.Parse( xmlReader.Value );

			while( xmlReader.Name != "TimesUsedInLosingGame" )
			{
				xmlReader.Read();
			}

			xmlReader.Read();

			TimesUsedInLosingGame = Int32.Parse( xmlReader.Value );
		}

		public static bool operator == ( BasicMove moveOne, BasicMove moveTwo )
		{
			bool bOneIsNull = false;
			bool bBothAreNull = false;

			try
			{
				string strTest = moveOne.Identifier;
			}
			catch( NullReferenceException nullRefExp )
			{
				string strTemp = nullRefExp.Message;

				bOneIsNull = true;
			}

			try
			{
				string strTest = moveTwo.Identifier;
			}
			catch( NullReferenceException nullRefExp )
			{
				string strTemp = nullRefExp.Message;

				if( bOneIsNull == true )
				{
					bBothAreNull = true;
				}
				else
				{
					bOneIsNull = true;
				}
			}

			if( bOneIsNull == true )
				return false;

			if( bBothAreNull == true )
				return true;

			if( moveOne.Identifier == moveTwo.Identifier
				&& moveOne.TimesUsed == moveTwo.TimesUsed
				&& moveOne.TimesUsedInWinningGame == moveTwo.TimesUsedInWinningGame
				&& moveOne.TimesUsedInLosingGame == moveTwo.TimesUsedInLosingGame )
				return true;

			return false;
		}

		public static bool operator != ( BasicMove moveOne, BasicMove moveTwo )
		{
			bool bOneIsNull = false;
			bool bBothAreNull = false;

			try
			{
				string strTest = moveOne.Identifier;
			}
			catch( NullReferenceException nullRefExp )
			{
				string strTemp = nullRefExp.Message;

				bOneIsNull = true;
			}

			try
			{
				string strTest = moveTwo.Identifier;
			}
			catch( NullReferenceException nullRefExp )
			{
				string strTemp = nullRefExp.Message;

				if( bOneIsNull == true )
				{
					bBothAreNull = true;
				}
				else
					bOneIsNull = true;
			}

			if( bOneIsNull == true )
				return true;

			if( bBothAreNull == true )
				return false;

			if( moveOne.Identifier == moveTwo.Identifier
				&& moveOne.TimesUsed == moveTwo.TimesUsed 
				&& moveOne.TimesUsedInWinningGame == moveTwo.TimesUsedInWinningGame
				&& moveOne.TimesUsedInLosingGame == moveTwo.TimesUsedInLosingGame )
				return false;

			return true;
		}

		public override bool Equals(object obj)
		{
			if( obj == null && GetType() != obj.GetType() )
				return false;

			BasicMove move = ( BasicMove )obj;

			return this == move;
		}

		public override int GetHashCode()
		{
			return base.GetHashCode() ^ Identifier.GetHashCode() ^ TimesUsed.GetHashCode();
		}
	}
}
