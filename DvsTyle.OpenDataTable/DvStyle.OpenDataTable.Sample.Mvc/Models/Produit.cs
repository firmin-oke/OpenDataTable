using System.ComponentModel.DataAnnotations;
using DvStyle.OpenDataTable.TableDef;

namespace DvStyle.OpenDataTable.Sample.Mvc.Models
{
    public enum CategorieProduit
    {
        Informatique,
        Bureautique,
        Mobilier,
        Alimentation
    }

    /// <summary>
    /// Modèle de démonstration. Chaque propriété exposée dans la table est décorée
    /// avec <see cref="DataTableModelAttributeDescription"/> pour piloter recherche/tri/type.
    /// </summary>
    public class Produit
    {
        public int Id { get; set; }

        [Display(Name = "Référence")]
        [DataTableModelAttributeDescription(Searchable = true, Orderable = true)]
        public string Reference { get; set; } = string.Empty;

        [Display(Name = "Nom du produit")]
        [DataTableModelAttributeDescription(Searchable = true, Orderable = true)]
        public string Nom { get; set; } = string.Empty;

        [Display(Name = "Catégorie")]
        [DataTableModelAttributeDescription(Searchable = true, Orderable = true)]
        public CategorieProduit Categorie { get; set; }

        [Display(Name = "Prix (€)")]
        [DataTableModelAttributeDescription(Searchable = true, Orderable = true)]
        public decimal Prix { get; set; }

        [Display(Name = "En stock")]
        [DataTableModelAttributeDescription(Searchable = true, Orderable = true)]
        public bool EnStock { get; set; }

        [Display(Name = "Date d'ajout")]
        [DataType(DataType.Date)]
        [DataTableModelAttributeDescription(Searchable = true, Orderable = true)]
        public DateTime DateAjout { get; set; }
    }

    /// <summary>
    /// Source de données en mémoire (démo, pas de base de données).
    /// </summary>
    public static class ProduitStore
    {
        private static readonly string[] Noms =
        {
            "Ordinateur portable", "Écran 27\"", "Clavier mécanique", "Souris sans fil",
            "Casque audio", "Chaise ergonomique", "Bureau réglable", "Ramette papier A4",
            "Stylo à bille", "Cahier spirale", "Café en grains", "Thé vert bio",
            "Webcam HD", "Disque SSD 1To", "Routeur Wi-Fi 6", "Lampe de bureau LED"
        };

        public static readonly List<Produit> Produits = Seed();

        private static List<Produit> Seed()
        {
            var categories = Enum.GetValues<CategorieProduit>();
            var baseDate = new DateTime(2026, 1, 1);
            var list = new List<Produit>();
            for (int i = 1; i <= 60; i++)
            {
                list.Add(new Produit
                {
                    Id = i,
                    Reference = $"REF-{i:D4}",
                    Nom = Noms[(i - 1) % Noms.Length],
                    Categorie = categories[i % categories.Length],
                    Prix = Math.Round(5 + (i * 7.5m % 950), 2),
                    EnStock = i % 3 != 0,
                    DateAjout = baseDate.AddDays(i * 3)
                });
            }
            return list;
        }
    }
}
