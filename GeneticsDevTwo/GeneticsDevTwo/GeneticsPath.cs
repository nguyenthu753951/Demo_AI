// GeneticsDevTwo Demonstrate Genetic Algorythm Techniques
// Copyright (C) 2006 pseudonym67
// 
// This program is free software; you can redistribute it and/or
// modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation; either version 2
// of the License, or (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA  02111-1307, USA.


using System;
using GeneticsLibrary;
using System.Collections;
using System.Text;
using RandomNumber;
using UsefulClasses;
using System.Drawing;
using System.Threading;
using System.Diagnostics;

namespace GeneticsDevTwo
{
	public enum PathDirections{ Up, Down, Left, Right, MaxValue };
	
	/// <summary>
	/// The GenticsPathItem is a piece of the string
	/// containing the direction and a bool value for if that 
	/// direction is valid
	/// </summary>
	public class GeneticsPathItem
	{
		private string strDirection;
		private bool bValid;
        private string strIdentifier;
		
		public string Direction
		{
			get
			{
				return strDirection;
			}
			set
			{
				strDirection = value;
			}
		}
		
		public bool Valid
		{
			get
			{
				return bValid;
			}
			set
			{
				bValid = value;
			}
		}

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
		
		public GeneticsPathItem( string direction, bool valid )
		{
			strDirection = direction;
			bValid = valid;
		}

        public GeneticsPathItem( string direction, bool valid, string identifier ) : this( direction, valid )
        {
            strIdentifier = identifier;
        }
            
	}

	public class GeneticsPathString : GeneticsStrings
	{
		/// <summary>
		/// Initial Starting length for a chromosome
		/// </summary>
		public static int InitialChromosomeLength = 10;

		/// <summary>
		/// How far did the chromosome travel before bumping into a dead end
		/// </summary>
		private int nLengthTravelled;

		/// <summary>
		/// Has the string reached the finish square
		/// </summary>
		private bool bHasReachedFinish;
		
		/// <summary>
		/// TextBox for printing progress messages
		/// </summary>
		private static ThreadSafeTextBox tsTextBox;

        /// <summary>
        /// reference to the board to check local information;
        /// </summary>
        private GeneticsBoard geneticsBoard;

        private bool bUseLocalInformation;
		
		/// Settings to control which progress strings to print
		/// 
		public static bool bShowCreationStrings = true;

        private static RandomNumberGenerator random = null;
		
 
		public int LengthTravelled
		{
			get
			{
				return nLengthTravelled;
			}
			set
			{
				nLengthTravelled = value;
			}
		}

		public bool HasReachedFinish
		{
			get
			{
				return bHasReachedFinish;
			}
			set
			{
				bHasReachedFinish = value;
			}
		}
		
		public ThreadSafeTextBox TextBox
		{
			get
			{
				return tsTextBox;
			}
			set
			{
				tsTextBox = value;
			}
		}
		
		public bool ShowCreationStrings
		{
			get
			{
				return bShowCreationStrings;
			}
			set
			{
				bShowCreationStrings = value;
			}
		}

        public bool UseLocalInformation
        {
            get
            {
                return bUseLocalInformation;
            }
            set
            {
                bUseLocalInformation = value;
            }
        }

		public GeneticsPathString()
		{
			ChromosomeLength = InitialChromosomeLength;
			nLengthTravelled = 0;
			if( random == null )
				random = new RandomNumberGenerator( true );

			InitialiseString();
		}
		
		public GeneticsPathString( ThreadSafeTextBox textBox ) 
		{
			ChromosomeLength = InitialChromosomeLength;
			nLengthTravelled = 0;
			tsTextBox = textBox;
			if( random == null )
				random = new RandomNumberGenerator( true );

			InitialiseString();
		}

        public GeneticsPathString( ref GeneticsBoard board, ThreadSafeTextBox textBox, bool useLocalInformation )
        {
            ChromosomeLength = InitialChromosomeLength;
            nLengthTravelled = 0;
            tsTextBox = textBox;
            if( random == null )
                random = new RandomNumberGenerator( true );

            geneticsBoard = board;
            bUseLocalInformation = useLocalInformation;
            InitialiseString();
        }

