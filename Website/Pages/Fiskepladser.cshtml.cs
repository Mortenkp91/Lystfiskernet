using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Website.Database;
using Website.Models;

namespace Website.Pages
{
    public class FiskepladserModel : PageModel
    {
        private readonly SqlDatabaseConnectionFactory _sqlConnectionFactory;

        /// <summary>
        /// Her er et eksempel på, hvordan vi gør brug af "services", som vi registerede i vores Startup.cs klasse. 
        /// Fordi at jeg har registeret SqlDatabaseConnectionFactory i vores "container", så kan jeg injecte den ind i constructoren af klassen.
        /// </summary>
        /// <param name="sqlDatabaseConnectionFactory"></param>
        public FiskepladserModel(SqlDatabaseConnectionFactory sqlDatabaseConnectionFactory)
        {
            _sqlConnectionFactory = sqlDatabaseConnectionFactory;
        }

        public List<Fiskeplads> Fiskepladser { get; set; }

        /// <summary>
        /// OnGet() bliver kaldt, når det er siden bliver loaded. Siden vi er på Fiskepladser siden, som nok viser alle fiskepladser i en tabel, så ville man nok tage fat i databasen her og hente alle fiskepladserne op.
        /// </summary>
        public void OnGet()
        {
            var fiskepladser = new List<Fiskeplads>();
            // Når man har med I/O operationer, dvs. operationer som foregår eksternt i applikationer, fx. at snakke med en database, som hostes et andet sted end web serveren, så er det vigtigt, at man husker, at lukke forbindelsen til databasen igen
            // Så man undgår at "leake" forbindelser og eventuelt løbe "tør" for forbindelser. Ved at bruge et "using" statement, så hjælper .NET frameworket med at sikre dette. Alt det kode, som er indkapsuleret i de to curly brackets - { }
            // er der for omfattet af denne sikring. Så herinde ville vi skrive vores SQL query til at hente fiskepladser.
            using(var sqlConnection = _sqlConnectionFactory.CreateSqlConnection())
            {
                // En meget simpel SQL Query, som henter alt i FISKEPLADSER tabellen.
                var query = "SELECT * FROM Fiskepladser";

                // Vi skal huske at åbne vores forbindelse til SQL Serveren.
                sqlConnection.Open();

                // Her sender vi så vores Query ned til SQL Serveren.
                using (var command = new SqlCommand(query, sqlConnection))
                {
                    // Vi fortæller her til SQL Serveren, at vi forventer at få et resultat tilbage. Derfor skal vi bruge ExecuteReader() funktionen.
                    SqlDataReader reader = command.ExecuteReader();

                    // Her kan det blive lidt tricky. Det der vises her kaldes for en "While"-løkke. 
                    // Løkken ville blive være med gentage koden i de to curly bracket - { } når den har gennemgået alle de rækker, som vi fik tilbage fra vores query.
                    while (reader.Read())
                    {
                        var fiskepladsId = reader.GetInt32(0);    // Jeg forventer her, at ID kolonnen er den første i tabellen. Derfor læser jeg fra kolonne 0. Indekses starter altid ved 0. Da jeg ved at ID'et er et tal, skal typen derfor være en "int" - også kaldet en integer.
                        var fiskepladsName = reader.GetString(1);  // Jeg forventer her, at navne kolonnen er nr. to i rækken i tabellen. Derfor læser jeg fra kolonne 1. Da navnet jo er en string - et stykke tekst - skal jeg derfor bruge GetString metoden.
                        var fiskepladsBeskrivelse = reader.GetString(2); // Jeg forventer her, at beskrivelses kolonnen er nr. tre i rækken i tabellen. Derfor læser jeg fra kolonne 2. Da navnet jo er en string - et stykke tekst - skal jeg derfor bruge GetString metoden.

                        // Så tilføjer vi den første fiskeplads til vores liste, som står på linje 34. 
                        fiskepladser.Add(new Fiskeplads
                        {
                            Id = fiskepladsId,
                            Name = fiskepladsName,
                            Description = fiskepladsBeskrivelse
                        });
                    }

                    // Nu er vores "while-løkke" færdigt og nu sætter vi Fiskepladser propertyen (Line 27), så vi kan få vist alle fiskepladserne ude på HTML siden.
                    Fiskepladser = fiskepladser;
                }
            }
        }
    }
}