using GGsDB.Models;
using GGsDB.Repos;
using System.Collections.Generic;

namespace GGsLib
{
    public class VideoGameService : IVideoGameService
    {
        IVideoGameRepo repo;
        public VideoGameService(IRepo repo)
        {
            this.repo = repo;
        }
        public void AddVideoGame(VideoGame videoGame)
        {
            repo.AddVideoGame(videoGame);
        }
        public void DeleteVideoGame(VideoGame videoGame)
        {
            repo.DeleteVideoGame(videoGame);
        }
        public List<VideoGame> GetAllVideoGames()
        {
            return repo.GetAllVideoGames();
        }
        public VideoGame GetVideoGameById(int id)
        {
            return repo.GetVideoGameById(id);
        }
        public VideoGame GetVideoGameByName(string name)
        {
            return repo.GetVideoGameByName(name);
        }
        public void UpdateVideoGame(VideoGame videoGame)
        {
            repo.UpdateVideoGame(videoGame);
        }
    }
}