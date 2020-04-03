using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Website.Configurations
{
    /// <summary>
    /// Denne klasse indeholder selve connection string til SQL Server. I .NET opbevarer man connection string i den fil der hedder appsettings.json, men fordi at vi ikke gider, at skulle læse direkte fra en JSON fil,
    /// så kan vi lave en klasse, som indeholder alle de konfigurationer vi måtte skulle bruge til vores SQL Server. Det er måden, at gøre tingene på i C#. 
    /// </summary>
    public class SqlServerConfiguration
    {
        /// <summary>
        /// Læg mærke til at navnet på denne "Property" hedder det samme, som i appsettings.json filen. Dette er meget vigtigt. Ellers kan .NET frameworket ikke finde ud af at "mappe" værdien i JSON filen til denne klasse.
        /// </summary>
        public string SqlServerConnectionString { get; set; }
    }
}
