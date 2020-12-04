using GGsDB.Entities;
using GGsDB.Models;
using System.Collections.Generic;

namespace GGsDB.Mappers
{
    public interface IVideoGameMapper
    {
        VideoGame ParseVideoGame(Videogames videogame);
        Videogames ParseVideoGame(VideoGame videogame);
        List<VideoGame> ParseVideoGame(ICollection<Videogames> videogames);
        ICollection<Videogames> ParseVideoGame(List<VideoGame> videogames);
    }
}