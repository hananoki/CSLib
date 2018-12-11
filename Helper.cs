using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace AppCommon {

	public static class JsonUtils {
		public static string ToJson<T>( T obj ) {
			var builder = new StringBuilder();
			var writer = new LitJson.JsonWriter( builder ) {
				PrettyPrint = true
			};
			LitJson.JsonMapper.ToJson( obj, writer );
			return builder.ToString();
		}
	}

	public static partial class Helper {
		/// <summary>
		/// 
		/// </summary>
		/// <param name="value"></param>
		public static void SetCurrentDirectory( string value ) {
			rt.setCurrentDir( value );
			Debug.Log( "setCurrentDir > {0}", value );
		}


		/// <summary>
		/// このプロセスのみ有効な環境変数を設定します
		/// </summary>
		/// <param name="path"></param>
		public static void SetEnvironmentPath( string path ) {
			rt.setEnv( "PATH", path, EnvironmentVariableTarget.Process );

			Debug.Log( "AddEnvPath > {0}".format( path ) );
		}
	}

	public static class Debug {
		[System.Diagnostics.Conditional( "DEBUG" )]
		public static void AllocConsole() {
			Win32.AllocConsole();
		}

		public static void Log( int m ) {
			Console.WriteLine( m );
		}

		public static void Error( Exception e ) {
			Console.WriteLine( e.ToString() );
		}

		public static void Log( string m, params object[] args ) {
#if TRACE
			if( string.IsNullOrEmpty( m ) ) return;
			Console.WriteLine( string.Format( m, args ) );
#endif
		}
	}

	public static partial class rt {
		public static string getEnv( string variable, EnvironmentVariableTarget target ) {
			return Environment.GetEnvironmentVariable( variable, target );
		}
		public static void setEnv( string variable, string value, EnvironmentVariableTarget target ) {
			Environment.SetEnvironmentVariable( variable, value, target );
		}

		public static string getCurrentDir() {
			return System.Environment.CurrentDirectory;
		}
		public static void setCurrentDir( string value ) {
			System.Environment.CurrentDirectory = value;
		}

		public static void sleep( int millisecondsTimeout ) {
			Thread.Sleep( millisecondsTimeout );
		}
	}

}
