using MovieStore.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieStore.Models.Views
{
    internal class MovieView
    {
        public int MovieId { get; set; }

        public string MovieTitle { get; set; }

        public int MovieYear { get; set; }

        IEnumerable<Actor> Actors { get; set; } = [];
    }
}
