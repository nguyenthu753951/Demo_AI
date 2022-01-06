using System;
using System.Collections;
using RandomNumber;

namespace GeneticsLibrary
{
	public enum SELECTIONMETHODS{ NONESELECTED, ROULETTEWHEEL, FIXEDPOINTSELECTION };
	public enum CROSSOVERMETHODS{ NONESELECTED, SINGLEPOINTCROSSOVER, RANDOMCROSSOVER, DUALCROSSOVER, POSITIONBASEDCROSSOVER };

	/// <summary>
	/// base class for implementing genetic algorythms
	/// </summary>
	public abstract class GeneticsBase
	{
		private ArrayList geneticsArray;

		private double dMutationRate;

		/// <summary>
		/// Population size defaults to 100
		/// </summary>
		private int nPopulationSize;
		private int nChromosomeLength;

		private SELECTIONMETHODS selMethod;
		private CROSSOVERMETHODS crossMethod;

		/// generational variables
		/// 
		private bool bUseGenerations;
		private int nNumberOfGenerations;
		private int nNumberOfCyclesPerGeneration;

		/// elitism switch
		/// 
		private bool bUseElitism;
		private int nNumberOfElites;

		/// Variables used for selection methods
		/// 
		
	
		/// Is the genetics array a fixed length array?
		/// 
		private bool bIsFixedLength;

		/// How large is the array
		/// If the value is left at zero the code will ignore it as it is a notional value
		/// 
		private int nMaximumLength;

        /// The starting length for an expanding genetics array
        /// 
        private int nStartLength;

        /// The Incremental Length for an expanding genetics array
        /// 
        private int nIncrementLength;

        /// The current length of the expanding genetics array
        /// must me <= nMaximumLength
        /// 
        private int nCurrentLength;

		/// <summary>
		/// The point in the array where a single crossover is made
		/// </summary>
		private int nSingleCrossOverPoint;

		/// <summary>
		/// The points in the array where dual point crossovers are made
		/// </summary>
		private int nDualPointCrossOverOne;
		private int nDualPointCrossOverTwo;


		/// <summary>
		/// How many points should we use for order based crossover
		/// must be less than genetic string size
		/// </summary>
		private int nPositionBasedCrossOverPoints;


		/// <summary>
		/// Fixed point selection allows the user to select the parents through
		/// various means.
		/// </summary>
		private int nFixedPointSelectionOne;
		private int nFixedPointSelectionTwo;


		/// <summary>
		/// parents as selected by one of the selection methods
		/// </summary>
		private GeneticsStrings gsParentOne = null;
		private GeneticsStrings gsParentTwo = null;

        /// <summary>
        /// Random Number Generator needs declaring at class level and initialised once
        /// The problem with declaring at a functional level is that the initialisation is based on 
        /// clock ticks so on a fast computer the generator will not give different strings as the 
        /// initialisation value is to all intents and purposes constant when repeatedly calling a function such 
        /// as the RouletteWheelSelection.
        /// </summary>
        private RandomNumberGenerator random = null;
		
		/// <summary>
		/// variables for controlling selection points in expanding arrays
		/// Ideally will mostly accept the defaults
		/// </summary>
		private bool bUpdateSingleCrossoverPointWithIncrement = false;

