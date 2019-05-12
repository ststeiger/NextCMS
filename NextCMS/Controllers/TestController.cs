﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NextCMS.Controllers 
{
    public class TestController : Controller
    {

        private readonly Services.IViewRenderService m_viewRenderService;


        public TestController(Services.IViewRenderService viewRenderService)
        {
            this.m_viewRenderService = viewRenderService;
        }





        // https://ppolyzos.com/2016/09/09/asp-net-core-render-view-to-string/
        // https://stackoverflow.com/questions/40912375/return-view-as-string-in-net-core

        [Route("test")]
        public async Task<IActionResult> Index()
        {
            // string viewModel = null;
            var viewModel = new NextCMS.Views.Pages.MyTestPageModel();

            var result = await this.m_viewRenderService.RenderToStringAsync("Pages/MyTestPage", viewModel); // viewModel has to be of specified type
            result = await this.m_viewRenderService.RenderToStringAsync("Pages/MyTestView", viewModel); // viewModel can be anything, if not defined

            result = await this.m_viewRenderService.RenderToStringAsync("DbFiles/MerraMioso", viewModel); // viewModel can be anything, if not defined

            return Content(result);
        }

    }
}