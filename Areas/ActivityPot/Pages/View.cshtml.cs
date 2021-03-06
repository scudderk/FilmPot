using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using static DatePot.Areas.ActivityPot.Models.Activitys;
using DatePot.Areas.ActivityPot.Data;

namespace DatePot.Areas.ActivityPot.Pages
{
    public class ViewModel : PageModel
    {
        private readonly IConfiguration _config;
        public ViewModel(IConfiguration config)
        {
            _config = config;
        }
        ActivityData fd = new ActivityData();
        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }
        [BindProperty]
        public UpdateFilmDetails UpdateFilmDetails { get; set; }

        public FilmDetails FilmDetails { get; set; }
        //public string Genre { get; set; }
        public List<SelectListItem> Genres { get; set; }
        public List<SelectListItem> Directors { get; set; }
        public List<SelectListItem> Users { get; set; }
        public async Task<IActionResult> OnGet()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            }
            try
            {
                string cs = _config.GetConnectionString("Default");
                FilmDetails = fd.GetFilmDetails(cs, Id).FirstOrDefault();
                if (FilmDetails != null)
                {
                    if (FilmDetails != null)
                    {
                        var genres = fd.GetGenreList(cs);
                        var directors = fd.GetDirectorsList(cs);
                        var users = fd.GetUserList(cs);

                        //Genre = genres.Where(x => x.GenreID == FilmDetails.GenreID).FirstOrDefault()?.GenreText;

                        Genres = new List<SelectListItem>();
                        Directors = new List<SelectListItem>();
                        Users = new List<SelectListItem>();

                        genres.ForEach(x =>
                        {
                            if (FilmDetails.GenreID == x.GenreID)
                            {
                                Genres.Add(new SelectListItem { Value = x.GenreID.ToString(), Text = x.GenreText, Selected = true });
                            }
                            else
                            {
                                Genres.Add(new SelectListItem { Value = x.GenreID.ToString(), Text = x.GenreText });
                            }
                        });
                        directors.ForEach(x =>
                        {
                            if (FilmDetails.DirectorID == x.DirectorID)
                            {
                                Directors.Add(new SelectListItem { Value = x.DirectorID.ToString(), Text = x.DirectorName, Selected = true });
                            }
                            else
                            {
                                Directors.Add(new SelectListItem { Value = x.DirectorID.ToString(), Text = x.DirectorName });
                            }
                        });
                        users.ForEach(x =>
                        {
                            if (FilmDetails.AddedByID == x.UserID)
                            {
                                Users.Add(new SelectListItem { Value = x.UserID.ToString(), Text = x.UserName, Selected = true });
                            }
                            else
                            {
                                Users.Add(new SelectListItem { Value = x.UserID.ToString(), Text = x.UserName });
                            }
                        });
                    }
                }

                return Page();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }
        public async Task<IActionResult> OnPost()
        {
            try
            {
                if (ModelState.IsValid == false)
                {
                    return Page();
                }
                string cs = _config.GetConnectionString("Default");
                fd.UpdateFilm(cs, UpdateFilmDetails);

                return RedirectToPage("./View", new { @Id = UpdateFilmDetails.FilmID, @redirect = "update" });
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }
        public async Task<IActionResult> OnPostArchive()
        {
            try
            {
                if (ModelState.IsValid == false)
                {
                    return Page();
                }
                string cs = _config.GetConnectionString("Default");
                fd.ArchiveFilm(cs, UpdateFilmDetails.FilmID);

                return RedirectToPage("./Index");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }
    }
}