		public GeneticsPathString( bool initialise )
		{
			ChromosomeLength = InitialChromosomeLength;
			if( random == null )
				random = new RandomNumberGenerator( true );

			if( initialise == true )
			{
				nLengthTravelled = 0;
				InitialiseString();
			}
		}


		public override void InitialiseString()
		{
			int nValue = 0;
			int nCount = 0;

			while( nCount != ChromosomeLength )
			{
				nValue = random.Random( ( int )PathDirections.MaxValue );

                if( UseLocalInformation == true )
                {
                    /// if using local information see which paths are valid for the square
                    /// 
                    string strSquare = geneticsBoard.GetStartSquare();
                    SquareInformation squareInfo = ( SquareInformation )geneticsBoard.GetSquareInformation( strSquare );
                    GeneticsPathItem pathItem = null;
                    string strCurrentSquare;
                    
                    if( GeneticsString.Count > 0 )
                    {
                        pathItem = ( GeneticsPathItem )GeneticsString[ GeneticsString.Count-1 ];
                        strCurrentSquare = pathItem.Identifier;
                    }
                    else
                        strCurrentSquare = strSquare;

                    bool bIsValid = false;

                    while( bIsValid != true )
                    {
                        nValue = random.Random( ( int )PathDirections.MaxValue );

				        squareInfo = geneticsBoard.GetSquareInformation( strCurrentSquare );

                        switch( nValue )
                        {
                            case ( int )PathDirections.Up:
                            {
                                if( squareInfo.Up == true )
                                {
                                    GeneticsString.Add( new GeneticsPathItem( "Up", false, strCurrentSquare ) );
                                    bIsValid = true;
                                }
                            }break;
                            case ( int )PathDirections.Right:
                            {
                                if( squareInfo.Right == true )
                                {
                                    GeneticsString.Add( new GeneticsPathItem( "Right", false, strCurrentSquare ) );
                                    bIsValid = true;
                                }
                            }break;
                            case ( int )PathDirections.Down:
                            {
                                if( squareInfo.Down == true )
                                {
                                    GeneticsString.Add( new GeneticsPathItem( "Down", false, strCurrentSquare ) );
                                    bIsValid = true;
                                }
                            }break;
                            case ( int )PathDirections.Left:
                                {
                                    if( squareInfo.Left == true )
                                    {
                                        GeneticsString.Add( new GeneticsPathItem( "Left", false, strCurrentSquare ) );
                                        bIsValid = true;
                                    }
                                }break;
                        }
                    }

                    nCount++;
                }
                else
                {
				    switch( nValue )
				    {
						    case ( int )PathDirections.Up: 
						    {
							    GeneticsString.Add( new GeneticsPathItem( "Up", false ) );
							    nCount++;
						    }break;
						    case ( int )PathDirections.Down: 
						    {
							    GeneticsString.Add( new GeneticsPathItem( "Down", false ) );
							    nCount++;
						    }break;
						    case ( int )PathDirections.Left: 
						    {
							    GeneticsString.Add( new GeneticsPathItem( "Left", false ) );
							    nCount++;
						    }break;
						    case ( int )PathDirections.Right: 
						    {
							    GeneticsString.Add( new GeneticsPathItem( "Right", false ) );
							    nCount++;
						    }break;
				    }
				}

			}
			
			
			if( ShowCreationStrings == true )
                tsTextBox.AppendTextWithColour( ToString(), Color.Black );
		}

		public override string ToString()
		{
			StringBuilder strString = new StringBuilder( "Directions taken by this chromosome :- " );

			for( int i=0; i<GeneticsString.Count; i++ )
			{
				strString.Append( " " + ( ( GeneticsPathItem )GeneticsString[ i ] ).Direction );
			}

			return strString.ToString();
		}
		
