using System.Collections.Generic;

public class MovieSearchResponse
{
    public int TotalResults { get; set; }
    public bool Response { get; set; }
    public List<MovieData> Search { get; set; }
}