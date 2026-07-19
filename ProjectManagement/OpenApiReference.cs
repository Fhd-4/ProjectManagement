using Microsoft.OpenApi;

namespace ProjectManagement
{
    internal class CustomOpenApiReference
    {
        public ReferenceType Type { get; set; }
        public string? Id { get; set; }
    }
}