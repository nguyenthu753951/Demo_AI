/*
 * Created by SharpDevelop.
 * User: pseudonym67
 * Date: 03/03/2006
 * Time: 09:21
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

 /* The following class is a pseudorandom number generator
  * originally developed by Takuji Nishimura and Makoto Matsumoto
  * This is a C# implementation of the C++ code in Game Coding
  * Complete by Mike McShaffry
  */
 
using System;

namespace RandomNumber
{
	/// <summary>
	/// Description of RandomNumberGenerator.
	/// </summary>
	public class RandomNumberGenerator
	{
		/// Period parameters
		/// 
		
		private const int CMATH_N = 624;
		private const int CMATH_M = 397;
		private const uint CMATH_MATRIX_A = 0x9908b0df; /// constant vector A
		private const uint CMATH_UPPER_MASK = 0x80000000; /// most significant w-r bits
		private const uint CMATH_LOWER_MASK = 0x7fffffff; /// least significant r bits
		
		/// Tempering parameters
		/// 
		
		private const uint CMATH_TEMPERINGMASK_B = 0x9d2c5680; 
		private const uint CMATH_TEMPERINGMASK_C = 0xefc60000;
		
		/// Data
		/// 
		
		private ulong rseed;
		private ulong[] mt;
		private int mti;
		private static ulong[] mag01 = new ulong[ 2 ];
		
		public RandomNumberGenerator()
		{
			mt = new ulong[ CMATH_N ];
			rseed = 1;
			mti = CMATH_N + 1;
			mag01[ 0 ] = 0x0;
			mag01[ 1 ] = CMATH_MATRIX_A;
		}
		
		/// <summary>
		/// Ease of use constructor for one line initialisation.
		/// </summary>
		/// <param name="randomize"></param>
		/// <returns></returns>
		public RandomNumberGenerator( bool randomise ) : this()
		{
			if( randomise == true )
			{
				Randomize();
			}
		}
		
		/// <summary>
		/// Constructor that allows the direct setting of the seed
		/// </summary>
		/// <param name="n"></param>
		/// <returns></returns>
		public RandomNumberGenerator( int n )
		{
			SetRandomSeed( n );
		}
		
		private ulong MathTemperingShiftU( ulong y )
		{
			return ( y >> 11 );
		}
		
		private ulong MathTemperingShiftS( ulong y )
		{
			return ( y << 7 );
		}
		
		private ulong MathTemperingShiftT( ulong y )
		{
			return ( y << 15 );
		}
		
		private ulong MathTemperingShiftL( ulong y )
		{
			return ( y >> 18 );
		}
	
		/// <summary>
		/// return a number from 0 - n
		/// </summary>
		/// <param name="n"> maximum number not included</param>
		/// <returns>random number</returns>
		public uint Random( uint n )
		{
			ulong y;
			
			if( n == 0 )
				return 0;
			
			
			if( mti >= CMATH_N )
			{
				int kk;
				
				/// seed generator not called
				/// 
				if( mti == CMATH_N+1 )
				{
					/// use default
					/// 
					SetRandomSeed( 4357 );
				}
				
				for( kk=0; kk<CMATH_N - CMATH_M; kk++ )
				{
					y = ( mt[ kk ] & CMATH_UPPER_MASK ) | ( mt[ kk ] & CMATH_LOWER_MASK );
					mt[ kk ] = mt[ kk + CMATH_M ] ^ ( y >> 1 ) ^ mag01[ y & 0x1 ];
				}
				
				for( ; kk<CMATH_N-1; kk++ )
				{
					y = ( mt[ kk ] & CMATH_UPPER_MASK ) | ( mt[ kk ] & CMATH_LOWER_MASK );
					mt[ kk ] = mt[ kk + ( CMATH_M-CMATH_N ) ] ^ ( y >> 1 ) ^ mag01[ y & 0x1 ];
				}
				
				y = ( mt[ CMATH_N-1 ] & CMATH_UPPER_MASK ) | ( mt[ 0 ] & CMATH_LOWER_MASK );
				mt[ CMATH_N-1 ] = mt[ CMATH_M-1 ] ^ ( y >> 1 ) ^ mag01[ y & 0x1 ];
				
				mti = 0;
			}
			
			y = mt[ mti++ ];
			y ^= MathTemperingShiftU( y );
			y ^= MathTemperingShiftS( y );
			y ^= MathTemperingShiftT( y );
			y ^= MathTemperingShiftL( y );
			
			return ( uint )( y%n );
			
		}
		
		/// <summary>
		/// return a number from 0-n
		/// override for the uint version 
		/// Just looks better without the casting in the code
		/// </summary>
		/// <param name="n"></param>
		/// <returns></returns>
		public int Random( int n )
		{
			return ( int )Random( ( uint )n );
		}
		
		/// <summary>
		/// Return a value between zero and one
		/// </summary>
		/// <returns></returns>
		public double BetweenZeroAndOne()
		{
			return ( ( double )Random( 100 ) ) / 100;
		}
		
		public void SetRandomSeed( long n )
		{
			/* setting initial seed to mt[ n ] using
			 * the generator line 25 of table 1 in
			  * Knuth 1981, The Art Of Computer Programming
			  * Vol 2 2nd Ed, pp107 
			  */
			  
			mt[ 0 ] = ( ulong )n & 0xffffffff;
			
			for( mti=1; mti<CMATH_N; mti++ )
			{
				mt[ mti ] = ( 69069 * mt[ mti -1 ] ) & 0xffffffff;
			}
			
			rseed = ( ulong )n;
		}
		
		/// <summary>
		/// overridden version taking an integer
		/// again looks better in code rather than any major practical use
		/// in fact care needs to be taken that the integer type can contain
		/// the value.
		/// </summary>
		/// <param name="n"></param>
		public void SetRandomSeed( int n )
		{
			SetRandomSeed( ( long )n );
		}
		
		public ulong GetRandomSeed()
		{
			return rseed;
		}
		
		public void Randomize()
		{
			SetRandomSeed( DateTime.Now.Ticks );
		}
	}
}
