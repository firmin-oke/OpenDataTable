using DvStyle.OpenDataTable.Binders;
using DvStyle.OpenDataTable.Sample.Mvc.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace DvStyle.OpenDataTable.Sample.Mvc.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            // La vue n'a besoin que du TYPE du modèle (asp-for) pour décrire les colonnes.
            return View(new Produit());
        }

        /// <summary>
        /// Endpoint de traitement serveur appelé en AJAX par la datatable.
        /// Reçoit la requête DataTables (liée par DataTableModelBinder) et renvoie la page filtrée/triée.
        /// </summary>
        [HttpPost]
        public IActionResult Data(DataTableServerRequestHeader request)
        {
            var query = ProduitStore.Produits.AsQueryable();
            long recordsTotal = query.LongCount();

            // Recherche globale
            var search = request.Search?.Value?.Trim();
            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(p =>
                    p.Reference.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                    p.Nom.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                    p.Categorie.ToString().Contains(search, StringComparison.OrdinalIgnoreCase) ||
                    p.Prix.ToString().Contains(search, StringComparison.OrdinalIgnoreCase));
            }
            int recordsFiltered = query.Count();

            // Tri (une seule colonne suffit pour la démo)
            var order = request.Order?.FirstOrDefault();
            if (order != null && request.Columns != null && order.Column >= 0 && order.Column < request.Columns.Length)
            {
                bool asc = order.Dir != "desc";
                query = request.Columns[order.Column].Data switch
                {
                    nameof(Produit.Reference) => asc ? query.OrderBy(p => p.Reference) : query.OrderByDescending(p => p.Reference),
                    nameof(Produit.Nom) => asc ? query.OrderBy(p => p.Nom) : query.OrderByDescending(p => p.Nom),
                    nameof(Produit.Categorie) => asc ? query.OrderBy(p => p.Categorie) : query.OrderByDescending(p => p.Categorie),
                    nameof(Produit.Prix) => asc ? query.OrderBy(p => p.Prix) : query.OrderByDescending(p => p.Prix),
                    nameof(Produit.EnStock) => asc ? query.OrderBy(p => p.EnStock) : query.OrderByDescending(p => p.EnStock),
                    nameof(Produit.DateAjout) => asc ? query.OrderBy(p => p.DateAjout) : query.OrderByDescending(p => p.DateAjout),
                    _ => query
                };
            }

            // Pagination
            var page = query
                .Skip(request.Start)
                .Take(request.Length > 0 ? request.Length : recordsFiltered)
                .ToArray();

            // Enveloppe au format exact attendu par DataTables (clés en minuscules) ;
            // les lignes restent en PascalCase (PropertyNamingPolicy = null) pour matcher les colonnes.
            return Json(new
            {
                draw = request.Draw,
                recordsTotal,
                recordsFiltered,
                data = page
            });
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
