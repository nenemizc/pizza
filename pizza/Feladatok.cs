using System;
using System.Collections.Generic;
using System.Text;

namespace pizza
{
    internal class Feladatok
    {
        List<Rendeles> lista;
        public Feladatok()
        {
            Adatbazis a = new();
            lista = a.Feltolt();

        }

        public void F3olcso()
        {
            var query = lista
                .Where(r => r.Pizza.Ar < 1500)
                .Select(r => new { r.Pizza.Nev, r.Pizza.Meret, r.Pizza.Ar })
                .Distinct()
                .OrderByDescending(p => p.Meret)
                .ThenBy(p => p.Nev);
            Console.WriteLine("Feladat 3:");
            foreach (var item in query)
            {
                Console.WriteLine($"Név: {item.Nev}, Méret: {item.Meret} cm, Ár: {item.Ar} Ft");
            }
        }

        public void F4nepszeru()
        {
            var query = lista
                .GroupBy(r => new { r.Pizza.Nev, r.Pizza.Meret })
                .Select(g => new { g.Key.Nev, g.Key.Meret, Db = g.Sum(r => r.Db) })
                .OrderByDescending(p => p.Db)
                .FirstOrDefault();
            Console.WriteLine("Feladat 4:");
            if (query != null)
            {
                Console.WriteLine($"Név: {query.Nev}, Méret: {query.Meret} cm, Darabszám: {query.Db}");
            }
        }
        public void F5esti()
        {
            var query = lista
                .Where(r => r.Ido >= new TimeSpan(18, 0, 0) && r.Ido <= new TimeSpan(19, 0, 0))
                .GroupBy(r => new { r.Pizza.Nev, r.Pizza.Meret })
                .Select(g => new { g.Key.Nev, g.Key.Meret, Db = g.Sum(r => r.Db) })
                .OrderBy(p => p.Nev)
                .ThenBy(p => p.Meret);

            Console.WriteLine("Feladat 5:");
            foreach (var item in query)
            {
                Console.WriteLine($"Név: {item.Nev}, Méret: {item.Meret} cm, Darabszám: {item.Db}");
            }
        }
        public void F6jolfogy()
        {
            var query = lista
                .GroupBy(r => r.Pizza.Nev)
                .Where(g => g.Select(r => r.Pizza.Meret).Distinct().Count() == 3)
                .Select(g => g.Key)
                .OrderBy(nev => nev);

            Console.WriteLine("Feladat 6:");
            foreach (var nev in query)
            {
                Console.WriteLine(nev);
            }
        }
        public void F7reklam()
        {
            var query = lista
                .GroupBy(r => new { r.Ido, CimId = r.Cim.Id })
                .Where(g => g.Sum(r => r.Db) > 3)
                .Select(g => new
                {
                    g.Key.Ido,
                    Cim = g.First().Cim,
                    Db = g.Sum(r => r.Db)
                })
                .OrderBy(r => r.Ido)
                .ThenBy(r => r.Cim.Nev);

            Console.WriteLine("Feladat 7:");
            foreach (var item in query)
            {
                Console.WriteLine($"Idő: {item.Ido}, Név: {item.Cim.Nev}, Utca: {item.Cim.Utca}, Házszám: {item.Cim.Hsz}");
            }
        }
        public void F8osszeg()
        {
            var query = lista
                .GroupBy(r => new { r.Ido, CimId = r.Cim.Id })
                .Select(g => new
                {
                    g.Key.Ido,
                    Cim = g.First().Cim,
                    Osszeg = g.Sum(r => r.Db * r.Pizza.Ar)
                })
                .OrderBy(r => r.Ido)
                .ThenBy(r => r.Cim.Nev);

            Console.WriteLine("Feladat 8:");
            Console.WriteLine("Idő\tNév\tUtca\tHázszám\tÖsszeg");
            foreach (var item in query)
            {
                Console.WriteLine($"{item.Ido}\t{item.Cim.Nev}\t{item.Cim.Utca}\t{item.Cim.Hsz}\t{item.Osszeg} Ft");
            }
        }
    }
}
