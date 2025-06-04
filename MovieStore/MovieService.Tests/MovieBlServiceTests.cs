//using Moq;
//using MovieStore.BL.Services;
//using MovieStore.DL.Interfaces;
//using MovieStore.Models.DTO;
//using Xunit;

//namespace MovieService.Tests
//{
//    public class MovieBlServiceTests
//    {
//        private readonly Mock<IMovieRepository> _movieRepositoryMock;
//        private readonly Mock<IActorRepository> _actorRepositoryMock;

//        private readonly List<Movie> _movies = new()
//        {
//            new Movie
//            {
//                Id = Guid.NewGuid().ToString(),
//                Title = "Movie 1",
//                Year = 2021,
//                ActorIds = [
//                    "157af604-7a4b-4538-b6a9-fed41a41cf3a",
//                    "baac2b19-bbd2-468d-bd3b-5bd18aba98d7"]
//            },
//            new Movie
//            {
//                Id = Guid.NewGuid().ToString(),
//                Title = "Movie 2",
//                Year = 2022,
//                ActorIds = [
//                    "157af604-7a4b-4538-b6a9-fed41a41cf3a",
//                    "5c93ba13-e803-49c1-b465-d471607e97b3"]
//            }
//        };

//        private readonly List<Actor> _actors = new()
//        {
//            new Actor { Id = "157af604-7a4b-4538-b6a9-fed41a41cf3a", Name = "Actor 1" },
//            new Actor { Id = "baac2b19-bbd2-468d-bd3b-5bd18aba98d7", Name = "Actor 2" },
//            new Actor { Id = "5c93ba13-e803-49c1-b465-d471607e97b3", Name = "Actor 3" },
//            new Actor { Id = "9badefdc-0714-4581-80ae-161cd0a5abbe", Name = "Actor 4" }
//        };

//        public MovieBlServiceTests()
//        {
//            _movieRepositoryMock = new Mock<IMovieRepository>();
//            _actorRepositoryMock = new Mock<IActorRepository>();
//        }

//        [Fact]
//        public async Task GetDetailedMovies_Ok()
//        {
//            // Arrange
//            _movieRepositoryMock
//                .Setup(x => x.GetAllMovies())
//                .ReturnsAsync(_movies);

//            _actorRepositoryMock
//                .Setup(x => x.GetActorById(It.IsAny<string>()))
//                .Returns((string id) => _actors.FirstOrDefault(x => x.Id == id));

//            var service = new MovieBLService((MovieStore.BL.Interfaces.IActorService)_movieRepositoryMock.Object, (MovieStore.BL.Interfaces.IMovieService)_actorRepositoryMock.Object);

//            // Act
//            var result = await service.GetDetailedMovies();

//            // Assert
//            Assert.NotNull(result);
//            Assert.Equal(2, result.Count); // 2 movies
//            Assert.All(result, movie =>
//            {
//                Assert.NotEmpty(movie.ActorIds);
//                Assert.All(movie.ActorIds, actor =>
//                {
//                    Assert.False(string.IsNullOrWhiteSpace(actor.Id));
//                    Assert.False(string.IsNullOrWhiteSpace(actor.Name));
//                });
//            });
//        }
//    }
//}