		public string FullDebugString()
		{
			StringBuilder strString = new StringBuilder( "Paths taken by this chromosome :- " );
			
			for( int i=0; i<GeneticsString.Count; i++ )
			{
				strString.Append( "\n Direction :- " + ( ( GeneticsPathItem )GeneticsString[ i ] ).Direction + " Valid :- " + ( ( GeneticsPathItem )GeneticsString[ i ] ).Valid.ToString() );
			}
			
			return strString.ToString();
		}
	}


	/// <summary>
	/// Summary description for GeneticsPath.
	/// </summary>
	public class GeneticsPath : GeneticsBase
	{
		/// <summary>
		/// Elitism Value for setting the elites. 
		/// In this project this will be initially based on the strings that get the furthest through the maze 
		/// and then later it will be who can get to the finish in the shortest time.
		/// </summary>
		private int nElitismValue;

		/// <summary>
		/// have any of the genetic strings managed to reach the finish
		/// </summary>
		private bool bIsFinishFound;


		private ArrayList elitismList;
		ArrayList holdingList;
		
		/// <summary>
		/// Text box for printing progress
		/// </summary>
		private ThreadSafeTextBox tsTextBox;

        /// <summary>
        /// reference to the board for using local information
        /// </summary>
        private GeneticsBoard geneticsBoard;

        private bool bUseLocalInformation;
		
		/// settings for display strings
		/// 
		private bool bShowCreationStrings = true;
		private bool bShowSinglePointCrossoverDetails = true;
		private bool bDebugRun = false;

        private RandomNumberGenerator random = null;
        
	    private static Mutex geneticsArrayMutex = null;

		public int ElitismValue
		{
			get
			{
				return nElitismValue;
			}
			set
			{
				nElitismValue = value;
			}
		}

		public bool IsFinishFound
		{
			get
			{
				return bIsFinishFound;
			}
			set
			{
				bIsFinishFound = value;
			}
		}
		
		public ThreadSafeTextBox TextBox
		{
			get
			{
				return tsTextBox;
			}
			set
			{
				tsTextBox = value;
			}
		}
		
		public bool ShowCreationStrings
		{
			get
			{
				return bShowCreationStrings;
			}
			set
			{
				bShowCreationStrings = value;
			}
		}
		
		public Mutex GeneticsArrayMutex
		{
			get
			{
				return geneticsArrayMutex;
			}
			set
			{
				geneticsArrayMutex = value;
			}
		}

        public bool UseLocalInformation 
        {
            get
            {
                return bUseLocalInformation;
            }
            set
            {
                bUseLocalInformation = value;
            }
        }

		public GeneticsPath()
		{
			bIsFinishFound = false;
			nElitismValue = 0;
			elitismList = new ArrayList();
			holdingList = new ArrayList();
			UseElitism = true;
			UseGenerations = true;
			tsTextBox = null;
            random = new RandomNumberGenerator( true );
            bUseLocalInformation = false;
		}
		
		public GeneticsPath( ThreadSafeTextBox textBox ) : this()
		{
			tsTextBox = textBox;
		}

        public GeneticsPath( ref GeneticsBoard board, ThreadSafeTextBox textBox, bool useLocalInformation ) : this()
        {
            tsTextBox = textBox;
            geneticsBoard = board;
            bUseLocalInformation = useLocalInformation;
        }
		
		/// <summary>
		/// Cycle through the array and set all GeneticPathItems to false
		/// </summary>
		public void ResetValid()
		{
			for( int i=0; i<GeneticsArray.Count; i++ )
			{
				GeneticsPathString strTemp = ( GeneticsPathString )GeneticsArray[ i ];

				for( int j=0;j<strTemp.GeneticsString.Count; j++ )
				{
					( ( GeneticsPathItem )strTemp.GeneticsString[ j ] ).Valid = false;
				}
			}
		}