		public int PopulationSize 
		{
			get
			{
				return nPopulationSize;
			}
			set
			{
				nPopulationSize = value;
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

		public double MutationRate
		{
			get
			{
				return dMutationRate;
			}
			set
			{
				dMutationRate = value;
			}
		}

		public SELECTIONMETHODS SelectionMethod
		{
			get
			{
				return selMethod;
			}
			set
			{
				selMethod = value;
			}
		}

		public CROSSOVERMETHODS CrossOverMethod
		{
			get
			{
				return crossMethod;
			}
			set
			{
				crossMethod = value;
			}
		}

		public bool IsFixedLength
		{
			get
			{
				return bIsFixedLength;
			}
			set
			{
				bIsFixedLength = value;
			}
		}

		public int MaxLength
		{
			get
			{
				return nMaximumLength;
			}
			set
			{
				nMaximumLength = value;
			}
		}

        public int StartingLength
        {
            get
            {
                return nStartLength;
            }
            set
            {
                nStartLength = value;
            }
        }

        public int IncrementLength
        {
            get
            {
                return nIncrementLength;
            }
            set
            {
                nIncrementLength = value;
            }
        }

        public int CurrentLength
        {
            get
            {
                return nCurrentLength;
            }
            set
            {
                nCurrentLength = value;
            }
        }

		public bool UseGenerations 
		{
			get
			{
				return bUseGenerations;
			}
			set
			{
				bUseGenerations = value;
			}
		}

		public int NumberOfGenerations
		{
			get
			{
				return nNumberOfGenerations;
			}
			set
			{
				nNumberOfGenerations = value;
			}
		}

		public int NumberOfCyclesPerGeneration
		{
			get
			{
				return nNumberOfCyclesPerGeneration;
			}
			set
			{
				nNumberOfCyclesPerGeneration = value;
			}
		}

		public bool UseElitism
		{
			get
			{
				return bUseElitism;
			}
			set
			{
				bUseElitism = value;
			}
		}

		public int NumberOfElites
		{
			get
			{
				return nNumberOfElites;
			}
			set
			{
				nNumberOfElites = value;
			}
		}

		public int SingleCrossOverPoint
		{
			get
			{
				return nSingleCrossOverPoint;
			}
			set
			{
				nSingleCrossOverPoint = value;
			}
		}

		public int DualPointCrossOverPointOne
		{
			get
			{
				return nDualPointCrossOverOne;
			}
			set
			{
				nDualPointCrossOverOne = value;
			}
		}

		public int DualPointCrossOverPointTwo
		{
			get
			{
				return nDualPointCrossOverTwo;
			}
			set
			{
				nDualPointCrossOverTwo = value;
			}
		}

		public int PositionBasedCrossOverPoints
		{
			get
			{
				return nPositionBasedCrossOverPoints;
			}
			set
			{
				nPositionBasedCrossOverPoints = value;
			}
		}

		public int FixedPointSelectionOne
		{
			get
			{
				return nFixedPointSelectionOne;
			}
			set
			{
				nFixedPointSelectionOne = value;
			}
		}

		public int FixedPointSelectionTwo
		{
			get
			{
				return nFixedPointSelectionTwo;
			}
			set
			{
				nFixedPointSelectionTwo = value;
			}
		}
		

		/// <summary>
		/// Allow access to the genetics array
		/// at the start not sure if this will continue
		/// </summary>
		public ArrayList GeneticsArray
		{
			get
			{
				return geneticsArray;
			}
			set
			{
				geneticsArray = value;
			}
		}
		
		public bool UpdateSingleCrossoverPointWithIncrement
		{
			get
			{
				return bUpdateSingleCrossoverPointWithIncrement;
			}
			set
			{
				bUpdateSingleCrossoverPointWithIncrement = value;
			}
		}


		public GeneticsBase()
		{
			try
			{
				geneticsArray = new ArrayList();

				/// start settings
				/// 
				MutationRate = 0.10;
				PopulationSize = 100;
				IsFixedLength = true;
				MaxLength = 0;
                IncrementLength = 0;
                StartingLength = 0;
                CurrentLength = 0;
				bUseGenerations = false;
				nNumberOfGenerations = 20;
				nNumberOfCyclesPerGeneration = 50;
				bUseElitism = false;
				nNumberOfElites = 25;
				selMethod = SELECTIONMETHODS.NONESELECTED;
				crossMethod = CROSSOVERMETHODS.NONESELECTED;
				nSingleCrossOverPoint = 0;
				nDualPointCrossOverOne = 0;
				nDualPointCrossOverTwo = 0;
				nPositionBasedCrossOverPoints = 0;
				nFixedPointSelectionOne = 0;
				nFixedPointSelectionTwo = 0;
				nMaximumLength = 0;
				nStartLength = 0;
        		nIncrementLength = 0;
        		nCurrentLength = 0;
                random = new RandomNumberGenerator( true );
			}
			catch( OutOfMemoryException outOMExp )
			{
				throw new GeneticsLibraryException( outOMExp, "Thrown trying to allocate array list in Genetics Base" );
			}
		}

		public GeneticsBase( int length ) : this()
		{
			MaxLength = length;
		}

		public void CrossOver( GeneticsStrings child )
		{
			switch( crossMethod )
			{
				case CROSSOVERMETHODS.NONESELECTED: throw new GeneticsLibraryException( "Error CrossOver called with no method selected." ); 
				case CROSSOVERMETHODS.SINGLEPOINTCROSSOVER: SinglePointCrossOver( child ); break;
				case CROSSOVERMETHODS.RANDOMCROSSOVER: RandomPointCrossOver( child ); break;
				case CROSSOVERMETHODS.DUALCROSSOVER: DualPointCrossOver( child ); break;
				case CROSSOVERMETHODS.POSITIONBASEDCROSSOVER: PositionBasedCrossOver( child ); break;
				default: throw new GeneticsLibraryException( "CrossOver function called with a crossover type not in the switch statement" ); 
			}
		}

		public void Selection()
		{
			switch( selMethod )
			{
				case SELECTIONMETHODS.NONESELECTED: throw new GeneticsLibraryException( "Error Selection calles with no method selected." );
				case SELECTIONMETHODS.ROULETTEWHEEL: RouletteWheelSelection(); break;
				case SELECTIONMETHODS.FIXEDPOINTSELECTION: FixedPointSelection(); break;
				default: throw new GeneticsLibraryException( "Selection method called with a selection type not in the switch statement" );
			}
		}

		#region Functions that must be overridden to implement this class

		/// <summary>
		/// Initialise function for setting up the starting array
		/// </summary>
		public abstract void Initialise();
		/// <summary>
		/// Mutation function to add mutations where applicable
		/// Note this will always be program specific
		/// </summary>
		public abstract void Mutation();
		/// <summary>
		/// Do a single run through the population of the genetics array
		/// </summary>
		public abstract void Run();


		#endregion

		#region Selection Functions


		/// <summary>
		/// This function uses a rand to select a member of the array and return it.
		/// Note the idea is that any calling functions will have removed all
		/// items that do not meet the fitness criterea before calling this function. 
		/// </summary>
		public void RouletteWheelSelection()
		{
	///		Random rand = new Random();

			int nSelected = random.Random( GeneticsArray.Count );
	///		int nSelected = rand.Next( GeneticsArray.Count );
			
			gsParentOne = ( GeneticsStrings )GeneticsArray[ nSelected ];

	///		System.Threading.Thread.Sleep( 2 );

			nSelected = random.Random( GeneticsArray.Count );
			
	///		nSelected = rand.Next( GeneticsArray.Count );

			while( gsParentOne == ( GeneticsStrings )GeneticsArray[ nSelected ] )
			{
				nSelected = random.Random( GeneticsArray.Count );
			///	nSelected = rand.Next( GeneticsArray.Count );
			}

			gsParentTwo = ( GeneticsStrings )GeneticsArray[ nSelected ];
		}

		public void FixedPointSelection()
		{
			if( nFixedPointSelectionOne > GeneticsArray.Count )
				throw new GeneticsLibraryException( "Fixed point selection called with a value for point one higher than the array count at " + nFixedPointSelectionOne.ToString() );

			if( nFixedPointSelectionTwo > GeneticsArray.Count )
				throw new GeneticsLibraryException( "Fixed point selection called with a value for point two higher than the array count at " + nFixedPointSelectionTwo.ToString() );

			gsParentOne = ( GeneticsStrings )GeneticsArray[ nFixedPointSelectionOne ];
			gsParentTwo = ( GeneticsStrings )GeneticsArray[ nFixedPointSelectionTwo ];

		}

		public void FixedPointSelection( int pointOne, int pointTwo )
		{
			nFixedPointSelectionOne = pointOne;
			nFixedPointSelectionTwo = pointTwo;
			FixedPointSelection();
		}

		#endregion

		#region Cross Over Functions


		/// <summary>
		/// Take the first parent up to the specified fixed point and copy that into the
		/// child then take the second parent from the specified fixed point and copy that 
		/// into the child.
		/// </summary>
		/// <returns></returns>
		public void SinglePointCrossOver( GeneticsStrings child )
		{
			if( SingleCrossOverPoint > gsParentOne.GeneticsString.Count )
				throw new GeneticsLibraryException( "Error in the Genetics Library as the Cross over point for child generation is greater than the size of the array" );

			if( SingleCrossOverPoint == 0 )
				throw new GeneticsLibraryException( "Error in the Genetics library as the point given for the single crossover point is zero which is pretty pointless." ); 

			child.GeneticsString.Clear();

			for( int i=0; i<SingleCrossOverPoint; i++ )
			{
				child.GeneticsString.Add( gsParentOne.GeneticsString[ i ] );
			}

			for( int i=SingleCrossOverPoint; i<gsParentTwo.GeneticsString.Count; i++ )
			{
				child.GeneticsString.Add( gsParentTwo.GeneticsString[ i ] );
			}
		}

		public void SinglePointCrossOver( GeneticsStrings child, int crossOverPoint )
		{
			SingleCrossOverPoint = crossOverPoint;

			SinglePointCrossOver( child );
		}

		/// <summary>
		/// copy the center piece from the second parent while the start and end points
		/// are copied from the first parent.
		/// </summary>
		/// <param name="child"></param>
		public void DualPointCrossOver( GeneticsStrings child )
		{
			if( DualPointCrossOverPointOne < 0 || DualPointCrossOverPointOne > child.ChromosomeLength )
				throw new GeneticsLibraryException( "Error in the Genetics Library as the Cross Over point for dual cross over point one is invalid at " + DualPointCrossOverPointOne.ToString() );

			if( DualPointCrossOverPointTwo < 0 || DualPointCrossOverPointOne > child.ChromosomeLength )
				throw new GeneticsLibraryException( "Error in the Genetics Library as the Cross Over point for dual cross over point two is invalid at " + DualPointCrossOverPointTwo.ToString() );

			if( DualPointCrossOverPointOne == 0 )
				throw new GeneticsLibraryException( "Error in the Genetics Library as the first cross over point is equal to 0 which is really pointless" );

			if( DualPointCrossOverPointOne > DualPointCrossOverPointTwo )
				throw new GeneticsLibraryException( "Error in the Genetics Library as the first cross over point has a value of " + DualPointCrossOverPointOne.ToString() + " is greater than the value for the second cross over point value of " + DualPointCrossOverPointTwo.ToString() );

			child.GeneticsString.Clear();

			for( int i=0; i<DualPointCrossOverPointOne; i++ )
			{
				child.GeneticsString.Add( gsParentOne.GeneticsString[ i ] );
			}

			for( int i=DualPointCrossOverPointOne; i<DualPointCrossOverPointTwo; i++ )
			{
				child.GeneticsString.Add( gsParentTwo.GeneticsString[ i ] );
			}

			for( int i=DualPointCrossOverPointTwo; i<gsParentOne.GeneticsString.Count; i++ )
			{
				child.GeneticsString.Add( gsParentOne.GeneticsString[ i ] );
			}
		}

		public void DualPointCrossOver( GeneticsStrings child, int crossOverPointOne, int crossOverPointTwo )
		{
			DualPointCrossOverPointOne = crossOverPointOne;
			DualPointCrossOverPointTwo = crossOverPointTwo;

			DualPointCrossOver( child );
		}

		/// <summary>
		/// A single point cross over that randomly selects the crossover point
		/// </summary>
		/// <param name="child"></param>
		public void RandomPointCrossOver( GeneticsStrings child )
		{
			int nPoint = random.Random( gsParentOne.GeneticsString.Count );

			child.GeneticsString.Clear();

			for( int i=0; i<nPoint; i++ )
			{
				child.GeneticsString.Add( gsParentOne.GeneticsString[ i ] );
			}

			for( int i=nPoint; i<gsParentTwo.GeneticsString.Count; i++ )
			{
				child.GeneticsString.Add( gsParentTwo.GeneticsString[ i ] );
			}
		}

		/// <summary>
		/// replace a number of items in the parent one string with items from parent two
		/// at random points.
		/// </summary>
		/// <param name="child"></param>
		public void PositionBasedCrossOver( GeneticsStrings child )
		{
			ArrayList arrayCheck = new ArrayList();
			int nCrossPoint = 0;
			bool bFound = false;

			if( nPositionBasedCrossOverPoints == 0 ) 
				throw new GeneticsLibraryException( "Error the number of position based crossover points has not been set or has been set to 0" );
 
			if( nPositionBasedCrossOverPoints > child.ChromosomeLength )
				throw new GeneticsLibraryException( "Error the number of position based crossover points exceeds the size of the array at " + nPositionBasedCrossOverPoints.ToString() );

			child.GeneticsString.Clear();

			for( int i=0; i<gsParentOne.GeneticsString.Count; i++ )
			{
				child.GeneticsString.Add( gsParentOne.GeneticsString[ i ] );
			}

			for( int i=0; i<nPositionBasedCrossOverPoints; )
			{
				nCrossPoint = random.Random( child.GeneticsString.Count );

				for( int n=0; n<arrayCheck.Count; n++ )
				{
					if( nCrossPoint == ( int )arrayCheck[ n ] )
						bFound = true;
				}

				if( bFound == false )
				{
					child.GeneticsString.RemoveAt( nCrossPoint );
					child.GeneticsString.Insert( nCrossPoint, gsParentTwo.GeneticsString[ i ] );
					arrayCheck.Add( nCrossPoint );
					i++;
				}
				else
					bFound = false;
			}
		}

		public void PositionBasedCrossOver( GeneticsStrings child, int numberOfPoints )
		{
			nPositionBasedCrossOverPoints = numberOfPoints;
			PositionBasedCrossOver( child );
		}

		#endregion

        public void SetExandingGeneticsArray( int maxLength, int startLength, int incrementLength )
        {
            if( startLength > maxLength || incrementLength > maxLength )
                throw new GeneticsLibraryException( "The parameters passed for the expanding genetics array of a maxlength of " + maxLength.ToString() +
                    " a startlength of " + startLength.ToString() + " and an incrementlength of " + incrementLength.ToString() + " is complete rubbish try again." );


            IsFixedLength = false;
            MaxLength = maxLength;
            StartingLength = startLength;
            IncrementLength = incrementLength;
            CurrentLength = StartingLength;
            ChromosomeLength = StartingLength;
        }
	}
}
