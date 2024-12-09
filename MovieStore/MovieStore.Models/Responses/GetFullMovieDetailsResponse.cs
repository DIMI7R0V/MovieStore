using MovieStore.Models.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieStore.Models.Responses
{
    class GetFullMovieDetailsResponse
    {
        IEnumerable<MovieView> Movies { get; set; } = [];
    }
}
