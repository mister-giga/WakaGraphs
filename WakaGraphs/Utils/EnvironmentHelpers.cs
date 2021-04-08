using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WakaGraphs.Utils
{
    class EnvironmentHelpers
    {
        public static string GetEnvVariable(string variable, string def = null, bool required = false)
        {
            var value = Environment.GetEnvironmentVariable(variable
#if DEBUG
                , EnvironmentVariableTarget.User
#endif
                );

            if(required && value == null)
                throw new ArgumentException($"{variable} is not provided", variable);

            if(value == null)
                return def;
            return value;
        }

        public static bool GetEnvVariable(string variable, bool def, bool required = false) => GetEnvVariable(variable, def.ToString(), required).Equals("True", StringComparison.OrdinalIgnoreCase);

    }
}
