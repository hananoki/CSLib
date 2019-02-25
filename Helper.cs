using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CsLib {

	public static class EncodeHelper {
		public static Encoding Shift_JIS {
			get {
				return Encoding.GetEncoding( "shift_jis" );
			}
		}
	}

	public static class WindowsFormsExtension {
		public static void SetDoubleBuffered( this ListView listview, bool b ) {
			PropertyInfo prop = listview.GetType().GetProperty( "DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic );
			prop.SetValue( listview, b, null );
		}
	}


	public static partial class Helper {

		public static ParallelOptions m_parallelOptions = new ParallelOptions();

		public static string m_appName;
		public static string m_appPath;

		public static string m_configPath {
			get {
				return $"{m_appPath}\\{m_appName}.json";
			}
		}

		public static void _init() {
			var location = Assembly.GetExecutingAssembly().Location;
			m_appName = location.GetBaseName();

			var exePath = Directory.GetParent( location );
			m_appPath = exePath.FullName;

			var info = new Win32.SystemInfo();
			Win32.GetSystemInfo( out info );

			if( 1 < info.dwNumberOfProcessors ) {
				m_parallelOptions.MaxDegreeOfParallelism = (int) info.dwNumberOfProcessors - 1;
			}
		}

		

		public static void WriteJson( object obj, string filepath, bool newline = true ) {
			using( var st = new StreamWriter( filepath ) ) {
				string json = JsonUtils.ToJson( obj, newline );
				st.Write( json );
			}
		}


		public static bool ReadJson<T>( ref T obj, string filepath ) {
			try {
				using( var st = new StreamReader( filepath ) ) {
					obj = LitJson.JsonMapper.ToObject<T>( st.ReadToEnd() );
				}
			}
			catch( FileNotFoundException ) {
				Debug.Log( $"FileNotFoundException: {filepath} が見つかりません" );
				return false;
			}
			catch( Exception ) {
			}
			return true;
		}


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

			Debug.Log( $"AddEnvPath > {path}" );
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
