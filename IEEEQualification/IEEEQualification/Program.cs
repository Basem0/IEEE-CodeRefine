using System;
using System.Collections.Generic;
using IEEEQualification.Data;
using IEEEQualification.Models;
using IEEEQualification.Services;

class Program
{
    static void Main(string[] args)
    {
        using var context = new AppDbContext();
        context.Database.EnsureCreated();

        // Services
        var authService = new AuthService(context);
        var movieService = new MovieService(context);
        var reviewService = new ReviewService(context);
        var watchlistService = new WatchlistService(context);
        var ui = new UiService();

        // Seed admin account if not exists
        authService.SeedAdmin();

        User? currentUser = null;

        while (true)
        {
            if (currentUser == null)
            {
                currentUser = ui.ShowAuthMenu(authService);
            }
            else if (currentUser.Role == UserRole.Admin)
            {
                var choice = ui.ShowAdminMenu();
                HandleAdminChoice(choice, movieService, ui, ref currentUser);
            }
            else
            {
                var choice = ui.ShowUserMenu();
                HandleUserChoice(choice, currentUser, movieService, reviewService, watchlistService, ui, ref currentUser);
            }
        }
    }

    #region Admin Handler
    static void HandleAdminChoice(string choice, MovieService movieService, UiService ui, ref User? currentUser)
    {
        try
        {
            switch (choice)
            {
                case "1":
                    var details = ui.GetMovieDetails();
                    movieService.AddMovie(details.title, details.desc, details.author, details.category);
                    Console.WriteLine("Movie added!");
                    break;

                case "2":
                    var moviesToEdit = movieService.GetAllMovies();
                    var editMovie = ui.GetMovieSelection(moviesToEdit);
                    if (editMovie != null)
                    {
                        var newDetails = ui.GetMovieDetails();
                        movieService.EditMovie(editMovie.Id, newDetails.title, newDetails.desc, newDetails.author, newDetails.category);
                        Console.WriteLine("Movie updated!");
                    }
                    break;

                case "3":
                    var moviesToDelete = movieService.GetAllMovies();
                    var delMovie = ui.GetMovieSelection(moviesToDelete);
                    if (delMovie != null)
                    {
                        Console.Write($"Are you sure you want to delete '{delMovie.Title}'? (y/n): ");
                        var confirm = Console.ReadLine()?.ToLower();
                        if (confirm == "y")
                        {
                            movieService.DeleteMovie(delMovie.Id);
                            Console.WriteLine("Movie deleted!");
                        }
                        else
                        {
                            Console.WriteLine("Delete canceled.");
                        }
                    }
                    break;

                case "4":
                    var allMovies = movieService.GetAllMovies();
                    ui.DisplayAllMovies(allMovies);
                    break;

                case "0":
                    currentUser = null;
                    Console.WriteLine("Logged out.");
                    break;

                default:
                    Console.WriteLine("Invalid choice.");
                    break;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
    #endregion

    #region User Handler
    static void HandleUserChoice(string choice, User user, MovieService movieService,
                                 ReviewService reviewService, WatchlistService watchlistService,
                                 UiService ui, ref User? currentUser)
    {
        try
        {
            switch (choice)
            {
                case "1":
                    ui.DisplayAllMovies(movieService.GetAllMovies(), reviewService);
                    break;

                case "2":
                    var keyword = ui.GetKeyword();
                    var searchResults = string.IsNullOrWhiteSpace(keyword)
                        ? movieService.GetAllMovies()
                        : movieService.SearchMovies(keyword);
                    ui.DisplayAllMovies(searchResults, reviewService);
                    break;

                case "3":
                    var keywordDetails = ui.GetKeyword("Enter movie name/author/category to view: ");
                    var moviesDetails = string.IsNullOrWhiteSpace(keywordDetails)
                        ? movieService.GetAllMovies()
                        : movieService.SearchMovies(keywordDetails);
                    var selectedDetailsMovie = ui.GetMovieSelection(moviesDetails);
                    if (selectedDetailsMovie != null)
                        ui.DisplayMovieDetails(selectedDetailsMovie, reviewService);
                    break;

                case "4":
                    var keywordReview = ui.GetKeyword("Enter movie to review: ");
                    var moviesForReview = movieService.SearchMovies(keywordReview);
                    var selectedReviewMovie = ui.GetMovieSelection(moviesForReview);
                    if (selectedReviewMovie != null)
                    {
                        var reviewInput = ui.GetReviewDetails(selectedReviewMovie);
                        reviewService.AddReview(user.Id, selectedReviewMovie.Id, reviewInput.rating, reviewInput.content);
                        Console.WriteLine("Review added!");
                    }
                    break;

                case "5": 
                    var keywordWatchlist = ui.GetKeyword("Enter movie to add to watchlist: ");
                    var moviesForWatchlist = movieService.SearchMovies(keywordWatchlist);
                    var selectedWatchlistMovie = ui.GetMovieSelection(moviesForWatchlist);
                    if (selectedWatchlistMovie != null)
                    {
                        watchlistService.AddToWatchlist(user.Id, selectedWatchlistMovie.Id);
                        Console.WriteLine("Movie added to watchlist!");
                    }
                    break;

                case "6":
                    var keywordWatched = ui.GetKeyword("Enter movie to mark as watched: ");
                    var moviesForWatched = movieService.SearchMovies(keywordWatched);
                    var selectedWatchedMovie = ui.GetMovieSelection(moviesForWatched);
                    if (selectedWatchedMovie != null)
                    {
                        watchlistService.MarkAsWatched(user.Id, selectedWatchedMovie.Id);
                        Console.WriteLine("Movie marked as watched!");
                    }
                    break;

                case "7":
                    var keywordUnwatched = ui.GetKeyword("Enter movie to mark as unwatched: ");
                    var moviesForUnwatched = movieService.SearchMovies(keywordUnwatched);
                    var selectedUnwatchedMovie = ui.GetMovieSelection(moviesForUnwatched);
                    if (selectedUnwatchedMovie != null)
                    {
                        watchlistService.MarkAsUnwatched(user.Id, selectedUnwatchedMovie.Id);
                        Console.WriteLine("Movie marked as unwatched!");
                    }
                    break;

                case "8": // Remove from watchlist
                    var keywordRemove = ui.GetKeyword("Enter movie to remove from watchlist: ");
                    var moviesForRemove = movieService.SearchMovies(keywordRemove);
                    var selectedRemoveMovie = ui.GetMovieSelection(moviesForRemove);
                    if (selectedRemoveMovie != null)
                    {
                        if (watchlistService.RemoveFromWatchlist(user.Id, selectedRemoveMovie.Id))
                            Console.WriteLine("Movie removed from watchlist!");
                        else
                            Console.WriteLine("Movie not in watchlist.");
                    }
                    break;

                case "9":
                    var history = watchlistService.GetWatchHistory(user.Id);
                    ui.DisplayWatchlist(history, "Watch History");
                    break;

                case "10":
                    var watchlist = watchlistService.GetCurrentWatchlist(user.Id);
                    ui.DisplayWatchlist(watchlist, "Current Watchlist");
                    break;

                case "0":
                    currentUser = null;
                    Console.WriteLine("Logged out.");
                    break;

                default:
                    Console.WriteLine("Invalid choice.");
                    break;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
    #endregion
}
