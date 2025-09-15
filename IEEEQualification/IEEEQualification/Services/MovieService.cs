using System;
using System.Collections.Generic;
using System.Linq;
using IEEEQualification.Data;
using IEEEQualification.Models;

namespace IEEEQualification.Services
{
    public class MovieService
    {
        private readonly AppDbContext _context;

        public MovieService(AppDbContext context)
        {
            _context = context;
        }

        public Movie AddMovie(string title, string description, string author, string category)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new Exception("Title is required");

            if (string.IsNullOrWhiteSpace(author))
                throw new Exception("Author is required");

            var existing = _context.Movies.FirstOrDefault(m =>
                m.Title.Equals(title, StringComparison.OrdinalIgnoreCase) &&
                m.Author.Equals(author, StringComparison.OrdinalIgnoreCase));

            if (existing != null)
                throw new Exception("Movie already exists");

            var movie = new Movie
            {
                Title = title.Trim(),
                Description = description?.Trim() ?? "",
                Author = author.Trim(),
                Category = category?.Trim() ?? ""
            };

            _context.Movies.Add(movie);
            _context.SaveChanges();
            return movie;
        }

        public Movie EditMovie(int id, string title, string description, string author, string category)
        {
            var movie = _context.Movies.Find(id);
            if (movie == null)
                throw new Exception("Movie not found");

            if (string.IsNullOrWhiteSpace(title))
                throw new Exception("Title is required");

            if (string.IsNullOrWhiteSpace(author))
                throw new Exception("Author is required");

            movie.Title = title.Trim();
            movie.Description = description?.Trim() ?? "";
            movie.Author = author.Trim();
            movie.Category = category?.Trim() ?? "";

            _context.SaveChanges();
            return movie;
        }

        public bool DeleteMovie(int id)
        {
            var movie = _context.Movies.Find(id);
            if (movie == null)
                return false;

            _context.Movies.Remove(movie);
            _context.SaveChanges();
            return true;
        }

        public List<Movie> SearchMovies(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
                return GetAllMovies();

            var searchTerm = keyword.ToLower().Trim();
            return _context.Movies
                .Where(m => m.Title.ToLower().Contains(searchTerm) ||
                           m.Description.ToLower().Contains(searchTerm) ||
                           m.Category.ToLower().Contains(searchTerm) ||
                           m.Author.ToLower().Contains(searchTerm))
                .ToList();
        }

        public List<Movie> GetAllMovies()
        {
            return _context.Movies.ToList();
        }

        public Movie GetMovieById(int id)
        {
            return _context.Movies.Find(id);
        }

        public List<Movie> GetMoviesByCategory(string category)
        {
            return _context.Movies
                .Where(m => m.Category.Equals(category, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        public List<Movie> GetMoviesByAuthor(string author)
        {
            return _context.Movies
                .Where(m => m.Author.Equals(author, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }
    }
}