		/// <summary>
		/// Initialise the genetics array
		/// </summary>
		public override void Initialise()
		{
			GeneticsArray.Clear();
			
			if( ShowCreationStrings == true )
				GeneticsPathString.bShowCreationStrings = true;
			else
				GeneticsPathString.bShowCreationStrings = false;

			/// set up the new strings
			/// 
			
			for( int i=0; i<PopulationSize; i++ )
			{
				if( tsTextBox != null )
				{
                    if( UseLocalInformation == false )
                    {
					    GeneticsArray.Add( new GeneticsPathString( tsTextBox ) );
                    }
                    else
                    {
                        GeneticsArray.Add( new GeneticsPathString( ref geneticsBoard, tsTextBox, true ) );
                    }
				}
				else
					GeneticsArray.Add( new GeneticsPathString() );
				
				if( tsTextBox != null )
				{
					tsTextBox.AppendTextWithColour( ".", Color.Black, false );
				}
			}
			
			CurrentLength = StartingLength;
			
			if( tsTextBox != null )
				tsTextBox.AppendTextWithColour( "\n", Color.Black, false );
		}

		/// <summary>
		/// randomly mutate one of the directions in the GeneticsPath
		/// or not
		/// Note Mutation is applied at the end of a run to all items in the array
		/// </summary>
		public override void Mutation()
		{
		///	RandomNumberGenerator randLocation = new RandomNumberGenerator( true );
		///	Random rand = new Random();
		///	Random randLocation = new Random();
			int nLocation = 0;
			int nValue = 0;
			GeneticsPathString gpsTemp = null;
            GeneticsPathItem gpiTemp = null;

			for( int i=0; i<PopulationSize; i++ )
			{
				if( random.BetweenZeroAndOne() < MutationRate )
				{
					gpsTemp = ( GeneticsPathString )GeneticsArray[ i ];

					/// Do not mutate sucessful strings
					/// 
					if( gpsTemp.HasReachedFinish == true )
						continue;

					nLocation = random.Random( gpsTemp.GeneticsString.Count );
                    gpiTemp = ( GeneticsPathItem )gpsTemp.GeneticsString[ nLocation ];

				///	nLocation = rand.Next( gpsTemp.GeneticsString.Count );

				///	System.Threading.Thread.Sleep( 2 );

					nValue = random.Random( ( int )PathDirections.MaxValue );
				///	nValue = rand.Next( ( int )PathDirections.MaxValue );

					switch( nValue )
					{
						case ( int )PathDirections.Up: 
						{
							gpsTemp.GeneticsString.RemoveAt( nLocation ); 
                            if( UseLocalInformation == true )
                            {
                                gpsTemp.GeneticsString.Insert( nLocation, new GeneticsPathItem( "Up", false, gpiTemp.Identifier ) );
                            }
                            else
							    gpsTemp.GeneticsString.Insert( nLocation, new GeneticsPathItem( "Up", false ) );
						}break;
						case ( int )PathDirections.Down: 
						{
							gpsTemp.GeneticsString.RemoveAt( nLocation );
                            if( UseLocalInformation == true )
                            {
                                gpsTemp.GeneticsString.Insert( nLocation, new GeneticsPathItem( "Down", false, gpiTemp.Identifier ) );
                            }
                            else
							    gpsTemp.GeneticsString.Insert( nLocation, new GeneticsPathItem( "Down", false ) );
						}break;
						case ( int )PathDirections.Left: 
						{
							gpsTemp.GeneticsString.RemoveAt( nLocation );
                            if( UseLocalInformation == true )
                            {
                                gpsTemp.GeneticsString.Insert( nLocation, new GeneticsPathItem( "Left", false, gpiTemp.Identifier ) );
                            }
                            else
							    gpsTemp.GeneticsString.Insert( nLocation, new GeneticsPathItem( "Left", false ) );
						}break;
						case ( int )PathDirections.Right: 
						{
							gpsTemp.GeneticsString.RemoveAt( nLocation );
                            if( UseLocalInformation == true )
                            {
                                gpsTemp.GeneticsString.Insert( nLocation, new GeneticsPathItem( "Right", false, gpiTemp.Identifier ) );
                            }
                            else
							    gpsTemp.GeneticsString.Insert( nLocation, new GeneticsPathItem( "Right", false ) );
						}break;
					}
				}

			///	System.Threading.Thread.Sleep( 2 );
			}
		}

