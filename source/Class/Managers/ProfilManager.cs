﻿using Swordfish.NET.Collections;
using Swordfish.NET.Collections.Auxiliary;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;

namespace Class
{
    public class ProfilManager : ObservableObject
    {

        private ConcurrentObservableSortedDictionary<Genre, ObservableCollection<Oeuvre>> listOeuvres;

        public ConcurrentObservableSortedDictionary<Genre, ObservableCollection<Oeuvre>> ListOeuvres
        {
            get { return listOeuvres; }
            private set
            {
                listOeuvres = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(ListOeuvresParGenre));
            }
        }


        public Oeuvre OeuvreSélectionnée { get; set; }


        private Genre genreSélectionné;

        public Genre GenreSélectionné
        {
            get { return genreSélectionné; }
            set
            {
                genreSélectionné = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(ListingDatesParGenre));
                OnPropertyChanged(nameof(ListOeuvresParGenre));
            }
        }

        public Watchlist MyWatchlist { get; set; }

        public ObservableCollection<Oeuvre> ListOeuvresParGenre
        {
            get
            {
                if(ListOeuvres.Keys.Count == 1)
                {
                    return null;
                }
                else return ListOeuvres[GenreSélectionné];
            }
        }

        public ObservableCollection<Oeuvre> ListFiltrée { get; private set; }

        public LinkedList<Serie> ListingSerie { get; private set; }

        public ConcurrentObservableSortedDictionary<Genre,ConcurrentObservableSortedSet<string>> ListingDates
        {
            get { return listingDates; }
            set
            {
                listingDates = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(ListingDatesParGenre));
            }
        }

        private ConcurrentObservableSortedDictionary<Genre, ConcurrentObservableSortedSet<string>> listingDates;

        public ConcurrentObservableSortedSet<string> ListingDatesParGenre
        {
            get 
            { 
                if(GenreSélectionné != null)
                {
                    return ListingDates[GenreSélectionné];
                }

                return new ConcurrentObservableSortedSet<string>();
            }
        }

        public ObservableCollection<string> ListingTris { get; private set; }

        public ObservableCollection<Oeuvre> ListRecherche { get; set; }

        public Serie SerieTemporaireAjout { get; set; }

        public string Nom { get; private set; }

        public ProfilManager(string nom)
        {
            Nom = nom;
            ListOeuvres = new ConcurrentObservableSortedDictionary<Genre, ObservableCollection<Oeuvre>>();
            ListingSerie = new LinkedList<Serie>();
            ListingDates = new ConcurrentObservableSortedDictionary<Genre, ConcurrentObservableSortedSet<string>>();
            ListFiltrée = new ObservableCollection<Oeuvre>();
            MyWatchlist = new Watchlist();
            ListingTris = new ObservableCollection<string>() { "Alphabétique", "Notes" };
            SerieTemporaireAjout = new Serie();

            /// temporaire, pour tester le Binding sur la Watchlist
            MyWatchlist.AjouterOeuvre(new Serie("Elite", DateTime.Now, "C'est cool", 4, "/images/Drame/Enola Holmes.jpg", 52, null, new HashSet<Genre>() { new Genre("Drame") }));
            MyWatchlist.AjouterOeuvre(new Serie("Harry", new DateTime(1999, 01, 15), "C'est cool", null, "/images/Drame/Notre ete.jpg", 52, new List<Auteur>(), new HashSet<Genre>() { new Genre("Action"), new Genre("Drame")}));
        }

      public void chargeDonnées() // temporaire
        {
            AjouterGenre(new Genre("Humour"));
            AjouterGenre(new Genre("Romance"));
            AjouterGenre(new Genre("Aventure"));
            AjouterGenre(new Genre("Action"));
            AjouterOeuvre(new Serie("Des vies froissees", new DateTime(2019, 10, 1), "Série mêlant Drame et Amour", null, "/images/Drame/Des vies froissees.jpg", 3, new List<Auteur>(), new HashSet<Genre>() { new Genre("Humour"), new Genre("Romance") }));
            AjouterOeuvre(new Serie("Enola Holmes", DateTime.Now, "Série mêlant Drame et Action", null, "/images/Drame/Enola Holmes.jpg", 3, new List<Auteur>(), new HashSet<Genre>() { new Genre("Aventure") }));
            AjouterOeuvre(new Serie("La mission", new DateTime(2000, 02, 20), "Pas vraiment une série", null, "/images/Drame/La mission.jpg", 0, new List<Auteur>(), new HashSet<Genre>() { new Genre("Action") }));
            AjouterOeuvre(new Serie("Notre été", new DateTime(2010, 02, 20), "Pas vraiment une série", 4, "/images/Drame/Notre ete.jpg", 0, new List<Auteur>(), new HashSet<Genre>() { new Genre("Action") }));
            AjouterOeuvre(new Serie("Notre hiver", new DateTime(2010, 02, 20), "Pas vraiment une série", 4, "/images/Drame/Notre ete.jpg", 0, new List<Auteur>(), new HashSet<Genre>() { new Genre("Action") }));
            AjouterOeuvre(new Serie("Notre automn", new DateTime(2010, 02, 20), "Pas vraiment une série", 4, "/images/Drame/Notre ete.jpg", 0, new List<Auteur>(), new HashSet<Genre>() { new Genre("Action") }));
            AjouterOeuvre(new Serie("Notre printemps", new DateTime(2010, 02, 20), "Pas vraiment une série", 4, "/images/Drame/Notre ete.jpg", 0, new List<Auteur>(), new HashSet<Genre>() { new Genre("Action") }));
        }

        /// <summary>
        /// Ajouter un genre dans le SortedDictionary des Oeuvres (ListOeuvres) et des Dates (ListingDates)
        /// </summary>
        /// <param name="g">Genre qui doit être ajouté</param>
        /// <returns>true si l'ajout du genre a réussi sinon false</returns>
        public void AjouterGenre(Genre genre)
        {
            if (genre == null) throw new NullReferenceException("Le genre est null");

            if (!ListOeuvres.ContainsKey(genre))
            {
                ListOeuvres.Add(genre, new ObservableCollection<Oeuvre>());
                ListingDates.Add(genre, new ConcurrentObservableSortedSet<string>() { "Toutes dates" });
            }
        }

        /// <summary>
        /// Supprimer un Genre dans le le SortedDictionary des Oeuvres (ListOeuvres) et des Dates (ListingDates)
        /// </summary>
        /// <param name="g">Genre qui doit être supprimé</param>
        /// <returns>true si la suppression du genre a réussi sinon false</returns>
        public void SupprimerGenre(Genre genre)
        {
            if (genre == null) throw new NullReferenceException("Le genre est null");

            if (ListOeuvres.ContainsKey(genre))
            {
                ListOeuvres.Remove(genre);
                ListingDates.Remove(genre);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="o">Oeuvre qui doit être ajoutée au SortedDictionary des Oeuvres (ListOeuvres)</param>
        /// <returns></returns>
        public bool AjouterOeuvre(Oeuvre oeuvre)
        {

            if (oeuvre is Serie serie) ListingSerie.AddFirst(serie);

            if (oeuvre == null) throw new NullReferenceException("L'oeuvre est null");

            if (checkAjoutOeuvre(oeuvre))
            {

                foreach (Genre genre in oeuvre.TagsGenres)
                {
                    if (ListOeuvres.ContainsKey(genre))
                    {
                        ListOeuvres.TryGetValue(genre, out ObservableCollection<Oeuvre> value);

                        if (!value.Contains(oeuvre))
                        {
                            ListOeuvres[genre].Add(oeuvre);
                            if (!ListingDates[genre].Contains(oeuvre.DateSortie.Year.ToString()))
                            {
                                ListingDates[genre].Add(oeuvre.DateSortie.Year.ToString());
                            }
                        }
                    }
                }
                return true;
            }
            else return false;
        }


        public void SupprimerOeuvre(Oeuvre oeuvre)
        {
            if (oeuvre is Serie serie) ListingSerie.Remove(serie);

            if (oeuvre == null) throw new NullReferenceException("L'oeuvre est null");

                foreach (Genre genre in oeuvre.TagsGenres)
                {
                    ListOeuvres.TryGetValue(genre, out ObservableCollection<Oeuvre> value);
                    if (value.Contains(oeuvre))
                    {
                        value.Remove(oeuvre);
                        CheckListDates(genre, oeuvre.DateSortie.Year.ToString());
                    OnPropertyChanged(nameof(ListOeuvres));//test
                        OnPropertyChanged(nameof(ListOeuvresParGenre)); //test
                        OnPropertyChanged(nameof(ListFiltrée));//test
                }
                }
        }

        public bool checkAjoutOeuvre(Oeuvre oeuvre)
        {
            if (string.IsNullOrWhiteSpace(oeuvre.Titre)) return false;
            if (oeuvre.DateSortie == null) return false;
            if (string.IsNullOrWhiteSpace(oeuvre.Description)) return false;
            if (string.IsNullOrWhiteSpace(oeuvre.ImageName)) return false;
            if (oeuvre.TagsGenres.Count == 0) return false;

            return true;
        }

        public void CheckListDates(Genre genre, string year)
        {
            int check = 0;

            ListOeuvres.TryGetValue(genre, out ObservableCollection <Oeuvre> value);
            foreach(Oeuvre oeuvre in value)
            {
                if(oeuvre.DateSortie.Year.ToString() == year)
                {
                    check = 1;
                }
            }

            if (check == 0)
            {
                ListingDates[genre].Remove(year);
            }
        }

        public bool Filtrage(string filtre)
        {
            bool result = true;

            if (filtre != null)
            {
                if (filtre.ToUpper().Equals("TOUTES DATES"))
                {
                    ListFiltrée = ListOeuvresParGenre;
                }
                else
                {
                    int dateFiltre = int.Parse(filtre);
                    ListFiltrée = new ObservableCollection<Oeuvre>(); //Dans notre cas on ne peut pas utiliser clear() car ListFiltrée et ListOeuvresParGenre pointe sur la même zone mémoire dans le tas
                    ListFiltrée.AddRange(ListOeuvresParGenre.Where(oeuvre => oeuvre.DateSortie.Year == dateFiltre));
                }
            }
            else
            {
                ListFiltrée = null;
                result = false;
            }
            OnPropertyChanged(nameof(ListFiltrée));
            return result;
        }

        public void tri(string tri)
        {
            if (ListFiltrée != null)
            {
                IEnumerable<Oeuvre> liste = null;

                if (tri.Equals("Alphabétique"))
                {
                    liste = ListFiltrée.OrderBy(oeuvre => oeuvre.Titre);
                }
                else if (tri.Equals("Notes"))
                {
                    liste = ListFiltrée.OrderByDescending(oeuvre => oeuvre.Note).ThenBy(oeuvre => oeuvre.Titre); //décroissant
                }
                ListFiltrée = new ObservableCollection<Oeuvre>();
                ListFiltrée.AddRange(liste);
                OnPropertyChanged(nameof(ListFiltrée));
            }
        }

        public void ChangeGenreSélectionné(ConcurrentObservableSortedDictionary<Genre, ObservableCollection<Oeuvre>> données, string Textgenre)
        {

            if (données.Count == 1)
            {
                GenreSélectionné = null;
            }

            int index = Array.IndexOf(ListOeuvres.Keys.ToArray(), new Genre(Textgenre));

            Genre[] listingGenre = données.Keys.ToArray();

            if (index == 0 && données.Count != 1)
            {
                GenreSélectionné = listingGenre[1];
            }

            else if (index != 0 && index == (ListOeuvres.Count) - 1)
            {
                GenreSélectionné = listingGenre[(ListOeuvres.Count) - 2];
            }

            else if (index != 0)
            {
                GenreSélectionné = listingGenre[index + 1];
            }
        }

        public ObservableCollection<Oeuvre> Recherche(string chaine)
        {
            return ListOeuvres.RechercherOeuvres(chaine);
        }

        public bool Equals(ProfilManager other)
        {
            return Nom.Equals(other.Nom);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(obj, null)) return false;
            if (ReferenceEquals(obj, this)) return true;
            if (GetType() != (obj.GetType())) return false;

            return Equals(obj as ProfilManager);
        }

        public override int GetHashCode()
        {
            return 217408413 + EqualityComparer<string>.Default.GetHashCode(Nom);
        }
    }
}
