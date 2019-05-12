
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;


namespace NextCMS.DbFiles
{

    /*
    @model NextCMS.DbFiles.MerraMiosoModel
@{
        ViewBag.Title = $"Hello World Podcast: Episode {Model.EpisodeNumber} - {Model.GuestName}";
        ViewBag.PageName = $"Hello World Podcast";
        ViewBag.PageImage = "/img/headers/podcast.jpg";
        }

    */


    public class MerraMiosoModel
    {
        public string Name { get; set; }
    }


}
