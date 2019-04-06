using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CsLib {

	// Base64形式の汎用操作を提供するクラス
	public static class Base64 {
		// 指定した通常の文字列をUTF-8としてBase64文字列に変換する
		public static string Encode( string str ) {
			return Encode( str, Encoding.UTF8 );
		}
		// 上記のエンコードが指定できるバージョン
		public static string Encode( string str, Encoding encode ) {
			if( string.IsNullOrEmpty( str ) ) return "";
			return Convert.ToBase64String( encode.GetBytes( str ) );
		}

		// 指定したBase64文字列をUTF-8として通常の文字列に変換する
		public static string Decode( string base64Str ) {
			return Decode( base64Str, Encoding.UTF8 );
		}
		// 上記のエンコードが指定できるバージョン
		public static string Decode( string base64Str, Encoding encode ) {
			if( string.IsNullOrEmpty( base64Str ) ) return "";
			return encode.GetString( Convert.FromBase64String( base64Str ) );
		}
	}

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
		/// UACが有効になっているかを調べる
		/// </summary>
		/// <returns>UACが有効になっている時はtrue。</returns>
		public static bool IsUacEnabled() {
			//キーを開く
			Microsoft.Win32.RegistryKey regKey =
					Microsoft.Win32.Registry.LocalMachine.OpenSubKey(
							@"SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System" );
			//キーが見つからない時はUACが有効ではない
			if( regKey == null )
				return false;
			//値を読み取る
			int val = (int) regKey.GetValue( "EnableLUA", 0 );
			//0以外の時はUACが有効
			return val != 0;
		}

		public static bool IsAdministrator() {
			//現在のユーザーを表すWindowsIdentityオブジェクトを取得する
			System.Security.Principal.WindowsIdentity wi =
					System.Security.Principal.WindowsIdentity.GetCurrent();
			//WindowsPrincipalオブジェクトを作成する
			System.Security.Principal.WindowsPrincipal wp =
					new System.Security.Principal.WindowsPrincipal( wi );
			//Administratorsグループに属しているか調べる
			return wp.IsInRole(
					System.Security.Principal.WindowsBuiltInRole.Administrator );
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
