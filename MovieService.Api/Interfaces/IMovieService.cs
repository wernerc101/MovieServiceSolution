namespace MovieService.Api.Interfaces
{
    public interface IMovieService
    {
        Task<MovieDetail> GetMovieDetailsAsync(string id);
        Task<MovieSearchResponse> SearchMoviesAsync(MovieSearchRequest request);
    }
}