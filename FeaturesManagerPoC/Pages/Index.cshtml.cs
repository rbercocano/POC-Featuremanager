using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.FeatureManagement;
using Microsoft.FeatureManagement.FeatureFilters;

namespace FeaturesManagerPoC.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IFeatureManager featureManager;
        private readonly ITargetingContextAccessor targetingContextAccessor;
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger, IFeatureManagerSnapshot featureManager, ITargetingContextAccessor targetingContextAccessor)
        {
            _logger = logger;
            this.featureManager = featureManager;
            this.targetingContextAccessor = targetingContextAccessor;
        }


        public async Task OnGet()
        {
            HttpContext.Session.SetString("test", "123");
            var x = await featureManager.IsEnabledAsync("Beta");
            var y = await featureManager.IsEnabledAsync("cqrs");
            var z = await featureManager.IsEnabledAsync("XYZ");
            Console.WriteLine(x);
            Console.WriteLine(y);
            Console.WriteLine(z);
        }
    }
}