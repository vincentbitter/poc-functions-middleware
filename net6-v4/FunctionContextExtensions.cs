using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;

namespace Poc
{
    public static class FunctionContextExtensions
    {
        public static HttpRequestData GetHttpRequestData(this FunctionContext functionContext)
        {
            try
            {
                var keyValuePair = functionContext.Features.SingleOrDefault(f => f.Key.Name == "IFunctionBindingsFeature");
                var functionBindingsFeature = keyValuePair.Value;
                var type = functionBindingsFeature.GetType();
                var inputData = type.GetProperty("InputData")
                    .GetValue(functionBindingsFeature) as IReadOnlyDictionary<string, object>;
                return inputData?.Values.SingleOrDefault(o => o is HttpRequestData) as HttpRequestData;
            }
            catch
            {
                return null;
            }
        }

        public static void SendHttpResponse(this FunctionContext context, HttpResponseData response)
        {
            var functionBindingsFeature = context.Features.SingleOrDefault(f => f.Key.Name == "IFunctionBindingsFeature").Value;
            var functionBindingsFeatureType = functionBindingsFeature.GetType();

            var outputBindingsInfo = functionBindingsFeatureType.GetProperty("OutputBindingsInfo").GetValue(functionBindingsFeature);
            var outputBindingsInfoType = outputBindingsInfo.GetType();
            if (outputBindingsInfoType.Name == "PropertyOutputBindingsInfo")
            {
                var propertyNames = GetPrivateFieldInfo(outputBindingsInfoType, "_propertyNames")
                    .GetValue(outputBindingsInfo);

                var list = propertyNames as IEnumerable<string>;
                if (!list.Contains("Response"))
                    throw new Exception("Function return type does not contain a Response property");

                GetPrivateFieldInfo(functionBindingsFeatureType, "_outputData")
                    .SetValue(functionBindingsFeature, new Dictionary<string, object> {
                        { "Response", response }
                    });
            }
            else
            {
                functionBindingsFeatureType.GetProperty("InvocationResult").SetValue(functionBindingsFeature, response);
            }
        }

        private static FieldInfo GetPrivateFieldInfo(Type type, string name)
        {
            return type.GetField(name, BindingFlags.NonPublic | BindingFlags.Instance);
        }
    }
}