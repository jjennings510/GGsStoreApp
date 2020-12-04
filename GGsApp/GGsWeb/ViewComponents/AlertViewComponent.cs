using GGsWeb.Features;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GGsWeb.ViewComponents
{
    public class AlertViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var alerts = TempData.DeserializeAlerts(Alert.TempDataKey);

            return View(alerts);
        }
    }
}
