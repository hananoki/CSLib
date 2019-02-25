using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsLib {
	public static class JsonUtils {
		public static string ToJson<T>( T obj, bool newline = true ) {
			var builder = new StringBuilder();
			var writer = new LitJson.JsonWriter( builder ) {
				PrettyPrint = newline
			};
			LitJson.JsonMapper.ToJson( obj, writer );
			return builder.ToString();
		}
	}
}
