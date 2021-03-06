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
using static DatePot.Areas.FilmPot.Models.Films;
using DatePot.Areas.FilmPot.Data;

namespace DatePot.Areas.FilmPot.Pages
{
    public class ViewModel : PageModel
    {
        private readonly IConfiguration _config;
        public ViewModel(IConfiguration config)
        {
            _config = config;
        }
        FilmData fd = new FilmData();
        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }
        [BindProperty]
        public UpdateFilmDetails UpdateFilmDetails { get; set; }

        public FilmDetails FilmDetails { get; set; }
        public List<FilmGenres> FilmGenres { get; set; }
        public List<FilmDirectors> FilmDirectors { get; set; }
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
                        FilmGenres = new List<FilmGenres>();
                        FilmDirectors = new List<FilmDirectors>();

                        FilmGenres = fd.GetFilmGenres(cs, Id);
                        FilmDirectors = fd.GetFilmDirectors(cs, Id);

                        genres.ForEach(x =>
                        {
                            Genres.Add(new SelectListItem { Value = x.GenreID.ToString(), Text = x.GenreText });
                        });
                        directors.ForEach(x =>
                        {
                            Directors.Add(new SelectListItem { Value = x.DirectorID.ToString(), Text = x.DirectorName });
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
        public async Task<JsonResult> OnPost(int FilmID, int AddedByID, string FilmName, DateTime ReleaseDate, DateTime AddedDate, bool Watched, int Runtime, List<int> Genre, List<int> Director)
        {
            try
            {
                string cs = _config.GetConnectionString("Default");
                fd.UpdateFilm(cs, FilmID, AddedByID, FilmName, ReleaseDate, AddedDate, Watched, Runtime);

                foreach (var item in Genre)
                {
                    if (item != 0)
                    {
                        fd.AddFilmGenres(cs, FilmID, Convert.ToInt32(item));
                    }

                }
                foreach (var item in Director)
                {
                    if (item != 0)
                    {
                        fd.AddFilmDirectors(cs, FilmID, Convert.ToInt32(item));
                    }

                }

                JsonResult result = null;
                result = new JsonResult(FilmID);
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }
        public async Task<JsonResult> OnPostDeleteGenre(int FilmGenreID, int FilmID)
        {
            try
            {
                string cs = _config.GetConnectionString("Default");
                fd.DeleteFilmGenre(cs, FilmGenreID);

                JsonResult result = null;
                result = new JsonResult(FilmID);
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }
        public async Task<JsonResult> OnPostDeleteDirector(int FilmDirectorID, int FilmID)
        {
            try
            {
                string cs = _config.GetConnectionString("Default");
                fd.DeleteFilmDirector(cs, FilmDirectorID);

                JsonResult result = null;
                result = new JsonResult(FilmID);
                return result;
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
