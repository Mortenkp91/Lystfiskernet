using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Website.Database;

namespace Website.Pages
{
    public class FiskepladsModel : PageModel
    {
        private readonly SqlDatabaseConnectionFactory _sqlConnectionFactory;

        /// <summary>
        /// Her er et eksempel på, hvordan vi gør brug af "services", som vi registerede i vores Startup.cs klasse. 
        /// Fordi at jeg har registeret SqlDatabaseConnectionFactory i vores "container", så kan jeg injecte den ind i constructoren af klassen.
        /// </summary>
        /// <param name="sqlDatabaseConnectionFactory"></param>
        public FiskepladsModel(SqlDatabaseConnectionFactory sqlDatabaseConnectionFactory)
        {
            _sqlConnectionFactory = sqlDatabaseConnectionFactory;
        }

        /// <summary>
        /// Navnet på fiskepladsen, som vi henter ud fra databasen. 
        /// Vi har det som en property her, så vi kan vise navnet ude i HTML siden. Der bliver referert til den via @Model.Navn
        /// </summary>
        public string Navn { get; set; }

        /// <summary>
        /// Beskrivelse på fiskepladsen, som vi henter ud fra databasen. 
        /// Vi har det som en property her, så vi kan vise navnet ude i HTML siden. Der bliver referert til den via @Model.Beskrivelse.
        /// </summary>
        public string Beskrivelse { get; set; }

        /// <summary>
        /// Når har valgt en fiskeplads fra listen og bliver sendt ind på en detalje siden, altså denne side,
        /// så skal vi bruge et ID fra URLen, så vi ved hvilken fiskeplads vi skal hente i databasen.
        /// I HTML siden står der øverst: @page "{fiskepladsId}"
        /// Som giver os mulighed for, at vi kan få et ID med i URLen.
        /// URLen bliver således: lystfiskernet.dk/fiskePlads/1
        /// Metoden OnGet tager derfor en parameter, fiskePladsId (meget vigtigt at denne hedder det samme som i HTMLen)
        /// Og så kan vi bruge ID'et til at slå op i databasen.
        /// Spørgsmålstegnet efter int (en datatype for tal) betyder at den kan være null, dvs. at den ikke er kommet med.
        /// </summary>
        /// <param name="fiskepladsId"></param>
        public IActionResult OnGet(int? fiskepladsId)
        {
            // et bool er en datatype, som kun kan være to ting. Sandt eller falsk eller 0 / 1;
            // Her vil vi gerne tjekke at fiskePladsId variablen har en værdi
            // og den nok ikke er 0. (Vi har nok aldrig nogen ID'er i databasen med værdien null).
            bool erFiskePladsIdTomt = fiskepladsId == null || fiskepladsId == 0;

            // Hvis ID'et er tomt, fx. at man ikke har skrevet et ID i URLen (lystfiskernet.dk/fiskePlads/)
            // Så vil vi ikke kalde ned i databasen, men i stedet, måske redirecte til en 404 HTML siden med en fejlbeskrivelse.
            if (erFiskePladsIdTomt)
            {
                return RedirectToPage("Error");
            }

            // Hvis det ikke er tomt, så kan vi bruge det.
            using (var sqlConnection = _sqlConnectionFactory.CreateSqlConnection())
            {
                // En meget simpel SQL Query, som henter alt i FISKEPLADSER tabellen.
                var query = "SELECT * FROM Fiskepladser WHERE ID = @Id";

                // Vi skal huske at åbne vores forbindelse til SQL Serveren.
                sqlConnection.Open();

                // Her sender vi så vores Query ned til SQL Serveren.
                using (var command = new SqlCommand(query, sqlConnection))
                {

                    // Nu skal vi jo finde den fiskeplads, som stemmer overens med det ID, som står i URL'en.
                    // Derfor skal vi have sendt en parameter med ind til vores SQL query. Det gør som vist her.
                    // Det er vigtigt, at man ikke begynder og klippe SQL sætninger med værdier sammen selv i en tekst string.
                    // Det er en længere forklaring. Man skal gøre, som der er vidst her.
                    command.Parameters.AddWithValue("@Id", fiskepladsId.Value);
                    // Vi fortæller her til SQL Serveren, at vi forventer at få et resultat tilbage. Derfor skal vi bruge ExecuteReader() funktionen.
                    SqlDataReader reader = command.ExecuteReader();

                    // Her kan det blive lidt tricky. Det der vises her kaldes for en "While"-løkke. 
                    // Løkken ville blive være med gentage koden i de to curly bracket - { } når den har gennemgået alle de rækker, som vi fik tilbage fra vores query.
                    // Vi forventer dog, at der kun er en række, som passer med dette ID, så løkken skulle kun køre igennem én gang.

                    if(!reader.HasRows)
                    {
                        return RedirectToPage("Error");
                    }

                    while (reader.Read())
                    {
                        var fiskepladsName = reader.GetString(1);  // Jeg forventer her, at navne kolonnen er nr. to i rækken i tabellen. Derfor læser jeg fra kolonne 1. Da navnet jo er en string - et stykke tekst - skal jeg derfor bruge GetString metoden.
                        var fiskepladsBeskrivelse = reader.GetString(2); // Jeg forventer her, at beskrivelses kolonnen er nr. tre i rækken i tabellen. Derfor læser jeg fra kolonne 2. Da navnet jo er en string - et stykke tekst - skal jeg derfor bruge GetString metoden.

                        // Og nu hvor vi har fået navnet ud fra databasen, så kan vi sætte det i vores property på linje 30, så det kan vises på HTML siden. (Derfor hvor vi bruger @{Model.Navn} eller @Html.DisplayFor(x => x.Navn))
                        Navn = fiskepladsName;
                        Beskrivelse = fiskepladsBeskrivelse;
                    }

                    // Nu er vores "while-løkke" færdigt og nu sætter vi Fiskepladser propertyen (Line 27), så vi kan få vist alle fiskepladserne ude på HTML siden.
                }
                return Page();
            }
        }
    }
}