using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Website.Models
{
    /// <summary>
    /// Her er et eksempel på, hvad C# er rigtig stærk i. C# er et objekt orienteret programmingssprog. Det betyder, at vi i størstedelen af situationer arbejder med klasser/objekter. En klasse/objekt
    /// er en repræsentation af et koncept.
    /// Forestil dig, at Ole viser en tegning af en pibe, som han har lavet.
    /// Efterfølgende spørger Ole dig, hvad det er.
    /// Det er forkert! Det er ikke en pibe, men en repræsentation af en pibe = en klasse/model. I dette tilfælde en visuel model på et papir.
    /// </summary>
    public class Fiskeplads
    {
        /// <summary>
        /// ID'et på fiskepladsen i databasen.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Navnet på fiskepladsen i databasen.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Evt. en beskrivelse af fiskepladsen.
        /// </summary>
        public string Description { get; set; }
    }
}
