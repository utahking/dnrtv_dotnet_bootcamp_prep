using System;
using System.Collections.Generic;
using prep.utility;

namespace prep.collections
{
  public class MovieLibrary
  {
    private readonly IList<Movie> movies;

    public MovieLibrary(IList<Movie> list_of_movies) {
      movies = list_of_movies;
    }

    public IEnumerable<Movie> all_movies() {
      return movies.one_at_a_time();
    }

    public void add(Movie movie) {
      if (contains_movie(movie))
        return;
      movies.Add(movie);
    }

    private bool contains_movie(Movie movie) {
      return movies.Contains(movie);
    }

    public IEnumerable<Movie> all_movies_matching(MovieCriteria criteria) {
      foreach (var movie in movies) {
        if (criteria(movie))
          yield return movie;
      }
    }

    public IEnumerable<Movie> all_movies_published_by_pixar() {
      foreach (var movie in movies) {
        if(movie.production_studio == ProductionStudio.Pixar)
          yield return movie;
      }
    }

    public IEnumerable<Movie> all_movies_published_by_pixar_or_disney() {
      foreach (var movie in movies) {
        if(movie.production_studio == ProductionStudio.Disney || movie.production_studio == ProductionStudio.Pixar)
          yield return movie;
      }
    }

    public IEnumerable<Movie> all_movies_not_published_by_pixar() {
      return all_movies_matching(movie => movie.production_studio != ProductionStudio.Pixar);
    }

    public IEnumerable<Movie> all_movies_published_after(int year) {
      return all_movies_matching(movie => movie.date_published.Year > year);
    }

    public IEnumerable<Movie> all_movies_published_between_years(int startingYear, int endingYear) {
      return all_movies_matching(movie => movie.date_published.Year >= startingYear && movie.date_published.Year <= endingYear);
    }

    public IEnumerable<Movie> all_kid_movies() {
      return all_movies_matching(movie => movie.genre == Genre.kids);
    }

    public IEnumerable<Movie> all_action_movies() {
      return all_movies_matching(movie => movie.genre == Genre.action);
    }

    public IEnumerable<Movie> sort_all_movies_by_date_published_descending() {
      return new MovieSorter(movies).Sort(new DelegateComparer<Movie>((movie, movie1) => movie1.date_published.CompareTo(movie.date_published)));
    }

    public IEnumerable<Movie> sort_all_movies_by_date_published_ascending() {
      return new MovieSorter(movies).Sort(new DelegateComparer<Movie>((movie, movie1) => movie.date_published.CompareTo(movie1.date_published)));
    }

    public IEnumerable<Movie> sort_all_movies_by_title_descending() {
      return new MovieSorter(movies).Sort(new DelegateComparer<Movie>((movie, movie1) => movie1.title.CompareTo(movie.title)));
    }

    public IEnumerable<Movie> sort_all_movies_by_title_ascending() {
      return new MovieSorter(movies).Sort(new DelegateComparer<Movie>((movie, movie1) => movie.title.CompareTo(movie1.title)));
    }

    public IEnumerable<Movie> sort_all_movies_by_movie_studio_and_year_published() {
      return new MovieSorter(movies).Sort(new DelegateComparer<Movie>((movie, movie1) => {
                                                                                      var result = movie1.production_studio.Equals(movie.production_studio) ? 0 : -1;
                                                                                      if (result == 0) {
                                                                                        result = movie1.rating.CompareTo(movie1.rating);
                                                                                        if(result == 0)
                                                                                          result = movie1.date_published.CompareTo(movie.date_published);
                                                                                      }
                                                                                      return result;
                                                                                    }));
    }
  }

  public class MovieSorter
  {
    private readonly List<Movie> movies;

    public MovieSorter(IEnumerable<Movie> movies) {
      this.movies = new List<Movie>(movies);
    }

    public IEnumerable<Movie> Sort(IComparer<Movie> comparer = null) {
      movies.Sort(comparer);
      return movies.one_at_a_time();
    }
  }

  public class DelegateComparer<T> : IComparer<T>
  {
    public Func<T, T, int> comparer;

    public DelegateComparer(Func<T, T, int> comparer) {
      this.comparer = comparer;
    }

    public int Compare(T x, T y) {
      return comparer(x, y);
    }
  }
}