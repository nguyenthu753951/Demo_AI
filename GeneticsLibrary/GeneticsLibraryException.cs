using System;

namespace GeneticsLibrary
{
	/// <summary>
	/// Override of the new exception class for the genetics library
	/// </summary>
	public class GeneticsLibraryException : NewException
	{
		public GeneticsLibraryException() : base()
		{
		}

		public GeneticsLibraryException( bool warning ) : this()
		{
			Warning = warning;
		}

		public GeneticsLibraryException( Exception exp ) : base( exp )
		{
		}

		public GeneticsLibraryException( Exception exp, bool warning ) : this( exp )
		{
			Warning = warning;
		}

		public GeneticsLibraryException( string message ) : base( message )
		{
		}

		public GeneticsLibraryException( bool warning, string message ) : this( message )
		{
			Warning = warning;
		}

		public GeneticsLibraryException( Exception exp, string additionalMessage ) : this( exp )
		{
			AdditionalMessage.Append( additionalMessage );
		}

		public GeneticsLibraryException( Exception exp, string additionalMessage, bool warning ) : this( exp, additionalMessage )
		{
			Warning = warning;
		}
	}
}
