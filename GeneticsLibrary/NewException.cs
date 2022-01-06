using System;
using System.Text;

namespace GeneticsLibrary
{
	/// <summary>
	/// Summary description for NewException.
	/// </summary>
	public class NewException : Exception
	{
		private bool bWrapper;
		private string strMessage;
		private string strType;
		private string strTargetSite;
		private string strStackTrace;
		private StringBuilder strAdditionalMessage;
		private bool bWarning;


		public bool Warning 
		{
			get
			{
				return bWarning;
			}
			set
			{
				bWarning = value;
			}
		}

		public string Type
		{
			get
			{
				return strType;
			}
		}

		public bool Wrapper
		{
			get
			{
				return bWrapper;
			}
		}

		public StringBuilder AdditionalMessage
		{
			get
			{
				return strAdditionalMessage;
			}
			set
			{
				strAdditionalMessage.Append( value );
			}
		}

		public NewException()
		{
			bWrapper = false;
			strAdditionalMessage = new StringBuilder();
			bWarning = false;
		}

		public NewException( bool warning ) : this()
		{
			bWarning = warning;
		}

		public NewException( Exception exp )
		{
			bWrapper = true;
			strMessage = exp.Message;
			strType = exp.GetType().ToString();
			strTargetSite  = exp.TargetSite.Name;
			strStackTrace = exp.StackTrace;
			strAdditionalMessage = new StringBuilder();
		}

		public NewException( Exception exp, bool warning ) : this( exp )
		{
			bWarning = warning;
		}

		public NewException( string message ) : base( message )
		{
			bWrapper = false;
			strAdditionalMessage = new StringBuilder();
			bWarning = false;
		}

		public NewException( bool warning, string message ) : this( message )
		{
			bWarning = true;
		}

		public NewException( Exception exp, string additionalMessage ) : this( exp )
		{
			strAdditionalMessage.Append( additionalMessage );
		}

		public NewException( Exception exp, string additionalMessage, bool warning ) : this( exp, additionalMessage )
		{
			bWarning = warning;
		}

		public new string Message()
		{
			StringBuilder strString = new StringBuilder();

			strString.Append( "Exception caught.\nError :- " );
			if( bWrapper == true )
				strString.Append( strMessage );
			else
				strString.Append( base.Message );
#if( DEBUG )
			strString.Append( "\nType of exception :- " );
			if( bWrapper == true )
				strString.Append( strType );
			else
				strString.Append( base.GetType().ToString() );
			strString.Append( "\nIn Function :- " );
			if( bWrapper == true )
				strString.Append( strTargetSite );
			else
				strString.Append( base.TargetSite );
			strString.Append( "\nCall stack :- " );
			if( bWrapper == true )
				strString.Append( strStackTrace );
			else
				strString.Append( base.StackTrace );
#endif
			if( strAdditionalMessage.Length != 0 )
				strString.Append( "\n" + strAdditionalMessage.ToString() );

			return strString.ToString();

		}

		/// <summary>
		/// when caught and not handled yet allow the ability to add further to the message
		/// </summary>
		/// <param name="strMessage"></param>
		public void AddtoMessage( string additionalMessage )
		{
			if( strAdditionalMessage.Length != 0 )
			{
				strAdditionalMessage.Append( "\n" + additionalMessage );
			}
			else
				strAdditionalMessage.Append( additionalMessage );
		}
	}
}
