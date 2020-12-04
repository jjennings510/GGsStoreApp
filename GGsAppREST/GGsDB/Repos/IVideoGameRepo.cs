using GGsDB.Models;
using System.Collections.Generic;

namespace GGsDB.Repos
{
    public interface IVideoGameRepo
    {
        void AddVideoGame(VideoGame videoGame);
        void UpdateVideoGame(VideoGame videoGame);
        VideoGame GetVideoGameById(int id);
        VideoGame GetVideoGameByName(string name);
        List<VideoGame> GetAllVideoGames();
        void DeleteVideoGame(VideoGame videoGame);
    }
}