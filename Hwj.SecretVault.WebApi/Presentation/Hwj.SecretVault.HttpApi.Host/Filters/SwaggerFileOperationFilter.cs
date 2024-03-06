using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Hwj.SecretVault.HttpApi.Host.Filters
{
    /// <summary>
    /// Swaager支持文件上传Filter
    /// </summary>
    public class SwaggerFileOperationFilter : IOperationFilter
    {
        /// <summary>
        /// 实现函数
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="context"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            string fileUploadMime = "multipart/form-data";
            // 检查 Swagger 文档中的请求体内容是否包含文件上传的 MIME 类型
            if (operation.RequestBody == null || !operation.RequestBody.Content.Any(x => x.Key.Equals(fileUploadMime, StringComparison.InvariantCultureIgnoreCase)))
                return;
            // 获取包含文件上传类型的参数集合
            var filesParams = context.MethodInfo.GetParameters().Where(p => p.ParameterType == typeof(IFormFile) || p.ParameterType == typeof(IFormFileCollection));
            foreach (var item in filesParams)
            {
                if (item.ParameterType == typeof(IFormFile))
                {
                    operation.RequestBody.Content[fileUploadMime].Schema.Properties = filesParams?.ToDictionary(k => k.Name!, v => new OpenApiSchema()
                    {
                        Type = "file",
                        Format = "binary"
                    });
                }
                if (item.ParameterType == typeof(IFormFileCollection))
                {
                    operation.RequestBody.Content[fileUploadMime].Schema.Properties = filesParams?.ToDictionary(k => k.Name!, v => new OpenApiSchema()
                    {
                        Type = "array",
                        Items = new OpenApiSchema()
                        {
                            Type = "file",
                            Format = "binary"
                        }
                    });
                }
            }
        }
    }
}