using System;

namespace CsLib {
	public static class Debug {
		[System.Diagnostics.Conditional( "DEBUG" )]
		public static void AllocConsole() {
			Win32.AllocConsole();
		}

		[System.Diagnostics.Conditional( "DEBUG" )]
		public static void Log( int m ) {
			Console.WriteLine( m );
		}

		[System.Diagnostics.Conditional( "DEBUG" )]
		public static void Error( Exception e ) {
			Console.WriteLine( e.ToString() );
		}

		[System.Diagnostics.Conditional( "DEBUG" )]
		public static void Log( string m, params object[] args ) {
#if TRACE
			if( string.IsNullOrEmpty( m ) ) return;
			Console.WriteLine( string.Format( m, args ) );
			CsLib.Log.Info( string.Format( m, args ) );
#endif
		}
	}
}
