using System.Text.Json;
using MoviesLibrary.Enums;
using MoviesLibrary.Services;
using System.Diagnostics;
using MoviesLibrary.Model;

namespace MoviesLibrary.Test
{
    [TestClass]
    public class UnitTest
    {
        readonly MovieService _movieService;
        readonly List<string> _listItems;

        public UnitTest()
        {
            _movieService = new MovieService();
            _listItems = new List<string> { "寻梦环游记", "斗罗大陆", "海上钢琴师" };
        }

        [TestMethod("搜索测试"), Priority(1), TestCategory("搜索")]
        public void TestMethodSearch()
        {
            _listItems.ForEach(item =>
            {
                ValueTuple<IEnumerable<Movie>, IEnumerable<TopModel>> result = _movieService.Search(item);
                Debug.WriteLine(JsonSerializer.Serialize(result.Item1), $"{item}_Movies");
                Debug.WriteLine(JsonSerializer.Serialize(result.Item2), $"{item}_Tops");
                Assert.IsTrue(result.Item1.Any() && result.Item2.Any(), $"获取“{item}”数据失败！");
            });
        }

        [TestMethod("获取详情"), TestCategory("详情")]
        public void TestMethodDetail_1()
        {
            string entId = "faXpYRH6Rnb4UR";
            var detail = _movieService.GetDetail(CatType.Film, entId);
            Debug.WriteLine(JsonSerializer.Serialize(detail), detail.Title);
            Assert.IsTrue(detail.EntId == entId);
        }

        [TestMethod("获取详情_分页查询"), TestCategory("详情")]
        public void TestMethodDetail_2()
        {
            string entId = "Q4RraX7lTzbqN3";
            var detail = _movieService.GetDetail(CatType.Teleplay, entId, 1, 20, PlayLinkType.imgo);
            Debug.WriteLine(JsonSerializer.Serialize(detail), detail.Title);
            Assert.IsTrue(detail.EntId == entId);
        }

        [TestMethod("获取排行榜数据"), TestCategory("排行榜")]
        public void TestMethodGetTops()
        {
            var tops = _movieService.GetTops(TopType.Default);
            Debug.WriteLine(JsonSerializer.Serialize(tops));
            Assert.IsTrue(tops.Any(), "获取排行榜数据失败！");

        }

        [TestMethod("获取排行榜数据_多种类型"), TestCategory("排行榜")]
        public void TestMethodGetTops2()
        {
            List<TopType> types = new()
            {
                TopType.Film,
                TopType.Teleplay,
                TopType.Variety,
                TopType.Anime
            };
            var tops = _movieService.GetTops(types);
            Debug.WriteLine(JsonSerializer.Serialize(tops));
            Assert.IsTrue(tops.Any(), "获取排行榜数据失败！");
        }

        [TestMethod("获取精彩推荐"), TestCategory("推荐")]
        public void TestMethodGetRecommends()
        {
            var recommends = _movieService.GetRecommends(CatType.Film);
            Debug.WriteLine(JsonSerializer.Serialize(recommends));
            Assert.IsTrue(recommends.Any(), "获取精彩推荐数据失败！");
        }
    }
}