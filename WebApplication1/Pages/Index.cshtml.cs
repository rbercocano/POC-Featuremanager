using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.FeatureManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IFeatureManagerSnapshot featureManager;

        public IndexModel(ILogger<IndexModel> logger, IFeatureManagerSnapshot featureManager)
        {
            _logger = logger;
            this.featureManager = featureManager;
        }

        public async Task OnGet()
        {
            var x =await featureManager.IsEnabledAsync("Beta");
            var y = await featureManager.IsEnabledAsync("cqrs");
            var z = await featureManager.IsEnabledAsync("XYZ");
            Console.WriteLine(x);
            Console.WriteLine(y);
            Console.WriteLine(z);
        }
    }
}
