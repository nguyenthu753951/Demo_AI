using System;
using System.Collections;

namespace GeneticsLibrary
{
	/// <summary>
	/// Genetics Strings are stored in the Genetics Base
	/// Array with idea being that future items are derived 
	/// from this class so that the items within a genetic 
	/// string ( loose defintion of the term string ) can 
	/// be as simple or as complex as the developer requires
	/// 
	/// Note this class cannot be abstract as instances are needed in the Genetics base class
	/// so it ends up being half and half an instantiable class with empty ( pure ) virtual functions.
	/// </summary>
	public class GeneticsStrings
	{
		private ArrayList geneticsStrings;
		private int nChromosomeLength;

		/// <summary>
		/// Option to prevent this item being changed in
		/// any crossover or mutation.
		/// </summary>
		private bool bIsFixed;

		public bool Fixed
		{
			get
			{
				return bIsFixed;
			}
			set
			{
				bIsFixed = value;
			}
		}

		public int Length
		{
			get
			{
				return geneticsStrings.Count;
			}
		}


		/// <summary>
		/// named as genetics string as it makes more sense
		/// when writing the code.
		/// </summary>
		public ArrayList GeneticsString
		{
			get
			{
				return geneticsStrings;
			}
			set
			{
				geneticsStrings = value;
			}
		}

		public int ChromosomeLength
		{
			get
			{
				return nChromosomeLength;
			}
			set
			{
				nChromosomeLength = value;
			}
		}


		public GeneticsStrings()
		{

			try
			{
				geneticsStrings = new ArrayList();
				Fixed = false;
			}
			catch( OutOfMemoryException outOMExp )
			{
				throw new GeneticsLibraryException( outOMExp, "Thrown trying to allocate the genetic strings array in Genetic items" );
			}
		}

		#region Functions that must be overridden to implement this class

		public virtual void InitialiseString()
		{
		}

		#endregion
	}
}
