using MoviesLibrary.Common.Enum;
using MoviesLibrary.Services;

namespace MoviesLibrary.Test
{
    [TestClass]
    public class UnitTest
    {
        readonly MovieService _movieService;
        readonly List<string> _listItems;

        public UnitTest()
        {
            _movieService = new MovieService(true);
            _listItems = new List<string> { "寻梦环游记", "斗罗大陆", "海上钢琴师" };
        }

        [TestMethod("搜索测试"), Priority(1)]
        public void TestMethodSearch()
        {
            _listItems.ForEach(item =>
            {
                var result = _movieService.Search(item);
                Assert.IsTrue(result.Any(), $"获取“{item}”数据失败！");
            });
        }

        [TestMethod("获取详情"), Priority(2)]
        public void TestMethodDetail_1()
        {
            string entId = "faXpYRH6Rnb4UR";
            var detail = _movieService.GetDetail(CatType.电影, entId);
            Assert.IsTrue(detail.EntId == entId);
        }

        [TestMethod("获取详情_分页查询"), Priority(2), TestCategory("获取指定页面范围的数据")]
        public void TestMethodDetail_2()
        {
            string entId = "Q4RraX7lTzbqN3";
            var detail = _movieService.GetDetail(CatType.电视剧, entId, 1, 20, PlayLinkType.imgo);
            Assert.IsTrue(detail.EntId == entId);
        }

        [TestMethod("获取排行榜数据"), Priority(3)]
        public void TestMethodGetTops()
        {
            var tops = _movieService.GetTops();
            Assert.IsTrue(tops.Any(), "获取排行榜数据失败！");
        }
    }
}