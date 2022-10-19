using Microsoft.FeatureManagement.FeatureFilters;

namespace FeaturesManagerPoC
{
    public class TestTargetingContextAccessor : ITargetingContextAccessor
    {
        private const string TargetingContextLookup = "TestTargetingContextAccessor.TargetingContext";
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TestTargetingContextAccessor(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        public ValueTask<TargetingContext> GetContextAsync()
        {
            HttpContext httpContext = _httpContextAccessor.HttpContext;
            if (httpContext.Items.TryGetValue(TargetingContextLookup, out object value))
            {
                return new ValueTask<TargetingContext>((TargetingContext)value);
            }
            var add = DateTime.Now.Second % 2 == 0;
            var x = _httpContextAccessor.HttpContext.Session.GetString("test") == null;
            TargetingContext targetingContext = new TargetingContext
            {
                Groups = new List<string>() { add ? "Mercy" : "Other" }
            };
            //httpContext.Items[TargetingContextLookup] = targetingContext;
            return new ValueTask<TargetingContext>(targetingContext);
        }
    }
}