		public override void Run()
		{
			holdingList.Clear();
			int nArrayCount = 0;
			int nMaxSize = 0;
			GeneticsPathString gpsTemp = null;
			
			if( bDebugRun == true )
				Debug.WriteLine( "Run started, getting longest travelled length" );
			
			for( int i=0; i<PopulationSize; i++ )
			{
				gpsTemp = ( GeneticsPathString )GeneticsArray[ i ];
				
				/// get the maximum travelled length
				/// 
				if( gpsTemp.LengthTravelled >= nMaxSize )
				{
					nMaxSize = gpsTemp.LengthTravelled;
				}
			}
			
			for( int i=0; i<PopulationSize; i++ )
			{
				gpsTemp = new GeneticsPathString( false );

				/// set the values for the fixed point selection
				/// 
				if( nArrayCount+1 < GeneticsArray.Count )
				{
					FixedPointSelectionOne = nArrayCount;
					FixedPointSelectionTwo = nArrayCount+1;
				}
				else
				{
					nArrayCount = 0;
					FixedPointSelectionOne = nArrayCount;
					FixedPointSelectionTwo = nArrayCount+1;
				}
				
				nArrayCount++;
				
				if( bDebugRun == true )
					Debug.WriteLine( "Calling Selection for population number " + i.ToString() );
                try
                {
				    Selection();
				
				    if( bDebugRun == true )
					    Debug.WriteLine( "Selection finshed calling CrossOver for population number " + i.ToString() );

				    CrossOver( gpsTemp );
                }
                catch( GeneticsLibraryException genExp )
                {
                    tsTextBox.AppendTextWithColour( genExp.Message(), Color.Red );

                    if( bDebugRun == true )
                        Debug.WriteLine( genExp.Message() );
                }

				holdingList.Add( gpsTemp );
			}
		
			if( bDebugRun == true )
				Debug.WriteLine( "Waiting for the mutex to release" );
			
			if( GeneticsArrayMutex != null )
				GeneticsArrayMutex.WaitOne( 3000, true );

			if( UseElitism == true )
			{
				if( bDebugRun == true )
					Debug.WriteLine( "Adding Elite Values" );
				
				AddEliteValues();	
			}

			GeneticsArray.Clear();

			for( int i=0; i<( PopulationSize - elitismList.Count ); i++ )
			{
				GeneticsArray.Add( holdingList[ i ] );
			}

			for( int i=0; i<elitismList.Count; i++ )
			{
				GeneticsArray.Add( elitismList[ i ] );
			}

			if( bDebugRun == true )
				Debug.WriteLine( "Calling mutation" );
			
			Mutation();
			
			/// Need to clean up the single crossover code
			/// so do it all in one place.
			/// 
			if( CrossOverMethod == CROSSOVERMETHODS.SINGLEPOINTCROSSOVER )
			{
				if( nMaxSize != 0 )
				{
					/// if the string is almost at the end of the current range
					/// concentrate on that area to give a boost
					/// 
					if( IsFixedLength == false && nMaxSize+3 >= CurrentLength )
						SingleCrossOverPoint = nMaxSize;
			
					if( IsFixedLength == false && SingleCrossOverPoint+3 >= nMaxSize && nMaxSize > ( CurrentLength/2 ) )
						SingleCrossOverPoint = nMaxSize;
				}
			}

			/// if a string has not reached the finish grow the genetics string
			/// but only of course if we are using a growing string
            /// Only Grow the string when string Items have reached the full length
            /// of the string. ie when necassary.
            /// 

            if( IsFixedLength == false && nMaxSize == CurrentLength )
            {
            	if( bDebugRun == true )
            		Debug.WriteLine( "Growing string : nMaxSize = " + nMaxSize.ToString() + " Current Length = " + CurrentLength.ToString() );
            	
            	if( CurrentLength == MaxLength )
            	{
            		tsTextBox.AppendTextWithColour( "Strings have reached the maximum length allowed", Color.Black );

                    if( bDebugRun == true )
                        Debug.WriteLine( "Strings have reached the maximum allowed length" );
            		
            		if( GeneticsArrayMutex != null )
						GeneticsArrayMutex.ReleaseMutex();
            		
            		return;
            	}

                GeneticsPathString gpsGrowTemp = null;
              ///  Random growRand = new Random();
                int nGrowValue = 0;

                /// Update the genetics string required length
                /// 

               ///	UpdateExpandingGeneticsArray();
             
              	///SingleCrossOverPoint = CurrentLength;
                if( ( CurrentLength + IncrementLength ) >= MaxLength )
            	{
                	CurrentLength = MaxLength;
            	}
            	else
					CurrentLength += IncrementLength;
               
                if( bShowSinglePointCrossoverDetails == true )
                {
                	tsTextBox.AppendTextWithColour( "Single Point CrossOver info\nCurrent Length = " + this.CurrentLength.ToString() + "\n Single Point Position " + this.SingleCrossOverPoint.ToString(), Color.DarkMagenta );
                }


                for( int i = 0; i < PopulationSize; i++ )
                {
                    gpsGrowTemp = ( GeneticsPathString )GeneticsArray[ i ];

                    /// don't grow successful strings
                    /// 
                    if( gpsGrowTemp.HasReachedFinish == true )
                    {
                       	Debug.Write( "A Genetics String has reached the finish\n" );
                        continue;
                    }
                    
                    if( bDebugRun == true )
                    	Debug.WriteLine( "Increasing the string length for population item " + i.ToString() + " Current Length = " + CurrentLength.ToString() + " Increment Length = " + IncrementLength.ToString() );
                    
                    int nCount = 0;
                    
                    while( nCount < IncrementLength )
                    {
                        nGrowValue = random.Random( ( int )PathDirections.MaxValue );

                        if( UseLocalInformation == true )
                        {
                            if( bDebugRun == true )
                                Debug.WriteLine( "Use Local Information is set to true" );

                            /// if using local information see which paths are valid for the square
                            /// 
                            string strSquare = geneticsBoard.GetStartSquare();
                            SquareInformation squareInfo = ( SquareInformation )geneticsBoard.GetSquareInformation( strSquare );
                            GeneticsPathItem pathItem = null;
                            string strCurrentSquare;


                            /// Need to find out where the current string has gotten to before you can add to it
                            /// 
                            
                            pathItem = ( GeneticsPathItem )gpsGrowTemp.GeneticsString[ gpsGrowTemp.GeneticsString.Count-1 ];
                            strCurrentSquare = pathItem.Identifier;
							
                            bool bIsValid = false;

                            while( bIsValid != true )
                            {
                                nGrowValue = random.Random( ( int )PathDirections.MaxValue );

				                squareInfo = geneticsBoard.GetSquareInformation( strCurrentSquare );

                                switch( nGrowValue )
                                {
                                    case ( int )PathDirections.Up:
                                    {
                                        if( squareInfo.Up == true )
                                        {
                                            gpsGrowTemp.GeneticsString.Add( new GeneticsPathItem( "Up", false, strCurrentSquare ) );
                                            bIsValid = true;
                                        }
                                    }break;
                                    case ( int )PathDirections.Right:
                                    {
                                        if( squareInfo.Right == true )
                                        {
                                            gpsGrowTemp.GeneticsString.Add( new GeneticsPathItem( "Right", false, strCurrentSquare ) );
                                            bIsValid = true;
                                        }
                                    }break;
                                    case ( int )PathDirections.Down:
                                    {
                                        if( squareInfo.Down == true )
                                        {
                                            gpsGrowTemp.GeneticsString.Add( new GeneticsPathItem( "Down", false, strCurrentSquare ) );
                                            bIsValid = true;
                                        }
                                    }break;
                                    case ( int )PathDirections.Left:
                                    {
                                        if( squareInfo.Left == true )
                                        {
                                            gpsGrowTemp.GeneticsString.Add( new GeneticsPathItem( "Left", false, strCurrentSquare ) );
                                            bIsValid = true;
                                        }
                                    }break;
                                }
                            }

                            nCount++;
                        }
                        else
                        {

                            switch( nGrowValue )
                            {
                                case( int )PathDirections.Up:
                                {
                            		gpsGrowTemp.GeneticsString.Add( new GeneticsPathItem( "Up", false ) );
                            		nCount++;
                                } break;
                                case( int )PathDirections.Down:
                            	{
                            		gpsGrowTemp.GeneticsString.Add( new GeneticsPathItem( "Down", false ) );
                            		nCount++;
                                } break;
                                case( int )PathDirections.Left:
                                {
                            		gpsGrowTemp.GeneticsString.Add( new GeneticsPathItem( "Left", false ) );
                            		nCount++;
                                } break;
                                case( int )PathDirections.Right:
                                {
                            		gpsGrowTemp.GeneticsString.Add( new GeneticsPathItem( "Right", false ) );
                            		nCount++;
                                } break;
                            }
                        }
                    }
                }
            }
            
            if( GeneticsArrayMutex != null )
				GeneticsArrayMutex.ReleaseMutex();
		}

