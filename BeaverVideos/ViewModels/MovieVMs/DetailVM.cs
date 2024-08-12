using BeaverVideos.Dto;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace BeaverVideos.ViewModels.MovieVMs
{
    public class DetailVM : MovieDetail
    {
        public List<SelectListItem> SelectListItems
        {
            get
            {
                var items = new List<SelectListItem>();
                PlayLinkSites.ForEach((item) =>
                {
                    items.Add(new SelectListItem()
                    {
                        Text = item,
                        Value = item,
                        Selected = true
                    });
                });
                return items;
            }
        }
    }
}
