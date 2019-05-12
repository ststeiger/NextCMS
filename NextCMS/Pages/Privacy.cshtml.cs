
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;


namespace NextCMS.Pages
{


    public class PrivacyModel 
        : PageModel
    {


        // https://www.learnrazorpages.com/razor-pages/handler-methods
        public void OnGet()
        {
            System.Console.WriteLine("GET");
        }


        public void OnPost()
        {
            System.Console.WriteLine("post");
        }


    }


}