		private void AddEliteValues()
		{
			int nFurthestThrough = 0;
			int nCurrentNumber = 0;

			elitismList.Clear();

			/// if you haven't yet found the finish then the elites are the ones that are moving furthest through
			/// the map.
			if( IsFinishFound == false )
			{

				/// Get the highest value
				/// 
				for( int i=0; i<PopulationSize; i++ )
				{
					if( ( ( GeneticsPathString )GeneticsArray[ i ] ).LengthTravelled > nFurthestThrough )
					{
						nFurthestThrough = ( ( GeneticsPathString )GeneticsArray[ i ] ).LengthTravelled;
					}
				}

				/// now get NumberOfElites
				/// 

				nCurrentNumber = nFurthestThrough;

				while( elitismList.Count != NumberOfElites )
				{
					for( int i=0; i<PopulationSize; i++ )
					{
						if( ( ( GeneticsPathString )GeneticsArray[ i ] ).LengthTravelled == nCurrentNumber )
						{
							/// double check plus it is possible that more than NumberOfElites will qualify
							/// 
							if( elitismList.Count < NumberOfElites )
							{
								elitismList.Add( GeneticsArray[ i ] );
							}
						}
					}

					nCurrentNumber--;
				}
			}
			else
			{
				/// if the finish is found then the best is the that gets to the finish quickest.
				/// 

				for( int i=0; i<PopulationSize; i++ )
				{
					if( ( ( GeneticsPathString )GeneticsArray[ i ] ).LengthTravelled > nFurthestThrough
						&& ( ( GeneticsPathString )GeneticsArray[ i ] ).HasReachedFinish == true )
					{
						nFurthestThrough = ( ( GeneticsPathString )GeneticsArray[ i ] ).LengthTravelled;
					}
				}

				nCurrentNumber = nFurthestThrough;

				while( elitismList.Count != NumberOfElites && nCurrentNumber != 0 )
				{
					for( int i=0; i<PopulationSize; i++ )
					{
						if( ( ( GeneticsPathString )GeneticsArray[ i ] ).LengthTravelled == nCurrentNumber
							&& ( ( GeneticsPathString )GeneticsArray[ i ] ).HasReachedFinish == true )
						{
							/// double check plus it is possible that more than NumberOfElites will qualify
							/// 
							if( elitismList.Count < NumberOfElites )
							{
								elitismList.Add( GeneticsArray[ i ] );
							}
						}
					}

					nCurrentNumber--;
				}
			}
		}
	}
}
