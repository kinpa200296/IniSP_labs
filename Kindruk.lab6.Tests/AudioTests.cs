using System;
using Audio;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Kindruk.lab6.Tests
{
    [TestClass]
    public class AudioTests
    {
        [TestMethod]
        public void CalculationsTests()
        {
            var playList = CreatePlayList();
            Assert.AreEqual(7.8, playList.Rating);
            Assert.AreEqual(1185.0, playList.TotalLength.TotalSeconds);
        }

        [TestMethod]
        public void PlayListManagerTests()
        {
            var playList1 = CreatePlayList();
            PlayListManager.Save(playList1, "LPF.pls");
            var playList2 = (PlayList)PlayListManager.Load("LPF.pls");
            Assert.AreEqual(playList1.Name, playList2.Name);
            Assert.AreEqual(playList1.Rating, playList2.Rating);
            Assert.AreEqual(playList1.TotalLength, playList2.TotalLength);
            Assert.AreEqual(playList1.Songs.Count, playList2.Songs.Count);
            for (var i = 0; i < playList1.Songs.Count; i++)
            {
                Assert.AreEqual(playList1.Songs[i].Name, playList2.Songs[i].Name);
                Assert.AreEqual(playList1.Songs[i].Length.Ticks, playList2.Songs[i].Length.Ticks);
                Assert.AreEqual(playList1.Songs[i].Performer, playList2.Songs[i].Performer);
                Assert.AreEqual(playList1.Songs[i].Genre, playList2.Songs[i].Genre);
                Assert.AreEqual(playList1.Songs[i].Rating, playList2.Songs[i].Rating);
            }
        }

        private static PlayList CreatePlayList()
        {
            var playList = new PlayList
            {
                Name = "LP Favorites"
            };
            playList.Songs.Add(new Song
            {
                Name = "Rebellion",
                Length = new TimeSpan(0, 0, 3, 44),
                Performer = "Linkin Park",
                Genre = Genres.Rock,
                Rating = 5
            });
            playList.Songs.Add(new Song
            {
                Name = "The Catalyst",
                Length = new TimeSpan(0, 0, 5, 39),
                Performer = "Linkin Park",
                Genre = Genres.Rock,
                Rating = 8
            });
            playList.Songs.Add(new Song
            {
                Name = "New Divide",
                Length = new TimeSpan(0, 0, 4, 31),
                Performer = "Linkin Park",
                Genre = Genres.Rock,
                Rating = 7
            });
            playList.Songs.Add(new Song
            {
                Name = "Numb",
                Length = new TimeSpan(0, 0, 3, 7),
                Performer = "Linkin Park",
                Genre = Genres.Rock,
                Rating = 9
            });
            playList.Songs.Add(new Song
            {
                Name = "Bleed It Out",
                Length = new TimeSpan(0, 0, 2, 44),
                Performer = "Linkin Park",
                Genre = Genres.Rock,
                Rating = 10
            });
            playList.CalculateRating();
            playList.CalculateTotalLength();
            return playList;
        }
    }
}
