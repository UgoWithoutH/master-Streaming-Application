﻿using Class;
using Swordfish.NET.Collections;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Test_Recherche
{
    class Program
    {
        static void Main(string[] args)
        {
            var ensemble = new ConcurrentObservableSortedDictionary<Genre, ObservableCollection<Oeuvre>>();

            var l1 = new ObservableCollection<Oeuvre>()
            {
                new Serie("Elite",new DateTime(2019,10,1),"Série mêlant Drame et Amour",0,"////",3, new List<Auteur>(), new HashSet<Genre>(){new Genre("Drame"), new Genre("Amour")}),
                new Serie("La casa de papel",DateTime.Now,"Série mêlant Drame et Action",null,"////",3, new List<Auteur>(), new HashSet<Genre>(){new Genre("Drame")}),
                new Serie("La petite maison dans la prairie",new DateTime(2000,02,20),"Pas vraiement une série",null,"////",0, new List<Auteur>(), new HashSet<Genre>(){new Genre("Drame")}),
            };

            var l2 = new ObservableCollection<Oeuvre>()
            {
                new Serie("Elite",new DateTime(2019,10,1),"Série mêlant Drame et Amour",null,"////",3, new List<Auteur>(), new HashSet<Genre>(){new Genre("Drame"), new Genre("Amour")}),
                new Serie("Une série",DateTime.Now,"Série mêlant Amour",null,"////",3, new List<Auteur>(), new HashSet<Genre>(){new Genre("Amour")}),
                new Serie("Bonne une série",new DateTime(2000,02,20),"Pas vraiment une série",null,"////",0, new List<Auteur>(), new HashSet<Genre>(){new Genre("Amour")}),
            };

            ensemble.Add(new Genre("Drame"), l1);
            ensemble.Add(new Genre("Amour"), l2);

            Console.WriteLine("Affichage Dictionary \n\n");

            foreach(var listing in ensemble)
            {
                Console.WriteLine($"Key :{listing.Key} ");
                foreach(Oeuvre oeuvre in listing.Value)
                {
                    Console.WriteLine($" Value : {oeuvre}");
                }
            }

            Console.WriteLine("\n\nRecherche d'Oeuvres\n");

            var MaRecherche = ensemble.RechercherOeuvres("elite");
            foreach (Oeuvre o in MaRecherche)
            {
                Console.WriteLine(o);
            }
            //Console.WriteLine(l1.Contains(new Serie("Elite", new DateTime(2019, 10, 1), "Série mêlant Drame et Amour", "////", 3)));

            //Console.WriteLine("\n");
            //Console.WriteLine("jean".StartsWith("jea"));
        }
    }
}
